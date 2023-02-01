using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class VerificationCodeConfig : EntityTypeConfiguration<VerificationCode>
    {
        public override void Configure(EntityTypeBuilder<VerificationCode> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.Property(p => p.AntiForgeryCode).HasMaxLength(20);
            builder.HasIndex(p => p.AntiForgeryCode).IsUnique();//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()"); ;//设置默认值
        }
    }
}