using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaCS.Persistence.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class User__EnableEmailNotifications_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "enable_email_notifications",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enable_email_notifications",
                table: "users");
        }
    }
}
