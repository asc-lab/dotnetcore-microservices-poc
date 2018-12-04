using System.Linq;
using Marten;
using PaymentService.Domain;

namespace PaymentService.DataAccess.Marten
{
    public class MartenPolicyAccountRepository : IPolicyAccountRepository
    {
        private readonly IDocumentSession documentSession;

        public MartenPolicyAccountRepository(IDocumentSession documentSession)
        {
            this.documentSession = documentSession;
        }

        public void Add(PolicyAccount policyAccount)
        {
            this.documentSession.Insert(policyAccount);
        }

        public PolicyAccount FindByNumber(string accountNumber)
        {
            return this.documentSession.Query<PolicyAccount>().FirstOrDefault(p => p.PolicyNumber == accountNumber);
        }
    }
}