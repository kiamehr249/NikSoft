using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class PublicContentMap : EntityTypeConfiguration<PublicContent>
    {

        public PublicContentMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_PublicContents");

            this.HasRequired(x => x.ContentCategory)
                .WithMany(x => x.PublicContents)
                .HasForeignKey(x => x.CategoryID);
        }
    }
}