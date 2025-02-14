using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jumpin.Migrations
{
    /// <inheritdoc />
    public partial class PhoneConfirmedproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PhoneConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneConfirmed",
                table: "Users");
        }
    }
}
