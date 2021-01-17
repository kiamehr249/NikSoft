using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserRoleMenuMap : EntityTypeConfiguration<UserRoleMenu>
    {

        public UserRoleMenuMap()
        {
            this.HasKey(t => new { t.NikMenuID, t.UserTypeGroupID, t.PermissionType });

            this.ToTable("UserRoleMenus");

            this.HasRequired(t => t.NikMenu)
                .WithMany(t => t.UserRoleMenus)
                .HasForeignKey(t => t.NikMenuID);

            this.HasRequired(t => t.UserTypeGroup)
                .WithMany(t => t.UserRoleMenus)
                .HasForeignKey(t => t.UserTypeGroupID);
        }
    }
}