using System.Data.Entity;

using Weitedianlan.model.Entity;

public partial class Oracle_ERP_Db : DbContext
{
    public Oracle_ERP_Db()
        : base("name=Oracle_ERP_Db")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // throw new UnintentionalCodeFirstException();
    }

    public DbSet<T_BDX_ORDER> t_BDX_ORDERs { set; get; }
}