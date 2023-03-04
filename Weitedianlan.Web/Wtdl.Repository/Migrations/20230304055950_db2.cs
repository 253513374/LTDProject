using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 4, 13, 59, 50, 381, DateTimeKind.Local).AddTicks(6222),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 4, 13, 59, 50, 381, DateTimeKind.Local).AddTicks(9802),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 4, 13, 53, 2, 644, DateTimeKind.Local).AddTicks(202));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 4, 13, 59, 50, 381, DateTimeKind.Local).AddTicks(6222));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 4, 13, 53, 2, 644, DateTimeKind.Local).AddTicks(202),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 4, 13, 59, 50, 381, DateTimeKind.Local).AddTicks(9802));
        }
    }
}
