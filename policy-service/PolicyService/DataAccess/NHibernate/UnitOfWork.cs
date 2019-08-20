using System;
using System.Threading.Tasks;
using NHibernate;
using PolicyService.Domain;

namespace PolicyService.DataAccess.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly ITransaction tx;
        private readonly OfferRepository offerRepository;
        private readonly PolicyRepository policyRepository;

        public IOfferRepository Offers => offerRepository;

        public IPolicyRepository Policies => policyRepository;


        public UnitOfWork(ISession session)
        {
            this.session = session;
            tx = session.BeginTransaction();
            offerRepository = new OfferRepository(session);
            policyRepository = new PolicyRepository(session);
        }

        public async Task CommitChanges()
        {
            await tx.CommitAsync();
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
                tx?.Dispose();
            }

        }
    }

    
}
