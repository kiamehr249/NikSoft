using NikSoft.ContentManager.Service;
using NikSoft.Model;
using NikSoft.Services;
using NikSoft.Utilities;
using StructureMap;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace NikSoft.ContentManager.WebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class ContentWebService : System.Web.Services.WebService
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        private NikPortalUser portalUser;
        private IUserService iUserServ;

        public ContentWebService()
        {
            SetCurrentUser();
        }

        public void SetCurrentUser()
        {
            portalUser = null;
            iUserServ = ObjectFactory.GetInstance<IUserService>();
            var sbl = new SingleSignOnService();
            var theUser = sbl.AuthenticateFromContext(iUserServ);
            if (null != theUser && theUser.ID > 0)
            {
                portalUser = theUser;
                return;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetContentCategory(int GroupID)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            var iCatPermission = ObjectFactory.GetInstance<ICategoryPermissionService>();

            var allpermits = iCatPermission.GetAll(x => x.PortalID == portalUser.PortalID && x.UserID == portalUser.ID).Select(x => x.CategoryID).ToList();

            DBUtilities db = new DBUtilities();
            string Query1 = @"Select cc1.ID, ISnull(cc3.Title+ ' - ', '') + ISnull( cc2.Title + ' - ', '') + cc1.Title AS Title From CM_ContentCategories cc1
                        Left Join CM_ContentCategories cc2  On cc1.ParentID = cc2.ID
                        LEFT Join CM_ContentCategories cc3  On cc2.ParentID = cc3.ID
                        Where cc1.PortalID = " + portalUser.PortalID + " AND cc1.GroupID = " + GroupID + " Order by cc3.ordering, cc2.ordering, cc1.ordering";
            var cmd = db.ReturnQuery(Query1);
            var listMaker = new ListJResultQuery();
            var Cats = listMaker.GetResults(cmd, true, "انتخاب کنید");
            return serializer.Serialize(Cats.Where(x => allpermits.Contains(x.ID) || x.ID == 0));
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetContentCategory2(int GroupID)
        {
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";

            DBUtilities db = new DBUtilities();
            string Query1 = @"Select cc1.ID, ISnull(cc3.Title+ ' - ', '') + ISnull( cc2.Title + ' - ', '') + cc1.Title AS Title From CM_ContentCategories cc1
                        Left Join CM_ContentCategories cc2  On cc1.ParentID = cc2.ID
                        LEFT Join CM_ContentCategories cc3  On cc2.ParentID = cc3.ID
                        Where cc1.PortalID = " + portalUser.PortalID + " AND cc1.GroupID = " + GroupID + " Order by cc3.ordering, cc2.ordering, cc1.ordering";
            var cmd = db.ReturnQuery(Query1);
            var listMaker = new ListJResultQuery();
            var Cats = listMaker.GetResults(cmd, true, "انتخاب همه");
            return serializer.Serialize(Cats);
        }


        private void SerializeIt(object obj)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.Write(serializer.Serialize(obj));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}
