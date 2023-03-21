using System;
using System.Collections.Generic;
using System.Linq;

namespace PolicyService.Domain;

public static class PolicyVersions
{
    public static PolicyVersion EffectiveOn(this IEnumerable<PolicyVersion> versions, DateTime effectiveDate)
    {
        return versions
            .Where(v => v.IsEffectiveOn(effectiveDate))
            .OrderByDescending(v => v.VersionNumber)
            .FirstOrDefault();
    }

    public static PolicyVersion WithNumber(this IEnumerable<PolicyVersion> versions, int number)
    {
        return versions.First(v => v.VersionNumber == number);
    }

    public static PolicyVersion FirstVersion(this IEnumerable<PolicyVersion> versions)
    {
        return versions.First(v => v.VersionNumber == 1);
    }

    public static PolicyVersion LastVersion(this IEnumerable<PolicyVersion> versions)
    {
        return versions.OrderByDescending(v => v.VersionNumber).First();
    }
}