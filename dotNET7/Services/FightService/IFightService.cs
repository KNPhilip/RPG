namespace dotNET7.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponseDto<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
    }
}