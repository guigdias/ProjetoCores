using FluentValidation;
using ProjetoCores.Domain.Entities;
using ProjetoCores.Domain.Interfaces;
namespace ProjetoCores.Domain.Services;

public class ColorService
{
    private readonly IValidator<Color> _validator;
    private readonly IColorRepository _repository;

    public ColorService(IColorRepository repository, IValidator<Color> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Color> Create(string name, string hex)
    {
       var (r, g, b) = ConvertHexToRgb(hex);

        var color = new Color(name, r, g, b);

        _validator.ValidateAndThrow(color);

        await _repository.Create(color);

        return color;
    }

    public Task<List<Color>> GetAll() => _repository.GetAll();

    public async Task<Color?> FindById(string id)
    {
        var color = await _repository.GetById(id);
        if (color is null)
            throw new KeyNotFoundException("Color not found");

        return color;
    }

    public async Task<Color> Update(string id, string name, string hex)
    {
        var color = await _repository.GetById(id);
        if (color is null)
            throw new KeyNotFoundException("Color not found");

        var (r, g, b) = ConvertHexToRgb(hex);

        color.Update(name, r, g, b);

        _validator.ValidateAndThrow(color);

        await _repository.Update(color);

        return color;

    }

    public Task<bool> Delete(string id) => _repository.Delete(id);

    public async Task<Color> MergeColors(List<string> colorsIds)
    {
        var colors = await _repository.GetByIdsAsync(colorsIds);
        if (colors.Count < 2)
            throw new ValidationException("At least 2 colors are required");
        if (colors.Count != colorsIds.Count)
            throw new ValidationException("One or more valid colors are missing");

        var mergedCount = await _repository.CountMergedAsync();
        var mergedName = $"Merged{mergedCount + 1}";
        var mergedColor = Color.CreateMergedColor(mergedName, colors);

        _validator.ValidateAndThrow(mergedColor);

        await _repository.Create(mergedColor);

        return mergedColor;
      
    }
    private (int r, int g, int b) ConvertHexToRgb(string hex)
    {
        try 
        {
            hex = hex.Replace("#", ""); // substituir # por um espaço em branco
            var r = Convert.ToInt32(hex.Substring(0, 2), 16); // R corresponderá aos 2 primeiros caracteres da string (0,1)
            var g = Convert.ToInt32(hex.Substring(2, 2), 16); // G corresponderá as posições (2,3)
            var b = Convert.ToInt32(hex.Substring(4, 2), 16); // B corresponderá as posições (4,5)

            // Parametro 16 significa para interpretar o numero como base hexadecimal
            return (r, g, b);
        }
        catch
        {
            throw new ArgumentException("Invalid Hex Format");
        }
    }

}