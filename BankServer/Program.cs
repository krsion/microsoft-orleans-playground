using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddUserSecrets<Program>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Debug);
    })

    .UseOrleans(siloBuilder =>
    {
        string? connectionString = siloBuilder.Configuration.GetValue<string>("Orleans:AzureTable:ConnectionString");
        TableServiceClient tableServiceClient = new(connectionString);

        siloBuilder
            .UseLocalhostClustering()
            .AddAzureTableGrainStorageAsDefault(options =>
            {
                options.TableServiceClient = tableServiceClient;
            })
            .AddAzureTableTransactionalStateStorageAsDefault(options =>
            {
                options.TableServiceClient = tableServiceClient;
            })
            .UseTransactions();
    })
    .RunConsoleAsync();
