using Atelie.Application.Models;
using Atelie.Application.Models.Customers;
using Atelie.Domain.Entities;
using Atelie.Domain.Exceptions;
using Atelie.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Atelie.Application.Services.Implementations;

public class CustomerService : ICustomerService
{
    private readonly DatabaseContext _context;
    private readonly IMapper _mapper;

    public CustomerService(DatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<ApiResult<string>> CreateCustomerAsync(CreateCustomerDto dto)
    {
        // Minimal validatsiya
        if (dto is null)
            return ApiResult<string>.Failure(["Ma'lumot yuborilmadi."]);

        if (string.IsNullOrWhiteSpace(dto.FullName))
            return ApiResult<string>.Failure(["Ism-familya majburiy."]);

        if (string.IsNullOrWhiteSpace(dto.Address))
            return ApiResult<string>.Failure(["Manzil majburiy."]);

        // Ixtiyoriy: telefon unikal bo‘lsin (agar berilgan bo‘lsa)
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            var phoneExists = await _context.Customers
                .AnyAsync(c => c.PhoneNumber == dto.PhoneNumber);
            if (phoneExists)
                return ApiResult<string>.Failure(new[] { "Ushbu telefon raqam allaqachon mavjud." });
        }

        var entity = _mapper.Map<Customer>(dto);
        await _context.Customers.AddAsync(entity);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mijoz muvaffaqiyatli qo‘shildi.");
    }

    public async Task<CustomerDto> GetByIdCustomerAsync(long id)
    {
        var entity = await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (entity is null)
            throw new NotFoundException("Customer not found");

        return _mapper.Map<CustomerDto>(entity);
    }

    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        var list = await _context.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.Id)
            .ToListAsync();

        return _mapper.Map<List<CustomerDto>>(list);
    }

    public async Task<ApiResult<string>> UpdateCustomerAsync(long id, UpdateCustomerDto dto)
    {
        if (dto is null)
            return ApiResult<string>.Failure(new[] { "Ma'lumot yuborilmadi." });

        var entity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (entity is null)
            return ApiResult<string>.Failure(new[] { "Mijoz topilmadi." });

        // Ixtiyoriy: telefon unikal bo‘lsin (agar berilgan bo‘lsa)
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
        {
            var phoneExists = await _context.Customers
                .AnyAsync(c => c.Id != id && c.PhoneNumber == dto.PhoneNumber);
            if (phoneExists)
                return ApiResult<string>.Failure(new[] { "Ushbu telefon raqam boshqa mijozga biriktirilgan." });
        }

        // Map DTO -> Entity (faqat kerakli fieldlarni yangilaymiz)
        entity.FullName = dto.FullName;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.Address = dto.Address;
        entity.CustomerStatus = dto.CustomerStatus;
        entity.TelegramUsername = dto.TelegramUsername;
        entity.SubmissionDate = dto.SubmissionDate;
        entity.ContactTime = dto.ContactTime;

        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mijoz ma'lumotlari yangilandi.");
    }

    public async Task<ApiResult<string>> DeleteCustomerAsync(long id)
    {
        var entity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (entity is null)
            return ApiResult<string>.Failure(new[] { "Mijoz topilmadi." });

        _context.Customers.Remove(entity);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mijoz o‘chirildi.");
    }
}