namespace BookNook.Client.Models
{
    public class Membership
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Status { get; set; }
        public string UserId { get; set; }
        public int MembershipLevelId { get; set; }


    }
}
