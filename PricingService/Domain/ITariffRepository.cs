using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public interface ITariffRepository
    {
        Tariff WithCode(string code);
        
        Tariff this[string code] { get; }

        void Add(Tariff tariff);
        
        bool Exists(string code);
    }
}
