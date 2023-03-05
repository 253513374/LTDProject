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

            // builder.HasIndex(p => p.CreateTime);//设置索引
            builder.HasIndex(p => p.QRCode);//设置索引
            builder.HasIndex(p => p.OpenId);//设置索引
            builder.HasIndex(e => new { e.QRCode, e.OpenId });//使用组合索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.ToTable(b =>
            {
                b.IsMemoryOptimized();
            }).HasAnnotation("SqlServer:MemoryOptimizedSize", 1024 * 1024);//预分配1GB 内存
        }
    }
}