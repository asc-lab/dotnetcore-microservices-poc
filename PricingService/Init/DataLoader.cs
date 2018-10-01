using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Init
{
    public class DataLoader
    {
        private IUnitOfWork unitOfWork;

        public DataLoader(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Seed()
        {
            if (!unitOfWork.Tariffs.Exists("TRI")) unitOfWork.Tariffs.Add(DemoTariffFactory.Travel());

            if (!unitOfWork.Tariffs.Exists("HSI")) unitOfWork.Tariffs.Add(DemoTariffFactory.House());

            if (!unitOfWork.Tariffs.Exists("FAI")) unitOfWork.Tariffs.Add(DemoTariffFactory.Farm());

            if (!unitOfWork.Tariffs.Exists("CAR")) unitOfWork.Tariffs.Add(DemoTariffFactory.Car());

            unitOfWork.CommitChanges();
        }
    }
}
