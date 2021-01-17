using NikSoft.Services;
using NikSoft.UILayer;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.EngineBase
{
    public partial class UserModuleLoader : WidgetUIContainer, IEngineContainer
    {


        private string controlpath = "";
        private string PureModuleKey = "";
        private const int MIN_MODULE_LENGHT = 4;
        private const int MAX_MODULE_LENGHT = 50;
        private const string PAGELOCK = "~/modules/BaseModules/EngineBase/LockPage.ascx";
        private const string PAGELOCK_INNOCENT = "~/modules/BaseModules/EngineBase/EmptyModule.ascx";

        public INikModuleService iNikModulesService { get; set; }

        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(ModuleName))
            {
                LoadLockPage();
                return;
            }

            if (ModuleName.Length < MIN_MODULE_LENGHT || ModuleName.Length > MAX_MODULE_LENGHT)
            {
                LoadLockPage();
                return;
            }
            var moduleinfo = iNikModulesService.GetAll(x => x.ModuleKey == ModuleName, x => new { x.ID, x.ModuleKey, x.IsGeneralModule, x.ModuleFile, x.IsXMLBase, x.Title, x.LoginRequired, x.SecondTitle });
            if (null == moduleinfo || 1 != moduleinfo.Count())
            {
                if (Request.QueryString["templateID"] != null)
                {
                    ph1.Controls.Add(LoadControl(PAGELOCK_INNOCENT));
                }
                else
                {
                    try
                    {
                        RenderAddressNotFound();
                        return;
                    }
                    catch (ThreadAbortException ex)
                    {
                        Response.Write(ex.Message);
                    }
                }
            }

            var tmInfo = moduleinfo.First();

            if (tmInfo.LoginRequired && Level.ToLower() != "Panel")
            {
                LoadLockPage();
                return;
            }
            else if (tmInfo.IsGeneralModule && null == PortalUser)
            {
                if (!string.IsNullOrWhiteSpace(ModuleParameters) && "default" != ModuleParameters.ToLower())
                {
                    RedirectToModuleWithForce("login?after=" + ModuleName + "/" + ModuleParameters);
                    return;
                }
                RedirectToModuleWithForce("login?after=" + ModuleName);
                return;
            }


            string othertest = ModuleName.Substring(0, 2).ToLower();

            string Title = moduleinfo.First().Title;
            if (Language != "fa" && !string.IsNullOrEmpty(moduleinfo.First().SecondTitle))
                Title = moduleinfo.First().SecondTitle;
            else
                Title = moduleinfo.First().Title;

            Page.Title = Title;

            PureModuleKey = ModuleName.Substring(3);
            if (othertest == "rd" || othertest == "cu")
            {
            }

            controlpath = tmInfo.ModuleFile;
            var newPath = "files/" + CurrentPortalPath + "/modules/";
            if (File.Exists(Server.MapPath("~/" + newPath + "m_" + tmInfo.ID + "_" + tmInfo.ModuleKey + ".ascx")))
            {
                controlpath = newPath + "m_" + tmInfo.ID + "_" + tmInfo.ModuleKey + ".ascx";
            }

            controlpath = "~/" + controlpath;

            if (tmInfo.IsXMLBase)
            {
                if (othertest == "rd" || othertest == "cu"
                    || othertest == "vd" || othertest == "rp")
                {
                    if (othertest == "rd")
                    {
                        //Read, Delete
                        controlpath = "~/modules/xmldata/rd_ui.ascx";
                    }
                    else if (othertest == "cu")
                    {
                        //Create, Update
                        controlpath = "~/modules/xmldata/cu_ui.ascx";
                    }
                    else if (othertest == "vd")
                    {
                        //View Detail
                        controlpath = "~/modules/xmldata/rd_ui.ascx";
                    }
                    else if (othertest == "rp")
                    {
                        //Report Viewer
                        controlpath = "~/modules/xmldata/rd_ui.ascx";
                    }
                }
            }
            try
            {
                if (string.Empty != controlpath)
                {
                    Control ctl = LoadControl(controlpath);
                    INikContentManage WidgetControl = ctl as INikContentManage;
                    if (WidgetControl != null)
                    {
                        WidgetControl.InitHost(this);
                        WidgetControl.ContentNotFound += () => {
                            MustLoadContentNotFoundPage = true;
                        };
                    }
                    else { }

                    ph1.Controls.Add(ctl);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }


        bool MustLoadContentNotFoundPage = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (MustLoadContentNotFoundPage)
            {
                RenderContentNotFound();
            }
        }


        public void RenderAddressNotFound()
        {
            var _404Path = Server.MapPath("~/404Pages/404P1.html");
            string text = File.ReadAllText(_404Path);
            //Response.Clear();
            //Response.ContentType = "text/html; charset=utf-8";
            //Response.Write(text);
            Response.StatusCode = 404;


            var l1 = new Literal();
            l1.Text = text;

            ph1.Controls.Add(l1);

            //must save IP to file, to fully black this one
            //Response.Flush();
            //Response.End();
        }


        public void RenderContentNotFound()
        {
            var _404Path = Server.MapPath("~/404Pages/404P1.html");
            string text = File.ReadAllText(_404Path);
            //Response.Clear();
            //Response.ContentType = "text/html; charset=utf-8";
            //Response.Write(text);
            Response.StatusCode = 404;


            var l1 = new Literal();
            l1.Text = text;

            ph1.Controls.Add(l1);

            //must save IP to file, to fully black this one
            //Response.Flush();
            //Response.End();
        }

        private void LoadLockPage()
        {
            Response.StatusCode = 404;
            ph1.Controls.Add(LoadControl(PAGELOCK));
        }

        public event NewItemEventHandler NewItem;

        public event GotoListEventHandler GotoList;

        public void gotoList()
        {
            throw new NotImplementedException();
        }

        public void gotonewItem()
        {
            throw new NotImplementedException();
        }

        public void gotoEditURI(string iID)
        {
            throw new NotImplementedException();
        }

        public string getEditURI(string iID)
        {
            throw new NotImplementedException();
        }

        public bool hasCUPermissionCurrentUser()
        {
            throw new NotImplementedException();
        }

        public bool hasRDPermissionCurrentUser()
        {
            throw new NotImplementedException();
        }

        public LinkButton btnDelete
        {
            get { throw new NotImplementedException(); }
        }

        public void gotoEditItem()
        {
            throw new NotImplementedException();
        }

        public string ModuleHeader { get; set; }

        public string EditItemKey
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string AddNewKey
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void ShowSearch()
        {
            throw new NotImplementedException();
        }

        public void HideSearch()
        {
            throw new NotImplementedException();
        }

        protected void LHome_Click(object sender, EventArgs e)
        {
            RedirectHome();
        }
    }
}