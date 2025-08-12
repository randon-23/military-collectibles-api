var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MilitaryCollectiblesBackend>("militarycollectiblesbackend");

builder.Build().Run();
