using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PaymentService.Domain
{
    public class PolicyAccount
    {
        [Key]
        public Guid Id { get; set; }
        public string PolicyAccountNumber { get; set; }
        public string PolicyNumber { get; set; }
        public ICollection<AccountingEntry> Entries { get; set; }

        public PolicyAccount()
        {
            Entries = new List<AccountingEntry>();
        }

        public PolicyAccount(string policyNumber, string policyAccountNumber)
        {
            Id = Guid.NewGuid();
            PolicyNumber = policyNumber;
            PolicyAccountNumber = policyAccountNumber;
            Entries = new List<AccountingEntry>();
        }

        public void ExpectedPayment(decimal amount, DateTimeOffset dueDate)
        {
            Entries.Add(new ExpectedPayment(this, DateTimeOffset.Now, dueDate, amount));
        }

        public void InPayment(decimal amount, DateTimeOffset incomeDate)
        {
            Entries.Add(new InPayment(this, DateTimeOffset.Now, incomeDate, amount));
        }

        public void OutPayment(decimal amount, DateTimeOffset paymentReleaseDate)
        {
            Entries.Add(new OutPayment(this, DateTimeOffset.Now, paymentReleaseDate, amount));
        }

        public decimal BalanceAt(DateTimeOffset effectiveDate)
        {
            List<AccountingEntry> effectiveEntries = Entries
                .Where(x => x.IsEffectiveOn(effectiveDate))
                .OrderBy(x => x.CreationDate)
                .ToList();

            decimal balance = 0M;
            effectiveEntries.ForEach(x => balance = x.Apply(balance));

            return balance;
        }
    }
}
