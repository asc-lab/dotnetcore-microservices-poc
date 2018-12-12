using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Init
{
    public class DataLoader
    {
        private IDataStore _dataStore;

        public DataLoader(IDataStore dataStore)
        {
            this._dataStore = dataStore;
        }

        public void Seed()
        {
            if (!_dataStore.Tariffs.Exists("TRI")) _dataStore.Tariffs.Add(DemoTariffFactory.Travel());

            if (!_dataStore.Tariffs.Exists("HSI")) _dataStore.Tariffs.Add(DemoTariffFactory.House());

            if (!_dataStore.Tariffs.Exists("FAI")) _dataStore.Tariffs.Add(DemoTariffFactory.Farm());

            if (!_dataStore.Tariffs.Exists("CAR")) _dataStore.Tariffs.Add(DemoTariffFactory.Car());

            _dataStore.CommitChanges();
        }
    }
}
