using GestionInmobiliariaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionInmobiliariaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Propiedad> Propiedades { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Propiedad>()
                .Property(p => p.Precio)
                .HasPrecision(18, 2);
        }
    }
}