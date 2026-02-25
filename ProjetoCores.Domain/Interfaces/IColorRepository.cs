using ProjetoCores.Domain.Entities;

namespace ProjetoCores.Domain.Interfaces;

public interface IColorRepository
{
    Task<List<Color>> GetAll();
    Task<Color?> GetById(string id);
    Task Create(Color color);
    Task<bool> Update(Color color);
    Task<bool> Delete(string id);
}
