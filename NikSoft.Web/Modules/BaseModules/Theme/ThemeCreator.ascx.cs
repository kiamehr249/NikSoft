using NikSoft.UILayer;
using NikSoft.UILayer.WebControls;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Theme
{
    public partial class ThemeCreator : NikUserControl
    {
        protected int themeID;

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ModuleParameters.IsNumeric())
            {
                RedirectTo("~/panel/rd_theme/view");
            }
            themeID = ModuleParameters.ToInt32();
            BoundData();
        }

        protected override void BoundData()
        {
            var skinList = new List<NikSkinTemplate>();
            var blockList = new List<BlockTemplate>();
            var cssList = new List<FileInfo>();
            var JSList = new List<FileInfo>();
            var theme = iThemeServ.Find(t => t.ID == themeID);
            if (theme == null)
            {
                RedirectTo("~/panel/rd_theme/view");
            }
            else
            {
                if (theme.PortalID != PortalUser.PortalID)
                {
                    newFilePanel.Visible = false;
                }
            }
            var files = new DirectoryInfo(Server.MapPath("~/" + theme.ThemePath)).GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file.Extension == ".ascx")
                {
                    var blockControl = LoadControl(Utilities.Utilities.PhysicalToVirtual(file.FullName)) as BlockTemplate;
                    if (blockControl != null)
                    {
                        blockList.Add(blockControl);
                    }
                    var skinControl = LoadControl(Utilities.Utilities.PhysicalToVirtual(file.FullName)) as NikSkinTemplate;
                    if (skinControl != null)
                    {
                        skinList.Add(skinControl);
                    }
                }
                else if (file.Extension == ".css")
                {
                    cssList.Add(file);
                }
                else if (file.Extension == ".js")
                {
                    JSList.Add(file);
                }
            }
            GVSkin.DataSource = skinList;
            GVSkin.DataBind();

            GVBlock.DataSource = blockList;
            GVBlock.DataBind();

            GVCss.DataSource = cssList;
            GVCss.DataBind();

            GvJS.DataSource = JSList;
            GvJS.DataBind();
        }

        protected string GetSkinTitle(object obj)
        {
            if (obj is BlockTemplate)
            {
                var skinTitle = (obj as BlockTemplate).Controls.GetAllChilds<SkinTitle>().FirstOrDefault();
                if (skinTitle != null)
                {
                    return skinTitle.Title;
                }
            }
            if (obj is NikSkinTemplate)
            {
                var skinTitle = (obj as NikSkinTemplate).Controls.GetAllChilds<SkinTitle>().FirstOrDefault();
                if (skinTitle != null)
                {
                    return skinTitle.Title;
                }
            }
            return string.Empty;
        }

        protected void GV_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (rbtUploadFile.Checked)
            {
                UploadAndSave();
            }
            else if (rbtCreateFile.Checked)
            {
                CreateAndSave();
            }
        }

        private void CreateAndSave()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان فایل را وارد کنید.");
            }
            if (txtFileName.Text.IsEmpty())
            {
                ErrorMessage.Add("نام انگلیسی فایل را وارد کنید.");
            }
            else
            {
                if (!txtFileName.Text.StringIsEnglish())
                {
                    ErrorMessage.Add("لطفا، نام انگلیسی را با حروف A تا Z و بدون فاصله وارد کنید.");
                }
            }
            if (ddlFileType.SelectedIndex == 0)
            {
                ErrorMessage.Add("نوع فایل را انتخاب کنید.");
            }
            if (ErrorMessage.Count > 0)
            {
                Notification.SetErrorMessage(ErrorMessage);
                return;
            }
            string fileName = string.Empty;
            string fileContent = string.Empty;
            switch (ddlFileType.SelectedIndex)
            {
                case 1:
                    {
                        fileName = Path.GetFileNameWithoutExtension(txtFileName.Text) + ".ascx";
                        fileContent = "<%@ Control Language=\"C#\" Inherits=\"NikSoft.UILayer.NikSkinTemplate\" %>\n";
                        fileContent += "<Nik:SkinTitle runat=\"server\" Title=\"" + txtTitle.Text + "\"></Nik:SkinTitle>";
                        break;
                    }
                case 2:
                    {
                        fileName = Path.GetFileNameWithoutExtension(txtFileName.Text) + ".ascx";
                        fileContent = "<%@ Control Language=\"C#\" Inherits=\"NikSoft.UILayer.BlockTemplate\" %>\n";
                        fileContent += "<Nik:SkinTitle runat=\"server\" Title=\"" + txtTitle.Text + "\"></Nik:SkinTitle>";
                        break;
                    }
                case 3:
                    {
                        fileName = Path.GetFileNameWithoutExtension(txtFileName.Text) + ".css";
                        fileContent = "";
                        break;
                    }
                case 4:
                    {
                        fileName = Path.GetFileNameWithoutExtension(txtFileName.Text) + ".js";
                        fileContent = "";
                        break;
                    }
            }
            var theme = iThemeServ.Find(t => t.ID == themeID);
            if (theme == null)
            {
                RedirectTo("~/panel/rd_theme/view");
            }
            if (File.Exists(Server.MapPath("~/" + theme.ThemePath + "/" + fileName)))
            {
                Notification.SetErrorMessage("فایل با همین نام قبلا ثبت شده است.");
                return;
            }
            using (StreamWriter outfile = new StreamWriter(Server.MapPath("~/" + theme.ThemePath + "/" + fileName), false, Encoding.UTF8))
            {
                outfile.Write(fileContent);
            }
            Notification.SetSuccessMessage("تغییرات با موفقیت انجام شد.");
            BoundData();
        }

        private void UploadAndSave()
        {
            if (!fuSkin.HasFile)
            {
                Notification.SetErrorMessage("هیچ فایلی انتخاب نشده است.");
                return;
            }
            if (Path.GetExtension(fuSkin.FileName).ToLower() != ".zip")
            {
                Notification.SetErrorMessage("فرمت فایل باید zip باشد.");
                return;
            }
            var theme = iThemeServ.Find(t => t.ID == themeID);
            if (theme == null)
            {
                RedirectTo("~/panel/rd_theme/view");
            }
            var isOK = UnZipAndCopy(theme.ThemePath.Substring(theme.ThemePath.IndexOf("/") + 1, theme.ThemePath.Length - theme.ThemePath.IndexOf("/") - 1));
            if (!isOK)
            {
                Notification.SetErrorMessage("آپلود فایل انجام نشد.");
            }
            else
            {
                Notification.SetSuccessMessage("فایل آپلود شد.");
                BoundData();
            }
        }

        private bool UnZipAndCopy(string themeTitle)
        {
            bool isOK;
            string fileName = string.Empty; ;
            isOK = Utilities.Utilities.UploadFile(fuSkin.PostedFile, "Cache", ref fileName, PortalUser.PortalFolderPath);
            if (!isOK)
            {
                return false;
            }
            Utilities.Utilities.UnZip("~/" + fileName, "~/files/cache/" + themeTitle);
            var files = new DirectoryInfo(Server.MapPath("~/files/cache/" + themeTitle)).GetFiles("*.*", SearchOption.AllDirectories).ToList();
            if (files.Count == 0)
            {
                return false;
            }
            Utilities.Utilities.CopyFile(Server.MapPath("~/files/cache/" + themeTitle), Server.MapPath("~/Templates/" + themeTitle));
            try
            {
                File.Delete(Server.MapPath("~/" + fileName));
                Directory.Delete(Server.MapPath("~/files/cache/" + themeTitle), true);
            }
            catch { }
            return true;
        }
    }
}