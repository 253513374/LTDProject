using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.EntityConfig
{
    internal class UserConfig : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.ID);//设置主键
            builder.Property(p => p.ID).ValueGeneratedOnAdd();

            builder.Property(p => p.CreateTime).HasDefaultValue(DateTime.Now);

            //   throw new NotImplementedException();
        }
    }
}