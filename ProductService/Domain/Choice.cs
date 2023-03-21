namespace ProductService.Domain;

public class Choice
{
    public Choice()
    {
    }

    public Choice(string code, string label)
    {
        Code = code;
        Label = label;
    }

    public string Code { get; }
    public string Label { get; }

    public ChoiceQuestion Question { get; private set; }
}