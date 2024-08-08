using OrderBackend.Models;

namespace OrderBackend.Models
{

    public class Pedido
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Status { get; set; }
    public bool PedidoRealizado { get; set; }
    public bool EstamosPreparandoTuPedido { get; set; }
    public bool TuPedidoFueDespachado { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }  // Propiedad de navegación
}

}