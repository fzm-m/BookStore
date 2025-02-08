using System.Reflection.Emit;
using BookNook.Configurations.Entities;
using BookNook.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookNook.Data
{
    public class BookNookDbContext : IdentityDbContext<BookNookUser, BookNookRole,string>
    {
        public BookNookDbContext(DbContextOptions<BookNookDbContext> options)
       : base(options)
        {
        }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<MembershipLevel> MembershipLevels { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Review> Reviews { get; set; }
      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleSeed());
            builder.ApplyConfiguration(new UserRoleSeed());
            builder.ApplyConfiguration(new UserSeed());

           
        }
    }
}
