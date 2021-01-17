using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class CategoryPermissionMap : EntityTypeConfiguration<CategoryPermission>
    {
        public CategoryPermissionMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_CategoryPermissions");

            this.HasRequired(x => x.ContentCategory)
                .WithMany(x => x.Permissions)
                .HasForeignKey(x => x.CategoryID);
        }
    }
}