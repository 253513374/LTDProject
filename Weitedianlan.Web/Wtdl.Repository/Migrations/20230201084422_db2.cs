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
            migrationBuilder.DropForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize");

            migrationBuilder.AlterColumn<int>(
                name: "LotteryActivityId",
                table: "Prize",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Participant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    POutOrder = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActivityParticipant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    ParticipantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityParticipant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityParticipant_LotteryActivity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "LotteryActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityParticipant_Participant_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityParticipant_ActivityId",
                table: "ActivityParticipant",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityParticipant_ParticipantId",
                table: "ActivityParticipant",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize",
                column: "LotteryActivityId",
                principalTable: "LotteryActivity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize");

            migrationBuilder.DropTable(
                name: "ActivityParticipant");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.AlterColumn<int>(
                name: "LotteryActivityId",
                table: "Prize",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Prize_LotteryActivity_LotteryActivityId",
                table: "Prize",
                column: "LotteryActivityId",
                principalTable: "LotteryActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
