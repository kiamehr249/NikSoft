using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.IO;
using System.Text;

namespace NikSoft.Web.Modules.BaseModules.ModuleEdit
{
    public partial class EditNikModule : WidgetUIContainer
    {


        public INikModuleService iModuleServ { get; set; }

        private int ModuleID;
        protected string Editor = string.Empty;
        protected string Mode = string.Empty;
        private int type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModuleParameters.IsNumeric())
            {
                RedirectTo("~/panel/ShowEditableModules/view");
                return;
            }

            var m = iModuleServ.Find(x => x.ModuleKey == ModuleParameters);
            if (null == m)
            {
                RedirectTo("~/panel/ShowEditableModules/view");
                return;
            }

            type = 2;
            ModuleID = m.ID;
            if (!IsPostBack)
            {
                FirstLoad(m);
            }
        }

        private void FirstLoad(NikModule rm)
        {
            if (rm == null)
            {
                RedirectTo("~/panel/ShowEditableModules/view");
                return;
            }

            switch (type)
            {
                case 2:
                    {
                        Mode = "text/html";
                        LoadModule(rm);
                        break;
                    }
                default:
                    {
                        RedirectTo("~/panel/ShowEditableModules/view");
                        break;
                    }
            }
        }

        private void LoadModule(NikModule theModule)
        {
            var path = theModule.ModuleFile;
            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(Server.MapPath("~/" + path))) as WidgetUIContainer;
            if (cntrl == null)
            {
                Notification.SetErrorMessage("ماژول قابل ویرایش نیست مسیر چک شود.");
                return;
            }
            var newPath = "files/" + PortalUser.PortalFolderPath + "/modules/";
            var t = Server.MapPath("~/" + newPath + "m_" + theModule.ID + "_" + theModule.ModuleKey + ".ascx");
            if (File.Exists(t))
            {
                using (StreamReader reader = new StreamReader(t, Encoding.UTF8))
                {
                    Editor = reader.ReadToEnd();
                }
            }
            else
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/" + path), Encoding.UTF8))
                {
                    Editor = reader.ReadToEnd();
                }
            }



            txtCode.Text = Editor;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var m = iModuleServ.Find(t => t.ID == ModuleID);
            var txt = txtCode.Text;
            switch (type)
            {
                case 2:
                    {
                        Mode = "text/html";
                        SaveModule(txt, m);
                        break;
                    }
                default:
                    {
                        RedirectTo("~/panel/ShowEditableModules/view");
                        break;
                    }
            }
        }

        private void SaveModule(string txt, NikModule m)
        {
            var path = m.ModuleFile;
            var newPath = "files/" + PortalUser.PortalFolderPath + "/modules/";
            if (!Utilities.Utilities.EnsureDirectory(Server.MapPath("~/" + newPath)))
            {
                Notification.SetErrorMessage("دسترسی به این ماژول امکان ندارد.");
                return;
            }

            WidgetUIContainer cntrl = null;
            try
            {
                cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(Server.MapPath("~/" + path))) as WidgetUIContainer;
            }
            catch
            {
                Notification.SetErrorMessage("این ماژول قابل لود شدن نیست.");
            }

            if (cntrl == null)
            {
                Notification.SetErrorMessage("متن اشتباه است.");
                return;
            }
            using (StreamWriter outfile = new StreamWriter(Server.MapPath("~/" + newPath + "m_" + m.ID + "_" + m.ModuleKey + ".ascx"), false, Encoding.UTF8))
            {
                outfile.Write(txt);
            }
        }

    }
}