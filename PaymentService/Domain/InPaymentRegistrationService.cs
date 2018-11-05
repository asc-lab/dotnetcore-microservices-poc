using System;
using System.Collections.Generic;

namespace PaymentService.Domain
{
    public class InPaymentRegistrationService
    {
        private readonly IUnitOfWorkProvider uowProvider;

        public InPaymentRegistrationService(IUnitOfWorkProvider uowProvider)
        {
            this.uowProvider = uowProvider;
        }
        
        public void RegisterInPayments(string directory, DateTimeOffset date)
        {
            BankStatementFile fileToImport = new BankStatementFile(directory, date, null);

            if (!fileToImport.Exists())
            {
                return;
            }

            List<BankStatement> bankStatements = fileToImport.Read();

            using (var uow = uowProvider.Create())
            {
                bankStatements.ForEach(x => RegisterInPayment(uow.PolicyAccountRespository, x));
                fileToImport.MarkProcessed();
                uow.CommitChanges();
            }
        }

        public void RegisterInPayment(IPolicyAccountRepository policyAccountRepository, BankStatement bankStatement)
        {
            var policyAccount = policyAccountRepository.FindByNumber(bankStatement.AccountNumber);

            if (policyAccount != null)
            {
                policyAccount.InPayment(bankStatement.Amount, bankStatement.AccountingDate);
            }
        }
    }
}
