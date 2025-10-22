using Atelie.Application.Models;
using Atelie.Application.Models.Orders;
using Atelie.Domain.Entities;
using Atelie.Domain.Enums;
using Atelie.Domain.Exceptions;
using Atelie.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Atelie.Application.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;
    public OrderService(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResult<string>> CreateOrderAsync(CreateOrderDto dto)
    {
     
        // --- Basic validation ---
        var customerExists = await _context.Customers
            .AnyAsync(c => c.Id == dto.CustomerId);

        if (!customerExists)
            throw new NotFoundException("Mijoz topilmadi.");

        if (dto.BasePrice < 0 || (dto.AdditionalCost ?? 0) < 0 || dto.AdvancePayment < 0)
            throw new ValidationException("Narxlar null bo'lishi mumkin emas");

        // --- Calculate payments ---
        var total = dto.BasePrice + (dto.AdditionalCost ?? 0m);
        var remaining = total - dto.AdvancePayment;
        if (remaining < 0) remaining = 0;

        // --- Generate order number if empty ---
        var orderNumber = await GenerateOrderNumberAsync();

        // --- Build entity ---
        var order = new Order
        {
            CustomerId = dto.CustomerId,
            OrderNumber = orderNumber,

            Color = dto.Color,
            LiningType = dto.LiningType,
            ButtonType = dto.ButtonType,

            // O‘lchamlar (Order entitiy’ingizda mos property’lar bor deb qabul qildim)
            ОГ = dto.ОГ,
            ОТ = dto.ОТ,
            ОБ = dto.ОБ,
            ШГ = dto.ШГ,
            ШС = dto.ШС,
            ДТС = dto.ДТС,
            ДТП2 = dto.ДТП2,
            ДЛ = dto.ДЛ,
            ПЛ = dto.ПЛ,
            Ру = dto.Ру,
            Шбс = dto.Шбс,
            Шн = dto.Шн,
            ДГ = dto.ДГ,

            BasePrice = dto.BasePrice,
            AdditionalCost = dto.AdditionalCost,
            AdvancePayment = dto.AdvancePayment,
            RemainingAmount = remaining,

            SpecialRequests = dto.SpecialRequests,
            Notes = dto.Notes, 
           
            Status = OrderStatus.Yangi,  // default: Created
            CompletedDate = dto.CompletedDate,

            OrderItems = new List<OrderItem>(),
            OrderWorkers = new List<OrderWorker>()
        };

        foreach (var it in dto.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                OrderType = it.OrderType,
                FabricId = it.FabricId
            });
        }

        foreach (var w in dto.Workers)
        {
            order.OrderWorkers.Add(new OrderWorker
            {
                UserId = w.UserId,
                Cost = w.Cost,
                WorkerRole = w.WorkerRole
            });
        }

        // --- Save with transaction ---
        await using var tx = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return ApiResult<string>.Success($"Buyurtma yaratildi {order.OrderNumber}");
        }
        catch (DbUpdateException ex)
        {
            await tx.RollbackAsync();
            throw new NotFoundException($"Saqlashda xatolik: {ex.Message}");
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync();
            return ApiResult<string>.Failure([$"Kutilmagan xatolik: {ex.Message}"]);
        }
    }

    public async Task<OrderDto> GetOrderByIdAsync(long id)
    {
        // ProjectTo uchun kerakli navigatsiyalar mapping’da ko‘rsatilgan bo‘lishi kifoya
        var dto = await _context.Orders
            .AsNoTracking()
            .Where(o => o.Id == id)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        if (dto == null)
            throw new NotFoundException("Buyurtma topilmadi.");

        return dto;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        var dto = await _context.Orders
            .AsNoTracking()
            .OrderByDescending(o => o.Id)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return dto;
    }

    public async Task<ApiResult<string>> UpdateOrderStatus(long id, OrderStatus status)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);

        if (order == null)
            throw new NotFoundException("Buyrtma topilmadi");

        order.Status = status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResult<string>.Success($"Order status {order.Status.ToString()} ga yangilandi");
    }

    public async Task<ApiResult<string>> SetDeleteStatusAsync(long id, bool isDeleted)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
            throw new NotFoundException("Order not found.");

        if (order.IsDeleted == isDeleted)
        {
            return ApiResult<string>.Success(
                isDeleted
                    ? "Order already deleted"
                    : "Order already active"
            );
        }

        order.IsDeleted = isDeleted;
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success(
            isDeleted
                ? "Order deleted successfully"
                : "Order restored successfully"
        );
    }


    /// <summary>
    /// AT-YYYYMMDD-#### formatida ketma-ket raqam generatsiyasi.
    /// Masalan: AT-20251008-0001
    /// </summary>
    private async Task<string> GenerateOrderNumberAsync()
    {
        var today = DateTime.UtcNow.Date; // yoki local vaqt kerak bo'lsa .Now
        var prefix = $"AT-{today:yyyyMMdd}-";

        // Oxirgi shu kun uchun yaratilgan orderni topamiz va inkrement qilamiz
        var lastToday = await _context.Orders
            .Where(o => o.OrderNumber.StartsWith(prefix))
            .OrderByDescending(o => o.Id)
            .Select(o => o.OrderNumber)
            .FirstOrDefaultAsync();

        int next = 1;
        if (!string.IsNullOrEmpty(lastToday))
        {
            var tail = lastToday.Substring(prefix.Length);
            if (int.TryParse(tail, out var seq))
                next = seq + 1;
        }

        return $"{prefix}{next.ToString("0000")}";
    }

}