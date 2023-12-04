using System.Security.Claims;
using TryAuthApp.Dtos;
using TryAuthApp.Models;

namespace TryAuthApp.Services.Token
{
    public interface ITokenService
    {
        TokenResponse? CreateToken(IEnumerable<Claim> claims);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
