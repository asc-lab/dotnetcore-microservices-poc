using System.Threading.Tasks;

namespace PricingService.Domain
{
    public interface ITariffRepository
    {
        Task<Tariff> WithCode(string code);
        
        Task<Tariff> this[string code] { get; }

        void Add(Tariff tariff);
        
        Task<bool> Exists(string code);
    }
}
