var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.LoanApp_ApiService>("apiservice");

builder.AddProject<Projects.LoanApp_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
