using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using StarWarsAPI;
using Xunit.Abstractions;

namespace UnitTestStarWars;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    public BasicTests(WebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetSwCharacters_ReturnOk()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/sw-characters");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetSwCharactersById_Found()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/sw-characters/1");
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddCharacter_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        var character = new Character()
        {
            Name = "Olaf",
            Faction = "Rebel Alliance",
            HomeWorld = "Eisgarn",
            Species = "Snowman"
        };
        
        var response = await client.PostAsJsonAsync("/sw-characters", character);
        response.EnsureSuccessStatusCode();
        _testOutputHelper.WriteLine(response.StatusCode.ToString());
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCharacter_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var character = new Character()
        {
            Name = "Name1",
            Faction = "Faction1",
            HomeWorld = "HomeWorld1",
            Species = "Species1"
        };

        var postResponse = await client.PostAsJsonAsync("/sw-characters", character);
        postResponse.EnsureSuccessStatusCode();
        var createdCharacter = await postResponse.Content.ReadFromJsonAsync<Character>();


        createdCharacter!.Name = "Huber";
        _testOutputHelper.WriteLine(createdCharacter.Name);
        var putResponse = await client.PutAsJsonAsync($"/sw-characters/{createdCharacter.Id}", createdCharacter);
        putResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        
    }

    [Fact]
    public async Task DeleteCharacter_ReturnsOk()
    {
        var client = _factory.CreateClient();
        var character = new Character()
        {
            Name = "Olaf",
            Faction = "Rebel Alliance",
            HomeWorld = "Eisgarn",
            Species = "Snowman"
        };
        
        var postResponse = await client.PostAsJsonAsync("/sw-characters", character);
        var createdCharacter = await postResponse.Content.ReadFromJsonAsync<Character>();

        var deleteResponse = await client.DeleteAsync($"/sw-characters/{createdCharacter?.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        _testOutputHelper.WriteLine(deleteResponse.StatusCode.ToString());
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetSwCharactersById_NotFoundId()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/sw-characters/100");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}