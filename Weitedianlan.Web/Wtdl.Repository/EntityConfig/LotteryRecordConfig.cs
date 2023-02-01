using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class LotteryRecordConfig : EntityTypeConfiguration<LotteryRecord>
    {
        public override void Configure(EntityTypeBuilder<LotteryRecord> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.HasIndex(p => p.CreateTime);//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");
        }
    }
}