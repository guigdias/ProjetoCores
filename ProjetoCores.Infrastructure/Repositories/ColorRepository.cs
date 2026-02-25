using MongoDB.Driver;
using ProjetoCores.Domain.Entities;
using ProjetoCores.Domain.Interfaces;
using ProjetoCores.Infrastructure.Configurations;

namespace ProjetoCores.Infrastructure.Repositories;

public class ColorRepository : IColorRepository
{
    private readonly IMongoCollection<Color> _collection; // _collection consegue utilizar os métodos do IMongoCollection

    public ColorRepository(MongoDbContext context)
    {
        _collection = context.GetCollection<Color>("Colors");
    }
    public async Task Create(Color color)  // Sem o <> pois não possui retorno
    {
        await _collection.InsertOneAsync(color);
    }
    public async Task<List<Color>> GetAll() // com <> pois possui retorno
    {
        return await _collection.Find(_ => true) // filtro vazio = busca todos os documentos
        .ToListAsync();
    }
    public async Task<Color?> GetById(string id) // com <> pois possui retorno
    {
        return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync(); // filtro com expressão lambda, para encontra pelo ID
    }
    public async Task<bool> Update(Color color)
    {
        var result = await _collection.ReplaceOneAsync(
            c => c.Id == color.Id, // filtro com expressão lambda, para encontra pelo ID
            color // novo documento do mongo
        );
        return result.ModifiedCount > 0; // verifica se foi atualizado
    }
    public async Task<bool> Delete(string id)
    {
        var result = await _collection.DeleteOneAsync(c => c.Id == id); // filtro com expressão lambda, para encontra pelo ID
        return result.DeletedCount > 0; // verifica se foi atualizado
    }
}
