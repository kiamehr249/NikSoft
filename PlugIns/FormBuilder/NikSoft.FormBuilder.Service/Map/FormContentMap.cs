using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class FormContentMap : EntityTypeConfiguration<FormContent>
    {
        public FormContentMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_FormContents");

            this.HasRequired(x => x.Form)
                .WithMany(x => x.FormContents)
                .HasForeignKey(x => x.FormID);
        }
    }
}