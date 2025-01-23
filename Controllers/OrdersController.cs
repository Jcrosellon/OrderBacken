using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderBackend.Data;
using OrderBackend.Models;

namespace OrderBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(
            int page = 1,
            int pageSize = 4,
            string? dateFilter = null,
            string? searchTerm = null,
            int? orderId = null // Parámetro explícito para buscar por número de pedido
        )
        {
            // Obtener el NIT del token
            var nit = User.Claims.FirstOrDefault(c => c.Type == "nit")?.Value;
            if (nit == null)
            {
                return Unauthorized("NIT not found in the token");
            }

            // Validar si el cliente existe
            var cliente = await _context.ClientesEstadosPedidosWeb.FirstOrDefaultAsync(c =>
                c.NIT == nit
            );
            if (cliente == null)
            {
                return NotFound("Client not found.");
            }

            IQueryable<Pedido> pedidosQuery;

            // Si se proporciona un orderId, buscar únicamente ese pedido
            if (orderId.HasValue)
            {
                pedidosQuery = _context.ClientesEstadosPedidosWebDetalles.Where(p =>
                    p.Id == orderId && p.ClienteId == cliente.Id
                );

                var pedido = await pedidosQuery.SingleOrDefaultAsync();

                if (pedido == null)
                {
                    return NotFound($"Order with ID {orderId} not found.");
                }

                return Ok(new { Total = 1, Pedidos = new List<Pedido> { pedido } });
            }

            // Construir la consulta base para el cliente
            pedidosQuery = _context.ClientesEstadosPedidosWebDetalles.Where(p =>
                p.ClienteId == cliente.Id
            );

            // Filtrar por fecha
            if (!string.IsNullOrEmpty(dateFilter))
            {
                var now = DateTime.UtcNow;
                pedidosQuery = dateFilter switch
                {
                    "last-3-months" => pedidosQuery.Where(p => p.Date >= now.AddMonths(-3)),
                    "last-6-months" => pedidosQuery.Where(p => p.Date >= now.AddMonths(-6)),
                    "last-12-months" => pedidosQuery.Where(p => p.Date >= now.AddMonths(-12)),
                    _ => pedidosQuery
                };
            }

            // Filtrar por término de búsqueda
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                if (searchTerm.Length >= 4 && int.TryParse(searchTerm, out int searchId))
                {
                    pedidosQuery = pedidosQuery.Where(p => p.Id.ToString().Contains(searchTerm));
                }
            }

            // Obtener total y los pedidos paginados
            var totalPedidos = await pedidosQuery.CountAsync();
            var pedidos = await pedidosQuery
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new { Total = totalPedidos, Pedidos = pedidos });
        }
    }
}
