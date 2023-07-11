namespace dotNET7.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponseDto<GetCharacterDto>> GetCharacterByIdAsync(int id);
        Task<ServiceResponseDto<List<GetCharacterDto>>> GetAllCharactersAsync();
        Task<ServiceResponseDto<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto request);
        Task<ServiceResponseDto<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto request);
    }
}