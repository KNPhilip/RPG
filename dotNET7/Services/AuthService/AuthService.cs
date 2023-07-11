namespace dotNET7.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly RPGContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(RPGContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponseDto<string>> Login(string username, string password)
        {
            ServiceResponseDto<string> response = new();

            try 
            {
                User? user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
                if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                    throw new Exception("Incorrect username or password.");
                
                response.Data = CreateJWT(user);
            }
            catch (Exception e) 
            {
                response.Success = false;
                response.Message = $"Something went wrong: {e.Message}";
            }
            
            return response;
        }

        public async Task<ServiceResponseDto<int>> Register(UserDto request, string password)
        {
            ServiceResponseDto<int> response = new();

            try 
            {
                if (await UserExists(request.Username)) 
                    throw new Exception("User already exists.");

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

        private string CreateJWT(User user) 
        {
            List<Claim> claims = new() 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Guest")
            };

            string? appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("Private security key not provided by the server!");

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
                
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}