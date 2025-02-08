using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "required")]
        public string RecipientName { get; set; }
        [Required(ErrorMessage = "required")]
        public string RecipientPhone { get; set; }
        [Required(ErrorMessage = "required")]
        public string RecipientAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
       
        public string? PaymentId { get; set; }
        public string? ShipmentId { get; set; }
        public string? UserName { get; set; }
        public string? ShipmentStatus { get; set; }
    }
}
