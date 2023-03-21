using System;

namespace PaymentService.Domain;

public class ExpectedPayment : AccountingEntry
{
    protected ExpectedPayment()
    {
    }

    public ExpectedPayment(PolicyAccount policyAccount, DateTimeOffset creationDate, DateTimeOffset effectiveDate,
        decimal amount) :
        base(policyAccount, creationDate, effectiveDate, amount)
    {
    }

    public override decimal Apply(decimal state)
    {
        return state - Amount;
    }
}