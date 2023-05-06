using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.EntityConfig
{
    internal class RedPacketRecordConfig : EntityTypeConfiguration<RedPacketRecord>
    {
        public override void Configure(EntityTypeBuilder<RedPacketRecord> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.HasIndex(p => p.CreateTime);//设置索引
            builder.HasIndex(p => p.QrCode);//设置索引
            builder.HasIndex(p => p.ReOpenId);//设置索引
            builder.HasIndex(e => new { e.QrCode, e.ReOpenId });//使用组合索引，

            builder.ToTable(b =>
            {
                b.IsMemoryOptimized();
            }).HasAnnotation("SqlServer:MemoryOptimizedSize", 1024 * 1024);//预分配1GB 内存
        }
    }
}