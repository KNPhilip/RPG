namespace dotNET7.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponseDto<int>> Register(UserDto request, string password);
        Task<ServiceResponseDto<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}