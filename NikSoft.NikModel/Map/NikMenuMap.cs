using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class NikMenuMap : EntityTypeConfiguration<NikMenu>
    {

        public NikMenuMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("NikMenus");

            this.HasRequired(t => t.Portal)
                .WithMany(t => t.NikMenus)
                .HasForeignKey(t => t.PortalID);

            this.HasRequired(t => t.Parent)
                .WithMany(t => t.Childs)
                .HasForeignKey(t => t.ParentID);
        }
    }
}