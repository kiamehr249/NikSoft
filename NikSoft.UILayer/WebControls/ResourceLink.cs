using NikSoft.NikModel;
using System.Web.UI;

namespace NikSoft.UILayer.WebControls
{
    public class ResourceLink : Control
    {
        public ResourceLinkType ResourceLinkType { get; set; }
        public string FilePath { get; set; }

        public bool ExplodesAdmin { get; set; }

        public bool IsTop { get; set; }

        protected override void OnInit(System.EventArgs e)
        {
            switch (ResourceLinkType)
            {
                case ResourceLinkType.Style:
                    {
                        Utilities.Utilities.CreateCSSLink(this.Page, ResolveUrl(FilePath), string.Empty, ExplodesAdmin);
                        break;
                    }
                case ResourceLinkType.Script:
                    {
                        Utilities.Utilities.CreateJSLink(this.Page, ResolveUrl(FilePath), ExplodesAdmin, IsTop);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            base.OnInit(e);
        }
    }
}