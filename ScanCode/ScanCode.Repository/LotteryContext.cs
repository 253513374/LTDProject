using Microsoft.EntityFrameworkCore;
using ScanCode.Model.Entity;
using ScanCode.Model.Entity.Analysis;
using ScanCode.Repository.EntityConfig;
using ScanCode.Repository.EntityConfig.Analysis;

namespace ScanCode.Repository
{
    public class LotteryContext : DbContext
    {
        public LotteryContext(DbContextOptions<LotteryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LotteryActivity>().ToTable("LotteryActivity");
            modelBuilder.Entity<LotteryRecord>().ToTable("LotteryRecord");
            modelBuilder.Entity<Prize>().ToTable("Prize");
            modelBuilder.Entity<User>().ToTable("tUser");
            modelBuilder.Entity<Agent>().ToTable("tAgent");
            modelBuilder.Entity<W_LabelStorage>().ToTable("W_LabelStorage");

            // modelBuilder.ApplyConfiguration(new )

            modelBuilder.ApplyConfiguration(new AgentConfig());
            modelBuilder.ApplyConfiguration(new LotteryActivityConfig());
            modelBuilder.ApplyConfiguration(new FileUploadRecordConfig());
            modelBuilder.ApplyConfiguration(new LotteryRecordConfig());
            modelBuilder.ApplyConfiguration(new PrizeConfig());
            modelBuilder.ApplyConfiguration(new VerificationCodeConfig());
            modelBuilder.ApplyConfiguration(new WLabelStorageConfig());
            modelBuilder.ApplyConfiguration(new ScanRedPacketConfig());
            modelBuilder.ApplyConfiguration(new RedPacketRecordConfig());
            modelBuilder.ApplyConfiguration(new ActivityPrizeConfig());

            modelBuilder.ApplyConfiguration(new OutAnalysisConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserAwardInfoConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<OutStorageAnalysis> OutStorageAnalyses { get; set; }

        /// <summary>
        /// 抽奖活动
        /// </summary>
        public DbSet<LotteryActivity> LotteryActivities { get; set; }//活动

        /// <summary>
        /// 奖品池
        /// </summary>
        public DbSet<Prize> Prizes { get; set; }//奖品

        /// <summary>
        /// 抽奖记录
        /// </summary>
        public DbSet<LotteryRecord> LotteryRecords { get; set; }//抽奖记录

        /// <summary>
        /// 客户表
        /// </summary>
        public DbSet<Agent> Agents { get; set; }//代理商

        /// <summary>
        /// 出库单出库数据表
        /// </summary>
        public DbSet<W_LabelStorage> WLabelStorages { get; set; }//标签存储

        /// <summary>
        /// 系统用户表
        /// </summary>
        public DbSet<User> Users { get; set; }//用户

        /// <summary>
        /// 导入对应验证码
        /// </summary>
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        /// <summary>
        /// 验证码文件按上传记录
        /// </summary>
        public DbSet<FileUploadRecord> FileUploadRecords { get; internal set; }

        /// <summary>
        /// 红包记录表
        /// </summary>
        public DbSet<RedPacketRecord> RedPacketRecords { get; internal set; }

        /// <summary>
        /// 红包配置表
        /// </summary>
        public DbSet<RedPacketCinfig> ScanRedPackets { get; internal set; }

        public DbSet<ActivityPrize> ActivityPrizes { get; internal set; }
        public DbSet<UserAwardInfo> UserAwardInfos { get; internal set; }
    }
}