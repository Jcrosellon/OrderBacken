// Para asegurar que los modelos estén correctamente configurados.

using Microsoft.EntityFrameworkCore;
using OrderBackend.Models;

namespace OrderBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
                .Property(p => p.ClienteId)
                .HasColumnName("ClienteId"); // Nombre de la columna en la base de datos

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");
        }

    }

}

