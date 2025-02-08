using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookNook.Data;

namespace BookNook.Domain
{
    public class Order
    {
        [Key]
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? RecipientName { get; set; }
        public string? RecipientPhone { get; set; }
        public string? RecipientAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public string? PaymentId { get; set; }
        public string? ShipmentId { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public string? ShipmentStatus { get; set; }
    }
}
