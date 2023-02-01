using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityParticipant");

            migrationBuilder.DropTable(
                name: "Participant");

            migrationBuilder.CreateTable(
                name: "ActivityPrize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserLimit = table.Column<int>(type: "int", nullable: false),
                    TotalLimit = table.Column<int>(type: "int", nullable: false),
                    WinnerCount = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinCashValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxCashValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsJoinActivity = table.Column<bool>(type: "bit", nullable: false),
                    LotteryActivityId = table.Column<int>(type: "int", nullable: true),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityPrize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityPrize_LotteryActivity_LotteryActivityId",
                        column: x => x.LotteryActivityId,
                        principalTable: "LotteryActivity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPrize_LotteryActivityId",
                table: "ActivityPrize",
                column: "LotteryActivityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPrize");

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
        }
    }
}
