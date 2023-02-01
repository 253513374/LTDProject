using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class UserConfig : EntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");
            //   throw new NotImplementedException();
        }
    }
}