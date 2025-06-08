using Microsoft.Extensions.DependencyInjection; // Add this to fix CS1061
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Transactions.Abstractions; // Add this to fix CS0246

await Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole(); // Enable console logging
        logging.SetMinimumLevel(LogLevel.Debug); // Set minimum log level
    })
    .UseOrleans(siloBuilder =>
    {
        siloBuilder
            .UseLocalhostClustering()
            .AddMemoryGrainStorageAsDefault()
            .UseTransactions();
    })
    .RunConsoleAsync();
