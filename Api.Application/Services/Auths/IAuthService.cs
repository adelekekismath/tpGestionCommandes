namespace Api.Application.Services.Auths;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Api.ViewModel.DTOs;
using Api.Domain.Entities;
public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(LoginRequest request);
    Task<IdentityUser?> RegisterAsync(UserCreateDto user);    
    Task<User?> WhoAmIAsync();
    Task<User?> GetByIdAsync(string id);
}