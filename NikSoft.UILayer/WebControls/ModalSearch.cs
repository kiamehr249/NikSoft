using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer.WebControls
{
    public class ModalSearch : PlaceHolder
    {

        protected override void Render(HtmlTextWriter writer)
        {
            writer.BeginRender();
            writer.Write(@"<div class='modal fade' id='searchmodal' role='dialog' aria-hidden='true'><div class='modal-dialog  modal-lg'><div class='modal-content'><div class='modal-body'>");
            base.Render(writer);
            writer.Write("</div></div></div></div>");
            writer.EndRender();
        }
    }
}
