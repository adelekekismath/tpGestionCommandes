using System.Net;
using System.Net.Http.Json;
using Api.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests;

public class ClientsEndpointsTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_Client_Returns_Created()
    {
        var dto = new ClientCreateDto("Doe", "Jane", "jane@ex.com", "0600000000", "Paris");
        var resp = await _client.PostAsJsonAsync("/api/clients", dto);
        Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
    }
    
    [Fact]
    public async Task Post_Client_Invalid_Returns_BadRequest()
    {
        var dto = new ClientCreateDto("D", "J", "invalid-email", "123", "");
        var resp = await _client.PostAsJsonAsync("/api/clients", dto);
        Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
       var content = await resp.Content.ReadAsStringAsync();
       Assert.Contains("Le nom doit contenir au moins 2 caractères.", content);
       Assert.Contains("Le prénom doit contenir au moins 2 caractères.", content);
       Assert.Contains("L'email est invalide.", content);
       Assert.Contains("Le téléphone doit contenir 10 chiffres.", content);
       Assert.Contains("La ville est requise.", content);
   }

    [Fact]
    public async Task Get_Clients_Returns_Ok()
    {
        var resp = await _client.GetAsync("/api/clients");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task Get_Client_ById_Returns_Ok()
    {
        var resp = await _client.GetAsync("/api/clients/1");
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task Get_Client_ById_NotFound()
    {
        var resp = await _client.GetAsync("/api/clients/9999");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task Put_Client_Returns_NoContent()
    {
        var dto = new ClientUpdateDto("Doe", "John", "john@ex.com", "0600000000", "Paris");
        var resp = await _client.PutAsJsonAsync("/api/clients/1", dto);
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
    }

    [Fact]
    public async Task Delete_Client_Returns_NoContent()
    {
        var resp = await _client.DeleteAsync("/api/clients/1");
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
    }

    [Fact]
    public async Task Delete_Client_Returns_NotFound()
    {
        var resp = await _client.DeleteAsync("/api/clients/9999");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}