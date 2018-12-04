using PaymentService.Domain;

namespace PaymentService.Init
{
    public class DataLoader
    {
        private readonly IUnitOfWork uow;

        public DataLoader(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public void Seed()
        {
            using (uow)
            {
                DemoAccountsFactory.DemoAccounts().ForEach(x => AddIfNotExists(x));
                uow.CommitChanges();
            }
        }

        private void AddIfNotExists(PolicyAccount account)
        {
            if (uow.PolicyAccounts.FindByNumber(account.PolicyNumber) == null)
            {
                uow.PolicyAccounts.Add(account);
            }
        }
    }
}
