using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public interface IDataStore : IDisposable
    {
        ITariffRepository Tariffs { get; }

        Task CommitChanges();
    }
}
