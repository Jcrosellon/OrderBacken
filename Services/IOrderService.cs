// Para encapsular la lógica de recuperación de pedidos.

using System.Threading.Tasks;
using OrderBackend.Models;

namespace OrderBackend.Services
{
    public interface IOrderService
    {
        Task<Pedido?> GetPedidoByNITAsync(string nit); // Cambiar Task<Pedido> a Task<Pedido?>
    }
}

