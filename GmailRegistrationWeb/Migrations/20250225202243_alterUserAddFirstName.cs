using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GmailRegistrationWeb.Migrations
{
    /// <inheritdoc />
    public partial class alterUserAddFirstName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "users",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "users");
        }
    }
}
