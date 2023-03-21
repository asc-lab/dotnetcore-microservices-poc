using System.Threading.Tasks;

namespace PricingService.Domain;

public interface ITariffRepository
{
    Task<Tariff> this[string code] { get; }
    Task<Tariff> WithCode(string code);

    void Add(Tariff tariff);

    Task<bool> Exists(string code);
}