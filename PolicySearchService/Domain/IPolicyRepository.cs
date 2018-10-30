using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchService.Domain
{
    public interface IPolicyRepository
    {
        void Add(Policy policy);

        List<Policy> Find(string queryText);
    }
}
