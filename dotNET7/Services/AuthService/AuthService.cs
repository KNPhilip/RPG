namespace dotNET7.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly RPGContext _context;

        public AuthService(RPGContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseDto<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponseDto<int>> Register(UserDto request, string password)
        {
            ServiceResponseDto<int> response = new();

            try 
            {
                if (await UserExists(request.Username)) 
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                }

                User user = new()
                {
                    Username = request.Username,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                response.Data = user.Id;
            }
            catch (Exception e) 
            {
                response.Success = false;
                response.Message = $"Something went wrong: {e.Message}";
            }
            
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            bool result = await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()) ? true : false;
            return result;
        }
    }
}