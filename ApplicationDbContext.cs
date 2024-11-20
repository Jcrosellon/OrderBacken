using Microsoft.EntityFrameworkCore;
using OrderBackend.Models;

namespace OrderBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> ClientesEstadosPedidosWeb { get; set; }
        public DbSet<Pedido> ClientesEstadosPedidosWebDetalles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración para la columna Total en la entidad Pedido
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasColumnType("decimal(18,2)"); // Ajusta la precisión y escala según sea necesario

            // Configuración de la relación entre Pedido y Cliente
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.ClientesEstadosPedidosWebDetalles) // Actualiza aquí también
                .HasForeignKey(p => p.ClienteId)
                .HasConstraintName("FK_Pedidos_Clientes");
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
