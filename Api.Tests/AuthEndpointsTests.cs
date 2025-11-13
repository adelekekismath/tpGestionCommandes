using System.Net;
using System.Net.Http.Json;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;

namespace Api.Tests;

public class AuthEndpointsTests : IClassFixture<CustomAuthFactory>
{
    private readonly HttpClient _client;

    public AuthEndpointsTests(CustomAuthFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Create_User_And_Return_Created()
    {
        var dto = new UserCreateDto("testuser1", "Password@1", "testuser1@test.com");

        var response = await _client.PostAsJsonAsync("/api/auth/register", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<User>();
        Console.WriteLine($"created!.Username) {created!.Username}");
        Assert.NotNull(created);
        Assert.Equal("testuser1", created!.Username);
    }

    [Fact]
    public async Task Login_Should_Return_Token_When_Credentials_Are_Valid()
    {
        var registerDto = new UserCreateDto("john", "Password1", "john@test.com");
        await _client.PostAsJsonAsync("/api/auth/register", registerDto);

        var loginDto = new { Username = "john", Password = "Password1" };
        var resp = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

        var result = await resp.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result!.AccessToken));
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Invalid_Credentials()
    {
        var loginDto = new { Username = "ghost", Password = "wrong" };

        var resp = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }

    [Fact]
    public async Task GetById_Should_Return_User_When_Token_Is_Valid()
    {
        var registerDto = new UserCreateDto("secureduser", "Password1", "secureduser@test.com");
        var resp = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
        var user = await resp.Content.ReadFromJsonAsync<User>();
        Console.WriteLine($"resp.statusCode {resp.StatusCode}");

        var loginDto = new { Username = "secureduser", Password = "Password1" };
        var loginResp = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

        Console.WriteLine($"loginResp.statusCode {loginResp.StatusCode}");
        var rawBody = await loginResp.Content.ReadAsStringAsync();
        Console.WriteLine($"Response Body: {rawBody}");
        var token = (await loginResp.Content.ReadFromJsonAsync<LoginResponse>())!.AccessToken;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var getResp = await _client.GetAsync($"/api/auth/{user!.Id}");
        Assert.Equal(HttpStatusCode.OK, getResp.StatusCode);
    }


    [Fact]
    public async Task GetById_Should_Return_Unauthorized_When_No_Token()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var resp = await _client.GetAsync("/api/auth/1");
        Assert.Equal(HttpStatusCode.Unauthorized, resp.StatusCode);
    }
}

public record LoginResponse(string AccessToken, DateTime ExpiresAt);