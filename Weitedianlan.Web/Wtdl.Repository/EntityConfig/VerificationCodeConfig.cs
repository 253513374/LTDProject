using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wtdl.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class VerificationCodeConfig : EntityTypeConfiguration<VerificationCode>
    {
        public override void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.Property(p => p.QRCode).HasMaxLength(20);
            builder.HasIndex(p => p.QRCode).IsUnique();//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()"); ;//设置默认值

            builder.ToTable(b =>
            {
                b.IsMemoryOptimized();
            }).HasAnnotation("SqlServer:MemoryOptimizedSize", 1 * 1024 * 1024);//预分配5GB 内存
        }
    }
}