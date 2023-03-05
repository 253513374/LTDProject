using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    public class AgentConfig : EntityTypeConfiguration<Agent>
    {
        public override void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasKey(h => h.Id); //设置主键

            //设置属性为自增长
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            //设置默认值
            builder.Property(p => p.datetiem);

            builder.Property(p => p.CreateTime).HasDefaultValueSql("GETDATE()");

            builder.ToTable(b =>
            {
                b.IsMemoryOptimized();
            }).HasAnnotation("SqlServer:MemoryOptimizedSize", 200 * 1024);//预分配 100MB 内存
            //builder.HasMany(a => a.WLabelStorage)
            //    .WithOne(w => w.Agent)
            //    .HasForeignKey(w => w.Dealers);
        }
    }
}