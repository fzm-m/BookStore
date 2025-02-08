using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Domain
{
    public class OrderDetails : BaseDomainModel
    {
        
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderId { get; set; }

        [NotMapped]
        public string? image { get; set; }
        [NotMapped]
        public string? ItemName { get; set; }
        [NotMapped]
        public string? Description { get; set; }
        [NotMapped]
        public decimal Price { get; set; }
        [NotMapped]
        public string? Comment { get; set; }

    }
}
