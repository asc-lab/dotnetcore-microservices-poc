using PricingService.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.DataAccess
{
    public class InMemoryTariffRepository : ITariffRepository
    {
        private IDictionary<string, Tariff> db = new ConcurrentDictionary<string, Tariff>();

        public void Add(Tariff tariff)
        {
            db.Add(tariff.Code, tariff);
        }

        public Tariff WithCode(string code)
        {
            return db[code];
        }
    }
}
