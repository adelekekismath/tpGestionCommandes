namespace Api.Application.Services.Auths;

using Api.ViewModel.DTOs;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using  Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _http;

    public AuthService(UserManager<IdentityUser> userManager, IConfiguration config, IHttpContextAccessor http)
    {
        _config = config;
        _userManager = userManager;
        _http = http;
    }

    public async Task<LoginResponse?> AuthenticateAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return null;

        Console.WriteLine($"User {user.Email} authenticated successfully.");

        var userRoles = await _userManager.GetRolesAsync(user);
        var role = userRoles.FirstOrDefault() ?? "User";

        var authClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),  
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role)
        };

        var authSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );


        var expires = DateTime.Now.AddMinutes(
            double.Parse(_config["Jwt:ExpiresMinutes"])
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: authClaims,
            expires: expires,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResponse(jwt, expires);
    }

    public async Task<IdentityUser?> RegisterAsync(UserCreateDto user)
    {
        var newUser = new IdentityUser
        {
            UserName = user.Username,
            Email = user.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var createdUser = await _userManager.CreateAsync(newUser, user.Password);
        await _userManager.AddToRoleAsync(newUser, "USER");

        return newUser;
    }
    public async Task<User?> WhoAmIAsync()
    {
        var userClaim = _http.HttpContext?.User;

        if (userClaim is null || !userClaim.Identity!.IsAuthenticated)
            return null;

        var userId = userClaim.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return null;

        var user = await _userManager.FindByIdAsync(userId);
        return new User
        {
            Id =  user.Id,
            Username = user.UserName,
            Email = user.Email
        };
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return new User
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email
        };
    }
}