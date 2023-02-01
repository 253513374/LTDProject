using Microsoft.EntityFrameworkCore;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.EntityConfig;

namespace Wtdl.Repository
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
            // modelBuilder.ApplyConfiguration(new UserConfig());
            //modelBuilder.Entity<W_LabelStorage>()
            //    .HasOne(p => p.Agent)
            //    .WithMany(b => b.WLabelStorage)
            //    .HasForeignKey(p => p.Dealers);

            // modelBuilder.Entity<Agent>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<LotteryActivity> LotteryActivities { get; set; }//活动
        public DbSet<Prize> Prizes { get; set; }//奖品

        public DbSet<LotteryRecord> LotteryRecords { get; set; }//抽奖记录

        public DbSet<Agent> Agents { get; set; }//代理商

        public DbSet<W_LabelStorage> WLabelStorages { get; set; }//标签存储

        public DbSet<User> Users { get; set; }//用户

        /// <summary>
        /// 导入对应验证码
        /// </summary>
        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public DbSet<FileUploadRecord> FileUploadRecords { get; internal set; }
    }
}