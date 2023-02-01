using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    public class WLabelStorageConfig : EntityTypeConfiguration<W_LabelStorage>
    {
        public override void Configure(EntityTypeBuilder<W_LabelStorage> builder)
        {
            builder.HasKey(p => p.ID);//设置主键
            builder.Property(p => p.ID)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.HasIndex(p => p.QRCode);//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");
            //builder
            //    .HasOne(p => p.Agent)
            //    .WithMany(b => b.WLabelStorage)
            //    .HasForeignKey(p => p.Dealers);
        }
    }
}