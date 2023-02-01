using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class db1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileUploadRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileCount = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UploadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileUploadRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotteryActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TotalParticipant = table.Column<int>(type: "int", nullable: false),
                    TotalWinner = table.Column<int>(type: "int", nullable: false),
                    AllowDuplicate = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultipleWinning = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LotteryRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClaimed = table.Column<bool>(type: "bit", nullable: false),
                    ClaimTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrizeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tUser",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PWD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tUser", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AntiForgeryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "W_LabelStorage",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OrderTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dealers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adminaccount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OutType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNumbels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtensionOrder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_W_LabelStorage", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    LotteryActivityId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prize_LotteryActivity_LotteryActivityId",
                        column: x => x.LotteryActivityId,
                        principalTable: "LotteryActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tAgent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AProvince = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ACity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AAddr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ATel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    APeople = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ABelong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AType = table.Column<int>(type: "int", nullable: true),
                    datetiem = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LotteryActivityId = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tAgent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tAgent_LotteryActivity_LotteryActivityId",
                        column: x => x.LotteryActivityId,
                        principalTable: "LotteryActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LotteryActivity_CreateTime",
                table: "LotteryActivity",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_LotteryRecord_CreateTime",
                table: "LotteryRecord",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Prize_CreateTime",
                table: "Prize",
                column: "CreateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Prize_LotteryActivityId",
                table: "Prize",
                column: "LotteryActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_tAgent_LotteryActivityId",
                table: "tAgent",
                column: "LotteryActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_AntiForgeryCode",
                table: "VerificationCodes",
                column: "AntiForgeryCode",
                unique: true,
                filter: "[AntiForgeryCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_W_LabelStorage_QRCode",
                table: "W_LabelStorage",
                column: "QRCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileUploadRecords");

            migrationBuilder.DropTable(
                name: "LotteryRecord");

            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropTable(
                name: "tAgent");

            migrationBuilder.DropTable(
                name: "tUser");

            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.DropTable(
                name: "W_LabelStorage");

            migrationBuilder.DropTable(
                name: "LotteryActivity");
        }
    }
}
