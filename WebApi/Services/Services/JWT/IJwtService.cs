using System.Security.Claims;
using System.Threading.Tasks;
using Entities.User;
using Microsoft.IdentityModel.Tokens;

namespace Services.Services.JWT
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
        ClaimsPrincipal GetPrincipalFromToken(string token, string signingKey);
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
    }
}