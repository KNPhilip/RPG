namespace dotNET7.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly RPGContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, RPGContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto<List<GetCharacterDto>>> AddCharacterAsync(AddCharacterDto request)
        {
            ServiceResponseDto<List<GetCharacterDto>> response = new();

            try 
            {
                Character newCharacter = _mapper.Map<Character>(request);
                _context.Characters.Add(newCharacter);
                newCharacter.User = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == GetUserId());
                await _context.SaveChangesAsync();

                response.Data = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c))
                    .ToListAsync();
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
            ServiceResponseDto<List<GetCharacterDto>> response = new();

            try 
            {
                Character? dbCharacter = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter is null)
                    throw new Exception($"You don't have a character with an id of '{id}'");

                _context.Characters.Remove(dbCharacter);
                await _context.SaveChangesAsync();
                response.Data = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
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
            ServiceResponseDto<List<GetCharacterDto>> response = new();

            try
            {
                List<Character> dbCharacters = await _context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .ToListAsync();
                if (dbCharacters is null || dbCharacters.Count == 0)
                    throw new Exception("You don't have any characters..");

                response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
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
            ServiceResponseDto<GetCharacterDto> response = new();

            try 
            {
                Character? dbCharacter = await _context.Characters
                    .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
                if (dbCharacter is null) 
                    throw new Exception($"You don't have a character with an id of '{id}'");

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
            ServiceResponseDto<GetCharacterDto> response = new();

            try 
            {
                Character? character = 
                    await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.Id && c.User!.Id == GetUserId());
                if (character is null) 
                    throw new Exception($"You don't have a character with an id of '{request.Id}'");

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

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext!.User.
            FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}