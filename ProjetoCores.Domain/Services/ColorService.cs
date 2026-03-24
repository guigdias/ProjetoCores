using FluentValidation;
using ProjetoCores.Domain.Entities;
using ProjetoCores.Domain.Interfaces;
using ProjetoCores.Domain.Exceptions;
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
        var color = Color.CreateColorFromHex(name, hex);

        _validator.Validate(color);

        await _repository.Create(color);

        return color;
    }
    public Task<List<Color>> GetAll() => _repository.GetAll();

    public async Task<Color?> FindById(string id)
    {
        var color = await _repository.GetById(id);
      
        return color;
    }

    public async Task<Color> GetColorOrThrow(string id)
    {
        var color = await _repository.GetById(id);

        if(color == null)
        throw new NotFoundException("Color not found");

        return color;
    }
    public async Task UpdateFromHex(string id, string hex)
    {
        var color = await GetColorOrThrow(id);

        color.UpdateColorFromHex(hex);

        await _repository.Update(color);

    }

    public Task<bool> Delete(string id) => _repository.Delete(id);

    public async Task<Color> MergeColors(List<string?> colorsIds)
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

}