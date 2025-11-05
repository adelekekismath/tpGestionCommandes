using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Api.Contracts;
using Microsoft.AspNetCore.Identity;
using Api.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{

    private readonly IConfiguration _config;
    private readonly AppDbContext _db;

    public AuthController(IConfiguration config, AppDbContext db)
    {
        _config = config;
        _db = db;
    }

    public record LoginRequest(string UserName, string Password);
    public record LoginResponse(string AccessToken, DateTime ExpiresAt);

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if (!ValidateUser(req.UserName, req.Password))
            return Unauthorized();

        var jwtSection = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, req.UserName),
            new(ClaimTypes.NameIdentifier, req.UserName),
        };

        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiresMinutes"]!));
        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new LoginResponse(jwt, expires));
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        return Ok(User.Identity?.Name ?? "anonymous");
    }

    

    private static string HashPassword(string password)
    {
        var hasher = new PasswordHasher<object>();
        return hasher.HashPassword(null, password);
    }



    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            PasswordHash = HashPassword(userDto.Password)
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    private bool ValidateUser(string username, string password)
    {
        var user = _db.Users.SingleOrDefault(u => u.Username == username);
        if (user == null)
            return false;

        var hasher = new PasswordHasher<User>();
        return hasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success;
    }


}