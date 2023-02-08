using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tAgent_LotteryActivity_LotteryActivityId",
                table: "tAgent");

            migrationBuilder.DropColumn(
                name: "IsClaimed",
                table: "LotteryRecord");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "ActivityPrize");

            migrationBuilder.AlterColumn<int>(
                name: "LotteryActivityId",
                table: "tAgent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Claimed",
                table: "LotteryRecord",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OpenId",
                table: "LotteryRecord",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "LotteryActivity",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "LotteryActivity",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateTable(
                name: "RedPacketRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedPacketRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedPackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActivity = table.Column<bool>(type: "bit", nullable: false),
                    RedPacketType = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<int>(type: "int", nullable: false),
                    MinCashValue = table.Column<int>(type: "int", nullable: false),
                    MaxCashValue = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedPackets", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_tAgent_LotteryActivity_LotteryActivityId",
                table: "tAgent",
                column: "LotteryActivityId",
                principalTable: "LotteryActivity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tAgent_LotteryActivity_LotteryActivityId",
                table: "tAgent");

            migrationBuilder.DropTable(
                name: "RedPacketRecords");

            migrationBuilder.DropTable(
                name: "RedPackets");

            migrationBuilder.DropColumn(
                name: "Claimed",
                table: "LotteryRecord");

            migrationBuilder.DropColumn(
                name: "OpenId",
                table: "LotteryRecord");

            migrationBuilder.AlterColumn<int>(
                name: "LotteryActivityId",
                table: "tAgent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimed",
                table: "LotteryRecord",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "LotteryActivity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "LotteryActivity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Identifier",
                table: "ActivityPrize",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tAgent_LotteryActivity_LotteryActivityId",
                table: "tAgent",
                column: "LotteryActivityId",
                principalTable: "LotteryActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
