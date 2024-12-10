using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class add_more_entities_for_HotelReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelReviews_Hotels_HotelId",
                table: "HotelReviews");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "HotelReviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "HotelReviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "HotelReviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HotelReviews_UserId",
                table: "HotelReviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelReviews_Hotels_HotelId",
                table: "HotelReviews",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HotelReviews_Users_UserId",
                table: "HotelReviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelReviews_Hotels_HotelId",
                table: "HotelReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_HotelReviews_Users_UserId",
                table: "HotelReviews");

            migrationBuilder.DropIndex(
                name: "IX_HotelReviews_UserId",
                table: "HotelReviews");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "HotelReviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "HotelReviews");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "HotelReviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelReviews_Hotels_HotelId",
                table: "HotelReviews",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "HotelId");
        }
    }
}
