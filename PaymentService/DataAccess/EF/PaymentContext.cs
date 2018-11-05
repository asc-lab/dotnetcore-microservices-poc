using Microsoft.EntityFrameworkCore;
using PaymentService.Domain;

namespace PaymentService.DataAccess.EF
{
    public class PaymentContext : DbContext
    {
        public virtual DbSet<PolicyAccount> PolicyAccounts { get; set; }
        public virtual DbSet<AccountingEntry> AccountingEntries { get; set; }

        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExpectedPayment>();
            modelBuilder.Entity<InPayment>();
            modelBuilder.Entity<OutPayment>();
            modelBuilder.Entity<PolicyAccount>();
        }
    }
}
