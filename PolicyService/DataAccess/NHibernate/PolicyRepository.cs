using NHibernate;
using PolicyService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.DataAccess.NHibernate
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ISession session;

        public PolicyRepository(ISession session)
        {
            this.session = session;
        }

        public void Add(Policy policy)
        {
            session.Save(policy);
        }

        public Policy WithNumber(string number)
        {
            return session.Query<Policy>().FirstOrDefault(p => p.Number == number);
        }
    }
}
