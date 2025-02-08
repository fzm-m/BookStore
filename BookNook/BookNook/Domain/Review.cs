using System.ComponentModel.DataAnnotations.Schema;
using BookNook.Data;

namespace BookNook.Domain
{
    public class Review : BaseDomainModel
    {
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public int ItemId { get; set; }

        [NotMapped]
        public string? image { get; set; }
        [NotMapped]
        public string? ItemName { get; set; }
        [NotMapped]
        public string? Description { get; set; }
        [NotMapped]
        public decimal Price { get; set; }

    }
}
