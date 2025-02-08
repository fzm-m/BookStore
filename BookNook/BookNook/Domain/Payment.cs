using System.ComponentModel.DataAnnotations;

namespace BookNook.Domain
{
    public class Payment 
    {
        [Key]
        public string? Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? PaymentAmount { get; set; }    
        public string? OrderId { get; set; }

    }
}
