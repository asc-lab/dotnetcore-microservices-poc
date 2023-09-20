namespace BlazorWasmClient.Components.Chat;

public record ChatMessage(string User, string Avatar, string Text)
{
    public string FormattedMessage => $"{User} says: {Text}";
}