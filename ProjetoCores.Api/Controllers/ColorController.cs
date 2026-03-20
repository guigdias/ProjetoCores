using Microsoft.AspNetCore.Mvc;
using ProjetoCores.Domain.Services;
using ProjetoCores.Api.DTOs;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace ProjetoCores.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorController : ControllerBase
{
    private readonly ColorService _colorService;
    private readonly IMapper _mapper;

    public ColorController(ColorService colorService, IMapper mapper)
    {
        _colorService = colorService;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateColorDto dto)
    {
        try
        {
            var color = await _colorService.Create(dto.Name, dto.Hex);

            return CreatedAtAction(nameof(GetById), new { id = color.Id }, _mapper.Map<ColorResponseDto>(color)); // 201
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors); // 400
        }
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var colors = await _colorService.GetAll();

        var response = _mapper.Map<IEnumerable<ColorResponseDto>>(colors);

        return Ok(response); // 200
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var color = await _colorService.FindById(id);
        if (color == null)
            return NotFound();

        return Ok(_mapper.Map<ColorResponseDto>(color));
    }
    [HttpPut("{id}")]
    public async Task <IActionResult> Put(string id, [FromBody] UpdateColorDto dto)
    {
        try 
        {
            await _colorService.UpdateFromHex(id, dto.Hex);

            return NoContent(); // 204
        }
        catch(ValidationException ex)
        {
            return BadRequest(ex.Errors); // 400
        }
        catch(Exception)
        {
            return NotFound(); // 404
        }
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(string id, [FromBody] JsonPatchDocument<UpdateColorDto> patchDoc)
    {
        if (patchDoc == null)
            return BadRequest();

        try
        {
            var color = await _colorService.FindById(id);
                if(color == null)
                    return NotFound();

            var dto = new UpdateColorDto
            {
                Name = color.Name,
                Hex = $"#{color.Red:X2}{color.Green:X2}{color.Blue:X2}"
            };

            patchDoc.ApplyTo(dto);

            var result = await _colorService.UpdateFromHex(id, dto.Hex);

            return Ok(result);
        }

        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
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
            var mergedColor = await _colorService.MergeColors(dto.colorsIds);
            return CreatedAtAction(nameof(GetById), new { id = mergedColor.Id },_mapper.Map<ColorResponseDto>(mergedColor));
        }
        catch(ValidationException ex)
        {
            return BadRequest(ex.Message);
        } 
    }
}
