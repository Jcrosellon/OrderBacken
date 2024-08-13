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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(
            int id,
            [FromBody] UpdateOrderStatusRequest request
        )
        {
            // Obtener el pedido por ID
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
            {
                return NotFound("Order not found.");
            }

            // Actualizar el estado del pedido basado en la solicitud
            pedido.Status = request.Status;
            pedido.StatusDate = DateTime.UtcNow;

            // Actualizar las fechas según el estado
            switch (request.Status)
            {
                case "Pedido realizado":
                    // No se actualizan las fechas para este estado
                    break;
                case "Estamos preparando tu pedido":
                    pedido.PreparingDate = DateTime.UtcNow;
                    pedido.ShippedDate = null;
                    pedido.DeliveredDate = null;
                    break;
                case "Tu pedido fue despachado":
                    pedido.ShippedDate = DateTime.UtcNow;
                    pedido.DeliveredDate = null;
                    break;
                case "Tu pedido fue entregado":
                    pedido.DeliveredDate = DateTime.UtcNow;
                    break;
                default:
                    return BadRequest("Invalid status.");
            }

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }
    }

    public class UpdateOrderStatusRequest
    {
        public string? Status { get; set; }
    }
}
