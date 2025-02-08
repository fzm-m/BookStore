using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "required")]
        public string? PromoCode { get; set; }
        [Required(ErrorMessage = "required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "required")]
        [Range(0.01, 0.99, ErrorMessage = "Discount percentage must be between 0.01 and 0.99.")]
        public decimal DiscountPercentage { get; set; }
        public int IsDelete { get; set; }

    }
}
