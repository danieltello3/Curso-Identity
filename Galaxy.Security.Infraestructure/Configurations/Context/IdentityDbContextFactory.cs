using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Galaxy.Security.Infraestructure.Configurations.Context
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("SecurityDb");

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();

            optionsBuilder.UseNpgsql(connectionString,
                opt => opt.MigrationsHistoryTable("__EFMigrationHistory"));
            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}
