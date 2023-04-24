using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtdl.Model.Entity.ERP;

namespace Wtdl.Repository
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