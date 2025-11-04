using System.Runtime.Serialization;

namespace Api.Tests;


public class CustomAuthFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostbuilder builder)
    {
        builder.ConfigureServices(FormatterServices =>
        {
            var descriptor = FormatterServices.SingleOrDefault(
                d => d.ServiceType == typeof(DBContextOptions<AppDbContext>)
            );
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("AuthTestDb"));
        });
    }

}