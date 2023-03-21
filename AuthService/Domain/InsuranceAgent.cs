using System.Collections.Generic;

namespace AuthService.Domain;

public class InsuranceAgent
{
    public InsuranceAgent(string login, string password, string avatar, List<string> availableProducts)
    {
        Login = login;
        Password = password;
        Avatar = avatar;
        AvailableProducts = availableProducts;
    }

    public string Login { get; }
    public string Password { get; }
    public string Avatar { get; }
    public List<string> AvailableProducts { get; }

    public bool PasswordMatches(string passwordToTest)
    {
        return Password == passwordToTest;
    }
}