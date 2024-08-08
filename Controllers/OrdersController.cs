// Para manejar la recuperación de pedidos basados en la identidad del cliente autenticado.


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderBackend.Data;
using System.Linq;
using System.Security.Claims;

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
            // Obtener el NIT desde el token
            var nit = User.FindFirstValue("nit");

            if (string.IsNullOrEmpty(nit))
            {
                return Unauthorized("NIT not found in the token.");
            }

            // Filtrar los pedidos por el NIT
            var orders = _context.Pedidos.Where(p => p.Cliente!.NIT == nit).ToList();

            return Ok(orders);
        }
    }
}


