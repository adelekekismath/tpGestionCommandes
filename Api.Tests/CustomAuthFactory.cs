using Api.Domain.Entities;
using Api.Databases.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;


public class CustomAuthFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureServices(services =>
        {
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

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase($"IntegrationTestsDb_{Guid.NewGuid()}"));

            var sp = services.BuildServiceProvider();

            Task.Run(async () =>
            {
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();
                db.ChangeTracker.Clear();
                
                const string userRoleName = "USER";

                var userRole = await roleManager.FindByNameAsync(userRoleName);

                if (userRole == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(userRoleName));
                }
                var role = await roleManager.FindByNameAsync("User");
                Console.WriteLine($"role?.NormalizedName {role}");


                var users = new[]
                {
                    new IdentityUser { UserName = "mockuser", Email = "mock@test.com", EmailConfirmed = true },
                    new IdentityUser { UserName = "testuser", Email = "test@test.com", EmailConfirmed = true }
                };

                foreach (var user in users)
                {
                    var existingUser = await userManager.FindByNameAsync(user.UserName);
                    

                    if (existingUser == null)
                    {
                        var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                        if (result.Succeeded)
                        {
                            if (!await userManager.IsInRoleAsync(user, userRoleName))
                            {
                                await userManager.AddToRoleAsync(user, userRoleName);
                            }
                        }
                    }
                }

                db.Clients.AddRange(
                    new Client { Nom = "Smith", Prenom = "Alice", Email = "alice@test.com", Adresse = "Paris", Telephone = "0600000000" },
                    new Client { Nom = "Jones", Prenom = "Bob", Email = "bob@test.com", Adresse = "Lyon", Telephone = "0700000000" }
                );

                await db.SaveChangesAsync();

            }).GetAwaiter().GetResult(); 
        });
    }
}