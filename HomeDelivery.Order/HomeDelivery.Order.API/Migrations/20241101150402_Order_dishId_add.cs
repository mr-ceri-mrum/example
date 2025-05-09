using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeDelivery.Order.API.Migrations
{
    /// <inheritdoc />
    public partial class Order_dishId_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DishId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DishId",
                table: "Orders",
                column: "DishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dishes_DishId",
                table: "Orders",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dishes_DishId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DishId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DishId",
                table: "Orders");
        }
    }
}
