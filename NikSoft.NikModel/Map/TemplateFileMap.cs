using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class TemplateFileMap : EntityTypeConfiguration<TemplateFile>
    {

        public TemplateFileMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("TemplateFiles");

            this.HasRequired(t => t.Template)
                .WithMany(t => t.TemplateFiles)
                .HasForeignKey(d => d.TemplateID);
        }
    }
}