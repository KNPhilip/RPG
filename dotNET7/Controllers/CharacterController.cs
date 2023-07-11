namespace RPG.Controllers
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
        public async Task<ActionResult<ServiceResponseDto<List<Character>>>> GetAllCharacters()
        {
            return Ok(await _characterService.GetAllCharactersAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponseDto<Character>>> GetCharacterById(int id)
        {
            return Ok(await _characterService.GetCharacterByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<List<Character>>>> AddCharacter(Character request) 
        {
            return Ok(await _characterService.AddCharacterAsync(request));
        }
    }
}