using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PricingService.DataAccess.Marten
{
    public class ProtectedSettersContractResolver : DefaultContractResolver
    {
        public ProtectedSettersContractResolver()
        {
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    var hasSetter = property.GetSetMethod(true) != null;
                    prop.Writable = hasSetter;
                }
            }

            return prop;
        }
    }
}
