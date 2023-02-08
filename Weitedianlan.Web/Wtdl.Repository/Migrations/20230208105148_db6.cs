using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RedPackets",
                table: "RedPackets");

            migrationBuilder.RenameTable(
                name: "RedPackets",
                newName: "ScanRedPackets");

            migrationBuilder.AddColumn<string>(
                name: "ScanRedPacketGuid",
                table: "ScanRedPackets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ScanRedPackets",
                table: "ScanRedPackets",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ScanRedPackets",
                table: "ScanRedPackets");

            migrationBuilder.DropColumn(
                name: "ScanRedPacketGuid",
                table: "ScanRedPackets");

            migrationBuilder.RenameTable(
                name: "ScanRedPackets",
                newName: "RedPackets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RedPackets",
                table: "RedPackets",
                column: "Id");
        }
    }
}
