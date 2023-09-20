namespace BlazorWasmClient.Shared;

public class ProductIcons
{
    public static IconName FromProductIcon(string productIcon) =>
        string.IsNullOrWhiteSpace(productIcon) ? IconName.Bus : Enum.Parse<IconName>(productIcon);
    
}