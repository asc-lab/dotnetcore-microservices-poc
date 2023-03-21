using System;
using System.Threading.Tasks;

namespace PricingService.Domain;

public interface IDataStore : IDisposable
{
    ITariffRepository Tariffs { get; }

    Task CommitChanges();
}