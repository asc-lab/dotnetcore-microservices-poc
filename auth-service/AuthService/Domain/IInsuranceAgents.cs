namespace AuthService.Domain
{
    public interface IInsuranceAgents
    {
        void Add(InsuranceAgent agent);

        InsuranceAgent FindByLogin(string login);
    }
}