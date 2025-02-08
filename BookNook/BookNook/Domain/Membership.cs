using BookNook.Data;

namespace BookNook.Domain
{
    public class Membership : BaseDomainModel
    {
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }

        public int MembershipLevelId { get; set; }


    }
}
