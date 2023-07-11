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

        [HttpGet]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> GetAllCharacters()
        {
            ServiceResponseDto<List<GetCharacterDto>> response = await _characterService.GetAllCharactersAsync();
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponseDto<GetCharacterDto>>> GetCharacterById(int id)
        {
            ServiceResponseDto<GetCharacterDto> response = await _characterService.GetCharacterByIdAsync(id);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto request) 
        {
            ServiceResponseDto<List<GetCharacterDto>> response = await _characterService.AddCharacterAsync(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponseDto<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto request) 
        {
            ServiceResponseDto<GetCharacterDto> response = await _characterService.UpdateCharacterAsync(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponseDto<List<GetCharacterDto>>>> DeleteCharacter(int id) 
        {
            ServiceResponseDto<List<GetCharacterDto>> response = await _characterService.DeleteCharacterAsync(id);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}