using Microsoft.EntityFrameworkCore;
using StarWarsAPI;

namespace Database;

public class StarWarsContext: DbContext
{
    public StarWarsContext(DbContextOptions<StarWarsContext> options) : base(options)
    {
        
    }
    
    public DbSet<Character> Characters { get; set; }
}