using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Domain
{
    public class Item : BaseDomainModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImagePath { get; set; }
        public int IsDelete { get; set; }
        public int ItemTypeId { get; set; }
        [NotMapped]
        public string? ItemTypeName { get; set; }
        public int PromotionId { get; set; }
        [NotMapped]
        public string? PromotionCode { get; set; }
        [NotMapped]
        public decimal DiscountedPrice { get; set; }
    }
}
