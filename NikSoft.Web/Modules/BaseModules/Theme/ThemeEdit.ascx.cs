using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Theme
{
    public partial class ThemeEdit : WidgetUIContainer
    {
        private int themeID;
        private string cntrlName;
        protected string Editor = string.Empty;
        protected string Mode = string.Empty;
        private int type;

        protected void Page_Load(object sender, EventArgs e)
        {
            err_msgs.Visible = false;
            if (!ModuleParameters.IsNumeric())
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            if (!Request.QueryString["t"].IsNumeric())
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            if (Request.QueryString["f"] == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            cntrlName = Request.QueryString["f"];
            type = Request.QueryString["t"].ToInt32();
            themeID = ModuleParameters.ToInt32();
            if (!IsPostBack)
            {
                LoadTheme();
            }
        }

        private void LoadTheme()
        {
            var theme = iThemeServ.Find(t => t.ID == themeID);
            if (theme == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            else
            {
                if (theme.PortalID != PortalUser.PortalID)
                {
                    btnSave.Visible = false;
                }
            }

            switch (type)
            {
                case 1:
                    {
                        Mode = "text/html";
                        LoadSkin(theme);
                        break;
                    }
                case 2:
                    {
                        Mode = "text/html";
                        LoadBlock(theme);
                        break;
                    }
                case 3:
                    {
                        Mode = "text/css";
                        LoadCss(theme);
                        break;
                    }
                case 4:
                    {
                        Mode = "text/javascript";
                        LoadJS(theme);
                        break;
                    }
                default:
                    {
                        RedirectTo("~/panel/rd_theme/view");
                        break;
                    }
            }
        }

        private void LoadCss(NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".css", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                Notification.SetErrorMessage("Exist same name aleady here<br />" + cntrlName + ".css");
                return;
            }
            using (StreamReader reader = new StreamReader(files[0].FullName, Encoding.UTF8))
            {
                Editor = reader.ReadToEnd();
            }
            txtCode.Text = Editor;
        }

        private void LoadJS(NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".js", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                Notification.SetErrorMessage("Exist same name aleady here<br />" + cntrlName + ".js");
                return;
            }
            using (StreamReader reader = new StreamReader(files[0].FullName, Encoding.UTF8))
            {
                Editor = reader.ReadToEnd();
            }
            txtCode.Text = Editor;
        }


        private void LoadBlock(NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".ascx", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(files[0].FullName)) as BlockTemplate;
            if (cntrl == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            using (StreamReader reader = new StreamReader(files[0].FullName, Encoding.UTF8))
            {
                Editor = reader.ReadToEnd();
            }
            txtCode.Text = Editor;
        }

        private void LoadSkin(NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".ascx", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(files[0].FullName)) as NikSkinTemplate;
            if (cntrl == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            using (StreamReader reader = new StreamReader(files[0].FullName, Encoding.UTF8))
            {
                Editor = reader.ReadToEnd();
            }
            txtCode.Text = Editor;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var theme = iThemeServ.Find(t => t.ID == themeID);
            var txt = txtCode.Text;
            switch (type)
            {
                case 1:
                    {
                        Mode = "text/html";
                        SaveSkin(txt, theme);
                        break;
                    }
                case 2:
                    {
                        Mode = "text/html";
                        SaveBlock(txt, theme);
                        break;
                    }
                case 3:
                    {
                        Mode = "text/css";
                        SaveCss(txt, theme);
                        break;
                    }
                case 4:
                    {
                        Mode = "text/javascript";
                        SaveJS(txt, theme);
                        break;
                    }
                default:
                    {
                        RedirectTo("~/panel/rd_theme/view");
                        break;
                    }
            }
        }

        private void SaveCss(string txt, NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".css", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                Notification.SetErrorMessage("Exist same name aleady here<br />" + cntrlName + ".css");
                return;
            }
            using (StreamWriter outfile = new StreamWriter(files[0].FullName, false, Encoding.UTF8))
            {
                outfile.Write(txt);
            }
        }


        private void SaveJS(string txt, NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".js", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                Notification.SetErrorMessage("Exist same name aleady here<br />" + cntrlName + ".js");
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            using (StreamWriter outfile = new StreamWriter(files[0].FullName, false, Encoding.UTF8))
            {
                outfile.Write(txt);
            }
        }

        private void SaveBlock(string txt, NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".ascx", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(files[0].FullName)) as BlockTemplate;
            if (cntrl == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            using (StreamWriter outfile = new StreamWriter(files[0].FullName, false, Encoding.UTF8))
            {
                outfile.Write(txt);
            }
        }

        private void SaveSkin(string txt, NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".ascx", SearchOption.AllDirectories).ToList();
            if (files.Count != 1)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }
            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(files[0].FullName)) as NikSkinTemplate;
            if (cntrl == null)
            {
                RedirectTo("~/panel/rd_theme/view");
                return;
            }

            var fullFileName = CreateTempFile(txt, theme);

            var cntrl2 = LoadControl(Utilities.Utilities.PhysicalToVirtual(fullFileName)) as NikSkinTemplate;

            List<int> plIDs = new List<int>();
            List<int> duplicateIDs = new List<int>();

            var isTextValid = true;

            //check for repeated data-mc attribute
            foreach (Control item in cntrl2.Controls)
            {
                if (item is HtmlGenericControl)
                {
                    if (!(item as HtmlGenericControl).Attributes["data-mc"].IsEmpty() && (item as HtmlGenericControl).Attributes["data-mc"].IsNumeric())
                    {
                        var datamcID = (item as HtmlGenericControl).Attributes["data-mc"].ToInt32();
                        if (plIDs.Contains(datamcID))
                        {
                            isTextValid = false;
                            duplicateIDs.Add(datamcID);
                        }
                        plIDs.Add(datamcID);
                    }
                }
            }

            if (!isTextValid)
            {
                Notification.SetErrorMessage("We have a problem pleas chack the place numbers");
                err_msgs.Visible = true;
                lNums.Text = "";
                foreach (var item in duplicateIDs)
                {
                    lNums.Text += "<li>" + item + "</li>";
                }
                return;
            }

            if (File.Exists(fullFileName))
            {
                File.Delete(fullFileName);
            }

            using (StreamWriter outfile = new StreamWriter(files[0].FullName, false, Encoding.UTF8))
            {
                outfile.Write(txt);
            }
        }



        private string CreateTempFile(string txt, NikModel.Theme theme)
        {
            var path = theme.ThemePath;
            var files = new DirectoryInfo(Server.MapPath("~/" + path)).GetFiles(cntrlName + ".ascx", SearchOption.AllDirectories).ToList();
            string fileName = string.Empty;
            string fileContent = string.Empty;
            fileName = Path.GetFileNameWithoutExtension(files[0].FullName) + ".ascx";
            fileContent = txt;
            if (File.Exists(Server.MapPath("~/" + theme.ThemePath + "/tmp_" + fileName)))
            {
                File.Delete(Server.MapPath("~/" + theme.ThemePath + "/tmp_" + fileName));
            }
            using (StreamWriter outfile = new StreamWriter(Server.MapPath("~/" + theme.ThemePath + "/tmp_" + fileName), false, Encoding.UTF8))
            {
                outfile.Write(fileContent);
            }
            return Server.MapPath("~/" + theme.ThemePath + "/tmp_" + fileName);
        }





    }//=C
}