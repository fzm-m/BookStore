using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookNook.Client.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
        [Required(ErrorMessage = "required")]
        [Range(0, 10, ErrorMessage = "The rating must be a positive integer between 0 and 10")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "required")]
        public string? Comment { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public int ItemId { get; set; }


        public string? image { get; set; }

        public string? ItemName { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
