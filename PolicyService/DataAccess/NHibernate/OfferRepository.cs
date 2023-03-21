using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using PolicyService.Domain;

namespace PolicyService.DataAccess.NHibernate;

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