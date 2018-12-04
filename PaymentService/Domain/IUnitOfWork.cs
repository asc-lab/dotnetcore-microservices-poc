using System;

namespace PaymentService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IPolicyAccountRepository PolicyAccounts { get; }

        void CommitChanges();
    }
}
