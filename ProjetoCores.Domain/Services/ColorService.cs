using ProjetoCores.Domain.Entities;
using ProjetoCores.Domain.Interfaces;

namespace ProjetoCores.Domain.Services;

public class ColorService
{
    private readonly IColorRepository _repository;

    public ColorService(IColorRepository repository)
    {
        _repository = repository;
    }

    public async Task<Color> Create(string name, int red, int green, int blue)
    {
        var color = new Color(name, red, green, blue);
        await _repository.Create(color);
        return color;
    }

    public Task<List<Color>> GetAll() => _repository.GetAll();

    public Task<Color?> FindById(string id) => _repository.GetById(id);

    public async Task<bool> Update(string id, string name, int red, int green, int blue)
    {
        var color = await _repository.GetById(id);

        if (color == null)
            throw new Exception("Color not found.");

        color.Update(name, red, green, blue);

        await _repository.Update(color);

        return true;
    }

    public Task<bool> Delete(string id) => _repository.Delete(id);
}