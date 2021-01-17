using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class TemplateMap : EntityTypeConfiguration<Template>
    {

        public TemplateMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("Templates");
        }
    }
}