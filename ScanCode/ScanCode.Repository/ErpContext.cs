using Microsoft.EntityFrameworkCore;
using ScanCode.Model.Entity.ERP;

namespace ScanCode.Repository
{
    public class ErpContext : DbContext
    {
        public ErpContext(DbContextOptions<ErpContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BdxOrder>().HasNoKey();
            modelBuilder.Entity<BdxOrder>().ToTable("T_BDX_ORDER");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BdxOrder> BdxOrders { get; set; }
    }
}