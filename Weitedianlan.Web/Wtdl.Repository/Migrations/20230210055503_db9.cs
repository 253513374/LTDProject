using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrizeNumber",
                table: "Prize",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrizeNumber",
                table: "Prize");
        }
    }
}
