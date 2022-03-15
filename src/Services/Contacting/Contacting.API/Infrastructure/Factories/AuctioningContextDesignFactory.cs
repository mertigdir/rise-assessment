using Contacting.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contacting.API.Infrastructure.Factories
{
    public class ContactingContextDesignFactory : IDesignTimeDbContextFactory<ContactingContext>
    {
        public ContactingContext CreateDbContext(params string[] args)
        {
            var options = new DbContextOptionsBuilder<ContactingContext>();
            var config = GetAppConfiguration();

            options.UseNpgsql(config["ConnectionString"], options => options.MigrationsAssembly(GetType().Assembly.GetName().Name)).UseSnakeCaseNamingConvention();

            return new ContactingContext(options.Options);
        }
        IConfiguration GetAppConfiguration()
        {
            var environmentName =
                      Environment.GetEnvironmentVariable(
                          "ASPNETCORE_ENVIRONMENT");

            var dir = Directory.GetParent(AppContext.BaseDirectory);

            if (EnvironmentName.Development.Equals(environmentName,
                StringComparison.OrdinalIgnoreCase))
            {
                var depth = 0;
                do
                    dir = dir.Parent;
                while (++depth < 5 && dir.Name != "bin");
                dir = dir.Parent;
            }

            var path = dir.FullName;

            var builder = new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json", true)
                    .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
