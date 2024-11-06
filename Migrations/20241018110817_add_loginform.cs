
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class add_loginform : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
               migrationBuilder.AddColumn<string>(
               name: "Discriminator",
               table: "Users", // Hoặc "AspNetUsers"
               type: "nvarchar(21)",
               maxLength: 21,
               nullable: false,
               defaultValue: ""); // Cung cấp giá trị mặc định nếu cần
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "Discriminator",
            table: "Users"); 
        }
    }
}
