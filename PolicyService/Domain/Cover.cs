using System;

namespace PolicyService.Domain;

public class Cover : ICloneable
{
    public Cover(string code, decimal price)
    {
        Code = code;
        Price = price;
    }

    protected Cover()
    {
    } //NH required

    public virtual string Code { get; protected set; }
    public virtual decimal Price { get; protected set; }


    object ICloneable.Clone()
    {
        return Clone();
    }

    public Cover Clone()
    {
        return new Cover(Code, Price);
    }
}