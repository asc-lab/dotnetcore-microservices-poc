using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        ITariffRepository Tariffs { get; }

        void CommitChanges();
    }
}
