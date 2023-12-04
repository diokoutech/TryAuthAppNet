using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TryAuthApp.Database;
using TryAuthApp.Dtos;
using TryAuthApp.Models;
using TryAuthApp.Services.Token;

namespace TryAuthApp.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly TryAuthDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _serviceToken;

        public AuthService(UserManager<AppUser> userManager, TryAuthDbContext context, ITokenService serviceToken)
        {
            _userManager = userManager;
            _context = context;
            _serviceToken = serviceToken;
        }

        public async Task<OutputLoginDto> LoginUserAsync(InputLoginDto model)
        {
            OutputLoginDto response = new();
            // check email
            var user = await _userManager.FindByNameAsync(model.Identifiant);
            if (user == null)
            {
                response.Message = "Error login User";
                response.IsSuccess = false;
                return response;
            }
            // check password
            bool result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
            {
                response.Message = "Error Login password";
                response.IsSuccess = false;
                return response;
            }
            // lier
            if (user.IsActive == 0)
            {
                response.Message = "Error Activate";
                response.IsSuccess = false;
                return response;
            }
            // create token 
            var claims = new[]
{
                new Claim("Email",user.Email),
                new Claim("UserId",user.Id),
                new Claim("Profil",user.Profil ?? ""),
                new Claim("Username",user.UserName ?? ""),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Profil ?? ""),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
            };
            var token = _serviceToken.CreateToken(claims);
            if (token != null)
            {
                var refreshToken = _serviceToken.GetRefreshToken();
                response.Message = "Success Login";
                response.IsSuccess = true;
                response.Token = token.TokenString;
                response.Profil = user.Profil;
                response.IdUser = user.Id;
                response.Name = user.UserName;
                response.ExpiresIn = token.ValidTo;
                response.RefreshToken = refreshToken;
                // save token and refresh token
                var tokenInfo = _context.TokenInfo.FirstOrDefault(a => a.Usename == user.UserName);
                if (tokenInfo == null)
                {
                    var info = new TokenInfo
                    {
                        Usename = user.UserName,
                        RefreshToken = refreshToken,
                        RefreshTokenExpiry = DateTime.Now.AddDays(1)
                    };
                    _context.TokenInfo.Add(info);
                }
                else
                {
                    tokenInfo.RefreshToken = refreshToken;
                    tokenInfo.RefreshTokenExpiry = DateTime.Now.AddDays(1);
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
                return response;
            }
            return response;
        }
        public async Task<ResponseDto>? CreateUser(InputCreateUser model)
        {
            var response = new ResponseDto();
            try
            {
                string defaultPassword = "P@ssword123";
                // creation user 
                var user = new AppUser();
                user.Profil = "Test";
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.IsActive = 1;
                // user creation
                // insert user
                var result = await _userManager.CreateAsync(user, defaultPassword);
                if (result.Succeeded)
                {
                    response.Message = "Success";
                    response.IsSuccess = true;
                    response.Result = result;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Errors = result.Errors.Select(e => e.Description);
                    if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
                    {
                        response.Message = string.Format("Dup UserName", user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "InvalidUserName"))
                    {
                        response.Message = string.Format("Invalid User name", user.UserName);
                    }
                    else if (result.Errors.Any(e => e.Code == "DuplicateEmail"))
                    {
                        response.Message = string.Format("Dupli Email", user.Email);
                    }
                    else
                    {
                        response.Message = "Error create";
                    }
                }
                return response;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
