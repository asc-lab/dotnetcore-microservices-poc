using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Init
{
    public class DataLoader
    {
        private ITariffRepository allTariffs;

        public DataLoader(ITariffRepository allTariffs)
        {
            this.allTariffs = allTariffs;
        }

        public void Seed()
        {
            if (!allTariffs.Exists("TRI")) allTariffs.Add(DemoTariffFactory.Travel());

            if (!allTariffs.Exists("HSI")) allTariffs.Add(DemoTariffFactory.House());

            if (!allTariffs.Exists("FAI")) allTariffs.Add(DemoTariffFactory.Farm());

            if (!allTariffs.Exists("CAR")) allTariffs.Add(DemoTariffFactory.Car());
        }
    }
}
