using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class FormModelMap : EntityTypeConfiguration<FormModel>
    {
        public FormModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_Forms");

            this.HasRequired(x => x.Parent)
                .WithMany(x => x.Childs)
                .HasForeignKey(x => x.ParentID);
        }
    }
}