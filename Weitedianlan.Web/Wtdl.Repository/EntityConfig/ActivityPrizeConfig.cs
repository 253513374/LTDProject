using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.EntityConfig
{
    internal class ActivityPrizeConfig : EntityTypeConfiguration<ActivityPrize>
    {
        public override void Configure(EntityTypeBuilder<ActivityPrize> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder
                .HasOne(p => p.LotteryActivity)
                .WithMany(a => a.Prizes)
                .HasForeignKey(p => p.LotteryActivityId);

            builder.Ignore(g => g.Identifier);
        }
    }
}