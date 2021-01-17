using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class VisualLinkMap : EntityTypeConfiguration<VisualLink>
    {
        public VisualLinkMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_VisualLinks");
        }
    }
}