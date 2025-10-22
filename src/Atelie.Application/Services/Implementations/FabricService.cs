using Atelie.Application.Models;
using Atelie.Application.Models.Fabrics;
using Atelie.Domain.Entities;
using Atelie.Domain.Exceptions;
using Atelie.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Atelie.Application.Services.Implementations;

public class FabricService : IFabricService
{
    
    private readonly IMapper _mapper;
    private readonly DatabaseContext _context;
    public FabricService(IMapper mapper, DatabaseContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<ApiResult<string>> CreateFabricAsync(CreateFabricDto dto)
    {
        var fabric = _mapper.Map<Fabric>(dto);

        await _context.Fabrics.AddAsync(fabric);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mato muvaffaqiyatli qo'shildi.");

    }

    public async Task<FabricDto> GetFabricByIdAsync(int id)
    {
        var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == id);

        if (fabric is null)
        {
            throw new NotFoundException("Fabric not found");
        }

        return _mapper.Map<FabricDto>(fabric);

    }

    public async Task<List<FabricDto>> GetAllFabricsAsync()
    {
        var fabrics = await _context.Fabrics.ToListAsync();

        return _mapper.Map<List<FabricDto>>(fabrics);
    }

    public async Task<ApiResult<string>> UpdateFabricAsync(long id, UpdateFabricDto dto)
    {
        var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == id);

        if (fabric is null)
        {
            throw new NotFoundException("Fabric not found");
        }

        _mapper.Map(dto, fabric);

        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mato muvaffaqiyatli yangilandi.");

    }

    public async Task<ApiResult<string>> DeleteFabricAsync(long id)
    {
        var fabric = await _context.Fabrics.FirstOrDefaultAsync(x => x.Id == id);

        if (fabric is null)
        {
            throw new NotFoundException("Fabric not found");
        }

        _context.Fabrics.Remove(fabric);
        await _context.SaveChangesAsync();

        return ApiResult<string>.Success("Mato muvaffaqiyatli o'chirildi.");
    }
}