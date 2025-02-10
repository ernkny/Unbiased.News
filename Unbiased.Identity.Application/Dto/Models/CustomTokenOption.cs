namespace Unbiased.Identity.Application.Dto.Models
{
    public class CustomTokenOption
    {
        public List<String> Audience { get; set; }
        public string Issuer { get; set; }

        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }

        public string SecurityKey { get; set; }
    }
}
