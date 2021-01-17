using System.Web.Routing;

namespace NikSoft.Routing
{
    public class EngineRouting
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Clear();
            routes.Ignore("{resource}.axd");
            routes.Ignore("{level}/{resource}.axd");
            routes.Ignore("{level}/{modulename}/{resource}.axd");

            routes.Ignore("{*allasmx}", new { allasmx = @".*\.asmx(/.*)?" });
            routes.Ignore("{*allcaptcha}", new { allcaptcha = @".*\.captcha.aspx(/.*)?" });

            routes.MapPageRoute("default2", "{level}/{modulename}/{moduleproperties}", "~/index.aspx", false, new RouteValueDictionary { { "modulename", "page" }, { "moduleproperties", "default" } });
            routes.MapPageRoute("default", "{level}/{modulename}/{*moduleproperties}", "~/index.aspx");
        }
    }
}