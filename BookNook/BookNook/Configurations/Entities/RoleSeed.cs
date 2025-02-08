using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookNook.Data;

namespace BookNook.Configurations.Entities
{
    public class RoleSeed : IEntityTypeConfiguration<BookNookRole>
    {
        public void Configure(EntityTypeBuilder<BookNookRole> builder)
        {
            builder.HasData(
                new BookNookRole
                {
                    Id = "ad2bcf0c-20db-474f-8407-5a6b159518ba",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new BookNookRole
                {
                    Id = "bd2bcf0c-20db-474f-8407-5a6b159518bb",
                    Name = "User",
                    NormalizedName = "USER"
                }
                );
        }
    }
}
