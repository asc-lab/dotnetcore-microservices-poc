using System.Threading.Tasks;
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
            documentSession.Insert(policyAccount);
        }

        public void Update(PolicyAccount policyAccount)
        {
            documentSession.Update(policyAccount);
        }

        public async Task<PolicyAccount> FindByNumber(string policyNumber)
        {
            return await documentSession
                .Query<PolicyAccount>()
                .FirstOrDefaultAsync(p => p.PolicyNumber == policyNumber);
        }

        public async Task<bool> ExistsWithPolicyNumber(string policyNumber)
        {
            return await documentSession
                .Query<PolicyAccount>()
                .AnyAsync(p => p.PolicyNumber == policyNumber);
        }
    }
}