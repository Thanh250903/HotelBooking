using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelApp.Migrations
{
    public partial class somethingchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Chỉ xóa nếu các cột này tồn tại
            if (migrationBuilder.ActiveProvider == "Microsoft.EntityFrameworkCore.SqlServer")
            {
                migrationBuilder.Sql(@"
                    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Payments' AND COLUMN_NAME = 'PaymentResponse')
                    BEGIN
                        ALTER TABLE Payments DROP COLUMN PaymentResponse
                    END
                ");

                migrationBuilder.Sql(@"
                    IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Payments' AND COLUMN_NAME = 'TransactionId')
                    BEGIN
                        ALTER TABLE Payments DROP COLUMN TransactionId
                    END
                ");
            }

            migrationBuilder.AlterColumn<double>(
                name: "TotalPrice",
                table: "Payments",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TotalPrice",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "PaymentResponse",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
