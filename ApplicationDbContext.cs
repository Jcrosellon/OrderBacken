// Para asegurar que los modelos estén correctamente configurados.

using Microsoft.EntityFrameworkCore;
using OrderBackend.Models;

namespace OrderBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Cliente> ClientesEstadosPedidosWeb { get; set; }
        public DbSet<Pedido> ClientesEstadosPedidosWebDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>().Property(p => p.Total).HasColumnType("decimal(18,2)");

            modelBuilder
                .Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.ClientesEstadosPedidosWebDetalles)
                .HasForeignKey(p => p.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");

            base.OnModelCreating(modelBuilder);
        }
    }
}
