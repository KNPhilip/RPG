namespace dotNET7.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly RPGContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(RPGContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            ServiceResponseDto<GetCharacterDto> response = new();
            try
            {
                Character? dbCharacter = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                    c.User!.Id == int.Parse(_httpContextAccessor.HttpContext!.User
                    .FindFirstValue(ClaimTypes.NameIdentifier)!));
                if (dbCharacter is null)
                    throw new Exception("Character not found.");

                Weapon weapon = new()
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = dbCharacter
                };

                _context.Weapons.Add(weapon);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = $"Something went wrong: {e.Message}";
            }

            return response;
        }
    }
}