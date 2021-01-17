using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class ContentCategoryMap : EntityTypeConfiguration<ContentCategory>
    {
        public ContentCategoryMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_ContentCategories");

            this.HasRequired(x => x.ContentGroup)
                .WithMany(x => x.ContentCategories)
                .HasForeignKey(x => x.GroupID);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentID);
        }
    }
}