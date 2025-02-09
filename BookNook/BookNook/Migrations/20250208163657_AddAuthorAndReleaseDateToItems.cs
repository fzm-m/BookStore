using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookNook.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorAndReleaseDateToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Items",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3781efa7-66dc-47f0-860f-e506d04102e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bfcbbecd-558b-4814-8e44-250a574a0fd5", "AQAAAAIAAYagAAAAEJGolGXkL3H/0Uut7JlHJwhOhrYTUikuo51M7p/ENyG38JH0IrgaFpBiAI//BwTZmA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Items");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3781efa7-66dc-47f0-860f-e506d04102e4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a3422a83-0a37-4226-93d6-21fff7daddd0", "AQAAAAIAAYagAAAAEKwVFtuy2ZqnKLz+KMcLjuvmPcnWn5n6g80H6CgueVtvFc0BfXQCyIwqjzuWDonqNQ==" });
        }
    }
}
