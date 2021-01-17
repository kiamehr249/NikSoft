using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_GeneralMenuItem : NikUserControl
    {
        public IGeneralMenuItemService iGeneralMenuItemServ { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
            Container.NewItem += GoToNewItem;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BoundData();
        }


        protected override void BoundData()
        {
            int _MID = int.Parse(Request.QueryString["MainID"].ToString());

            var query = iGeneralMenuItemServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID && t.GeneralMenuID == _MID);

            HypBackLevel.NavigateUrl = "/panel";

            if (Request.QueryString["ParentID"] != null)
            {
                int _PID = int.Parse(Request.QueryString["ParentID"].ToString());
                query.Add(t => t.ParentID == _PID);
                var pItem = iGeneralMenuItemServ.Find(x => x.ID == _PID);
                if (pItem.ParentID != null)
                {
                    HypBackLevel.NavigateUrl += ("/rd_GeneralMenuItem/default?MainID=" + _MID + "&ParentID=" + pItem.ParentID);
                }
                else
                {
                    HypBackLevel.NavigateUrl += ("/rd_GeneralMenuItem/default?MainID=" + _MID);
                }

            }
            else
            {
                query.Add(t => t.ParentID == null);
                HypBackLevel.NavigateUrl += ("/rd_GeneralMenus");
            }


            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }

            base.FillManageFrom(iGeneralMenuItemServ, query);

        }

        protected void GoToNewItem()
        {
            if (Request.QueryString["ParentID"] != null)
                RedirectTo("~/panel/cu_GeneralMenuItem/add?MainID=" + Request.QueryString["MainID"].ToString() + "&ParentID=" + Request.QueryString["ParentID"].ToString());
            else
                RedirectTo("~/panel/cu_GeneralMenuItem/add?MainID=" + Request.QueryString["MainID"]);
            return;
        }

        protected string GetLink(int id)
        {
            return "/panel/rd_GeneralMenuItem/default?MainID=" + Request.QueryString["MainID"].ToString() + "&ParentID=" + id;
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
            var DataItem = iGeneralMenuItemServ.Find(x => x.ID == id);
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
                    iGeneralMenuItemServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iGeneralMenuItemServ.Find(t => t.ID == ItemID);
            if (action == "up")
            {
                var t = iGeneralMenuItemServ.GetAll(x => x.Ordering < item.Ordering && x.ParentID == item.ParentID).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iGeneralMenuItemServ.SaveChanges();
                }
            }
            else
            {
                var t = iGeneralMenuItemServ.GetAll(x => x.Ordering > item.Ordering && x.ParentID == item.ParentID).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iGeneralMenuItemServ.SaveChanges();
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
                    var deletedItems = iGeneralMenuItemServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        if (!Item.ImgUrl.IsEmpty())
                        {
                            Utilities.Utilities.RemoveItemFile(Item.ImgUrl);
                        }
                        if (Item.Childs.Count > 0)
                        {
                            Notification.SetErrorMessage("به دلیل داشتن زیر منو حذف انجام نشد.");
                            return;
                        }
                    }
                    iGeneralMenuItemServ.Remove(deletedItems);
                    iGeneralMenuItemServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iGeneralMenuItemServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}