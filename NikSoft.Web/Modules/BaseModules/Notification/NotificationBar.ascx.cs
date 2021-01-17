using NikSoft.NikModel;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Notification
{
    public partial class NotificationBar : UserControl
    {
        private HtmlGenericControl ul = new HtmlGenericControl("ul");
        private List<string> message = new List<string>();

        private MessageType messageType = MessageType.Information;
        public MessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        private Layout layout = Layout.TopRight;
        public Layout Layout
        {
            get { return layout; }
            set { layout = value; }
        }

        protected string type = string.Empty;
        protected string layOut = string.Empty;
        protected string finalMessage = string.Empty;
        protected int timeOut = 0;

        protected override void OnPreRender(EventArgs e)
        {
            switch (layout)
            {
                case Layout.Top:
                    {
                        layOut = "top";
                        break;
                    }
                case Layout.TopCenter:
                    {
                        layOut = "topCenter";
                        break;
                    }
                case Layout.TopLeft:
                    {
                        layOut = "topLeft";
                        break;
                    }
                case Layout.TopRight:
                    {
                        layOut = "topRight";
                        break;
                    }
                case Layout.Center:
                    {
                        layOut = "center";
                        break;
                    }
                case Layout.CenterLeft:
                    {
                        layOut = "centerLeft";
                        break;
                    }
                case Layout.CenterRight:
                    {
                        layOut = "centerRight";
                        break;
                    }
                case Layout.Bottom:
                    {
                        layOut = "bottom";
                        break;
                    }
                case Layout.BottomCenter:
                    {
                        layOut = "bottomCenter";
                        break;
                    }
                case Layout.BottomLeft:
                    {
                        layOut = "bottomLeft";
                        break;
                    }
                case Layout.BottomRight:
                    {
                        layOut = "bottomRight";
                        break;
                    }
            }

            switch (messageType)
            {
                case MessageType.Alert:
                    {
                        type = "alert";
                        break;
                    }
                case MessageType.Information:
                    {
                        type = "information";
                        break;
                    }
                case MessageType.Error:
                    {
                        type = "error";
                        break;
                    }
                case MessageType.Warning:
                    {
                        type = "warning";
                        break;
                    }
                case MessageType.Notification:
                    {
                        type = "notification";
                        break;
                    }
                case MessageType.Success:
                    {
                        type = "success";
                        break;
                    }
            }
            foreach (var item in message)
            {
                if (item.IsEmpty())
                {
                    continue;
                }
                finalMessage += item + "<br/>";
            }
            finalMessage = finalMessage.Replace("\n", "");
            base.OnPreRender(e);
        }

        public void AddMessage(List<string> msg, MessageType mtype, Layout mlayout, int timeout = 0)
        {
            this.timeOut = timeout * 1000;
            this.messageType = mtype;
            this.layout = mlayout;
            message.AddRange(msg);
        }

        public void AddMessage(string msg, MessageType mtype, Layout mlayout, int timeout = 0)
        {
            this.timeOut = timeout * 1000;
            this.messageType = mtype;
            this.layout = mlayout;
            message.AddRange(msg.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        public void Clear()
        {
            message.Clear();
        }
    }
}