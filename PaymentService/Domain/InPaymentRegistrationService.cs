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
            BankStatementFile fileToImport = new BankStatementFile(directory, date, null);

            if (!fileToImport.Exists())
            {
                return;
            }

            List<BankStatement> bankStatements = fileToImport.Read();

            using (uow)
            {
                bankStatements.ForEach(x => RegisterInPayment(uow.PolicyAccounts, x));
                fileToImport.MarkProcessed();
                uow.CommitChanges();
            }
        }

        public void RegisterInPayment(IPolicyAccountRepository policyAccountRepository, BankStatement bankStatement)
        {
            var policyAccount = policyAccountRepository.FindByNumber(bankStatement.AccountNumber);

            policyAccount?.InPayment(bankStatement.Amount, bankStatement.AccountingDate);
        }
    }
}
