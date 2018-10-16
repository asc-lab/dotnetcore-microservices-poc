using System;

namespace PaymentService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IPolicyAccountRepository PolicyAccountRespository { get; }

        void CommitChanges();
    }
}
