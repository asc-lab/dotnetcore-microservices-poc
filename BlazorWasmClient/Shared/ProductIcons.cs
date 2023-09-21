namespace BlazorWasmClient.Shared;

public static class ProductIcons
{
    private static readonly IDictionary<string, IconName> ProductIconsMap = new Dictionary<string, IconName>
    {
        ["plane"] = IconName.Plane,
        ["building"] = IconName.Building,
        ["apple"] = IconName.Seedling,
        ["car"] = IconName.Car
    };
    
    public static IconName FromProductIcon(string productIcon)
    {
        if (string.IsNullOrWhiteSpace(productIcon) || !ProductIconsMap.ContainsKey(productIcon))
            return IconName.Bus;

        return ProductIconsMap[productIcon];
    }
        
    
}