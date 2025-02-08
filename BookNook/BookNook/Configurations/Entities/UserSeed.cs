using BookNook.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookNook.Configurations.Entities
{
    public class UserSeed : IEntityTypeConfiguration<BookNookUser>
    {
        public void Configure(EntityTypeBuilder<BookNookUser> builder)
        {
            var hasher = new PasswordHasher<BookNookUser>();
            builder.HasData(
                new BookNookUser
                {
                    Id = "3781efa7-66dc-47f0-860f-e506d04102e4",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    FirstName = "Admin",
                    LastName = "User",
                    UserName = "admin@localhost.com",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"),
                    EmailConfirmed = true // Set to true, otherwise you won't be able to login 
                }
                );
        }
    }
}
