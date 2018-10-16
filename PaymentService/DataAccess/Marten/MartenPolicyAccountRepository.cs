using Marten;
using PaymentService.Domain;
using System.Collections.Generic;
using System.Linq;

namespace PaymentService.DataAccess.Marten
{
    public class MartenPolicyAccountRepository : IPolicyAccountRepository
    {
        private readonly IDocumentSession session;

        public MartenPolicyAccountRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Add(PolicyAccount policyAccount)
        {
            session.Insert(policyAccount);
        }

        public ICollection<PolicyAccount> FindAll()
        {
            return session.Query<PolicyAccount>().ToList();
        }

        public PolicyAccount FindByNumber(string accountNumber)
        {
            return session.Query<PolicyAccount>().FirstOrDefault(x => x.PolicyAccountNumber == accountNumber);
        }

        public PolicyAccount FindForPolicy(string policyNumber)
        {
            return session.Query<PolicyAccount>().FirstOrDefault(x => x.PolicyNumber == policyNumber);
        }
    }
}
