namespace AccountTransfer.Interfaces;

public interface IAccountGrain : IGrainWithStringKey
{
    Task Withdraw(int amount);

    Task Deposit(int amount);

    Task<int> GetBalance();
}
