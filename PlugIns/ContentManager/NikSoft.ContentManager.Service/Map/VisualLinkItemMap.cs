using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class VisualLinkItemMap : EntityTypeConfiguration<VisualLinkItem>
    {
        public VisualLinkItemMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_VisualLinkItems");

            this.HasRequired(x => x.VisualLink)
                .WithMany(x => x.VisualLinkItems)
                .HasForeignKey(x => x.VisualLinkID);
        }
    }
}