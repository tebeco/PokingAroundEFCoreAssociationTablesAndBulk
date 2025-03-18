var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PokingAroundEFCoreAssociationTablesAndBulk_ApiService>("apiservice");

builder.Build().Run();
