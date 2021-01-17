using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class ContentGroupMap : EntityTypeConfiguration<ContentGroup>
    {

        public ContentGroupMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_ContentGroups");
        }
    }
}