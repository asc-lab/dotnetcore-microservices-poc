using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;
using System;
using System.Linq;

namespace PaymentService.DataAccess.EF
{
    public class EfPolicyAccountRepository : IPolicyAccountRepository
    {
        private PaymentContext paymentContext;

        public EfPolicyAccountRepository(PaymentContext context)
        {
            paymentContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(PolicyAccount policyAccount)
        {
            paymentContext.Add(policyAccount);
        }

        public PolicyAccount FindByNumber(string accountNumber)
        {
            return paymentContext.PolicyAccounts
                .Include(x => x.Entries)
                .FirstOrDefault(x => x.PolicyAccountNumber == accountNumber);
        }
    }
}
