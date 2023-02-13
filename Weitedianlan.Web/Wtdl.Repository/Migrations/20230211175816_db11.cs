using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VerificationCodes_AntiForgeryCode",
                table: "VerificationCodes");

            migrationBuilder.RenameColumn(
                name: "VCode",
                table: "VerificationCodes",
                newName: "Captcha");

            migrationBuilder.RenameColumn(
                name: "AntiForgeryCode",
                table: "VerificationCodes",
                newName: "QRCode");

            migrationBuilder.RenameColumn(
                name: "VerificationCode",
                table: "RedPacketRecords",
                newName: "Captcha");

            migrationBuilder.AddColumn<DateTime>(
                name: "IssueTime",
                table: "RedPacketRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveTime",
                table: "RedPacketRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_QRCode",
                table: "VerificationCodes",
                column: "QRCode",
                unique: true,
                filter: "[QRCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VerificationCodes_QRCode",
                table: "VerificationCodes");

            migrationBuilder.DropColumn(
                name: "IssueTime",
                table: "RedPacketRecords");

            migrationBuilder.DropColumn(
                name: "ReceiveTime",
                table: "RedPacketRecords");

            migrationBuilder.RenameColumn(
                name: "QRCode",
                table: "VerificationCodes",
                newName: "AntiForgeryCode");

            migrationBuilder.RenameColumn(
                name: "Captcha",
                table: "VerificationCodes",
                newName: "VCode");

            migrationBuilder.RenameColumn(
                name: "Captcha",
                table: "RedPacketRecords",
                newName: "VerificationCode");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_AntiForgeryCode",
                table: "VerificationCodes",
                column: "AntiForgeryCode",
                unique: true,
                filter: "[AntiForgeryCode] IS NOT NULL");
        }
    }
}
