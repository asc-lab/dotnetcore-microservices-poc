namespace PolicyService.Domain;

public class Address
{
    protected Address()
    {
    }

    public Address(string country, string zipCode, string city, string street)
    {
        Country = country;
        ZipCode = zipCode;
        City = city;
        Street = street;
    }

    public virtual string Country { get; protected set; }
    public virtual string ZipCode { get; protected set; }
    public virtual string City { get; protected set; }
    public virtual string Street { get; protected set; }

    public static Address Of(string country, string zipCode, string city, string street)
    {
        return new Address(country, zipCode, city, street);
    }
}