using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wtdl.Admin.Authenticated.IdentityModel;

namespace Wtdl.Admin.Authenticated
{
    public class CustomIdentityDbContext : IdentityDbContext<WtdlUser, WtdlRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, WtdlRolePermission, IdentityUserToken<string>>
    {
        //public DbSet<WtdlUser> Users;

        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WtdlUser>(b =>
            {
                b.ToTable("WtdlUser", "Identity");
            });

            builder.Entity<WtdlRole>(b =>
            {
                b.ToTable("WtdlRole", "Identity");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
                        {
                            b.ToTable("WtdlUserRole", "Identity");
                        });
            builder.Entity<WtdlRolePermission>(b =>
            {
                b.ToTable("WtdlRolePermission", "Identity");

                b.HasOne(rp => rp.Role)
                .WithMany(r => r.RoleClaims)
                .HasForeignKey(rp => rp.RoleId)
                .IsRequired();
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }
}