using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI.HtmlControls;

namespace NikSoft.Web
{
    public partial class index : NikmehrPage, INotification
    {

        public IUnitOfWork uow { get; set; }
        public IUserService iUserServ { get; set; }
        public ITemplateFileService iPagecssFileServ { get; set; }
        public INikSettingService iNikSettingServ { get; set; }
        public IPortalService iPortalServ { get; set; }
        public IPortalAddressService iPortalAddrServ { get; set; }


        public ITemplateService iTemplateServ { get; set; }



        private const string ADMIN_MODULE = "admin";
        private const string UI_MODULE = "modules";
        private Portal portal = null;

        protected override void InitializeCulture()
        {
            //read and fill, modulename, level and moduleparameters
            base.InitializeCulture();
            if (IsAnyHackAttempt)
            {
                Issue404Response();
                return;
            }
            GetPortal();
            if (Is404Issued)
            {
                return;
            }

            GetLoginnedUser();
            SetRouting();

            if (Level.ToLower() == "panel" && null != PortalUser)
            {
                Direction = "rtl";
                Language = "fa";
            }
            else
            {
                Direction = portal.Direction;
                Language = portal.PortalLanguage.ToString();
                Language = Language.IsEmpty() ? "fa" : Language.Trim().ToLower();
            }

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Language);
            Page.Title = iNikSettingServ.GetSettingValue("SiteTitle", NikSettingType.SystemSetting, CurrentPortalID);


            if (null != PortalUser && Level.ToLower() == "panel")
            {
                var portal2 = iPortalServ.Find(x => x.ID == PortalUser.PortalID);
                if (portal2.ShowOwnLogo)
                {
                    this.Page.Title = iNikSettingServ.GetSettingValue("SiteTitle", NikSettingType.SystemSetting, portal2.ID);
                }
            }
        }


        private void LoadFavIcon()
        {
            if (portal == null)
            {
                return;
            }
            if (portal.Favicon.IsEmpty())
            {
                return;
            }

            var link = new HtmlLink();

            if (null != PortalUser && Level.ToLower() == "panel")
            {
                var portal2 = iPortalServ.Find(x => x.ID == PortalUser.PortalID);
                if (portal2.ShowOwnLogo)
                {
                    link.Attributes.Add("type", "image/png");
                    link.Attributes.Add("rel", "shortcut icon");
                    link.Attributes.Add("href", ResolveUrl("~/" + portal2.Favicon));
                }
                else
                {
                    link.Attributes.Add("type", "image/png");
                    link.Attributes.Add("rel", "shortcut icon");
                    link.Attributes.Add("href", ResolveUrl("~/" + portal.Favicon));
                }

            }
            else
            {
                link.Attributes.Add("type", "image/png");
                link.Attributes.Add("rel", "shortcut icon");
                link.Attributes.Add("href", ResolveUrl("~/" + portal.Favicon));
            }




            h1.Controls.Add(link);
        }

        private const string LOCALHOST = "localhost";

        private void GetPortal()
        {
            if (null != Request.QueryString["news"] && !string.IsNullOrWhiteSpace(Request.QueryString["news"]))
            {
                ModuleParameters = Request.QueryString["news"];
            }



            if (Request.QueryString["templateID"] != null)
            {
                try
                {
                    var templateID = Request.QueryString["templateID"].ToInt32();
                    var t = iTemplateServ.Find(x => x.ID == templateID);
                    portal = t.Portal;
                }
                catch
                {

                }
            }
            else
            {
                if (Request.IsLocal)
                {
                    if (string.IsNullOrWhiteSpace(Level))
                    {
                        Response.Redirect("~/home/page/default", true);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }
                    else
                    {
                        if (Level == "panel")
                        {
                            portal = iPortalServ.Find(x => x.ID == 1);
                        }
                        else
                        {
                            //TODO:
                            portal = iPortalServ.Find(x => Level == x.Alias);
                            if (null == portal)
                                portal = iPortalServ.Find(x => x.ID == 1);
                        }
                    }
                }
                else
                {// ! is localhost
                    if (string.IsNullOrWhiteSpace(Level))
                    {

                        portal = iPortalServ.Find(x => Level == x.Alias && x.PortalAddresses.Any(t => t.DomainAddress == Domain));
                        if (null == portal)
                        {
                            var addressList = iPortalAddrServ.Find(x => x.DomainAddress == Domain);
                            var portalList = iPortalServ.GetAll(x => x.Domain == Domain);
                            if (null != addressList)
                            {
                                portal = addressList.Portal;
                            }
                            else
                            {
                                if (portalList.Count > 0)
                                {
                                    portal = portalList[0];
                                }
                                if (portal == null)
                                {
                                    portalList = iPortalServ.GetAll(x => Level == x.Alias);
                                    if (null != portalList && 0 < portalList.Count)
                                    {
                                        portal = portalList[0];
                                    }
                                    else
                                    {
                                        portal = iPortalServ.Find(x => x.ID == 1);
                                    }
                                }
                            }
                        }
                        //TODO: BUG
                        if (null == portal)
                        {
                            var portalList2 = iPortalServ.GetAll(x => x.Alias == Level);
                            if (portalList2.Count > 0)
                            {
                                portal = portalList2[0];
                            }
                        }
                        else
                        {
                            Response.Redirect("~/" + portal.Alias + "/page/default", true);
                            Context.ApplicationInstance.CompleteRequest();
                            return;
                        }
                    }
                    else
                    {
                        if (Level == "panel")
                        {
                            portal = iPortalServ.Find(x => x.ID == 1);
                        }
                        else
                        {
                            portal = iPortalServ.Find(x => Level == x.Alias && x.PortalAddresses.Any(t => t.DomainAddress == Domain));
                            if (null == portal)
                            {
                                var portalList = iPortalServ.GetAll(x => x.Domain == Domain);
                                if (portalList.Count > 0)
                                {
                                    portal = portalList[0];
                                }
                            }
                        }
                    }
                }
            }


            //TODO: we must issue a 404 not found
            //in case
            if (null == portal)
            {
                Issue404Response();
                return;
            }
            CurrentPortalID = portal.ID;
            CurrentPortalPath = portal.AliasFolder;

        }

        private void GetLoginnedUser()
        {
            PortalUser = null;
            var sbl = new SingleSignOnService();
            var theUser = sbl.AuthenticateFromContext(iUserServ);
            if (null != theUser && theUser.ID > 0)
            {
                PortalUser = theUser;
                var lang = iPortalServ.Find(x => x.ID == PortalUser.PortalID, x => x.PortalLanguage);
                ConvertYeKe = lang.ToString().Trim().ToLower() == "fa".Trim().ToLower();
                return;
            }
        }

        protected void SetRouting()
        {
            TemplateType = TemplateType.HomePage;
            switch (Level)
            {
                case "home":
                    {
                        if ("page" != ModuleName)
                        {
                            TemplateType = TemplateType.InnerPage;
                        }
                        break;
                    }
                case "panel":
                    {
                        TemplateType = TemplateType.PanelHome;
                        if ("page" != ModuleName)
                        {
                            TemplateType = TemplateType.PanelInner;
                        }
                        break;
                    }
                default:
                    {
                        TemplateType = TemplateType.HomePage;
                        var portals = iPortalServ.GetAll(x => Level == x.Alias && x.PortalAddresses.Any(t => t.DomainAddress == Domain));
                        if (null != portal && portals.Count > 0)
                        {
                            if ("page" != ModuleName)
                            {
                                TemplateType = TemplateType.InnerPage;
                            }
                        }
                        else
                        {
                            portals = iPortalServ.GetAll(x => Level == x.Alias);
                            if (null != portal && portals.Count > 0)
                            {
                                if ("page" != ModuleName)
                                {
                                    TemplateType = TemplateType.InnerPage;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadFavIcon();
            if (null != portal)
            {
                Utilities.Utilities.MetaDescription(this, portal.MetaDescription);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            phtml.Attributes.Add("lang", Language);
            phtml.Attributes.Add("dir", Direction);
        }

        public void SetErrorMessage(string msg)
        {
            nb.AddMessage(msg, MessageType.Error, Layout.Top, 15);
        }

        public void SetSuccessMessage(string msg)
        {
            nb.AddMessage(msg, MessageType.Success, Layout.Top, 15);
        }

        public void SetOptionalMessage(string msg, MessageType mtype, Layout mlayout, int timeout = 0)
        {
            nb.AddMessage(msg, mtype, mlayout, timeout);
        }

        public void SetErrorMessage(List<string> msg)
        {
            nb.AddMessage(msg, MessageType.Error, Layout.Top, 15);
        }

        public void SetSuccessMessage(List<string> msg)
        {
            nb.AddMessage(msg, MessageType.Success, Layout.Top, 15);
        }

        public void SetOptionalMessage(List<string> msg, MessageType mtype, Layout mlayout, int timeout = 0)
        {
            nb.AddMessage(msg, mtype, mlayout, timeout);
        }

    }
}