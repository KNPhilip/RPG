namespace dotNET7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<ServiceResponseDto<AttackResultDto>>> WeaponAttack(WeaponAttackDto request) 
        {
            ServiceResponseDto<AttackResultDto> response = await _fightService.WeaponAttack(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("skill")]
        public async Task<ActionResult<ServiceResponseDto<AttackResultDto>>> SkillAttack(SkillAttackDto request) 
        {
            ServiceResponseDto<AttackResultDto> response = await _fightService.SkillAttack(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<FightResultDto>>> FightAttack(FightRequestDto request) 
        {
            ServiceResponseDto<FightResultDto> response = await _fightService.Fight(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponseDto<List<HighscoreDto>>>> GetHighScore() 
        {
            ServiceResponseDto<List<HighscoreDto>> response = await _fightService.GetHighScore();
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}