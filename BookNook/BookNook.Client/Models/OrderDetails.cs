using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Client.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string? OrderId { get; set; }

        public string? image { get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }

        public decimal Price { get; set; }
        public string? Comment { get; set; }
    }
}
