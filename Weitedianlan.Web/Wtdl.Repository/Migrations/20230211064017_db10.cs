using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Prize");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubscribe",
                table: "ScanRedPackets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubscribe",
                table: "ScanRedPackets");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Prize",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
