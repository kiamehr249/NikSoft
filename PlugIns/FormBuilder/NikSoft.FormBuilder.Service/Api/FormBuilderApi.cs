using NikSoft.Model;
using NikSoft.Services;
using StructureMap;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace NikSoft.FormBuilder.Service.Api
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class FormBuilderApi : WebService
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        private NikPortalUser portalUser;
        private IUserService iUserServ;

        public FormBuilderApi()
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
        public string GetControlListItems(int listControlId)
        {
            HttpContext.Current.Response.Clear();
            Context.Response.ContentType = "application/json";

            var iListControlItemModelServ = ObjectFactory.GetInstance<IListControlItemModelService>();

            var listItems = iListControlItemModelServ.GetAll(x => x.ListControlID == listControlId, y => new { y.ID, y.Title }).ToList();
            listItems.Insert(0, new {ID = 0, Title = "انتخاب کنید" });
            return serializer.Serialize(listItems);
        }


        private void GetResponse(object obj)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.Write(serializer.Serialize(obj));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

    }
}