using NikSoft.Model;
using System.Data.Entity;

namespace NikSoft.FormBuilder.Service
{
    public class FbContext : BaseContext, IFbUnitOfWork
    {
        static FbContext()
        {
            Database.SetInitializer<FbContext>(null);
        }

        public FbContext() : base("name=NikDBContext")
        {

        }

        public DbSet<FormModel> Forms { get; set; }
        public DbSet<TextBoxModel> TextBoxes { get; set; }
        public DbSet<ListControlModel> ListControls { get; set; }
        public DbSet<ListControlItemModel> ListItems { get; set; }
        public DbSet<FileUploadModel> FileUploads { get; set; }
        public DbSet<CheckBoxModel> CheckBoxes { get; set; }
        public DbSet<FormContent> FormContents { get; set; }
        public DbSet<FormContentItem> FormContentItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new FormModelMap());
            modelBuilder.Configurations.Add(new TextBoxModelMap());
            modelBuilder.Configurations.Add(new ListControlModelMap());
            modelBuilder.Configurations.Add(new ListControlItemModelMap());
            modelBuilder.Configurations.Add(new FileUploadModelMap());
            modelBuilder.Configurations.Add(new FormContentMap());
            modelBuilder.Configurations.Add(new FormContentItemMap());
            modelBuilder.Configurations.Add(new CheckBoxModelMap());
        }
    }
}