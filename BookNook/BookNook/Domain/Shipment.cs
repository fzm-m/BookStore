using System.ComponentModel.DataAnnotations;

namespace BookNook.Domain
{
    public class Shipment 
    {
        [Key]
        public string? Id { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Status { get; set; }
        public DateTime ShipmentDate { get; set; }
       
        public string? OrderId { get; set; }

    }
}
