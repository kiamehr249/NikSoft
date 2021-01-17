using NikSoft.Model;
using NikSoft.Services;
using StructureMap;
using System;
using System.Web.UI;

namespace NikSoft.UILayer
{
    public class NikBaseUserControl : UserControl
    {
        public IUserService iUserService { get; set; }
        public IUnitOfWork uow { get; set; }


        public NikBaseUserControl()
        {
            ObjectFactory.BuildUp(this);
        }

        protected override void OnLoad(EventArgs e)
        {
            //SetValues(this.Controls);
            base.OnLoad(e);
        }

        protected int CurrentPortalID
        {
            get
            {
                return ((Page) as INikRoutedPage).CurrentPortalID;
            }
        }

        protected string CurrentPortalPath
        {
            get
            {
                return ((Page) as INikRoutedPage).CurrentPortalPath;
            }
        }

        protected string Level
        {
            get
            {
                return ((Page) as INikRoutedPage).Level;
            }
        }

        protected string ModuleName
        {
            get
            {
                return ((Page) as INikRoutedPage).ModuleName;
            }
        }

        protected string ModuleParameters
        {
            get
            {
                return ((Page) as INikRoutedPage).ModuleParameters;
            }
        }

        public NikPortalUser PortalUser
        {
            get
            {
                var NikUserInfo = (Page) as IPortalUser;
                return NikUserInfo != null ? NikUserInfo.PortalUser : null;
            }
            set
            {
                ((Page) as IPortalUser).PortalUser = value;
            }
        }

        protected bool AuthenticateUser()
        {
            if (null == PortalUser)
            {
                return false;
            }
            return true;
        }

        protected string GetRoutedClientURI(string uri)
        {
            return Page.ResolveClientUrl(uri);
        }

        protected void RedirectTo(string locationToGo)
        {
            Response.Redirect(locationToGo, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        public INotification Notification
        {
            get { return (Page) as INotification; }
        }

    }
}
