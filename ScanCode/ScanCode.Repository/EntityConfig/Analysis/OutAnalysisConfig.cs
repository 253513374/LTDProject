using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity.Analysis;

namespace ScanCode.Repository.EntityConfig.Analysis
{
    public class OutAnalysisConfig : EntityTypeConfiguration<OutStorageAnalysis>
    {
        public override void Configure(EntityTypeBuilder<OutStorageAnalysis> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.ToTable("OutStorage", "Analysis");
        }
    }
}