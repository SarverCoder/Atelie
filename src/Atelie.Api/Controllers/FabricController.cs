using Atelie.Application.Models.Fabrics;
using Atelie.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Atelie.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FabricController : ControllerBase
{
    private readonly IFabricService _fabricService;

    public FabricController(IFabricService fabricService)
    {
        _fabricService = fabricService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFabric([FromBody] CreateFabricDto dto)
    {
        var result = await _fabricService.CreateFabricAsync(dto);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFabricById(int id)
    {
        var fabric = await _fabricService.GetFabricByIdAsync(id);
        if (fabric == null)
            return NotFound(new { Message = "Fabric not found." });
        return Ok(fabric);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFabrics()
    {
        var fabrics = await _fabricService.GetAllFabricsAsync();
        return Ok(fabrics);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFabric(int id, [FromBody] UpdateFabricDto dto)
    {
        var result = await _fabricService.UpdateFabricAsync(id, dto);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFabric(int id)
    {
        var result = await _fabricService.DeleteFabricAsync(id);
        if (result.Succeeded)
            return Ok(result);
        return BadRequest(result);
    }

}
