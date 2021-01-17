using NikSoft.UILayer;
using System;
using System.Web.UI.HtmlControls;

namespace NikSoft.Web.Modules.BaseModules.FileManager
{
    public partial class PanelFileManager : WidgetUIContainer
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var form = this.Page.FindControl("mainForm") as HtmlForm;
            form.Enctype = "multipart/form-data";
        }
    }
}