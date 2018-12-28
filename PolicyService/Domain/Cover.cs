using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public class Cover : ICloneable
    {
            public virtual string Code { get; protected set; }
            public virtual decimal Price { get; protected set; }
    
            public Cover(string code, decimal price)
            {
                Code = code;
                Price = price;
            }

        protected Cover() { } //NH required

        public Cover Clone()
        {
            return new Cover(Code, Price);
        }


        object ICloneable.Clone()
        {
            return Clone();
        }

    }
}
