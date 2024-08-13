using System;

namespace OrderBackend.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? PreparingDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string? Status { get; set; }
        public decimal Total { get; set; }
        public int ClienteId { get; set; } // Asegúrate de que sea ClienteId
        public Cliente? Cliente { get; set; } // Relación con Cliente
    }

    public static class PedidoStatus
    {
        public const string PedidoRealizado = "Pedido realizado";
        public const string Preparando = "Estamos preparando tu pedido";
        public const string Despachado = "Tu pedido fue despachado";
        public const string Entregado = "Tu pedido fue entregado";
    }
}
