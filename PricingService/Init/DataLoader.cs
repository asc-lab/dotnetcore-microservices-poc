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
            allTariffs.Add(DemoTariffFactory.Travel());
        }
    }
}
