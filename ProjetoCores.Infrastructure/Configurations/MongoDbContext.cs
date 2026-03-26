using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ProjetoCores.Infrastructure.Configurations;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbContext(IConfiguration configuration, IMongoClient client)
    {
        var databaseName = configuration["Mongo:Database"];

        _database = client.GetDatabase(databaseName);
    }
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}