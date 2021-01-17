using NikSoft.Utilities;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NikSoft.UILayer
{
    public class BlockTemplate : UserControl
    {

        public void AddControl(Control control)
        {
            var mp = FindMpControl("data-mp");
            if (mp == null)
            {
                this.Controls.Add(control);
            }
            else
            {
                mp.Controls.Add(control);
            }
        }

        private HtmlGenericControl FindMpControl(string attr)
        {
            var all = this.Controls.GetAllChilds<HtmlGenericControl>();
            return all.SingleOrDefault(t => t.Attributes[attr] != null);
        }

        public void AddTitle(string title, bool asurl, string url)
        {
            var mt = FindMpControl("data-mt");
            if (mt != null)
            {
                if (asurl)
                {
                    mt.Controls.Add(new LiteralControl("<a href='" + url + "'>" + title + "</a>"));
                }
                else
                {
                    mt.Controls.Add(new LiteralControl("<span>" + title + "</span>"));
                }
            }
        }
    }
}