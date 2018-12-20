using System;
using System.Collections.Generic;

namespace PaymentService.Domain
{
    public class InPaymentRegistrationService
    {
        private readonly IDataStore dataStore;

        public InPaymentRegistrationService(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }
        
        public void RegisterInPayments(string directory, DateTimeOffset date)
        {
            var fileToImport = new BankStatementFile(directory, date);

            if (!fileToImport.Exists())
            {
                return;
            }

            using (dataStore)
            {
                fileToImport
                    .Read()
                    .ForEach(bs => dataStore.PolicyAccounts.FindByNumber(bs.AccountNumber)?.InPayment(bs.Amount, bs.AccountingDate));
                
                fileToImport.MarkProcessed();
                
                dataStore.CommitChanges();
            }
        }

        
    }
}
