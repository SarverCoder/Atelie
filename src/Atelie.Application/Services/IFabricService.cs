using Atelie.Application.Models;
using Atelie.Application.Models.Fabrics;

namespace Atelie.Application.Services;

public interface IFabricService
{
    Task<ApiResult<string>> CreateFabricAsync(CreateFabricDto dto);
    Task<FabricDto> GetFabricByIdAsync(int id);
    Task<List<FabricDto>> GetAllFabricsAsync();
    Task<ApiResult<string>> UpdateFabricAsync(long id, UpdateFabricDto dto);
    Task<ApiResult<string>> DeleteFabricAsync(long id);
}