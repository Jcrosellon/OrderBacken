using System;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; } // Relación con Cliente
    }
}
