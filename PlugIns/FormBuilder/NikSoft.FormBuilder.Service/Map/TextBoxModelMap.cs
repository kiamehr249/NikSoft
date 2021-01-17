using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class TextBoxModelMap : EntityTypeConfiguration<TextBoxModel>
    {
        public TextBoxModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_TextBoxes");

            this.HasRequired(x => x.Form)
                .WithMany(x => x.TextBoxes)
                .HasForeignKey(x => x.FormID);
        }
    }
}