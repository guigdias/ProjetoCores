using MongoDB.Driver;
using FluentValidation;
using FluentValidation.AspNetCore;
using ProjetoCores.Domain.Interfaces;
using ProjetoCores.Domain.Services;
using ProjetoCores.Infrastructure.Configurations;
using ProjetoCores.Infrastructure.Repositories;
using ProjetoCores.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<ColorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MergeColorsDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateColorDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateColorDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();


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
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();