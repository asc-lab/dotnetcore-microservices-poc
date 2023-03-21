using System;
using System.Threading.Tasks;

namespace PaymentService.Domain;

public interface IDataStore : IDisposable
{
    IPolicyAccountRepository PolicyAccounts { get; }

    Task CommitChanges();
}