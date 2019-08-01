using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PricingService.Domain;

namespace PricingService.Init
{
    public class DataLoader
    {
        private readonly IDataStore dataStore;
        private readonly IDictionary<string, Func<Tariff>> builders = new Dictionary<string, Func<Tariff>>
        {
            {"TRI", DemoTariffFactory.Travel },
            {"HSI", DemoTariffFactory.House },
            {"FAI", DemoTariffFactory.Farm },
            {"CAR", DemoTariffFactory.Car }
        };

        public DataLoader(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task Seed()
        {
            await AddTariffIfNotExists("TRI");

            await AddTariffIfNotExists("HSI");

            await AddTariffIfNotExists("FAI");

            await AddTariffIfNotExists("CAR");

            await dataStore.CommitChanges();
        }

        private async Task AddTariffIfNotExists(string code)
        {
            var alreadyExists = await dataStore.Tariffs.Exists(code);

            if (!alreadyExists)
            {
                dataStore.Tariffs.Add(builders[code]());
            }
        }
    }
}
