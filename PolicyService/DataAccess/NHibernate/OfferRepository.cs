using NHibernate;
using PolicyService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Offer WithNumber(string number)
        {
            return session.Query<Offer>()
                .FirstOrDefault(o => o.Number == number);
        }
    }
}
