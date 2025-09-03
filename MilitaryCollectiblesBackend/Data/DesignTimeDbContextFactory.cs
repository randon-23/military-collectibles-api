using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MilitaryCollectiblesBackend.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MilitaryCollectiblesDbContext>
    {
        public MilitaryCollectiblesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MilitaryCollectiblesDbContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new MilitaryCollectiblesDbContext(optionsBuilder.Options);
        }
    }
}
