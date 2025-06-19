using AccountTransfer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Journaling;

namespace AccountTransfer.Grains;

#pragma warning disable ORLEANSEXP005  
public sealed class AccountGrain([FromKeyedServices("balance")] IDurableValue<int> balance) : DurableGrain, IAccountGrain
{
    public async Task Deposit(int amount)
    {
        balance.Value += amount;
        await WriteStateAsync();
    }

    public async Task Withdraw(int amount)
    {
        balance.Value -= amount;
        await WriteStateAsync();
    }

    public Task<int> GetBalance()
    {
        return Task.FromResult(balance.Value);
    }
}