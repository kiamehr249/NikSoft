using NikSoft.NikModel;
using NikSoft.PlugIn;
using StructureMap;

namespace NikSoft.UILayer
{
    public class NikmehrPage : PlugInPage, INikRoutedPage, IPortalUser
    {
        public TemplateType TemplateType { get; set; }
        public int SelectedPageID { get; set; }
        public bool ConvertYeKe { get; set; }
        public string Language { get; set; }
        public string Direction { get; set; }

        public NikmehrPage()
        {
            SelectedPageID = 0;
            ObjectFactory.BuildUp(this);
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();
        }

        public void Issue404()
        {
            Issue404Response();
        }

        public string Domain
        {
            get
            {
                var domain = Request.Url.Host;
                if (domain.StartsWith("http://"))
                {
                    domain = domain.Replace("http://", "");
                }
                if (domain.StartsWith("https://"))
                {
                    domain = domain.Replace("https://", "");
                }
                if (domain.StartsWith("www."))
                {
                    domain = domain.Replace("www.", "");
                }
                if (domain.EndsWith("/"))
                {
                    domain = domain.Replace("/", string.Empty);
                }
                return domain;
            }
        }


    }
}
