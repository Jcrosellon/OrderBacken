// Para asegurar que los modelos estén correctamente configurados.

using Microsoft.EntityFrameworkCore;
using OrderBackend.Models;

namespace OrderBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; } // Añadir esta línea
    }
}

