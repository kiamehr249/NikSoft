using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class UserRolModuleMap : EntityTypeConfiguration<UserRoleModule>
    {

        public UserRolModuleMap()
        {
            this.HasKey(t => new { t.NikModuleID, t.UserTypeGroupID, t.PermissionType });

            this.ToTable("UserRoleModules");

            this.HasRequired(t => t.NikModule)
                .WithMany(t => t.UserRoleModules)
                .HasForeignKey(t => t.NikModuleID);
            this.HasRequired(t => t.UserTypeGroup)
                .WithMany(t => t.UserRoleModules)
                .HasForeignKey(t => t.UserTypeGroupID);
        }
    }
}