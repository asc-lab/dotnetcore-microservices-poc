using System;

namespace PaymentService.Api.Exceptions
{
    public class BankStatementsFileNotFound : Exception
    {
        public BankStatementsFileNotFound(Exception ex) :
            base("Bank statements file not found.", ex)
        {
        }
    }
}
