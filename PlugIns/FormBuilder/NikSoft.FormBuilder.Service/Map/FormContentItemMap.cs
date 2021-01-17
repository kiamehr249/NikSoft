using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class FormContentItemMap : EntityTypeConfiguration<FormContentItem>
    {
        public FormContentItemMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_FormContentItems");

            this.HasRequired(x => x.FormContent)
                .WithMany(x => x.FormContentItems)
                .HasForeignKey(x => x.FormContentID);
        }
    }
}