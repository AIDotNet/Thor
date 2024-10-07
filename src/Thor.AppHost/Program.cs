var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Thor_Service>("thor-service");

builder.Build().Run();
