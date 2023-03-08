using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class adddb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 8, 16, 51, 26, 897, DateTimeKind.Local).AddTicks(5526),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 548, DateTimeKind.Local).AddTicks(9274));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 8, 16, 51, 26, 897, DateTimeKind.Local).AddTicks(9088),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 549, DateTimeKind.Local).AddTicks(2434));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 548, DateTimeKind.Local).AddTicks(9274),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 8, 16, 51, 26, 897, DateTimeKind.Local).AddTicks(5526));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 549, DateTimeKind.Local).AddTicks(2434),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 8, 16, 51, 26, 897, DateTimeKind.Local).AddTicks(9088));
        }
    }
}
