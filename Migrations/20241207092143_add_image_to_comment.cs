using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class add_image_to_comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "HotelReviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "HotelReviews");
        }
    }
}
