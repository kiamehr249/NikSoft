using NikSoft.Model;
using System.IO;
using System.Web.UI;

namespace NikSoft.PlugIn
{
    public class PlugInPage : Page
    {
        protected bool IsAnyHackAttempt = false;
        protected bool Is404Issued = false;
        public string Level { get; set; }
        public string ModuleName { get; set; }
        public string ModuleParameters { get; set; }
        public int CurrentPortalID { get; set; }
        public NikPortalUser PortalUser { get; set; }
        public string CurrentPortalPath { get; set; }
        public PlugInPage()
        {
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            var purelevel = Page.RouteData.Values["level"];
            var puremodulename = Page.RouteData.Values["modulename"];
            var puremoduleproperties = Page.RouteData.Values["moduleproperties"];

            if (null != purelevel)
            {
                Level = purelevel.ToString().ToLower();
            }

            if (string.IsNullOrWhiteSpace(Level))
            {
                Level = "home";
            }
            else if (Level != InputEncode(Level))
            {
                Level = "home";
                IsAnyHackAttempt = true;
            }

            if (null != puremodulename)
            {
                ModuleName = puremodulename.ToString().ToLower();
            }

            if (string.IsNullOrWhiteSpace(ModuleName))
            {
                ModuleName = "page";
            }
            else if (ModuleName != InputEncode(ModuleName))
            {
                ModuleName = "page";
                IsAnyHackAttempt = true;
            }


            if (null != puremoduleproperties)
            {
                ModuleParameters = puremoduleproperties.ToString().ToLower();
            }

            if (string.IsNullOrWhiteSpace(ModuleParameters))
            {
                ModuleParameters = "default";
            }
            else if (ModuleParameters != InputEncode(ModuleParameters))
            {
                ModuleParameters = "default";
                IsAnyHackAttempt = true;
            }
        }

        private string InputEncode(string st)
        {
            string temp = st;
            string temp2 = st.ToLower();
            temp2 = temp2.Replace("'", "");
            temp2 = temp2.Replace(";", "");
            temp2 = temp2.Replace("#", "");
            temp2 = temp2.Replace("\"", "");
            temp2 = temp2.Replace("%", "");
            temp2 = temp2.Replace("--", "");
            temp2 = temp2.Replace("=", "");
            temp2 = temp2.Replace(" ", "");
            temp2 = temp2.Replace("$", "");
            temp2 = temp2.Replace("+", "");
            temp2 = temp2.Replace("(", "");
            temp2 = temp2.Replace(")", "");
            temp2 = temp2.Replace("&", "");
            temp2 = temp2.Replace("update ", "");
            temp2 = temp2.Replace("delete ", "");
            temp2 = temp2.Replace("drop ", "");
            temp2 = temp2.Replace("alter ", "");
            temp2 = temp2.Replace("dbo ", " ");
            temp2 = temp2.Replace("update", "");
            temp2 = temp2.Replace("insert", "");
            temp2 = temp2.Replace("delete", "");
            temp2 = temp2.Replace("drop", "");
            temp2 = temp2.Replace("alter", "");
            temp2 = temp2.Replace("dbo ", "");
            temp2 = temp2.Replace("schema", "");

            if (temp2 == st.ToLower())
            {
                return temp;
            }
            return temp2;
        }

        public void Issue404Response()
        {
            Is404Issued = true;
            var _404Path = Server.MapPath("~/404Pages/404P1.html");
            string text = File.ReadAllText(_404Path);
            Response.Clear();
            Response.ContentType = "text/html; charset=utf-8";
            Response.Write(text);
            Response.StatusCode = 404;
            //must save IP to file, to fully black this one
            Response.Flush();
            Response.End();
        }


    }
}
