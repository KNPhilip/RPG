namespace dotNET7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponseDto<int>>> Register(UserDto request) 
        {
            ServiceResponseDto<int> response = await _authService.Register(request, request.Password);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponseDto<string>>> Login(UserDto request) 
        {
            ServiceResponseDto<string> response = await _authService.Login(request.Username, request.Password);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}