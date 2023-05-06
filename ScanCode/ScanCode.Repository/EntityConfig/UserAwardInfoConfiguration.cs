using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.EntityConfig
{
    public class UserAwardInfoConfiguration : IEntityTypeConfiguration<UserAwardInfo>
    {
        public void Configure(EntityTypeBuilder<UserAwardInfo> builder)
        {
            // 设置主键
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();//自增长

            // 设置字段属性
            // 设置字段属性
            builder.Property(u => u.WeChatOpenId).HasMaxLength(50);
            builder.Property(u => u.UserName).HasMaxLength(50);
            builder.Property(u => u.PhoneNumber).HasMaxLength(20);
            builder.Property(u => u.AwardName).HasMaxLength(100);
            builder.Property(u => u.AwardDescription).HasMaxLength(500);

            builder.Property(u => u.FullAddress).HasMaxLength(300);
            builder.Property(u => u.City).HasMaxLength(50);
            builder.Property(u => u.ProvinceOrState).HasMaxLength(50);
            builder.Property(u => u.Country).HasMaxLength(50);

            //设置索引
            builder.HasIndex(u => u.QrCode).IsUnique();
            builder.HasIndex(h => h.WeChatOpenId).IsUnique();
            // 设置表名
            builder.ToTable("UserAwardInfos");
        }
    }
}