using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public class Price
    {
        private Dictionary<string, decimal> coverPrices;

        public IReadOnlyDictionary<string, decimal> CoverPrices => new ReadOnlyDictionary<string, decimal>(coverPrices);

        public Price(Dictionary<string, decimal> coverPrices)
        {
            this.coverPrices = coverPrices;
        }
    }
}
