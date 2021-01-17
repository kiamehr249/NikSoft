using System;

namespace NikSoft.UILayer.WebControls
{
    public class NikConfirmLinkButton : NikLinkButton
    {
        private string messageText = "Are you sure?";
        public string MessageText
        {
            get { return messageText; }
            set { messageText = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.OnClientClick = "return confirm('" + messageText + "');";
            base.OnLoad(e);
        }
    }
}