using Database;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace StarWarsAPI;

public class CharacterRepository
{
    private readonly StarWarsContext _context;

    public CharacterRepository(StarWarsContext context)
    {
        _context = context;
    }
    /*private static List<Character?> _characters = new List<Character?>()
    {
        new Character() {Id = 1, Name = "Luke Skywalker", Faction = "Rebel Allaince", HomeWorld = "Tatooine", Species = "Human"},
        new Character() {Id = 2, Name = "Darth Vader", Faction = "Galactic Empire", HomeWorld = "Tatooine", Species = "Human"},
        new Character() {Id = 3, Name = "R2-D2", Faction = "Rebel Alliance", HomeWorld = "Naboo", Species = "Droid"}
    };
    */
    public async Task<List<Character>> GetAllAsync() => await _context.Characters.ToListAsync();


    public async Task<Character?> GetCharacterByIdAsync(int id) =>
        await _context.Characters.FirstOrDefaultAsync(s => s.Id == id);

    public async Task AddAsync(Character? character)
    {
        if (character != null)
        {
            character.Id = 0;
            await _context.Characters.AddAsync(character);
        }

        await _context.SaveChangesAsync();
    }

    public async Task Update(Character character)
    {
        await _context.Characters
            .Where(c => c.Id == character.Id)
            .ExecuteUpdateAsync(c =>
                c.SetProperty(p => p.HomeWorld, character.HomeWorld)
                    .SetProperty(p => p.Name, character.Name)
                    .SetProperty(p => p.Faction, character.Faction)
                    .SetProperty(p => p.Species, character.Species));
    }
    public async Task DeleteAsync(int id) => await _context.Characters.Where(s => s.Id == id).ExecuteDeleteAsync();
}