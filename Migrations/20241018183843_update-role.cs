using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    /// <inheritdoc />
    public partial class updaterole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa chính UserTokens
            migrationBuilder.DropPrimaryKey("PK_UserTokens", "UserTokens");

            // Thay đổi cột Name trong UserTokens
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Thay đổi cột LoginProvider trong UserTokens
            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Thêm lại khóa chính UserTokens
            migrationBuilder.AddPrimaryKey("PK_UserTokens", "UserTokens", new[] { "Name", "LoginProvider" });

            // Xóa khóa chính UserLogins
            migrationBuilder.DropPrimaryKey("PK_UserLogins", "UserLogins");

            // Thay đổi cột ProviderKey trong UserLogins
            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Thay đổi cột LoginProvider trong UserLogins
            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Thêm lại khóa chính UserLogins
            migrationBuilder.AddPrimaryKey("PK_UserLogins", "UserLogins", new[] { "LoginProvider", "ProviderKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa khóa chính UserTokens
            migrationBuilder.DropPrimaryKey("PK_UserTokens", "UserTokens");

            // Đặt lại cột Name trong UserTokens
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Đặt lại cột LoginProvider trong UserTokens
            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Thêm lại khóa chính UserTokens
            migrationBuilder.AddPrimaryKey("PK_UserTokens", "UserTokens", new[] { "Name", "LoginProvider"});

            // Xóa khóa chính UserLogins
            migrationBuilder.DropPrimaryKey("PK_UserLogins", "UserLogins");

            // Đặt lại cột ProviderKey trong UserLogins
            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "UserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Đặt lại cột LoginProvider trong UserLogins
            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "UserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            // Thêm lại khóa chính cũ UserLogins
            migrationBuilder.AddPrimaryKey("PK_UserLogins", "UserLogins", new[] { "LoginProvider", "ProviderKey" });
        }
    }
}
