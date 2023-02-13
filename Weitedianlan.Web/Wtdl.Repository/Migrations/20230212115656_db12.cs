using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActivityName",
                table: "ScanRedPackets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "ScanRedPackets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WishingWord",
                table: "ScanRedPackets",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivityName",
                table: "ScanRedPackets");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "ScanRedPackets");

            migrationBuilder.DropColumn(
                name: "WishingWord",
                table: "ScanRedPackets");
        }
    }
}
