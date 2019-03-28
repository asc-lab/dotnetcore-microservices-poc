using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Domain
{
    public interface IPolicyAccountRepository
    {
        void Add(PolicyAccount policyAccount);

        void Update(PolicyAccount policyAccount);

        Task<PolicyAccount> FindByNumber(string accountNumber);
    }
}