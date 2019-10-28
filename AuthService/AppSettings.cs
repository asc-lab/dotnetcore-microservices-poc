namespace AuthService
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string[] AllowedAuthOrigins { get; set; }
    }
}