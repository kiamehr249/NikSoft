using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class PortalMap : EntityTypeConfiguration<Portal>
    {

        public PortalMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("Portals");

            this.HasOptional(t => t.Parent)
                .WithMany(t => t.Childs)
                .HasForeignKey(d => d.ParentID);
        }
    }
}