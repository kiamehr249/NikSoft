using System.Drawing;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer.WebControls
{
    [ToolboxData("<{0}:NikTextBox runat=\"server\" MinLength=\"3\" MinLengthMessage=\"باید حداقل {0} کاراکتر باشد\" MaxLengthMessage=\"باید حداکثر {0} کاراکتر باشد\"></{0}:NikTextBox>")]
    [ToolboxBitmap(typeof(TextBox))]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class NikTextBox : TextBox
    {
        public int MinLength { get; set; }

        private string minlenMsg = "باید حداقل {0} کاراکتر باشد";
        private string maxlenMsg = "باید حداقل {0} کاراکتر باشد";
        private string emptyMsg = "نمی تواند خالی باشد";
        private string publicMsg = "فرمت صحیح نیست";

        public string MinLengthMessage
        {
            get { return minlenMsg; }
            set { minlenMsg = value; }
        }
        public string MaxLengthMessage
        {
            get { return maxlenMsg; }
            set { maxlenMsg = value; }
        }

        public string EmptyMessage
        {
            get { return emptyMsg; }
            set { emptyMsg = value; }
        }

        public string PublicMessage
        {
            get { return publicMsg; }
            set { publicMsg = value; }
        }

        private bool emptyTextIsValid = true;
        public bool EmptyTextIsValid
        {
            get { return emptyTextIsValid; }
            set { emptyTextIsValid = value; }
        }

        private string boxTitle = string.Empty;
        public string BoxTitle
        {
            get { return boxTitle; }
            set { boxTitle = value; }
        }

        public string Validate()
        {
            var msg = string.Empty;
            var text = this.Text.Trim();
            if (text.Length == 0 && !emptyTextIsValid)
            {
                msg = EmptyMessage;
                return msg;
            }
            if (text.Length < MinLength && MinLength > 0)
            {
                msg += string.Format(MinLengthMessage, MinLength) + "\n";
            }
            if (text.Length > MaxLength && MaxLength > 0)
            {
                msg += string.Format(MaxLengthMessage, MaxLength) + "\n";
            }
            return msg;
        }
    }
}