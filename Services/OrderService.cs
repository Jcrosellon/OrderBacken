// Para encapsular la lógica de recuperación de pedidos.


using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderBackend.Data;
using OrderBackend.Models;

namespace OrderBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido?> GetPedidoByNITAsync(string nit)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .SingleOrDefaultAsync(p => p.Cliente!.NIT == nit);

            if (pedido == null)
            {
                throw new PedidoNotFoundException($"Pedido for NIT {nit} not found.");
            }
            return pedido;
        }

        [Serializable]
        public class PedidoNotFoundException : Exception
        {
            public PedidoNotFoundException() { }

            public PedidoNotFoundException(string? message) : base(message) { }

            public PedidoNotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
        }
    }
}
