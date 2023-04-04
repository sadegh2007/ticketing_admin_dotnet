using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Ticketing.HttpApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToTicketComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TicketComments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedById",
                table: "TicketComments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DeletedAt",
                table: "Tickets",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_DeletedAt",
                table: "TicketComments",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketComments_DeletedById",
                table: "TicketComments",
                column: "DeletedById");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketComments_Users_DeletedById",
                table: "TicketComments",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketComments_Users_DeletedById",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_DeletedAt",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_DeletedAt",
                table: "TicketComments");

            migrationBuilder.DropIndex(
                name: "IX_TicketComments_DeletedById",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TicketComments");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "TicketComments");
        }
    }
}
