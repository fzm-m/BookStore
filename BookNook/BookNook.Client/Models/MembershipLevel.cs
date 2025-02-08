using System.ComponentModel.DataAnnotations;

namespace BookNook.Client.Models
{
    public class MembershipLevel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "required")]
        public string MembershipLevelName { get; set; }
        [Required(ErrorMessage = "required")]
        [Range(0.01, 0.99, ErrorMessage = "Discount percentage must be between 0.01 and 0.99.")]
        public decimal DiscountRate { get; set; }
        public int IsDelete { get; set; }
     
    }
}
