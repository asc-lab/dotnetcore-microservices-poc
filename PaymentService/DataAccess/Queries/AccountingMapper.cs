using PaymentService.Domain;
using System;
using System.Collections.Generic;

namespace PaymentService.Queries
{
    public static class AccountingMapper
    {
        public static AccountingEntry ConvertToAccounting(object value)
        {
            var dapperRowProperties = value as IDictionary<string, object>;
            switch (dapperRowProperties["Discriminator"])
            {
                case "InPayment":
                    return GetObject<InPayment>(dapperRowProperties);
                case "ExpectedPayment":
                    return GetObject<ExpectedPayment>(dapperRowProperties);
                case "OutPayment":
                    return GetObject<OutPayment>(dapperRowProperties);
                default:
                    return null;
            }
        }

        private static T GetObject<T>(IDictionary<string, object> dict)
        {
            Type type = typeof(T);
            var obj = Activator.CreateInstance(type, true);

            foreach (var kv in dict)
            {
                var prop = type.GetProperty(kv.Key);
                if (prop != null)
                    prop.SetValue(obj, kv.Value);
            }
            return (T)obj;
        }
    }
}
