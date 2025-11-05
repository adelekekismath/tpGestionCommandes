using Api.Domain.Entities;
using Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

public class CustomAuthFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
            // 🔹 Supprimer les anciens contextes
            var dbContextDescriptors = services
                .Where(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>))
                .ToList();
            foreach (var descriptor in dbContextDescriptors)
                services.Remove(descriptor);

            var appDbDescriptors = services
                .Where(d => d.ServiceType == typeof(AppDbContext))
                .ToList();
            foreach (var descriptor in appDbDescriptors)
                services.Remove(descriptor);

            // 🔹 Créer une base InMemory unique
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            var sp = services.BuildServiceProvider();

            // 🔹 Recréer la base et insérer des données de test
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                db.ChangeTracker.Clear();
                db.ChangeTracker.AutoDetectChangesEnabled = false;

                var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

                db.Users.AddRange(
                    new User { Username = "mockuser", PasswordHash = hasher.HashPassword(null, "P@ssw0rd!") },
                    new User { Username = "testuser", PasswordHash = hasher.HashPassword(null, "Test1234") }
                );

                db.Clients.AddRange(
                    new Client { Nom = "Smith", Prenom = "Alice", Email = "alice@test.com", Adresse = "Paris", Telephone = "0600000000" },
                    new Client { Nom = "Jones", Prenom = "Bob", Email = "bob@test.com", Adresse = "Lyon", Telephone = "0700000000" }
                );

                db.SaveChanges();
                db.ChangeTracker.AutoDetectChangesEnabled = true;
            }

            // 🔹 Authentification fictive pour tests
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });
    }
}

public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "mockuser"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
