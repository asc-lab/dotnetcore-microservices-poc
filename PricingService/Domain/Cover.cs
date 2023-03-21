namespace PricingService.Domain;

public class Cover
{
    public Cover(string code, decimal price)
    {
        Code = code;
        Price = price;
    }

    public string Code { get; }
    public decimal Price { get; private set; }

    public void SetPrice(decimal price)
    {
        Price = price;
    }
}