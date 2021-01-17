using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class CheckBoxModelMap : EntityTypeConfiguration<CheckBoxModel>
    {
        public CheckBoxModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_CheckBoxes");

            this.HasRequired(x => x.Form)
                .WithMany(x => x.CheckBoxes)
                .HasForeignKey(x => x.FormID);
        }
    }
}