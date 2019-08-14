using DynamicExpresso;
using static System.String;

namespace PricingService.Domain
{
    public class BasePremiumCalculationRule
    {
        public string CoverCode { get; private set; }
        public string ApplyIfFormula { get; private set; }
        public string BasePriceFormula { get; private set; }

        public BasePremiumCalculationRule(string coverCode, string applyIfFormula, string basePriceFormula)
        {
            CoverCode = coverCode;
            ApplyIfFormula = applyIfFormula;
            BasePriceFormula = basePriceFormula;
        }

        public bool Applies(Cover cover, Calculation calculation)
        {
            if (cover.Code != CoverCode)
                return false;

            if (IsNullOrEmpty(ApplyIfFormula))
                return true;

            var (paramDefinitions,values) = calculation.ToCalculationParams();
            return (bool)new Interpreter()
                .Parse(ApplyIfFormula, paramDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

        public decimal CalculateBasePrice(Calculation calculation)
        {
            var (paramDefinitions, values) = calculation.ToCalculationParams();

            return (decimal)new Interpreter()
                .Parse(BasePriceFormula, paramDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

    }
}
