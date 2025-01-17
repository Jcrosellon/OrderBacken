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
            string? searchTerm = null
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

            IQueryable<Pedido> pedidosQuery = _context.ClientesEstadosPedidosWebDetalles.Where(P =>
                P.ClienteId == cliente.Id
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

            // Filtrar por número de pedido solo si el termino tiene 6 dijitos
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();

                if (searchTerm.Length == 6 && int.TryParse(searchTerm, out int searchId))
                {
                    pedidosQuery = pedidosQuery.Where(p => p.Id == searchId);
                }
                else
                {
                    return BadRequest(
                        "El número de pedido debe tener exactamente 6 dijistos numericos"
                    );
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
