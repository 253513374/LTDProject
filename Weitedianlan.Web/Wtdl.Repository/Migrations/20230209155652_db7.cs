using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize");

            migrationBuilder.DropIndex(
                name: "IX_Prize_LotteryActivityId",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "LotteryActivityId",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "MaxCashValue",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "MinCashValue",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "TotalLimit",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "UserLimit",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "WinnerCount",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "PrizeId",
                table: "LotteryRecord");

            migrationBuilder.DropColumn(
                name: "MaxCashValue",
                table: "ActivityPrize");

            migrationBuilder.DropColumn(
                name: "MinCashValue",
                table: "ActivityPrize");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "LotteryRecord",
                newName: "PrizeNumber");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "LotteryRecord",
                newName: "ActivityNumber");

            migrationBuilder.AlterColumn<int>(
                name: "CashValue",
                table: "Prize",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "UniqueNumber",
                table: "Prize",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CashValue",
                table: "LotteryRecord",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ActivityDescription",
                table: "LotteryRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityNumber",
                table: "LotteryActivity",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CashValue",
                table: "ActivityPrize",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "PrizeNumber",
                table: "ActivityPrize",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniqueNumber",
                table: "ActivityPrize",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniqueNumber",
                table: "Prize");

            migrationBuilder.DropColumn(
                name: "ActivityDescription",
                table: "LotteryRecord");

            migrationBuilder.DropColumn(
                name: "ActivityNumber",
                table: "LotteryActivity");

            migrationBuilder.DropColumn(
                name: "PrizeNumber",
                table: "ActivityPrize");

            migrationBuilder.DropColumn(
                name: "UniqueNumber",
                table: "ActivityPrize");

            migrationBuilder.RenameColumn(
                name: "PrizeNumber",
                table: "LotteryRecord",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ActivityNumber",
                table: "LotteryRecord",
                newName: "ActivityId");

            migrationBuilder.AlterColumn<decimal>(
                name: "CashValue",
                table: "Prize",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Prize",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LotteryActivityId",
                table: "Prize",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxCashValue",
                table: "Prize",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinCashValue",
                table: "Prize",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "Prize",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLimit",
                table: "Prize",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserLimit",
                table: "Prize",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinnerCount",
                table: "Prize",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "CashValue",
                table: "LotteryRecord",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PrizeId",
                table: "LotteryRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "CashValue",
                table: "ActivityPrize",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "MaxCashValue",
                table: "ActivityPrize",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinCashValue",
                table: "ActivityPrize",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Prize_LotteryActivityId",
                table: "Prize",
                column: "LotteryActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize",
                column: "LotteryActivityId",
                principalTable: "LotteryActivity",
                principalColumn: "Id");
        }
    }
}
