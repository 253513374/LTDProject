using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addredconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 26, 10, 36, 19, 163, DateTimeKind.Local).AddTicks(5624),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 25, 17, 29, 16, 827, DateTimeKind.Local).AddTicks(6011));

            migrationBuilder.AddColumn<int>(
                name: "RedPacketConfigType",
                table: "ScanRedPackets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedPacketConfigType",
                table: "ScanRedPackets");

            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "RedPacketRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 25, 17, 29, 16, 827, DateTimeKind.Local).AddTicks(6011),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 26, 10, 36, 19, 163, DateTimeKind.Local).AddTicks(5624));
        }
    }
}
