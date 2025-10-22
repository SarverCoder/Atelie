using Atelie.Application.Models.Dashboards;
using Atelie.Domain.Entities;
using Atelie.Domain.Enums;
using Atelie.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Atelie.Application.Services.Implementations;

public class DashboardService : IDashboardService
{

    private readonly DatabaseContext _context;

    public DashboardService(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<DashboardSummaryDto> GetAsync(DateTime? from = null, DateTime? to = null, int recentTake = 10)
    {
        // Helper: har qanday DateTime-ni UTC ga aylantiradi
        static DateTime ToUtc(DateTime dt) => dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc) // Unspecified bo'lsa, UTC deb belgilaymiz
        };

        var nowUtc = DateTime.UtcNow;

        // Joriy oy boshi — darrov UTC Kind bilan
        var startOfThisMonthUtc = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        // Diapazonlar: kelgan bo'lsa UTCga o'tkaz, bo'lmasa default
        var start = from.HasValue ? ToUtc(from.Value) : startOfThisMonthUtc;
        var end = to.HasValue ? ToUtc(to.Value) : startOfThisMonthUtc.AddMonths(1);

        // Oldingi oy
        var prevStart = start.AddMonths(-1);
        var prevEnd = start;

        var qThis = _context.Set<Order>()
            .AsNoTracking()
            .Where(o => o.CreatedAt >= start && o.CreatedAt < end && o.Status != OrderStatus.BekorQilindi);

        var qPrev = _context.Set<Order>()
            .AsNoTracking()
            .Where(o => o.CreatedAt >= prevStart && o.CreatedAt < prevEnd && o.Status != OrderStatus.BekorQilindi);

        var thisList = await qThis
            .Select(o => new { o.Id, o.Status, Amount = o.AdvancePayment })
            .ToListAsync();

        var prevList = await qPrev
            .Select(o => new { o.Id, o.Status, Amount = o.AdvancePayment })
            .ToListAsync();

        int totalOrdersThis = thisList.Count;
        int totalOrdersPrev = prevList.Count;

        int activeThis = thisList.Count(x => x.Status != OrderStatus.BekorQilindi && x.Status != OrderStatus.Tayyor);
        int activePrev = prevList.Count(x => x.Status != OrderStatus.BekorQilindi && x.Status != OrderStatus.Tayyor);

        int readyThis = thisList.Count(x => x.Status == OrderStatus.Tayyor);
        int readyPrev = prevList.Count(x => x.Status == OrderStatus.Tayyor);

        decimal revenueThis = thisList.Sum(x => x.Amount);
        decimal revenuePrev = prevList.Sum(x => x.Amount);

        return new DashboardSummaryDto
        {
            TotalOrders = totalOrdersThis,
            TotalOrdersMoM = PercentChange(totalOrdersThis, totalOrdersPrev),

            ActiveOrders = activeThis,
            ActiveOrdersMoM = PercentChange(activeThis, activePrev),

            ReadyOrders = readyThis,
            ReadyOrdersMoM = PercentChange(readyThis, readyPrev),

            TotalRevenue = revenueThis,
            TotalRevenueMoM = PercentChange(revenueThis, revenuePrev)
        };


    }

    public async Task<List<DashboardLastOrderDto>> GetRecentOrdersAsync()
    {
        return await _context.Set<Order>().AsNoTracking()
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .Where(x => x.Status != OrderStatus.BekorQilindi)
            .OrderByDescending(o => o.CreatedAt)
            .Take(5)
            .Select(o => new DashboardLastOrderDto
            {
                OrderNumber = o.OrderNumber,
                CustomerName = o.Customer.FullName,
                ItemsShort = string.Join(" + ", o.OrderItems.Select(i => i.OrderType.ToString()).Distinct()),
                CreatedAt = o.CreatedAt,
                Status = o.Status.ToString(),
                Amount = o.BasePrice + (o.AdditionalCost ?? 0m)
            })
            .ToListAsync();
    }

    public async Task<DashboardStatusDto> GetStatusCountsAsync(OrderStatus status)
    {
        var count = await _context.Set<Order>()
            .AsNoTracking()
            .CountAsync(o => o.Status == status);

        return new DashboardStatusDto
        {
            Status = status.ToString(),
            Count = count
        };
    }

    private static decimal? PercentChange(decimal current, decimal previous)
    {
        if (previous == 0) return current == 0 ? 0 : 100; // yoki null qilishing mumkin
        return Math.Round(((current - previous) / previous) * 100m, 2);
    }
}