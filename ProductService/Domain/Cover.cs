using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Domain
{
    public class Cover
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Optional { get; private set; }
        public decimal? SumInsured { get; private set; }

        public Product Product { get; private set; }

        public Cover()
        { }

        public Cover(string code,string name, string description, bool optional, decimal? sumInsured)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Description = description;
            Optional = optional;
            SumInsured = sumInsured;
        }
    }
}
