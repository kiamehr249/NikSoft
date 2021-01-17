using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class ListControlModelMap : EntityTypeConfiguration<ListControlModel>
    {
        public ListControlModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_ListControls");

            this.HasRequired(x => x.Form)
                .WithMany(x => x.ListControls)
                .HasForeignKey(x => x.FormID);

            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentID);
        }
    }
}