using Marten;
using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.DataAccess.Marten
{
    public class MartenDataStore : IDataStore
    {
        private readonly IDocumentSession session;
        private readonly ITariffRepository tariffs;

        public MartenDataStore(IDocumentStore documentStore)
        {
            session = documentStore.LightweightSession();
            tariffs = new MartenTariffRepository(session);
        }

        public ITariffRepository Tariffs => tariffs;

        public async Task CommitChanges()
        {
            await session.SaveChangesAsync();
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
