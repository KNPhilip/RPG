namespace RPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponseDto<List<Character>>> GetAllCharactersAsync();
        Task<ServiceResponseDto<Character>> GetCharacterByIdAsync(int id);
        Task<ServiceResponseDto<List<Character>>> AddCharacterAsync(Character request);
    }
}