using System.Collections.Generic;

namespace PaymentService.Domain
{
    public interface IPolicyAccountRepository
    {
        PolicyAccount FindForPolicy(string policyNumber);

        PolicyAccount FindByNumber(string accountNumber);

        void Add(PolicyAccount policyAccount);

        ICollection<PolicyAccount> FindAll();
    }
}