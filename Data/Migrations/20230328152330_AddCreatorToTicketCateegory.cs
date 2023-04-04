using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Ticketing.HttpApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorToTicketCateegory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "TicketCategories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TicketCategories_CreatorId",
                table: "TicketCategories",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketCategories_Users_CreatorId",
                table: "TicketCategories",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketCategories_Users_CreatorId",
                table: "TicketCategories");

            migrationBuilder.DropIndex(
                name: "IX_TicketCategories_CreatorId",
                table: "TicketCategories");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "TicketCategories");
        }
    }
}
