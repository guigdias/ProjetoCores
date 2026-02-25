using MongoDB.Driver;
using ProjetoCores.Domain.Interfaces;
using ProjetoCores.Domain.Services;
using ProjetoCores.Infrastructure.Configurations;
using ProjetoCores.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Aspire Connection
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("mongo");
    return new MongoClient(connectionString);
});

//Mongo Context
builder.Services.AddSingleton<MongoDbContext>();

// Repository
builder.Services.AddScoped<IColorRepository, ColorRepository>();

// Service
builder.Services.AddScoped<ColorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();