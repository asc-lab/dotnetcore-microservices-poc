using System;

namespace PaymentService.Api.Exceptions;

public class BankStatementsFileReadingError : BussinesExceptions
{
    public BankStatementsFileReadingError(Exception ex) :
        base("Policy Account not found. BankStatementsFileReadingError", ex)
    {
    }
}