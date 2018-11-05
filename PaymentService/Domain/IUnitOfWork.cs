using System;

namespace PaymentService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IPolicyAccountRepository PolicyAccountRespository { get; }

        void CommitChanges();
    }

    public interface IUnitOfWorkProvider
    {
        IUnitOfWork Create();
    }
}
