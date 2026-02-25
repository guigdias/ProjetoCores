var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo");

var api = builder.AddProject<Projects.ProjetoCores_Api>("api").WithReference(mongo);

builder.Build().Run();
