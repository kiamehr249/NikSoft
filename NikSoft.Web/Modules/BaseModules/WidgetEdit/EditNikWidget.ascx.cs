using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.IO;
using System.Text;

namespace NikSoft.Web.Modules.BaseModules.WidgetEdit
{
    public partial class EditNikWidget : WidgetUIContainer
    {


        public IWidgetService iwidgetServ { get; set; }

        private int ModuleID;
        private string cntrlName;
        protected string Editor = string.Empty;
        protected string Mode = string.Empty;
        private int type;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ModuleParameters.IsNumeric())
            {
                RedirectTo("~/panel");
                return;
            }

            var mId = ModuleParameters.ToInt32();

            var m = iwidgetServ.Find(x => x.ID == mId);
            if (null == m)
            {
                RedirectTo("~/paenl");
                return;
            }


            //cntrlName = m.
            type = 2;
            ModuleID = m.ID;
            if (!IsPostBack)
            {
                FirstLoad(m);
            }
        }

        private void FirstLoad(Widget rm)
        {
            if (rm == null)
            {
                RedirectTo("~/panel/showeditablemodules/view");
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
                        RedirectTo("~/panel/showeditablemodules/view");
                        break;
                    }
            }
        }

        private void LoadModule(Widget theModule)
        {
            var path = theModule.WidgetDefinition.Url;
            if (!string.IsNullOrWhiteSpace(theModule.NewUrl))
            {
                path = theModule.NewUrl;
            }

            var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(Server.MapPath("~/" + path))) as WidgetUIContainer;
            if (cntrl == null)
            {
                Notification.SetErrorMessage("You can not edit this widget");
                return;
            }
            using (StreamReader reader = new StreamReader(Server.MapPath("~/" + path), Encoding.UTF8))
            {
                Editor = reader.ReadToEnd();
            }
            txtCode.Text = Editor;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var m = iwidgetServ.Find(t => t.ID == ModuleID);
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
                        RedirectTo("~/panel/showeditablemodules/view");
                        break;
                    }
            }
        }

        private void SaveModule(string txt, Widget m)
        {
            var path = m.WidgetDefinition.Url;
            WidgetUIContainer cntrl = null;


            var newPath = "files/" + PortalUser.PortalFolderPath + "/widgets/";
            if (!Utilities.Utilities.EnsureDirectory(Server.MapPath("~/" + newPath)))
            {
                Notification.SetErrorMessage("Can not access to the edit widget");
                return;
            }

            try
            {
                cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(Server.MapPath("~/" + path))) as WidgetUIContainer;
            }
            catch
            {
            }

            if (cntrl == null)
            {
                Notification.SetErrorMessage("The entry text is wrong");
                return;
            }

            //, PortalUser.PortalFolderPath

            using (StreamWriter outfile = new StreamWriter(Server.MapPath("~/" + newPath + "w_" + m.ID + ".ascx"), false, Encoding.UTF8))
            {
                outfile.Write(txt);
                m.NewUrl = newPath + "w_" + m.ID + ".ascx";
                iwidgetServ.SaveChanges(PortalUser.PortalID);
            }

        }


    }
}