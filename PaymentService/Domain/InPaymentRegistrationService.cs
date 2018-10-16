using System;
using System.Collections.Generic;

namespace PaymentService.Domain
{
    public class InPaymentRegistrationService
    {
        private readonly IPolicyAccountRepository policyAccountRepository;

        //[Transactional]
        public void RegisterInPayments(String directory, DateTimeOffset date)
        {
            BankStatementFile fileToImport = new BankStatementFile(directory, date);

            if (!fileToImport.Exists())
            {
                return;
            }

            List<BankStatement> bankStatements = fileToImport.Read();
            bankStatements.ForEach(x => RegisterInPayment(x));
            fileToImport.MarkProcessed();
            //unitOfWork.CommitChanges();
        }

        public void RegisterInPayment(BankStatement bankStatement)
        {
            var policyAccount = policyAccountRepository
                    .FindByNumber(bankStatement.AccountNumber);

            if (policyAccount != null)
            {
                policyAccount.InPayment(bankStatement.Amount, bankStatement.AccountingDate);
            }
        }
    }
}
