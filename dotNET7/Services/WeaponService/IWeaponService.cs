namespace dotNET7.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponseDto<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}