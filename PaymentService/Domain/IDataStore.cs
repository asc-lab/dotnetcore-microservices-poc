using System;

namespace PaymentService.Domain
{
    public interface IDataStore : IDisposable
    {
        IPolicyAccountRepository PolicyAccounts { get; }

        void CommitChanges();
    }
}
