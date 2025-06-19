namespace AccountTransfer.Interfaces;

public interface IAtmGrain : IGrainWithIntegerKey
{
    Task Transfer(
        IAccountGrain fromAccount,
        IAccountGrain toAccount,
        int amountToTransfer);
}
