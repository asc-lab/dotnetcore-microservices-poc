using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IOfferRepository Offers { get; }

        IPolicyRepository Policies { get; }

        Task CommitChanges();
    }

    public interface IUnitOfWorkProvider
    {
        IUnitOfWork Create();
    }
}
