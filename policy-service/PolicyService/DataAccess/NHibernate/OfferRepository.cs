using NHibernate;
using PolicyService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;

namespace PolicyService.DataAccess.NHibernate
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ISession session;

        public OfferRepository(ISession session)
        {
            this.session = session;
        }

        public void Add(Offer offer)
        {
            session.Save(offer);
        }

        public async Task<Offer> WithNumber(string number)
        {
            return await session.Query<Offer>()
                .FirstOrDefaultAsync(o => o.Number == number);
        }
    }
}
