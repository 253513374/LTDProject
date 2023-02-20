using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;
using Weitedianlan.Model.Entity.Analysis;

namespace Wtdl.Repository.EntityConfig.Analysis
{
    public class CountAnalysisConfig : EntityTypeConfiguration<CountAnalysis>
    {
        public override void Configure(EntityTypeBuilder<CountAnalysis> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.ToTable("TotalCount", "Analysis");
            //throw new NotImplementedException();
        }
    }
}