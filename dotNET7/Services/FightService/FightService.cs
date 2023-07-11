namespace dotNET7.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly RPGContext _context;
        
        public FightService(RPGContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseDto<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            ServiceResponseDto<AttackResultDto> response = new();
            try 
            {
                Character? opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (opponent is null)
                    throw new Exception("Opponent not found..");

                Character? attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                if (attacker is null)
                    throw new Exception("Attacker not found..");

                if (attacker.Weapon is null)
                    throw new Exception("Attacker's weapon not found..");

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent!.Defeats);

                if (damage > 0)
                    opponent.HitPoints += damage;

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
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