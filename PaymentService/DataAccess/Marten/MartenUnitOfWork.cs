using System;
using Marten;
using PaymentService.Domain;

namespace PaymentService.DataAccess.Marten
{
    public class MartenUnitOfWork : IUnitOfWork
    {
        private readonly IDocumentSession session;

        public MartenUnitOfWork(IDocumentStore documentStore)
        {
            session = documentStore.LightweightSession();
            PolicyAccounts = new MartenPolicyAccountRepository(session);
        }

        public IPolicyAccountRepository PolicyAccounts { get; }

        public void CommitChanges()
        {
            session.SaveChanges();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                session.Dispose();
            }
            
        }
    }
}