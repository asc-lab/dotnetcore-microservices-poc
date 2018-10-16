using System;

namespace PaymentService.Domain
{
    public abstract class AccountingEntry
    {
        public long Id { get; private set; }
        public PolicyAccount PolicyAccount { get; private set; }
        public DateTimeOffset CreationDate { get; private set; }
        public DateTimeOffset EffectiveDate { get; private set; }
        public decimal Amount { get; private set; }

        public AccountingEntry(PolicyAccount policyAccount, DateTimeOffset creationDate, DateTimeOffset effectiveDate, decimal amount)
        {
            PolicyAccount = policyAccount;
            CreationDate = creationDate;
            EffectiveDate = effectiveDate;
            Amount = amount;
        }

        public abstract decimal Apply(decimal state);

        public bool IsEffectiveOn(DateTimeOffset theDate)
        {
            return EffectiveDate <= theDate;
        }
    }
}
