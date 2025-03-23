using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaCS.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class Images_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "resources",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "locations",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "resources");

            migrationBuilder.DropColumn(
                name: "image_url",
                table: "locations");
        }
    }
}
