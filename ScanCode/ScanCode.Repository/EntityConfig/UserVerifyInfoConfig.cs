using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.EntityConfig
{
    internal class UserVerifyInfoConfig : EntityTypeConfiguration<UserVerifyInfo>
    {
        public override void Configure(EntityTypeBuilder<UserVerifyInfo> builder)
        {
            builder.HasKey(p => p.PhoneNumber); // 设置主键
            builder.Property(p => p.PhoneNumber) // 设置属性
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.VerificationCode) // 设置属性
                .IsRequired()
                .HasMaxLength(4);

            builder.Property(p => p.CodeSentTime) // 设置属性
                .IsRequired();

            builder.Property(p => p.IsVerified) // 设置属性
                .IsRequired();

            builder.ToTable("UserVerifyInfo"); // 设置表名
        }
    }
}