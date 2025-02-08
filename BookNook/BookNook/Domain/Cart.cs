using System.ComponentModel.DataAnnotations.Schema;
using BookNook.Data;

namespace BookNook.Domain
{
    public class Cart : BaseDomainModel
    {
        public DateTime DateAdded { get; set; }
        public string? UserId { get; set; }
        public int ItemId { get; set; }
        public int ItemNumber { get; set; }

        [NotMapped]
        public string? ImagePath { get; set; }
        [NotMapped]
        public string? Name { get; set; }
        [NotMapped]
        public string? Description { get; set; }
        [NotMapped]
        public decimal Price { get; set; }
        [NotMapped]
        public decimal DiscountedPrice { get; set; }
    }
}
