using Galaxy.Security.Domain.Entities;
using Galaxy.Security.Infraestructure.Configurations.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Galaxy.Security.Infraestructure.Configurations.Context
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : IdentityDbContext<UserExtension>(options)
    {

        public DbSet<Reclamo> Reclamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reclamo>()
                .Property(e => e.Fecha)
                .HasColumnType("timestamp without time zone");
            modelBuilder.Entity<Reclamo>()
                .HasIndex(e => e.Codigo)
                .IsUnique();
        }


    }
}
