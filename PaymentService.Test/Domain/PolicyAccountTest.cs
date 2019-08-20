using System;
using PaymentService.Domain;
using Xunit;
using static Xunit.Assert;

namespace PaymentService.Tests.Domain
{
    public class PolicyAccountTest
    {
        [Fact]
        public void NewlyCreatedAccountIsActive()
        {
            var account = new PolicyAccount("A", "A", "C", "C");
            Equal(PolicyAccountStatus.Active, account.Status);
        }
        
        [Fact]
        public void CanRegisterInPayment()
        {
            var incomeDate = new DateTimeOffset(new DateTime(2018, 1, 1));
            var account = new PolicyAccount("A", "A", "C", "C");
            account.InPayment(10M, incomeDate);

            Equal(1, account.Entries.Count);
            Equal(10M, account.BalanceAt(incomeDate));
        }

        [Fact]
        public void CanRegisterOutpayment()
        {
            var paymentReleaseDate = new DateTimeOffset(new DateTime(2018, 1, 1));
            var account = new PolicyAccount("B", "B", "D", "D");
            account.OutPayment(10M, paymentReleaseDate);

            Equal(1, account.Entries.Count);
            Equal(-10M, account.BalanceAt(paymentReleaseDate));
        }

        [Fact]
        public void CanRegisterExpectdPayment()
        {
            var dueDate = new DateTimeOffset(new DateTime(2018, 1, 1));
            var account = new PolicyAccount("C", "C", "E", "E");
            account.ExpectedPayment(10M, dueDate);

            Equal(1, account.Entries.Count);
            Equal(-10M, account.BalanceAt(dueDate));
        }

        [Fact]
        public void CanProperlyCalculateBalance()
        {
            var dueDate1 = new DateTimeOffset(new DateTime(2018, 1, 1));
            var dueDate2 = new DateTimeOffset(new DateTime(2018, 10, 1));
            var incomeDate = new DateTimeOffset(new DateTime(2018, 5, 1));

            var account = new PolicyAccount("D", "D", "F", "F");
            account.ExpectedPayment(10M, dueDate1);
            account.ExpectedPayment(15M, dueDate2);
            account.InPayment(25M, incomeDate);

            Equal(-10M, account.BalanceAt(dueDate1));
            Equal(15M, account.BalanceAt(incomeDate));
            Equal(0M, account.BalanceAt(dueDate2));
        }

        [Fact]
        public void CanCloseAccountAndReturnMoney()
        {
            var dueDate = new DateTimeOffset(new DateTime(2018, 1, 1));
            var account = new PolicyAccount("C", "C", "E", "E");
            account.ExpectedPayment(10M, dueDate);
            account.InPayment(10M, dueDate);
            
            account.Close(new DateTime(2018,6,1), 5M);
            
            Equal(PolicyAccountStatus.Terminated, account.Status);
        }
    }
}
