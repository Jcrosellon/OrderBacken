using System.Threading.Tasks;
using OrderBackend.Models;

namespace OrderBackend.Services
{
    public interface IOrderService
    {
        Task<Pedido?> GetPedidoByNITAsync(string nit);
    }
}
