using System;
using System.Globalization;
using System.Web.UI;

namespace NikSoft.UILayer.WebControls
{
    public class ListCounter : Control
    {

        public ListCounter()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.NamingContainer == null)
                throw new ApplicationException("The parent naming container cannot be null. This Control must be used inside another control");

            if (!(this.NamingContainer is IDataItemContainer))
                throw new ApplicationException("The parent container must implement the IDataItemContainer interface. Something like DataGrid or GridView mustbe used.");

            IDataItemContainer dataItemContainer = (IDataItemContainer)this.NamingContainer;

            writer.Write(string.Format(this.IndexFormat, dataItemContainer.DataItemIndex + this.IndexOffset));
        }

        public string IndexFormat
        {
            get
            {
                object o = this.ViewState["IndexFormat"];
                return (o == null) ? "{0}" : (string)o;
            }
            set
            {
                if (string.Compare(value, this.IndexFormat, true, CultureInfo.InvariantCulture) == 0)
                    return;

                this.ViewState["IndexFormat"] = value;
            }
        }

        public int IndexOffset
        {
            get
            {
                object o = this.ViewState["IndexOffset"];
                return (o == null) ? 1 : (int)o;
            }
            set
            {
                if (value == this.IndexOffset)
                    return;

                this.ViewState["IndexOffset"] = value;
            }
        }
    }
}