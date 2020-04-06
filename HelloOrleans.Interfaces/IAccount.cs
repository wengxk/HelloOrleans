namespace HelloOrleans.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DomainModels.Events;
    using Models;
    using Orleans;
    using Orleans.EventSourcing.CustomStorage;

    public interface IAccount : IGrainWithIntegerKey,
        ICustomStorageInterface<Account, AccountEvent>
    {
        Task Deposit(decimal amount);

        Task Withdrawal(decimal amount);

        Task<IEnumerable<AccountEvent>> RetrieveConfirmedEvents();

        Task<decimal> GetBalance();
    }
}