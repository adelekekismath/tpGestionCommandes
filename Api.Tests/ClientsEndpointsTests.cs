using System.Net;
using System.Net.Http.Json;
using Api.ViewModel.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using Api.Domain.Entities;

namespace Api.Tests;

public class ClientsEndpointsTests : IClassFixture<CustomAuthFactory>
{
    private readonly HttpClient _client;

    private static string? _token;
    
    private readonly CustomAuthFactory _factory;

    public ClientsEndpointsTests(CustomAuthFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        

        _token ??= AuthenticateOnceAsync().GetAwaiter().GetResult();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);
    }
    
    private async Task<string> AuthenticateOnceAsync()
    {
        var uniqueUsername = $"clientUser_{DateTime.UtcNow.Ticks}";
        var registerDto = new UserCreateDto(uniqueUsername, "Password1", $"{uniqueUsername}@example.com");
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new { Username = uniqueUsername, Password = "Password1" };
        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        var loginData = await loginResp.Content.ReadFromJsonAsync<LoginResponse>();
        return loginData!.AccessToken;
    }

    private async Task<int> GetOrCreateClientAsync()
    {
        var clients = await _client.GetFromJsonAsync<List<ClientReadDto>>("/api/clients");

        if (clients is { Count: > 0 })
        {
            return clients.First().Id;
        }

        var dto = new ClientBaseDto("Doe", "Jane", "jane@ex.com", "0600000000", "Paris");
        var response = await _client.PostAsJsonAsync("/api/clients", dto);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<ClientReadDto>();
        return created!.Id;
    }

    [Fact]
    public async Task Post_Client_Returns_Created()
    {
        var dto = new ClientBaseDto("Smith", "John", "john.smith@example.com", "0700000000", "Lyon");
        var response = await _client.PostAsJsonAsync("/api/clients", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        _client.DefaultRequestHeaders.Authorization = null;
    }

    [Fact]
    public async Task Post_Client_Invalid_Returns_BadRequest()
    {
        var dto = new ClientBaseDto("D", "J", "invalid-email", "123", "");
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
        var id = await GetOrCreateClientAsync();
        var resp = await _client.GetAsync($"/api/clients/{id}");
        Console.WriteLine("Response Status: " + resp.StatusCode);
        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
    }

    [Fact]
    public async Task Get_Client_ById_NotFound()
    {
        var resp = await _client.GetAsync("/api/clients/999999");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }

    [Fact]
    public async Task Put_Client_Returns_NoContent()
    {
        var id = await GetOrCreateClientAsync();
        var dto = new ClientBaseDto("Doe", "John", "john@ex.com", "0600000000", "Paris");

        var resp = await _client.PutAsJsonAsync($"/api/clients/{id}", dto);
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
    }

    [Fact]
    public async Task Delete_Client_Returns_NoContent()
    {
        var dto = new ClientBaseDto("Temp", "User", "temp@example.com", "0611111111", "Nice");
        var respCreate = await _client.PostAsJsonAsync("/api/clients", dto);
        respCreate.EnsureSuccessStatusCode();

        var created = await respCreate.Content.ReadFromJsonAsync<ClientReadDto>();
        var id = created!.Id;

        var resp = await _client.DeleteAsync($"/api/clients/{id}");
        Assert.Equal(HttpStatusCode.NoContent, resp.StatusCode);
    }

    [Fact]
    public async Task Delete_Client_Returns_NotFound()
    {
        var resp = await _client.DeleteAsync("/api/clients/999999");
        Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
    }
}
