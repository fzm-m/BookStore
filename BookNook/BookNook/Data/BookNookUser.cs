using BookNook.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookNook.Data
{
    public class BookNookUser:IdentityUser<string>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<Cart> Carts { get; set; } = new List<Cart>(); 
            public List<Membership> Memberships { get; set; } = new List<Membership>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Review> Reviews { get; set; } = new List<Review>(); 
    }
}
