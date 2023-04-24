using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "UserAwardInfos");

            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityNumber",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrizeNumber",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrizeType",
                table: "UserAwardInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 24, 22, 59, 34, 40, DateTimeKind.Local).AddTicks(2724),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 2, 14, 17, 27, 911, DateTimeKind.Local).AddTicks(3387));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "UserAwardInfos");

            migrationBuilder.DropColumn(
                name: "ActivityNumber",
                table: "UserAwardInfos");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "UserAwardInfos");

            migrationBuilder.DropColumn(
                name: "PrizeNumber",
                table: "UserAwardInfos");

            migrationBuilder.DropColumn(
                name: "PrizeType",
                table: "UserAwardInfos");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "UserAwardInfos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 4, 2, 14, 17, 27, 911, DateTimeKind.Local).AddTicks(3387),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 4, 24, 22, 59, 34, 40, DateTimeKind.Local).AddTicks(2724));
        }
    }
}
