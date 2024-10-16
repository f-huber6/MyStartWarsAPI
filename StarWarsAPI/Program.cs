using System.Reflection;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarWarsAPI;


var assembly = Assembly.GetExecutingAssembly(); 
var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StarWarsContext>(options =>
{
    options.UseSqlite(conf.GetConnectionString("DefaultConnection"), sqlliteOptions =>
    {
        sqlliteOptions.MigrationsAssembly(assembly.FullName);
    });
});

builder.Services.AddScoped<CharacterRepository>();

var app = builder.Build();

app.MapGet("/sw-characters", async (string? faction, string? homeworld, string? species, [FromServices] CharacterRepository rep) =>
{
    var characters = await rep.GetAllAsync();

    if (!string.IsNullOrEmpty(faction))
    {
        characters = characters.Where(s => s.Faction == faction).ToList();
    }

    if (!string.IsNullOrEmpty(homeworld))
    {
        characters = characters.Where(s => s.HomeWorld == homeworld).ToList();
    }

    if (!string.IsNullOrEmpty(species))
    {
        characters = characters.Where(s => s.Species == species).ToList();
    }

    return Results.Ok(characters);
});

app.MapGet("/sw-characters/{id:int}", async (int id, [FromServices] CharacterRepository rep) =>
{
    var character = await rep.GetCharacterByIdAsync(id);
    if (character == null)
    {
        return Results.NotFound();
    }
    
    return Results.Ok(character);
});
 app.MapPost("/sw-characters", async ([FromBody] Character character, [FromServices] CharacterRepository rep) =>
{
    await rep.AddAsync(character);
    return Results.Created($"/sw-characters/{character.Id}", character);
});

app.MapPut("/sw-characters/{id:int}", async (int id, Character character, [FromServices] CharacterRepository rep) =>
{
    var existingCharacterToChange = await rep.GetCharacterByIdAsync(id);

    if (existingCharacterToChange == null)
    {
        Results.NotFound();
    }

    character.Id = id;
    await rep.Update(character);

    return Results.Ok(character);
});

app.MapDelete("/sw-characters/{id:int}", async (int id, [FromServices] CharacterRepository rep) =>
{
    var existingCharacterForDelete = await rep.GetCharacterByIdAsync(id);

    if (existingCharacterForDelete == null)
    {
        Results.NotFound();
    }

    await rep.DeleteAsync(id);

    return Results.NoContent();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// um Program für UnitTests zugänglich zu machen:
public partial class Program { }
