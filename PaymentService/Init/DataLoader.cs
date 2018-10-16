using PaymentService.Domain;

namespace PaymentService.Init
{
    public class DataLoader
    {
        private IUnitOfWork unitOfWork;

        public DataLoader(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Seed()
        {
            DemoAccountsFactory.DemoAccounts().ForEach(x => AddIfNotExists(x));
            unitOfWork.CommitChanges();
        }

        private void AddIfNotExists(PolicyAccount account)
        {
            if (unitOfWork.PolicyAccountRespository.FindByNumber(account.PolicyAccountNumber) == null)
            {
                unitOfWork.PolicyAccountRespository.Add(account);
            }
        }
    }
}
