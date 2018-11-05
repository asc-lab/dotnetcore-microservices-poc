using Microsoft.EntityFrameworkCore.Storage;
using PaymentService.Domain;
using System;

namespace PaymentService.DataAccess.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly PaymentContext context;
        private readonly IDbContextTransaction tran;

        public EfUnitOfWork(PaymentContext paymentContext)
        {
            context = paymentContext;
            tran = paymentContext.Database.BeginTransaction();
            PolicyAccountRespository = new EfPolicyAccountRepository(paymentContext);
        }

        public IPolicyAccountRepository PolicyAccountRespository { get; }

        public void CommitChanges()
        {
            context.SaveChanges();
            tran.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && context != null)
            {
                tran.Dispose();
                context.Dispose();
            }

        }
    }

    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly PaymentContext paymentContext;

        public UnitOfWorkProvider(PaymentContext paymentContext)
        {
            this.paymentContext = paymentContext;
        }

        public IUnitOfWork Create()
        {
            return new EfUnitOfWork(paymentContext);
        }
    }
}
