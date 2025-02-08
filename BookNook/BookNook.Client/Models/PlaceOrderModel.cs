using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class PlaceOrderModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "required")]
        public string? RecipientName { get; set; }
        [Required(ErrorMessage = "required")]
        public string? RecipientPhone { get; set; }
        [Required(ErrorMessage = "required")]
        public string? RecipientAddress { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderDetails>? orderDetailsList  { get; set; }
 
 
        [Required(ErrorMessage = "required")]
        public string? PaymentMethod { get; set; }
        [Required(ErrorMessage = "required")]
        public string? PaymentAmount { get; set; }

        public string? TrackingNumber { get; set; }
        public string? Status { get; set; }
        public DateTime ShipmentDate { get; set; }
    }
}
