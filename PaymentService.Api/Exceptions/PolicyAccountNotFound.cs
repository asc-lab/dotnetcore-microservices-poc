using System;

namespace PaymentService.Api.Exceptions
{
    public class PolicyAccountNotFound : Exception
    {
        public PolicyAccountNotFound(string accountNumber) :
            base($"Policy Account not found. Looking for account with number: {accountNumber}")
        {
        }
    }
}
