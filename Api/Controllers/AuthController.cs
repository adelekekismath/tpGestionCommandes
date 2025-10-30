using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IConfiguration _config;

    public AuthController(IConfiguration config) => _config = config;

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

    private static bool ValidateUser(string username, string password)
    {
        return username == "admin" && password == "P@ssw0rd!";
    }

}