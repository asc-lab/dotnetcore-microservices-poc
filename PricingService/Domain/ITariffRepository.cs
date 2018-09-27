using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public interface ITariffRepository
    {
        Tariff WithCode(string code);

        void Add(Tariff tariff);
    }
}
