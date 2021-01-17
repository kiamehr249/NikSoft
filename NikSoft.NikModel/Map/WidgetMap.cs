using System.Data.Entity.ModelConfiguration;

namespace NikSoft.NikModel
{
    public class WidgetMap : EntityTypeConfiguration<Widget>
    {

        public WidgetMap()
        {
            this.HasKey(t => t.ID);

            this.ToTable("Widgets");

            this.HasRequired(t => t.WidgetDefinition)
                .WithMany(t => t.Widgets)
                .HasForeignKey(d => d.WidgetDefinitionID);
        }
    }
}