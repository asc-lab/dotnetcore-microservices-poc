namespace ProductService.Domain;

public class Cover
{
    public Cover()
    {
    }

    public Cover(string code, string name, string description, bool optional, decimal? sumInsured)
    {
        Id = Guid.NewGuid();
        Code = code;
        Name = name;
        Description = description;
        Optional = optional;
        SumInsured = sumInsured;
    }

    public Guid Id { get; }
    public string Code { get; }
    public string Name { get; }
    public string Description { get; }
    public bool Optional { get; }
    public decimal? SumInsured { get; }

    //public Product Product { get; private set; }
}