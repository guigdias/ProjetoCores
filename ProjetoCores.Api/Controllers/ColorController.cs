using Microsoft.AspNetCore.Mvc;
using ProjetoCores.Domain.Services;
using ProjetoCores.Api.DTOs;
using FluentValidation;

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
    public async Task<IActionResult> Post([FromBody] CreateColorDto dto)
    {
        try
        {
            var color = await _colorService.Create(dto.Name, dto.Hex);

            return CreatedAtAction(nameof(GetById), new { id = color.Id }, color); // 201
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors); // 400
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var colors = await _colorService.GetAll();
        return Ok(colors); // 200
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var color = await _colorService.FindById(id);
            return Ok(color); // 200
        }
        catch(KeyNotFoundException)
        {
            return NotFound(); // 404
        }
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> Put(string id, [FromBody] UpdateColorDto dto)
    {
        try 
        {
            var newColor = await _colorService.Update(id, dto.Name, dto.Hex);
            return NoContent(); // 204
        }
        catch(KeyNotFoundException)
        {
            return NotFound(); // 404
        }
        catch(ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        catch(ArgumentException ex)
        {
            return BadRequest(ex.Message); // 400
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _colorService.Delete(id);
            if(!deleted)
                return NotFound(); // 404

        return NoContent(); // 204
    }

    [HttpPost("merge")]
    public async Task <IActionResult> Merge(MergeColorsDto dto)
    {
        try 
        {
            var MergedColor = await _colorService.MergeColors(dto.colorsIds);
            return CreatedAtAction(nameof(GetById), new { Id = MergedColor.Id }, MergedColor);
        }
        catch(ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
        
    }
}
