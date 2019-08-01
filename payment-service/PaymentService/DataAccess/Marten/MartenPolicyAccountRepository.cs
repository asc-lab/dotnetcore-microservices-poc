using System.Linq;
using System.Threading;
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
            this.documentSession.Insert(policyAccount);
        }

        public void Update(PolicyAccount policyAccount)
        {
            this.documentSession.Update(policyAccount);
        }

        public async Task<PolicyAccount> FindByNumber(string accountNumber)
        {
            return await this.documentSession
                .Query<PolicyAccount>()
                .FirstOrDefaultAsync(p => p.PolicyNumber == accountNumber);
        }
    }
}