using System.Data.Entity.ModelConfiguration;

namespace NikSoft.ContentManager.Service
{
    public class GeneralMenuItemMap : EntityTypeConfiguration<GeneralMenuItem>
    {

        public GeneralMenuItemMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("CM_GeneralMenuItems");

            this.HasRequired(x => x.GeneralMenu)
                .WithMany(x => x.GeneralMenuItems)
                .HasForeignKey(x => x.GeneralMenuID);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentID);
        }
    }
}