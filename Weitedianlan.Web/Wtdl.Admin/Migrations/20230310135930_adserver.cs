using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Admin.Migrations
{
    /// <inheritdoc />
    public partial class adserver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Identity",
                table: "WtdlRolePermission",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 10, 21, 59, 30, 432, DateTimeKind.Local).AddTicks(2585),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 2, 24, 21, 34, 8, 260, DateTimeKind.Local).AddTicks(3845));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Identity",
                table: "WtdlRolePermission",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 2, 24, 21, 34, 8, 260, DateTimeKind.Local).AddTicks(3845),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 10, 21, 59, 30, 432, DateTimeKind.Local).AddTicks(2585));
        }
    }
}
