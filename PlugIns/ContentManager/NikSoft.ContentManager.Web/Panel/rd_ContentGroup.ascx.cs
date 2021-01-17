using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_ContentGroup : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }


        protected override void BoundData()
        {
            var query = iContentGroupServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            base.FillManageFrom(iContentGroupServ, query);
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
            var DataItem = iContentGroupServ.Find(x => x.ID == id);
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
                    iContentGroupServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iContentGroupServ.Find(t => t.ID == ItemID);
            if (action == "down")
            {
                var t = iContentGroupServ.GetAll(x => x.Ordering < item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentGroupServ.SaveChanges();
                }
            }
            else
            {
                var t = iContentGroupServ.GetAll(x => x.Ordering > item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentGroupServ.SaveChanges();
                }
            }
        }

        protected string GetName(int id)
        {
            var Tuser = iUserServ.Find(x => x.ID == id);
            string fullName = Tuser.FirstName + " " + Tuser.LastName;
            return fullName;
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
                    var deletedItems = iContentGroupServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        Utilities.Utilities.RemoveItemFile(Item.ImgUrl);
                    }
                    iContentGroupServ.Remove(deletedItems);
                    iContentGroupServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iContentGroupServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}