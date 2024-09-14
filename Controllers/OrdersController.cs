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
        public IActionResult GetOrders(
            int page = 1,
            int pageSize = 4,
            string? dateFilter = null,
            string? searchTerm = null
        )
        {
            var nit = User.Claims.FirstOrDefault(c => c.Type == "nit")?.Value;
            if (nit == null)
            {
                return Unauthorized("NIT not found in the token.");
            }

            var cliente = _context.Clientes.FirstOrDefault(c => c.NIT == nit);
            if (cliente == null)
            {
                return NotFound("Client not found.");
            }

            IQueryable<Pedido> pedidosQuery = _context.Pedidos.Where(p =>
                p.ClienteId == cliente.Id
            );

            // Filtrar por fecha
            if (!string.IsNullOrEmpty(dateFilter))
            {
                var now = DateTime.UtcNow;
                switch (dateFilter)
                {
                    case "last-3-months":
                        pedidosQuery = pedidosQuery.Where(p => p.Date >= now.AddMonths(-3));
                        break;
                    case "last-6-months":
                        pedidosQuery = pedidosQuery.Where(p => p.Date >= now.AddMonths(-6));
                        break;
                    case "last-12-months":
                        pedidosQuery = pedidosQuery.Where(p => p.Date >= now.AddMonths(-12));
                        break;
                }
            }

            // Filtrar por número de pedido o NIT (searchTerm)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                pedidosQuery = pedidosQuery.Where(p => p.Id.ToString().Contains(searchTerm));
            }

            var pedidos = pedidosQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(pedidos);
        }
    }
}
