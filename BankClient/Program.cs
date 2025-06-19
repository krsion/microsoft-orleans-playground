using System.Security.Principal;
using AccountTransfer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseLocalhostClustering();
    })
    .UseConsoleLifetime()
    .Build();

await host.StartAsync();

var client = host.Services.GetRequiredService<IClusterClient>();

var accountNames = new[] { "Alice", "Bob" };
var random = Random.Shared;

IAccountGrain alice = client.GetGrain<IAccountGrain>("Alice");
IAccountGrain bob = client.GetGrain<IAccountGrain>("Bob");
IAtmGrain atm = client.GetGrain<IAtmGrain>(0);

for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Before: Alice: {await alice.GetBalance()}, Bob: {await bob.GetBalance()}");
    await atm.Transfer(alice, bob, 100);
    Console.WriteLine($"After:  Alice: {await alice.GetBalance()}, Bob: {await bob.GetBalance()}\n");
    await Task.Delay(TimeSpan.FromMilliseconds(200));
}

Console.ReadLine();
