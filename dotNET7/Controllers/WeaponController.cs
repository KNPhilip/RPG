namespace dotNET7.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponseDto<GetCharacterDto>>> AddWeapon(AddWeaponDto request)
        {
            ServiceResponseDto<GetCharacterDto> response = await _weaponService.AddWeapon(request);
            if (response.Data is null)
                return NotFound(response);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}