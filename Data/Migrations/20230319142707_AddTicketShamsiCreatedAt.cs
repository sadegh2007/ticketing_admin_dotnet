using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Ticketing.HttpApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketShamsiCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShamsiCreatedAt",
                table: "Tickets",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShamsiCreatedAt",
                table: "TicketComments",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShamsiCreatedAt",
                table: "Departments",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShamsiCreatedAt",
                table: "Categories",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ShamsiCreatedAt",
                table: "Tickets",
                column: "ShamsiCreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_ShamsiCreatedAt",
                table: "TicketComments",
                column: "ShamsiCreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ShamsiCreatedAt",
                table: "Departments",
                column: "ShamsiCreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ShamsiCreatedAt",
                table: "Categories",
                column: "ShamsiCreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tickets_ShamsiCreatedAt",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_ShamsiCreatedAt",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ShamsiCreatedAt",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ShamsiCreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ShamsiCreatedAt",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ShamsiCreatedAt",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "ShamsiCreatedAt",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ShamsiCreatedAt",
                table: "Categories");
        }
    }
}
