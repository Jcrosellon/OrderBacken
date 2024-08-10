using OrderBackend.Models;

namespace OrderBackend.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? NIT { get; set; }
        public string? Password { get; set; }
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>(); // Inicialización para evitar null
    }

}
