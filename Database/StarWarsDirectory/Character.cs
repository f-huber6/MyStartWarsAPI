using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarWarsAPI;

public class Character
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string? Name { get; set; }
    [StringLength(50)]
    public string? Faction { get; set; }
    [StringLength(50)]
    public string? HomeWorld { get; set; }
    [StringLength(50)]
    public string? Species { get; set; }
}
