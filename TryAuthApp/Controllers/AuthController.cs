using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TryAuthApp.Dtos;
using TryAuthApp.Services.Auth;
using TryAuthApp.Services.Token;

namespace TryAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService userService)
        {
            _authService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody]InputCreateUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.CreateUser(model);
                if (result != null && result.IsSuccess)
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            return BadRequest("Veuiller vérfiier vos entrants."); // status code 400
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OutputLoginDto), StatusCodes.Status201Created)]
        public async Task<ActionResult> Login([FromBody] InputLoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return Ok(result);
            }
            return BadRequest("Veuiller vérfiier vos entrants."); // status code 400
        }
    }
}
