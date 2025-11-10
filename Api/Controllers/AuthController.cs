using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Api.Databases.Contexts;
using Api.Domain.Entities;
using Api.ViewModel.Validation;
using Api.ViewModel.DTOs;
using Api.Application.Services.Auths;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{

    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var loginResponse = await _authService.AuthenticateAsync(req);
        if (loginResponse.AccessToken == null)
        {
            return Unauthorized();
        }
        return Ok(loginResponse);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _authService.GetByIdAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [Authorize]
    [HttpGet("whoami")]
    public async Task<IActionResult> WhoAmI()
    {
        var user = await _authService.WhoAmIAsync();

        if (user == null)
            return Unauthorized();

        return Ok(new 
        {
            user.Id,
            user.Username,
            user.Email
        });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto userDto)
    {
        var user = await _authService.RegisterAsync(userDto);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }


}