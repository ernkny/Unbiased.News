using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Helpers
{
    public static class SigningSecurityKey
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
