using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class GroupPermissionMap : EntityTypeConfiguration<GroupPermission>
    {
        public GroupPermissionMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_GroupPermissions");

            this.HasRequired(x => x.ContentGroup)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.GroupID);
        }
    }
}