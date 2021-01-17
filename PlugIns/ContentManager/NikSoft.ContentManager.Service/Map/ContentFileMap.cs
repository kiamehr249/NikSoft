using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class ContentFileMap : EntityTypeConfiguration<ContentFile>
    {
        public ContentFileMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_ContentFiles");

            this.HasRequired(x => x.PublicContent)
                .WithMany(x => x.ContentFiles)
                .HasForeignKey(x => x.PublicContentID);
        }
    }
}