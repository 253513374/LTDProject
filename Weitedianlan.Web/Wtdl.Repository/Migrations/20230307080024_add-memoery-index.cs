using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addmemoeryindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 548, DateTimeKind.Local).AddTicks(9274),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 5, 21, 9, 17, 659, DateTimeKind.Local).AddTicks(4271));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 549, DateTimeKind.Local).AddTicks(2434),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 5, 21, 9, 17, 659, DateTimeKind.Local).AddTicks(7740));

            migrationBuilder.CreateIndex(
                name: "IX_W_LabelStorage_CreateTime",
                table: "W_LabelStorage",
                column: "CreateTime")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.CreateIndex(
                name: "IX_W_LabelStorage_ID",
                table: "W_LabelStorage",
                column: "ID")
                .Annotation("SqlServer:Clustered", false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_W_LabelStorage_CreateTime",
                table: "W_LabelStorage");

            migrationBuilder.DropIndex(
                name: "IX_W_LabelStorage_ID",
                table: "W_LabelStorage");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "W_LabelStorage",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 3, 5, 21, 9, 17, 659, DateTimeKind.Local).AddTicks(4271),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 548, DateTimeKind.Local).AddTicks(9274));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "tUser",
                type: "datetime2",
                nullable: true,
                defaultValue: new DateTime(2023, 3, 5, 21, 9, 17, 659, DateTimeKind.Local).AddTicks(7740),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValue: new DateTime(2023, 3, 7, 16, 0, 24, 549, DateTimeKind.Local).AddTicks(2434));
        }
    }
}