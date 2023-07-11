namespace dotNET7.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> GetAllCharacters()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await _characterService.GetAllCharactersAsync();
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("my-characters"), AllowAnonymous]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> GetCharactersForAuthenticatedUser()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await _characterService.GetCharactersForAuthenticatedUserAsync(id);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponseDto<GetCharacterDto>>> GetCharacterById(int id)
        {
            var response = await _characterService.GetCharacterByIdAsync(id);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto request) 
        {
            var response = await _characterService.AddCharacterAsync(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponseDto<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto request) 
        {
            var response = await _characterService.UpdateCharacterAsync(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> DeleteCharacter(int id) 
        {
            var response = await _characterService.DeleteCharacterAsync(id);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}