using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Theme
{
    public partial class cu_Theme : NikUserControl
    {

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }
            if (ItemID == 0)
            {
                if (txtEnTitle.Text.IsEmpty() || !txtEnTitle.Text.StringIsEnglish())
                {
                    ErrorMessage.Add("لطفا از حروف انگلیسی از A تا Z استفاده کنید.");
                }
                if (fuFile.HasFile)
                {
                    if (!Utilities.Utilities.CheckFileFormat(fuFile.FileName, "zip"))
                    {
                        ErrorMessage.Add("فایل باید به صورت .zip باشد.");
                    }
                }
            }
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void GetEditData()
        {
            fileupload.Visible = false;
            colentitle.Visible = false;
            var data = iThemeServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            ThemImage.ImageUrl = "/" + data.ThemeImg;
        }

        private bool UnZipAndCopy()
        {
            var isOK = false;
            string fileName = string.Empty; ;
            isOK = Utilities.Utilities.UploadFile(fuFile.PostedFile, "Cache", ref fileName, PortalUser.PortalFolderPath);
            if (!isOK)
            {
                return false;
            }
            Utilities.Utilities.UnZip("~/" + fileName, "~/files/cache/" + txtEnTitle.Text);
            var files = new DirectoryInfo(Server.MapPath("~/files/cache/" + txtEnTitle.Text)).GetFiles("*.*", SearchOption.AllDirectories).ToList();
            if (files.Count == 0)
            {
                return false;
            }
            bool noSkin = true;
            foreach (var item in files.Where(t => t.Extension == ".ascx"))
            {
                var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(item.FullName)) as NikSkinTemplate;
                if (cntrl != null)
                {
                    noSkin = false;
                    break;
                }
            }
            if (noSkin)
            {
                return false;
            }
            Utilities.Utilities.CopyFile(Server.MapPath("~/files/cache/" + txtEnTitle.Text), Server.MapPath("~/Templates/" + txtEnTitle.Text));
            try
            {
                File.Delete(Server.MapPath("~/" + fileName));
                Directory.Delete(Server.MapPath("~/files/cache/" + txtEnTitle.Text), true);
            }
            catch { }
            return true;
        }

        private void SaveNewData()
        {

            if (!Directory.Exists(Server.MapPath("~/Templates/" + txtEnTitle.Text)))
            {
                Directory.CreateDirectory(Server.MapPath("~/Templates/" + txtEnTitle.Text));
            }

            if (fuFile.HasFile)
            {
                var isOK = UnZipAndCopy();
                if (isOK)
                {
                }
                else
                {
                    Notification.SetErrorMessage("Upload File Faild! something wrong");
                }
            }
            string vpath = "ThemeImage";
            string Icon = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref Icon, PortalUser.PortalFolderPath);
            }
            var theme = iThemeServ.Create();
            theme.Title = txtTitle.Text.Trim();
            theme.ThemePath = "Templates/" + txtEnTitle.Text;
            theme.ThemeImg = Icon;
            theme.PortalID = PortalUser.PortalID;
            iThemeServ.Add(theme);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            try
            {
                Response.Write("0");
                string vpath = "ThemeImage";
                string Icon = "";
                var isokImmage = false;
                if (fuIcon.HasFile)
                {
                    isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref Icon, PortalUser.PortalFolderPath);
                }
                var theme = iThemeServ.Find(y => y.ID == ItemID);
                theme.Title = txtTitle.Text.Trim();
                if (isokImmage)
                {
                    if (!Icon.IsEmpty())
                    {
                        Utilities.Utilities.RemoveItemFile(theme.ThemeImg);
                        theme.ThemeImg = Icon;
                    }
                }

                uow.SaveChanges(PortalUser.ID);
                Container.gotoList();
            }
            catch
            {
                Notification.SetErrorMessage("آیکن آپلود نشد.");
            }
        }

        protected void BtnRemoveImg_Click(object sender, EventArgs e)
        {
            var DataItem = iThemeServ.Find(y => y.ID == ItemID);
            if (!DataItem.ThemeImg.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ThemeImg);
                DataItem.ThemeImg = "";
                ThemImage.ImageUrl = "";
                iThemeServ.SaveChanges();
            }
        }
    }
}