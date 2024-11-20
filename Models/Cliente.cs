using OrderBackend.Models;

namespace OrderBackend.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? NIT { get; set; }
        public string? Password { get; set; }
        public ICollection<Pedido> ClientesEstadosPedidosWebDetalles { get; set; } =
            new List<Pedido>();
    }
}
