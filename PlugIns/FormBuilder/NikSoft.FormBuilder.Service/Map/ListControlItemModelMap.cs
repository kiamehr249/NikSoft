using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class ListControlItemModelMap : EntityTypeConfiguration<ListControlItemModel>
    {
        public ListControlItemModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_ListItems");

            this.HasRequired(x => x.ListControl)
                .WithMany(x => x.ListItems)
                .HasForeignKey(x => x.ListControlID);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentID);
        }
    }
}