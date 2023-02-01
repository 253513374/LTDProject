using Microsoft.EntityFrameworkCore;
using Weitedianlan.Model.Entity;

namespace Weitedianlan.Service
{
    /// <summary>
    /// 数据库访问上下文
    /// </summary>
    public class Db : DbContext
    {
        public Db()
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var commandstr = ConfigSet.GetConfigjson();
            //  var sql = "Data Source=192.168.1.96;Initial Catalog=WTDL;User ID=sa;Password=new";
            optionsBuilder.UseSqlServer(commandstr, b => b.UseRowNumberForPaging());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Agent> Agent { get; set; }
        public virtual DbSet<W_LabelStorage> W_LabelStorage { get; set; }
    }
}