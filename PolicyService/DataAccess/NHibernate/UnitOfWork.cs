using NHibernate;
using PolicyService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            session = sessionFactory.OpenSession();
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
            if (disposing && session!=null)
            {
                tx.Dispose();
                session.Dispose();
            }

        }
    }

    public class UnitOfWorkProvider : IUnitOfWorkProvider
    {
        private readonly ISessionFactory sessionFactory;

        public UnitOfWorkProvider(ISessionFactory sessionFactory)
        {
            this.sessionFactory = sessionFactory;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(sessionFactory);
        }
    }
}
