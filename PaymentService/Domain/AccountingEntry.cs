using System;

namespace PaymentService.Domain;

public abstract class AccountingEntry
{
    protected AccountingEntry()
    {
    }

    public AccountingEntry(PolicyAccount policyAccount, DateTimeOffset creationDate, DateTimeOffset effectiveDate,
        decimal amount)
    {
        Id = Guid.NewGuid();
        PolicyAccount = policyAccount;
        CreationDate = creationDate;
        EffectiveDate = effectiveDate;
        Amount = amount;
    }

    public Guid Id { get; protected set; }
    public PolicyAccount PolicyAccount { get; protected set; }
    public DateTimeOffset CreationDate { get; protected set; }
    public DateTimeOffset EffectiveDate { get; protected set; }
    public decimal Amount { get; protected set; }

    public abstract decimal Apply(decimal state);

    public bool IsEffectiveOn(DateTimeOffset theDate)
    {
        return EffectiveDate <= theDate;
    }
}