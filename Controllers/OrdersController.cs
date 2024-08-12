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
        public IActionResult GetOrders()
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

            var pedidos = _context.Pedidos.Where(p => p.ClienteId == cliente.Id).ToList();

            return Ok(pedidos);
        }
    }
}
