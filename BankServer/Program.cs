using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Journaling;

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
#pragma warning disable ORLEANSEXP005
        _ = siloBuilder
            .UseLocalhostClustering()
            .AddAzureAppendBlobStateMachineStorage(options =>
            {
                options.ConfigureBlobServiceClient(connectionString);
                options.ContainerName = "orleans-grain-int-balance-journal";
            });
    })
    .RunConsoleAsync();
