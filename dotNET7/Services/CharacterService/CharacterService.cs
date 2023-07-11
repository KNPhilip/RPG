namespace dotNET7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly RPGContext _context;

        public CharacterService(IMapper mapper, RPGContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto request)
        {
            var response = new ServiceResponseDto<List<GetCharacterDto>>();

            try 
            {
                Character newCharacter = _mapper.Map<Character>(request);
                _context.Characters.Add(newCharacter);
                await _context.SaveChangesAsync();

                response.Data = await GetFoundCharacters();
            }
            catch (Exception e) 
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }

            return response;
        }

        public async Task<ServiceResponseDto<List<GetCharacterDto>>> DeleteCharacterAsync(int id)
        {
            var response = new ServiceResponseDto<List<GetCharacterDto>>();

            try 
            {
                Character? dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (dbCharacter is null)
                    throw new Exception($"Character with id '{id}' not found.");

                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();
                response.Data = await GetFoundCharacters();
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
                List<GetCharacterDto> foundCharacters = await GetFoundCharacters();
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
                Character? dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
                if (dbCharacter is null) 
                    throw new Exception($"Character with Id '{id}' not found.");

                response.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
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
                Character? character = 
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (character is null) 
                    throw new Exception($"Character with Id '{request.Id}' not found.");

                _mapper.Map(request, character);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception e) 
            {
                response.Message = $"Something went wrong: {e.Message}";
                response.Success = false;
            }

            return response;
        }

        private async Task<List<GetCharacterDto>> GetFoundCharacters() 
        {
            List<Character> dbCharacters = await _context.Characters.ToListAsync();
            List<GetCharacterDto> returning = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return returning;
        }
    }
}