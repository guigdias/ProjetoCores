using ProjetoCores.Domain.Entities;

namespace ProjetoCores.Domain.Interfaces;

public interface IColorRepository
{
    Task Create(Color color);
    Task<List<Color>> GetAll();
    Task<Color?> GetById(string id);
    Task<List<Color>> GetByIdsAsync(List<string?> ids);
    Task<bool> Update(Color color);
    Task<bool> Delete(string id);
    Task<int> CountMergedAsync();
}
