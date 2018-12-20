using PaymentService.Domain;

namespace PaymentService.Init
{
    public class DataLoader
    {
        private readonly IDataStore dataStore;

        public DataLoader(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public void Seed()
        {
            using (dataStore)
            {
                DemoAccountsFactory.DemoAccounts().ForEach(x => AddIfNotExists(x));
                dataStore.CommitChanges();
            }
        }

        private void AddIfNotExists(PolicyAccount account)
        {
            if (dataStore.PolicyAccounts.FindByNumber(account.PolicyNumber) == null)
            {
                dataStore.PolicyAccounts.Add(account);
            }
        }
    }
}
