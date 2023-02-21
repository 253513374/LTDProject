using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wtdl.Repository.Migrations
{
    /// <inheritdoc />
    public partial class dbcotext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Analysis");

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
                    ActivityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TotalParticipant = table.Column<int>(type: "int", nullable: false),
                    TotalWinner = table.Column<int>(type: "int", nullable: false),
                    AllowDuplicate = table.Column<bool>(type: "bit", nullable: false),
                    AllowMultipleWinning = table.Column<bool>(type: "bit", nullable: false),
                    ActivityImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    IsSuccessPrize = table.Column<bool>(type: "bit", nullable: false),
                    OpenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Claimed = table.Column<int>(type: "int", nullable: false),
                    ClaimTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<int>(type: "int", nullable: false),
                    PrizeDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotteryRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutStorage",
                schema: "Analysis",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    OrderNumbels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutStorage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrizeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Probability = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<int>(type: "int", nullable: false),
                    IsJoinActivity = table.Column<bool>(type: "bit", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prize", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RedPacketRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Captcha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceiveTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssueTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MchbillNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MchId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WxAppId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReOpenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendListid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NonceStr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaySign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RedPacketRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScanRedPackets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScanRedPacketGuid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WishingWord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActivity = table.Column<bool>(type: "bit", nullable: false),
                    IsSubscribe = table.Column<bool>(type: "bit", nullable: false),
                    RedPacketType = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<int>(type: "int", nullable: false),
                    MinCashValue = table.Column<int>(type: "int", nullable: false),
                    MaxCashValue = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdminUser = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanRedPackets", x => x.Id);
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
                    QRCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Captcha = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "ActivityPrize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueNumber = table.Column<int>(type: "int", nullable: false),
                    PrizeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Unredeemed = table.Column<int>(type: "int", nullable: false),
                    Probability = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CashValue = table.Column<int>(type: "int", nullable: false),
                    IsJoinActivity = table.Column<bool>(type: "bit", nullable: false),
                    LotteryActivityId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
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
                    LotteryActivityId = table.Column<int>(type: "int", nullable: true),
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
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityPrize_LotteryActivityId",
                table: "ActivityPrize",
                column: "LotteryActivityId");

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
                name: "IX_tAgent_LotteryActivityId",
                table: "tAgent",
                column: "LotteryActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_QRCode",
                table: "VerificationCodes",
                column: "QRCode",
                unique: true,
                filter: "[QRCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_W_LabelStorage_QRCode",
                table: "W_LabelStorage",
                column: "QRCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityPrize");

            migrationBuilder.DropTable(
                name: "FileUploadRecords");

            migrationBuilder.DropTable(
                name: "LotteryRecord");

            migrationBuilder.DropTable(
                name: "OutStorage",
                schema: "Analysis");

            migrationBuilder.DropTable(
                name: "Prize");

            migrationBuilder.DropTable(
                name: "RedPacketRecords");

            migrationBuilder.DropTable(
                name: "ScanRedPackets");

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
