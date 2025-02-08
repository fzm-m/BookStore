namespace BookNook.Domain
{
    public class Promotion : BaseDomainModel
    {
        public string? PromoCode { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int IsDelete { get; set; }


 
    }
}
