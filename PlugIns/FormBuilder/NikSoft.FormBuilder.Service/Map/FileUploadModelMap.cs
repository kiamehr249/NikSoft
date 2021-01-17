using System.Data.Entity.ModelConfiguration;

namespace NikSoft.FormBuilder.Service
{
    public class FileUploadModelMap : EntityTypeConfiguration<FileUploadModel>
    {
        public FileUploadModelMap()
        {
            this.HasKey(t => t.ID);
            this.ToTable("FB_FileUploads");

            this.HasRequired(x => x.Form)
                .WithMany(x => x.FileUploads)
                .HasForeignKey(x => x.FormID);
        }
    }
}