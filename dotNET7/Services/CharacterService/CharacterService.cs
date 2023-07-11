namespace RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new() {
            new Character 
            {
                Id = 1
            },
            new Character 
            {
                Id = 2,
                Name = "Sam"
            }
        };

        public async Task<ServiceResponseDto<List<Character>>> AddCharacterAsync(Character request)
        {
            var response = new ServiceResponseDto<List<Character>>();
            characters.Add(request);
            response.Data = characters;
            return response;
        }

        public async Task<ServiceResponseDto<List<Character>>> GetAllCharactersAsync()
        {
            ServiceResponseDto<List<Character>> response = new()
            {
                Data = characters
            };
            return response;
        }

        public async Task<ServiceResponseDto<Character>> GetCharacterByIdAsync(int id)
        {
            Character? character = characters.FirstOrDefault(c => c.Id == id);
            ServiceResponseDto<Character> response = new()
            {
                Data = character
            };
            return response;
        }
    }
}