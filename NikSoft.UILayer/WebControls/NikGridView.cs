using NikSoft.Utilities;
using System;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer.WebControls
{
    public class NikGridView : GridView, IPageableItemContainer
    {

        public NikGridView()
			: base() {
            CellPadding = 0;
            BorderWidth = 0;
            GridLines = System.Web.UI.WebControls.GridLines.None;
            CellSpacing = 0;
            BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            PagerSettings.Visible = false;
            AutoGenerateColumns = false;
        }

        private string pagingQueryString = "startrow";
        public string PagingQueryString
        {
            get { return pagingQueryString; }
            set { pagingQueryString = value; }
        }

        private const string DefaultCss = "table table-striped table-bordered table-hover";

        /// <summary>
        /// TotalRowCountAvailable event key
        /// </summary>
        private static readonly object EventTotalRowCountAvailable = new object();

        protected override void OnLoad(EventArgs e)
        {
            if (this.CssClass.IsEmpty())
            {
                this.CssClass = DefaultCss;
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// Set the control with appropriate parameters and bind to right chunk of data.
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <param name="databind"></param>
        void IPageableItemContainer.SetPageProperties(int startRowIndex, int maximumRows, bool databind)
        {
            int newPageIndex = (startRowIndex / maximumRows);
            this.PageSize = maximumRows;

            if (this.PageIndex != newPageIndex)
            {
                bool isCanceled = false;
                if (databind)
                {
                    //  create the event arguments and raise the event
                    GridViewPageEventArgs args = new GridViewPageEventArgs(newPageIndex);
                    this.OnPageIndexChanging(args);

                    isCanceled = args.Cancel;
                    newPageIndex = args.NewPageIndex;
                }

                //  if the event wasn't cancelled change the paging values
                if (!isCanceled)
                {
                    this.PageIndex = newPageIndex;

                    if (databind)
                        this.OnPageIndexChanged(EventArgs.Empty);
                }
                if (databind)
                    this.RequiresDataBinding = true;
            }
        }

        /// <summary>
        /// IPageableItemContainer's StartRowIndex = PageSize * PageIndex properties
        /// </summary>
        int IPageableItemContainer.StartRowIndex
        {
            get { return this.PageSize * this.PageIndex; }
        }

        /// <summary>
        /// IPageableItemContainer's MaximumRows  = PageSize property
        /// </summary>
        int IPageableItemContainer.MaximumRows
        {
            get { return this.PageSize; }
        }

        /// <summary>
        ///
        /// </summary>
        event EventHandler<PageEventArgs> IPageableItemContainer.TotalRowCountAvailable
        {
            add { base.Events.AddHandler(NikGridView.EventTotalRowCountAvailable, value); }
            remove { base.Events.RemoveHandler(NikGridView.EventTotalRowCountAvailable, value); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTotalRowCountAvailable(PageEventArgs e)
        {
            EventHandler<PageEventArgs> handler = (EventHandler<PageEventArgs>)base.Events[NikGridView.EventTotalRowCountAvailable];
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}