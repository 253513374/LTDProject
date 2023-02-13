using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenId",
                table: "RedPacketRecords",
                newName: "wxappid");

            migrationBuilder.AddColumn<string>(
                name: "mch_billno",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mch_id",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "re_openid",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "send_listid",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "total_amount",
                table: "RedPacketRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mch_billno",
                table: "RedPacketRecords");

            migrationBuilder.DropColumn(
                name: "mch_id",
                table: "RedPacketRecords");

            migrationBuilder.DropColumn(
                name: "re_openid",
                table: "RedPacketRecords");

            migrationBuilder.DropColumn(
                name: "send_listid",
                table: "RedPacketRecords");

            migrationBuilder.DropColumn(
                name: "total_amount",
                table: "RedPacketRecords");

            migrationBuilder.RenameColumn(
                name: "wxappid",
                table: "RedPacketRecords",
                newName: "OpenId");
        }
    }
}
