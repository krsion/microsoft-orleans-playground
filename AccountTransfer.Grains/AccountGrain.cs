using AccountTransfer.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Journaling;

namespace AccountTransfer.Grains;

#pragma warning disable ORLEANSEXP005  
public sealed class AccountGrain(
    [FromKeyedServices("balance")] IDurableValue<int> balance,
    IStateMachineManager stateMachineManager) : Grain, IAccountGrain
{
    public async Task Deposit(int amount)
    {
        balance.Value += amount;
        await stateMachineManager.WriteStateAsync(default);
    }

    public async Task Withdraw(int amount)
    {
        balance.Value -= amount;
        await stateMachineManager.WriteStateAsync(default);
    }

    public Task<int> GetBalance() => Task.FromResult(balance.Value);
}