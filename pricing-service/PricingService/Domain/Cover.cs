namespace PricingService.Domain
{
    public class Cover
    {
        public string Code { get; private set; }
        public decimal Price { get; private set; }

        public Cover(string code, decimal price)
        {
            Code = code;
            Price = price;
        }

        public void SetPrice(decimal price)
        {
            Price = price;
        }
    }
}