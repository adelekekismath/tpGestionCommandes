using Api.Domain.Entities;
using Api.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Api.Tests;

public class CustomAuthFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
            );
            if (descriptor != null)
                services.Remove(descriptor);

            var mockDb = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());

            var fakeUsers = new List<User>
            {
                new() {
                    Id = 1,
                    Username = "mockuser",
                    PasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<User>().HashPassword(null, "P@ssw0rd!")
                },
                new() {
                    Id = 2,
                    Username = "testuser",
                    PasswordHash = new Microsoft.AspNetCore.Identity.PasswordHasher<User>().HashPassword(null, "Test1234")
                }
            }.AsQueryable();

            var mockUsers = new Mock<DbSet<User>>();
            mockUsers.As<IQueryable<User>>().Setup(m => m.Provider).Returns(fakeUsers.Provider);
            mockUsers.As<IQueryable<User>>().Setup(m => m.Expression).Returns(fakeUsers.Expression);
            mockUsers.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(fakeUsers.ElementType);
            mockUsers.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(fakeUsers.GetEnumerator());

            mockDb.Setup(db => db.Users).Returns(mockUsers.Object);

            services.AddSingleton(mockDb.Object);

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
        System.Text.Encoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

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
