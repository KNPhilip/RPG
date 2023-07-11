namespace dotNET7.Controllers
{
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
            var response = await _characterService.GetAllCharactersAsync();
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
    }
}