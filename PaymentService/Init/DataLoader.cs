using PaymentService.Domain;

namespace PaymentService.Init
{
    public class DataLoader
    {
        private readonly IUnitOfWorkProvider uowProvider;

        public DataLoader(IUnitOfWorkProvider uowProvider)
        {
            this.uowProvider = uowProvider;
        }

        public void Seed()
        {
            using (var uow = uowProvider.Create())
            {
                DemoAccountsFactory.DemoAccounts().ForEach(x => AddIfNotExists(uow.PolicyAccountRespository, x));
                uow.CommitChanges();
            }
        }

        private void AddIfNotExists(IPolicyAccountRepository policyAccountRespository, PolicyAccount account)
        {
            if (policyAccountRespository.FindByNumber(account.PolicyAccountNumber) == null)
            {
                policyAccountRespository.Add(account);
            }
        }
    }
}
