using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentService.Domain
{
    public class InPaymentRegistrationService
    {
        private readonly IDataStore dataStore;

        public InPaymentRegistrationService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        
        public async Task RegisterInPayments(string directory, DateTimeOffset date)
        {
            var fileToImport = new BankStatementFile(directory, date);

            if (!fileToImport.Exists())
            {
                return;
            }

            using (dataStore)
            {
                foreach (var txLine in fileToImport.Read())
                {
                    var account = await dataStore.PolicyAccounts.FindByNumber(txLine.AccountNumber);
                    account?.InPayment(txLine.Amount, txLine.AccountingDate);
                    
                    dataStore.PolicyAccounts.Update(account);
                }
                
                fileToImport.MarkProcessed();
                
                await dataStore.CommitChanges();
            }
        }

        
    }
}
