namespace dotNET7.Services.CharacterService
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

        private readonly IMapper _mapper;

        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto request)
        {
            var response = new ServiceResponseDto<List<GetCharacterDto>>();

            try 
            {
                Character newCharacter = _mapper.Map<Character>(request);
                newCharacter.Id = characters.Max(c => c.Id) + 1;
                characters.Add(newCharacter);
                
                response.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception e) 
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponseDto<List<GetCharacterDto>>> GetAllCharactersAsync()
        {
            var response = new ServiceResponseDto<List<GetCharacterDto>>();

            try 
            {
                List<GetCharacterDto> foundCharacters = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                if (foundCharacters is null) 
                    throw new Exception($"No characters found..");

                response.Data = foundCharacters;
            }
            catch (Exception e)
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }
            
            return response;
        }

        public async Task<ServiceResponseDto<GetCharacterDto>> GetCharacterByIdAsync(int id)
        {
            var response = new ServiceResponseDto<GetCharacterDto>();

            try 
            {
                Character? character = characters.FirstOrDefault(c => c.Id == id);
                if (character is null) 
                    throw new Exception($"Character with Id '{id}' not found.");

                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e)
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponseDto<GetCharacterDto>> UpdateCharacterAsync(UpdateCharacterDto request)
        {
            var response = new ServiceResponseDto<GetCharacterDto>();

            try 
            {
                Character? character = characters.FirstOrDefault(c => c.Id == request.Id);
                if (character is null) 
                    throw new Exception($"Character with Id '{request.Id}' not found.");

                _mapper.Map(request, character);
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e) 
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }

            return response;
        }
    }
}