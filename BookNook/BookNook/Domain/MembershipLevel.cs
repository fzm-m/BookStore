namespace BookNook.Domain
{
    public class MembershipLevel : BaseDomainModel
    {
        public string MembershipLevelName { get; set; }
        public decimal DiscountRate { get; set; }
        public int IsDelete { get; set; }

    }
}
