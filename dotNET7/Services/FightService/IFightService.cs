namespace dotNET7.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponseDto<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponseDto<AttackResultDto>> SkillAttack(SkillAttackDto request);
        Task<ServiceResponseDto<FightResultDto>> Fight(FightRequestDto request);
        Task<ServiceResponseDto<List<HighscoreDto>>> GetHighScore();
    }
}