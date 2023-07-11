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
    }
}