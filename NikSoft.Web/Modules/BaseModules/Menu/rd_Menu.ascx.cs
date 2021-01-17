using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Menu
{
    public partial class rd_Menu : NikUserControl
    {
        public INikMenuService iNikMenuServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
            BoundData();
        }

        private void BindCombo()
        {
            var data = iNikMenuServ.GetAll(t => t.PortalID == PortalUser.ID).OrderBy(t => t.ParentID).ThenBy(t => t.Ordering).ToList();
            if (ItemID != 0)
            {
                data = data.Where(t => t.ID != ItemID).ToList();
            }
            ddlParent.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        protected override void BoundData()
        {
            var query = iNikMenuServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            if (ddlParent.SelectedIndex != 0)
            {
                var ParentID = int.Parse(ddlParent.SelectedValue);
                query.Add(t => t.ParentID == ParentID);
            }
            base.FillManageFrom(iNikMenuServ, query);
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
            var MenuItem = iNikMenuServ.Find(t => t.ID == id);
            switch (e.CommandName)
            {
                case "MoveUp":
                    RearrangePriority("up", id);
                    break;
                case "MoveDown":
                    RearrangePriority("down", id);
                    break;
                case "enabled":
                    MenuItem.Enabled = !MenuItem.Enabled;
                    iNikMenuServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iNikMenuServ.Find(t => t.ID == ItemID);
            if (action == "down")
            {
                var t = iNikMenuServ.GetAll(x => x.Ordering < item.Ordering && x.ParentID == item.ParentID).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iNikMenuServ.SaveChanges();
                }
            }
            else
            {
                var t = iNikMenuServ.GetAll(x => x.Ordering > item.Ordering && x.ParentID == item.ParentID).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iNikMenuServ.SaveChanges();
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
                    var deletedItems = iNikMenuServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        Utilities.Utilities.RemoveItemFile(Item.ImageURI);
                    }
                    iNikMenuServ.Remove(deletedItems);
                    uow.SaveChanges();
                    Notification.SetSuccessMessage("آیتم های انتخاب شده حذف شدند.");
                }
                else
                {
                    Notification.SetErrorMessage("حداقل یکی از آیتم ها را جهت حذف انتخاب کنید.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iUserServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}