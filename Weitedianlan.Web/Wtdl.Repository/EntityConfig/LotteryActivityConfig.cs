using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class LotteryActivityConfig : EntityTypeConfiguration<LotteryActivity>
    {
        public override void Configure(EntityTypeBuilder<LotteryActivity> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.HasIndex(p => p.CreateTime);//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.HasMany(p => p.Prizes)
                .WithOne(l => l.LotteryActivity)
                .HasForeignKey(p => p.LotteryActivityId);

            builder.Ignore(g => g.ShowPrizes);
        }
    }
}