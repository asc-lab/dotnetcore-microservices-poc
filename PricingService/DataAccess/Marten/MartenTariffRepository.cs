using System.Threading.Tasks;
using Marten;
using PricingService.Domain;

namespace PricingService.DataAccess.Marten
{
    public class MartenTariffRepository : ITariffRepository
    {
        private readonly IDocumentSession session;

        public MartenTariffRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public void Add(Tariff tariff)
        {
            session.Insert(tariff);
        }

        public async Task<bool> Exists(string code)
        {
            return await session.Query<Tariff>().AnyAsync(t => t.Code == code);
        }

        public async Task<Tariff> WithCode(string code)
        {
            return await session.Query<Tariff>().FirstOrDefaultAsync(t => t.Code == code);
        }

        public Task<Tariff> this[string code] => WithCode(code);
    }
}
