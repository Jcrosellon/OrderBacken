using OrderBackend.Models;

namespace OrderBackend.Models
{
    public class Cliente
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? NIT { get; set; }
    public string? Password { get; set; }
    public ICollection<Pedido>?Pedidos { get; set; }  // Relación uno a muchos
}

}
