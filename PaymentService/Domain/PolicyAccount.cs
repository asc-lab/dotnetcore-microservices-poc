using System;
using System.Collections.Generic;
using System.Linq;

namespace PaymentService.Domain
{
    public class PolicyAccount
    {
        public Guid Id { get; protected set; }
        public string PolicyAccountNumber { get; protected set; }
        public string PolicyNumber { get; protected set; }
        public Owner Owner { get; protected set; }
        
        public PolicyAccountStatus Status { get; protected set; }

        public ICollection<AccountingEntry> Entries { get; protected set; }

        public PolicyAccount()
        {
            Entries = new List<AccountingEntry>();
        }

        public PolicyAccount(string policyNumber, string policyAccountNumber, string ownerFirstName, string ownerLastLastName)
        {
            Id = Guid.NewGuid();
            PolicyNumber = policyNumber;
            PolicyAccountNumber = policyAccountNumber;
            Owner = new Owner(ownerFirstName,ownerLastLastName);
            Status = PolicyAccountStatus.Active;
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

        public void Close(DateTime closingDate, decimal? amountToReturn)
        {
            if (!IsActive())
                return;

            if (amountToReturn.HasValue)
            {
                OutPayment(amountToReturn.Value, closingDate);
            }

            Status = PolicyAccountStatus.Terminated;
        }

        public bool IsActive() => Status == PolicyAccountStatus.Active;
    }

    public class Owner
    {
        public string FirstName { get;  protected set; }
        public string LastName { get;  protected set; }

        protected Owner()
        {
        }

        public Owner(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
