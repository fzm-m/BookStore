using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class Payment
    {
        public string Id { get; set; }
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "required")]
        public string? PaymentMethod { get; set; }
        [Required(ErrorMessage = "required")]
        public string? PaymentAmount { get; set; }    
        public string? OrderId { get; set; }

    }
}
