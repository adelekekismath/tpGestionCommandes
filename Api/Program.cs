using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Api.Databases.Contexts;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Unicode;
using Microsoft.EntityFrameworkCore.InMemory;
using Api.Application.Services.Auths;
using Api.Application.Services.Commandes;
using Api.Application.Services.Clients;
using Api.Application.Services.Produits;
using Api.Application.Services.LignesCommandes;
using Api.Application.Services.Categories;
using Api.Databases.UnitOfWork;
using Api.Databases.Repositories.BaseRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Api.ViewModel.Validation;
using Api.Domain.Entities;

[assembly: InternalsVisibleTo("Api.Tests")]


var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
    );
}

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

var requireAuthPolicy = new AuthorizationPolicyBuilder()
	.RequireAuthenticatedUser()
	.Build();

builder.Services.AddAuthorizationBuilder()
	.SetFallbackPolicy(requireAuthPolicy);


builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

builder.Services.AddTransient<ICommandeService, CommandeService>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IProduitService, ProduitService>();
builder.Services.AddTransient<ILigneCommandeService, LigneCommandeService>();
builder.Services.AddTransient<ICategorieService, CategorieService>();
builder.Services.AddTransient<IBaseRepository<Categorie>, BaseRepository<Categorie>>();
builder.Services.AddTransient<IAuthService, AuthService>();

builder.Services.AddControllers()
.AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<ClientCreateDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TP API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Entrer 'Bearer {token}'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleHelper.EnsureRolesCreated(roleManager);
}

app.UseExceptionHandler("/error");

if (app.Environment.IsDevelopment())
{
    Console.WriteLine("In Development Environment");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Bienvenue sur mon API");



app.Run();

public  partial class  Program{};
