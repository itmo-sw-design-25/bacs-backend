using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaCS.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class User__ImageUrl_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "users");
        }
    }
}
