using System;

namespace PolicyService.Domain;

public class PolicyCover
{
    public PolicyCover(Cover cover, ValidityPeriod coverPeriod)
    {
        Code = cover.Code;
        Premium = cover.Price;
        CoverPeriod = coverPeriod;
    }

    protected PolicyCover()
    {
    } //NH required

    public virtual string Code { get; protected set; }
    public virtual decimal Premium { get; protected set; }
    public virtual ValidityPeriod CoverPeriod { get; protected set; }

    public PolicyCover EndOn(DateTime endDate)
    {
        var originalDaysCovered = CoverPeriod.Days;
        var daysNotUsed = originalDaysCovered - CoverPeriod.EndOn(endDate).Days;
        var premium = decimal.Round
        (
            Premium - Premium * decimal.Divide(daysNotUsed, originalDaysCovered)
            , 2
        );

        return new PolicyCover
        {
            Code = Code,
            Premium = premium,
            CoverPeriod = CoverPeriod.EndOn(endDate)
        };
    }
}