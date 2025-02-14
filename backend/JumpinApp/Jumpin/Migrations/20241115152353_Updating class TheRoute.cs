using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jumpin.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingclassTheRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Routes");
        }
    }
}
