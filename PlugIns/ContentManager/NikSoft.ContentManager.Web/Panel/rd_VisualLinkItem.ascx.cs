using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_VisualLinkItem : NikUserControl
    {
        public IVisualLinkItemService iVisualLinkItemServ { get; set; }
        public int linkCatID;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
            Container.NewItem += GoToNewItem;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int catid = 0;
            if(!int.TryParse(ModuleParameters, out catid))
            {
                Response.Redirect("/panel/rd_visuallink/view");
                return;
            }

            linkCatID = catid;

            BoundData();
        }

        protected void GoToNewItem()
        {
            RedirectTo("~/panel/cu_visuallinkitem/add?ParentID=" + linkCatID);
        }

        protected override void BoundData()
        {
            var query = iVisualLinkItemServ.ExpressionMaker();
            query.Add(t => t.VisualLinkID == linkCatID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            base.FillManageFrom(iVisualLinkItemServ, query);
        }

        protected void breset_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            BoundData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundData();
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            var DataItem = iVisualLinkItemServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "MoveUp":
                    RearrangePriority("up", id);
                    break;
                case "MoveDown":
                    RearrangePriority("down", id);
                    break;
                case "enabled":
                    DataItem.Enabled = !DataItem.Enabled;
                    iVisualLinkItemServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iVisualLinkItemServ.Find(t => t.ID == ItemID);
            if (action == "up")
            {
                var t = iVisualLinkItemServ.GetAll(x => x.Ordering < item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iVisualLinkItemServ.SaveChanges();
                }
            }
            else
            {
                var t = iVisualLinkItemServ.GetAll(x => x.Ordering > item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iVisualLinkItemServ.SaveChanges();
                }
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].ToString();
                    List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                    var deletedItems = iVisualLinkItemServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        if (!Item.Img1.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.Img1);
                        if (!Item.Img2.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.Img2);
                        if (!Item.Img3.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.Img3);
                        if (!Item.Img4.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.Img4);
                    }
                    iVisualLinkItemServ.Remove(deletedItems);
                    iVisualLinkItemServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iVisualLinkItemServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}