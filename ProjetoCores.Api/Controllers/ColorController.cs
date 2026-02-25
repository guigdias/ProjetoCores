using Microsoft.AspNetCore.Mvc;
using ProjetoCores.Domain.Services;
using ProjetoCores.Api.DTOs;

namespace ProjetoCores.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly ColorService _colorService;

    public ColorController(ColorService colorService)
    {
        _colorService = colorService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateColorDto dto)
    {
        var color = await _colorService.Create(dto.Name, dto.Red, dto.Green, dto.Blue);

        return CreatedAtAction(nameof(GetById), new { id = color.Id }, color);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var colors = await _colorService.GetAll();
        return Ok(colors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var color = await _colorService.FindById(id);

        if (color == null) return NotFound();
        return Ok(color);
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> Put(string id, UpdateColorDto dto)
    {
        var newColor = await _colorService.Update(id, dto.Name, dto.Red, dto.Green, dto.Blue);
            
        if(!newColor) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _colorService.Delete(id);

        if (!deleted) return NotFound();
        return NoContent();
    }
}
