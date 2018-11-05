using System.Collections.Generic;

namespace PaymentService.Domain
{
    public interface IPolicyAccountRepository
    {
        void Add(PolicyAccount policyAccount);

        PolicyAccount FindByNumber(string accountNumber);
    }
}