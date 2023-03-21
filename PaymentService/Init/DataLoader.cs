using System.Threading.Tasks;
using PaymentService.Domain;

namespace PaymentService.Init;

public class DataLoader
{
    private readonly IDataStore dataStore;

    public DataLoader(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task Seed()
    {
        using (dataStore)
        {
            foreach (var demoAccount in DemoAccountsFactory.DemoAccounts()) await AddIfNotExists(demoAccount);

            await dataStore.CommitChanges();
        }
    }

    private async Task AddIfNotExists(PolicyAccount account)
    {
        if (await dataStore.PolicyAccounts.FindByNumber(account.PolicyNumber) == null)
            dataStore.PolicyAccounts.Add(account);
    }
}