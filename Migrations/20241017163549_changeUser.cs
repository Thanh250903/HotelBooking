using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class changeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_Users_UserId",
                table: "RoomBookings");

            migrationBuilder.DropIndex(
                name: "IX_RoomBookings_UserId",
                table: "RoomBookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoomBookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUser",
                table: "RoomBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_ApplicationUser",
                table: "RoomBookings",
                column: "ApplicationUser");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_Users_ApplicationUser",
                table: "RoomBookings",
                column: "ApplicationUser",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomBookings_Users_ApplicationUser",
                table: "RoomBookings");

            migrationBuilder.DropIndex(
                name: "IX_RoomBookings_ApplicationUser",
                table: "RoomBookings");

            migrationBuilder.DropColumn(
                name: "ApplicationUser",
                table: "RoomBookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoomBookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_UserId",
                table: "RoomBookings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomBookings_Users_UserId",
                table: "RoomBookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
