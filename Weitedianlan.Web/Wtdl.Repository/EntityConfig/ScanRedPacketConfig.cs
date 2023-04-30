using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Wtdl.Model.Entity;
using Wtdl.Model.Enum;

namespace Wtdl.Repository.EntityConfig
{
    public class ScanRedPacketConfig : EntityTypeConfiguration<RedPacketCinfig>
    {
        public override void Configure(EntityTypeBuilder<RedPacketCinfig> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.Property(x => x.IsActivity).IsRequired();
            builder.Property(x => x.RedPacketType).IsRequired();
            builder.Property(x => x.CashValue).IsRequired();
            builder.Property(x => x.MinCashValue).IsRequired();
            builder.Property(x => x.MaxCashValue).IsRequired();

            builder.Property(p => p.RedPacketType)
                .HasConversion(
                    v => (int)v,
                    x => (RedPacketType)x
                );
        }
    }
}