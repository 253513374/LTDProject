using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtdl.Model.Entity;

namespace Wtdl.Repository.EntityConfig
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
            builder.Property(u => u.PostalCode).HasMaxLength(10);

            // 设置表名
            builder.ToTable("UserAwardInfos");
        }
    }
}