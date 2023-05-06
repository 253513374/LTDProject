using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScanCode.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "UserAwardInfos",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 17, 29, 16, 827, DateTimeKind.Local).AddTicks(6011),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 16, 20, 50, 293, DateTimeKind.Local).AddTicks(3436));

            migrationBuilder.CreateIndex(
                name: "IX_UserAwardInfos_QrCode",
                table: "UserAwardInfos",
                column: "QrCode",
                unique: true,
                filter: "[QrCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAwardInfos_WeChatOpenId",
                table: "UserAwardInfos",
                column: "WeChatOpenId",
                unique: true,
                filter: "[WeChatOpenId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAwardInfos_QrCode",
                table: "UserAwardInfos");

            migrationBuilder.DropIndex(
                name: "IX_UserAwardInfos_WeChatOpenId",
                table: "UserAwardInfos");

            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 16, 20, 50, 293, DateTimeKind.Local).AddTicks(3436),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 17, 29, 16, 827, DateTimeKind.Local).AddTicks(6011));
        }
    }
}
