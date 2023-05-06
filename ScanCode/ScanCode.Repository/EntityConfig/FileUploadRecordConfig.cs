using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScanCode.Model.Entity;
using ScanCode.Model.Enum;

namespace ScanCode.Repository.EntityConfig
{
    internal class FileUploadRecordConfig : EntityTypeConfiguration<FileUploadRecord>
    {
        public override void Configure(EntityTypeBuilder<FileUploadRecord> builder)
        {
            builder.HasKey(p => p.Id);//设置主键
            builder.Property(p => p.Id)//设置属性为自增长
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Status)
                .HasConversion(
                    v => (int)v,
                    x => (ImportStatus)x
                );

            builder.Property(p => p.CreateTime).HasDefaultValueSql("getdate()");
        }
    }
}