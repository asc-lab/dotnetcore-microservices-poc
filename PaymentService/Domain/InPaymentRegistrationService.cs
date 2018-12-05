using System;
using System.Collections.Generic;

namespace PaymentService.Domain
{
    public class InPaymentRegistrationService
    {
        private readonly IUnitOfWork uow;

        public InPaymentRegistrationService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        
        public void RegisterInPayments(string directory, DateTimeOffset date)
        {
            var fileToImport = new BankStatementFile(directory, date);

            if (!fileToImport.Exists())
            {
                return;
            }

            using (uow)
            {
                fileToImport
                    .Read()
                    .ForEach(bs => uow.PolicyAccounts.FindByNumber(bs.AccountNumber)?.InPayment(bs.Amount, bs.AccountingDate));
                
                fileToImport.MarkProcessed();
                
                uow.CommitChanges();
            }
        }

        
    }
}
