using System;
using System.Threading.Tasks;
using Marten;
using PricingService.Domain;

namespace PricingService.DataAccess.Marten;

public class MartenDataStore : IDataStore
{
    private readonly IDocumentSession session;

    public MartenDataStore(IDocumentStore documentStore)
    {
        session = documentStore.LightweightSession();
        Tariffs = new MartenTariffRepository(session);
    }

    public ITariffRepository Tariffs { get; }

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
        if (disposing) session.Dispose();
    }
}