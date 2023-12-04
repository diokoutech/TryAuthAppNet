using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TryAuthApp.Database;
using TryAuthApp.Dtos;
using TryAuthApp.Services.Token;

namespace TryAuthApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _serviceToken;
        private readonly TryAuthDbContext _context;

        public TokenController(ITokenService serviceToken, TryAuthDbContext context)
        {
            _serviceToken = serviceToken;
            _context = context;
        }
        [HttpPost]
        public ActionResult<InputRefreshToken> Refresh(InputRefreshToken model)
        {
            if (model is null)
                return BadRequest("Vérifier les informations entrées.");
            var principal = _serviceToken.GetPrincipalFromExpiredToken(model.AccessToken);
            var username = principal.FindFirst("Username")?.Value;
            var user = _context.TokenInfo
                .SingleOrDefault(u => u.Usename == username);
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiry <= DateTime.Now)
                return BadRequest("Le token entré est invalide.");
            var newAccessToken = _serviceToken.CreateToken(principal.Claims);
            var newRefreshToken = _serviceToken.GetRefreshToken();
            user.RefreshToken = newRefreshToken;
            _context.SaveChanges();
            return Ok(new InputRefreshToken()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            });
        }
        /// <summary>
        /// Supprime le refresh token de l'utilisateur
        /// </summary>
        /// <returns></returns>
        [HttpPost, Authorize]
        public IActionResult Revoke()
        {
            try
            {
                var username = User.FindFirst("Username")?.Value;
                var user = _context.TokenInfo.SingleOrDefault(u => u.Usename == username);
                if (user is null)
                    return BadRequest();
                user.RefreshToken = String.Empty;
                _context.SaveChanges();
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
