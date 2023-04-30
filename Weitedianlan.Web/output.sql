IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF SCHEMA_ID(N'Analysis') IS NULL EXEC(N'CREATE SCHEMA [Analysis];');
GO

COMMIT;
GO

IF SERVERPROPERTY('IsXTPSupported') = 1 AND SERVERPROPERTY('EngineEdition') <> 5
    BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM [sys].[filegroups] [FG] JOIN [sys].[database_files] [F] ON [FG].[data_space_id] = [F].[data_space_id] WHERE [FG].[type] = N'FX' AND [F].[type] = 2)
        BEGIN
        ALTER DATABASE CURRENT SET AUTO_CLOSE OFF;
        DECLARE @db_name nvarchar(max) = DB_NAME();
        DECLARE @fg_name nvarchar(max);
        SELECT TOP(1) @fg_name = [name] FROM [sys].[filegroups] WHERE [type] = N'FX';

        IF @fg_name IS NULL
            BEGIN
            SET @fg_name = @db_name + N'_MODFG';
            EXEC(N'ALTER DATABASE CURRENT ADD FILEGROUP [' + @fg_name + '] CONTAINS MEMORY_OPTIMIZED_DATA;');
            END

        DECLARE @path nvarchar(max);
        SELECT TOP(1) @path = [physical_name] FROM [sys].[database_files] WHERE charindex('\', [physical_name]) > 0 ORDER BY [file_id];
        IF (@path IS NULL)
            SET @path = '\' + @db_name;

        DECLARE @filename nvarchar(max) = right(@path, charindex('\', reverse(@path)) - 1);
        SET @filename = REPLACE(left(@filename, len(@filename) - charindex('.', reverse(@filename))), '''', '''''') + N'_MOD';
        DECLARE @new_path nvarchar(max) = REPLACE(CAST(SERVERPROPERTY('InstanceDefaultDataPath') AS nvarchar(max)), '''', '''''') + @filename;

        EXEC(N'
            ALTER DATABASE CURRENT
            ADD FILE (NAME=''' + @filename + ''', filename=''' + @new_path + ''')
            TO FILEGROUP [' + @fg_name + '];')
        END
    END

IF SERVERPROPERTY('IsXTPSupported') = 1
EXEC(N'
    ALTER DATABASE CURRENT
    SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT ON;')
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [FileUploadRecords] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NULL,
    [FileName2] nvarchar(max) NULL,
    [FileSize] bigint NOT NULL,
    [FileCount] bigint NOT NULL,
    [FilePath] nvarchar(max) NULL,
    [FileHash] nvarchar(max) NULL,
    [UploadTime] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (getdate()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_FileUploadRecords] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [LotteryActivity] (
    [Id] int NOT NULL IDENTITY,
    [ActivityNumber] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [StartTime] datetime2 NULL,
    [EndTime] datetime2 NULL,
    [Status] int NOT NULL,
    [IsActive] bit NOT NULL,
    [TotalParticipant] int NOT NULL,
    [TotalWinner] int NOT NULL,
    [AllowDuplicate] bit NOT NULL,
    [AllowMultipleWinning] bit NOT NULL,
    [ActivityImage] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (GETDATE()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_LotteryActivity] PRIMARY KEY ([Id])
);
GO

COMMIT;
GO

CREATE TABLE [LotteryRecord] (
    [Id] int NOT NULL IDENTITY,
    [IsSuccessPrize] bit NOT NULL,
    [OpenId] nvarchar(450) NULL,
    [UserName] nvarchar(max) NULL,
    [QRCode] nvarchar(450) NULL,
    [UserPhoneNumber] nvarchar(max) NULL,
    [Time] datetime2 NOT NULL,
    [Claimed] int NOT NULL,
    [ClaimTime] datetime2 NULL,
    [ActivityNumber] nvarchar(max) NULL,
    [ActivityName] nvarchar(max) NULL,
    [ActivityDescription] nvarchar(max) NULL,
    [PrizeNumber] nvarchar(max) NULL,
    [PrizeName] nvarchar(max) NULL,
    [Type] int NOT NULL,
    [CashValue] int NOT NULL,
    [PrizeDescription] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (GETDATE()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_LotteryRecord] PRIMARY KEY NONCLUSTERED ([Id])
) WITH (MEMORY_OPTIMIZED = ON);
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Analysis].[OutStorage] (
    [Id] int NOT NULL IDENTITY,
    [Year] int NOT NULL,
    [Month] int NOT NULL,
    [OrderNumbels] nvarchar(max) NULL,
    [Count] float NOT NULL,
    CONSTRAINT [PK_OutStorage] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Prize] (
    [Id] int NOT NULL IDENTITY,
    [UniqueNumber] nvarchar(max) NULL,
    [PrizeNumber] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Probability] float NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [ImageUrl] nvarchar(max) NULL,
    [Type] int NOT NULL,
    [CashValue] int NOT NULL,
    [IsJoinActivity] bit NOT NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (GETDATE()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_Prize] PRIMARY KEY ([Id])
);
GO

COMMIT;
GO

CREATE TABLE [RedPacketRecords] (
    [Id] int NOT NULL IDENTITY,
    [QrCode] nvarchar(450) NULL,
    [Captcha] nvarchar(max) NULL,
    [CashAmount] decimal(18,2) NOT NULL,
    [ReceiveTime] datetime2 NOT NULL,
    [IssueTime] datetime2 NOT NULL,
    [MchbillNo] nvarchar(max) NULL,
    [MchId] nvarchar(max) NULL,
    [WxAppId] nvarchar(max) NULL,
    [ReOpenId] nvarchar(450) NULL,
    [TotalAmount] nvarchar(max) NULL,
    [SendListid] nvarchar(max) NULL,
    [NonceStr] nvarchar(max) NULL,
    [PaySign] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL,
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_RedPacketRecords] PRIMARY KEY NONCLUSTERED ([Id])
) WITH (MEMORY_OPTIMIZED = ON);
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ScanRedPackets] (
    [Id] int NOT NULL IDENTITY,
    [ScanRedPacketGuid] nvarchar(max) NULL,
    [ActivityName] nvarchar(max) NULL,
    [SenderName] nvarchar(max) NULL,
    [WishingWord] nvarchar(max) NULL,
    [IsActivity] bit NOT NULL,
    [IsSubscribe] bit NOT NULL,
    [RedPacketType] int NOT NULL,
    [CashValue] int NOT NULL,
    [MinCashValue] int NOT NULL,
    [MaxCashValue] int NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_ScanRedPackets] PRIMARY KEY ([Id])
);
GO

COMMIT;
GO

CREATE TABLE [tAgent] (
    [Id] int NOT NULL IDENTITY,
    [AID] nvarchar(max) NULL,
    [AName] nvarchar(max) NULL,
    [AProvince] nvarchar(max) NULL,
    [ACity] nvarchar(max) NULL,
    [AAddr] nvarchar(max) NULL,
    [ATel] nvarchar(max) NULL,
    [APeople] nvarchar(max) NULL,
    [ABelong] nvarchar(max) NULL,
    [AType] int NULL,
    [datetiem] datetime2 NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (getdate()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_tAgent] PRIMARY KEY NONCLUSTERED ([Id])
) WITH (MEMORY_OPTIMIZED = ON);
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [tUser] (
    [ID] int NOT NULL IDENTITY,
    [UserID] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [PWD] nvarchar(max) NULL,
    [AgentID] nvarchar(max) NULL,
    [Flag] int NOT NULL,
    [CreateTime] datetime2 NULL DEFAULT '2023-04-02T14:17:27.9113387+08:00',
    CONSTRAINT [PK_tUser] PRIMARY KEY ([ID])
);
GO

CREATE TABLE [UserAwardInfos] (
    [Id] int NOT NULL IDENTITY,
    [WeChatOpenId] nvarchar(50) NULL,
    [UserName] nvarchar(50) NULL,
    [PhoneNumber] nvarchar(20) NULL,
    [AwardName] nvarchar(100) NULL,
    [AwardDescription] nvarchar(500) NULL,
    [DateReceived] datetime2 NOT NULL,
    [FullAddress] nvarchar(300) NULL,
    [City] nvarchar(50) NULL,
    [ProvinceOrState] nvarchar(50) NULL,
    [Country] nvarchar(50) NULL,
    [PostalCode] nvarchar(10) NULL,
    [IsShipped] bit NOT NULL,
    CONSTRAINT [PK_UserAwardInfos] PRIMARY KEY ([Id])
);
GO

COMMIT;
GO

CREATE TABLE [VerificationCodes] (
    [Id] int NOT NULL IDENTITY,
    [QRCode] nvarchar(20) NULL,
    [Captcha] nvarchar(max) NULL,
    [FileHash] nvarchar(max) NULL,
    [CreateTime] datetime2 NOT NULL DEFAULT (GETDATE()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_VerificationCodes] PRIMARY KEY NONCLUSTERED ([Id])
) WITH (MEMORY_OPTIMIZED = ON);
GO

CREATE TABLE [W_LabelStorage] (
    [ID] int NOT NULL IDENTITY,
    [QRCode] nvarchar(21) NULL,
    [OrderTime] datetime2 NOT NULL,
    [OutTime] datetime2 NOT NULL,
    [Dealers] nvarchar(18) NULL,
    [Adminaccount] nvarchar(18) NULL,
    [OutType] nvarchar(5) NULL,
    [OrderNumbels] nvarchar(28) NULL,
    [ExtensionName] nvarchar(5) NULL,
    [ExtensionOrder] nvarchar(5) NULL DEFAULT N'',
    [CreateTime] datetime2 NOT NULL DEFAULT (getdate()),
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_W_LabelStorage] PRIMARY KEY NONCLUSTERED ([ID])
) WITH (MEMORY_OPTIMIZED = ON);
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ActivityPrize] (
    [Id] int NOT NULL IDENTITY,
    [UniqueNumber] int NOT NULL,
    [PrizeNumber] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Amount] int NOT NULL,
    [Unredeemed] int NOT NULL,
    [Probability] float NOT NULL,
    [IsActive] bit NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [Type] int NOT NULL,
    [CashValue] int NOT NULL,
    [IsJoinActivity] bit NOT NULL,
    [LotteryActivityId] int NULL,
    [RowVersion] rowversion NULL,
    [CreateTime] datetime2 NOT NULL,
    [AdminUser] nvarchar(max) NULL,
    CONSTRAINT [PK_ActivityPrize] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ActivityPrize_LotteryActivity_LotteryActivityId] FOREIGN KEY ([LotteryActivityId]) REFERENCES [LotteryActivity] ([Id])
);
GO

CREATE INDEX [IX_ActivityPrize_LotteryActivityId] ON [ActivityPrize] ([LotteryActivityId]);
GO

CREATE INDEX [IX_LotteryActivity_CreateTime] ON [LotteryActivity] ([CreateTime]);
GO

COMMIT;
GO

ALTER TABLE [LotteryRecord] ADD INDEX [IX_LotteryRecord_OpenId] NONCLUSTERED ([OpenId]);
GO

ALTER TABLE [LotteryRecord] ADD INDEX [IX_LotteryRecord_QRCode] NONCLUSTERED ([QRCode]);
GO

ALTER TABLE [LotteryRecord] ADD INDEX [IX_LotteryRecord_QRCode_OpenId] NONCLUSTERED ([QRCode], [OpenId]);
GO

BEGIN TRANSACTION;
GO

CREATE INDEX [IX_Prize_CreateTime] ON [Prize] ([CreateTime]);
GO

COMMIT;
GO

ALTER TABLE [RedPacketRecords] ADD INDEX [IX_RedPacketRecords_CreateTime] NONCLUSTERED ([CreateTime]);
GO

ALTER TABLE [RedPacketRecords] ADD INDEX [IX_RedPacketRecords_QrCode] NONCLUSTERED ([QrCode]);
GO

ALTER TABLE [RedPacketRecords] ADD INDEX [IX_RedPacketRecords_QrCode_ReOpenId] NONCLUSTERED ([QrCode], [ReOpenId]);
GO

ALTER TABLE [RedPacketRecords] ADD INDEX [IX_RedPacketRecords_ReOpenId] NONCLUSTERED ([ReOpenId]);
GO

ALTER TABLE [VerificationCodes] ADD INDEX [IX_VerificationCodes_QRCode] NONCLUSTERED ([QRCode]);
GO

ALTER TABLE [W_LabelStorage] ADD INDEX [IX_W_LabelStorage_ID] NONCLUSTERED ([ID]);
GO

ALTER TABLE [W_LabelStorage] ADD INDEX [IX_W_LabelStorage_OutTime] NONCLUSTERED ([OutTime]);
GO

ALTER TABLE [W_LabelStorage] ADD INDEX [IX_W_LabelStorage_QRCode] NONCLUSTERED ([QRCode]);
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230402061727_add-db', N'7.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAwardInfos]') AND [c].[name] = N'PostalCode');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserAwardInfos] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [UserAwardInfos] DROP COLUMN [PostalCode];
GO

ALTER TABLE [UserAwardInfos] ADD [ActivityName] nvarchar(max) NULL;
GO

ALTER TABLE [UserAwardInfos] ADD [ActivityNumber] nvarchar(max) NULL;
GO

ALTER TABLE [UserAwardInfos] ADD [Area] nvarchar(max) NULL;
GO

ALTER TABLE [UserAwardInfos] ADD [PrizeNumber] nvarchar(max) NULL;
GO

ALTER TABLE [UserAwardInfos] ADD [PrizeType] nvarchar(max) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tUser]') AND [c].[name] = N'CreateTime');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [tUser] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [tUser] ADD DEFAULT '2023-04-24T22:59:34.0402724+08:00' FOR [CreateTime];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230424145934_addr', N'7.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [UserAwardInfos] ADD [QrCode] nvarchar(max) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tUser]') AND [c].[name] = N'CreateTime');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [tUser] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [tUser] ADD DEFAULT '2023-04-25T16:20:50.2933436+08:00' FOR [CreateTime];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230425082050_addqrcode', N'7.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAwardInfos]') AND [c].[name] = N'QrCode');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [UserAwardInfos] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [UserAwardInfos] ALTER COLUMN [QrCode] nvarchar(450) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tUser]') AND [c].[name] = N'CreateTime');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [tUser] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [tUser] ADD DEFAULT '2023-04-25T17:29:16.8276011+08:00' FOR [CreateTime];
GO

CREATE UNIQUE INDEX [IX_UserAwardInfos_QrCode] ON [UserAwardInfos] ([QrCode]) WHERE [QrCode] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_UserAwardInfos_WeChatOpenId] ON [UserAwardInfos] ([WeChatOpenId]) WHERE [WeChatOpenId] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230425092916_addindex', N'7.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[tUser]') AND [c].[name] = N'CreateTime');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [tUser] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [tUser] ADD DEFAULT '2023-04-26T10:36:19.1635624+08:00' FOR [CreateTime];
GO

ALTER TABLE [ScanRedPackets] ADD [RedPacketConfigType] int NOT NULL DEFAULT 0;
GO

COMMIT;
GO

ALTER TABLE [RedPacketRecords] ADD [ActivityName] nvarchar(max) NULL;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230426023619_addredconfig', N'7.0.4');
GO

COMMIT;
GO

