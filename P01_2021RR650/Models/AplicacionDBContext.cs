using Microsoft.EntityFrameworkCore;

namespace P01_2021RR650.Models
{
    public class AplicacionDBContext : DbContext
    {
        public AplicacionDBContext(DbContextOptions<AplicacionDBContext> options) : base(options) 
        { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Reserva>().ToTable("Reservas");
            modelBuilder.Entity<Sucursal>().ToTable("Sucursales");
            modelBuilder.Entity<Parqueo>().ToTable("Parqueos");
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Parqueo> Parqueos { get; set; }
    }
}
