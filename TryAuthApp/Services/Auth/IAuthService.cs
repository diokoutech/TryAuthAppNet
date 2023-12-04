using TryAuthApp.Dtos;

namespace TryAuthApp.Services.Auth
{
    public interface IAuthService
    {
        Task<ResponseDto>? CreateUser(InputCreateUser model);
        Task<OutputLoginDto> LoginUserAsync(InputLoginDto model);
    }
}