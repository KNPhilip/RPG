namespace dotNET7.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly RPGContext _context;
        
        public FightService(RPGContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponseDto<FightResultDto>> Fight(FightRequestDto request)
        {
            ServiceResponseDto<FightResultDto> response = new();
            try
            {
                List<Character> characters = await _context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .Where(c => request.CharacterIds.Contains(c.Id))
                    .ToListAsync();

                bool defeated = false;
                while(!defeated)
                {
                    foreach(Character attacker in characters) 
                    {
                        List<Character> opponents = characters.Where(c => c.Id != attacker.Id).ToList();
                        Character opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;
                        if (useWeapon && attacker.Weapon is not null) 
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if (!useWeapon && attacker.Skills is not null)
                        {
                            Skill skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            response.Data.Log
                                .Add($"{attacker.Name} wasn't able to attack!");
                            continue;
                        }

                        response.Data.Log
                            .Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage");

                        if (opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeated!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }

                characters.ForEach(c => 
                {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = $"Something went wrong: {e.Message}";
            }

            return response;
        }

        public async Task<ServiceResponseDto<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            ServiceResponseDto<AttackResultDto> response = new();
            try
            {
                Character? opponent = await _context.Characters
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (opponent is null)
                    throw new Exception("Opponent not found..");

                Character? attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                if (attacker is null)
                    throw new Exception("Attacker not found..");

                if (attacker.Skills is null)
                    throw new Exception("Attacker's weapon not found..");

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if (skill is null)
                    throw new Exception($"{attacker.Name} doesn't know that skill..");

                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
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

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + (new Random().Next(attacker.Intelligence));
            damage -= new Random().Next(opponent!.Defeats);

            if (damage > 0)
                opponent.HitPoints += damage;
            return damage;
        }

        public async Task<ServiceResponseDto<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            ServiceResponseDto<AttackResultDto> response = new();
            try
            {
                Character? opponent = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                Character? attacker = await _context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
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

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if (attacker.Weapon is null)
                throw new Exception("Attacker has no weapon..");
            if (attacker is null)
                throw new Exception("Attacker not found..");
            if (opponent is null)
                throw new Exception("Opponent not found..");

            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent!.Defeats);

            if (damage > 0)
                opponent.HitPoints += damage;
            return damage;
        }
    }
}