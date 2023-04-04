using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Ticketing.HttpApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteToPermissionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Permissions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Route",
                table: "Permissions");
        }
    }
}
