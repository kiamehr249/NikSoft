using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class WidgetDefinitionMap : EntityTypeConfiguration<WidgetDefinition>
    {

        public WidgetDefinitionMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("WidgetDefinitions");
        }
    }
}