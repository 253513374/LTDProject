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

            builder.HasIndex(p => p.ID);//设置索引
            builder.HasIndex(p => p.QRCode);//设置索引
            builder.HasIndex(p => p.OutTime);//设置索引

            builder.Property(p => p.CreateTime).HasDefaultValueSql("getdate()");
            builder.Property(p => p.ExtensionOrder).HasDefaultValue("");

            builder.ToTable(b =>
            {
                b.IsMemoryOptimized();
            }).HasAnnotation("SqlServer:MemoryOptimizedSize", 5 * 1024 * 1024);//预分配5GB 内存

            builder.Property(p => p.OrderNumbels).HasMaxLength(28);
            builder.Property(p => p.QRCode).HasMaxLength(21);
            builder.Property(p => p.Adminaccount).HasMaxLength(18);
            builder.Property(p => p.Dealers).HasMaxLength(18);
            builder.Property(p => p.ExtensionName).HasMaxLength(5);
            builder.Property(p => p.OutType).HasMaxLength(5);
            builder.Property(p => p.ExtensionOrder).HasMaxLength(5);

            //builder
            //    .HasOne(p => p.Agent)
            //    .WithMany(b => b.WLabelStorage)
            //    .HasForeignKey(p => p.Dealers);
        }
    }
}