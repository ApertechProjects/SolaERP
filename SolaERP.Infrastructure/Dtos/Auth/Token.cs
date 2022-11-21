namespace SolaERP.Infrastructure.Dtos.Auth
{
    public class Token
    {
        public string AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
