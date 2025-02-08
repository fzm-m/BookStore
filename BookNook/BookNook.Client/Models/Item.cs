using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Client.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Please enter a valid number with up to two decimal places.")]
        [Range(0.01, float.MaxValue, ErrorMessage = "The price must be greater than 0.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "required")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Please enter a valid positive integer.")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "required")]
        public int ItemTypeId { get; set; }
     
        public string? ItemTypeName { get; set; }
        public int IsDelete { get; set; }

        public int PromotionId { get; set; }

        public string? ImagePath { get; set; }
        public string? PromotionCode { get; set; }
        public decimal DiscountedPrice { get; set; }


    }
}
