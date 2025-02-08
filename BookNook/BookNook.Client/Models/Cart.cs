

using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Client.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string? UserId { get; set; }
        public int ItemId { get; set; }
        public int ItemNumber { get; set; }
        public string? ImagePath { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public bool IsSelected { get; set; }
        public decimal DiscountedPrice { get; set; }
    }
}
