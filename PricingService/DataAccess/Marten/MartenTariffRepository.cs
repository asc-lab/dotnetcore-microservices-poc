using Marten;
using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public bool Exists(string code)
        {
            return session.Query<Tariff>().Where(t => t.Code == code).Any();
        }

        public Tariff WithCode(string code)
        {
            return session.Query<Tariff>().FirstOrDefault(t => t.Code == code);
        }
    }
}
