using NikSoft.NikModel;
using System;

namespace NikSoft.UILayer
{
    public class WidgetLoader : ContainerBase, IWidgetHost
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AddModuletoPageAndBuildSkin(this, AdminorUi.UiPart);
        }
    }
}