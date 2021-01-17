using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_ContentCategory : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        protected string SelectData = string.Empty;
        protected int? ParentID;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
            Container.NewItem += GoToNewItem;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int PID;
            if (!int.TryParse(ModuleParameters, out PID))
            {
                ParentID = null;
            }
            else
            {
                ParentID = PID;
                var PItem = iContentCategoryServ.Find(x => x.ID == PID && x.PortalID == PortalUser.PortalID);
                if (PItem == null)
                {
                    Response.StatusCode = 404;
                    IssueContentNotFound();
                }
            }



            if (!IsPostBack)
            {
                BindCombo();
            }
            BoundData();
        }

        protected void GoToNewItem()
        {
            if (ParentID != null)
            {
                RedirectTo("~/panel/cu_ContentCategory/add?ParentID=" + ModuleParameters);
            }
            else
            {
                RedirectTo("~/panel/cu_ContentCategory/add");
            }
        }

        private void BindCombo()
        {
            var data = iContentGroupServ.GetAll(t => t.PortalID == PortalUser.PortalID).ToList();
            ddlGroup.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }


        protected override void BoundData()
        {
            
            int PID;
            if (!int.TryParse(ModuleParameters, out PID))
            {
                RdOptions.Visible = false;
            }
            else
            {
                var TopPID = iContentCategoryServ.Find(x => x.ID == PID, y => new { y.ParentID }).ParentID;
                HypBackLevel.NavigateUrl = "/panel/rd_ContentCategory/" + TopPID;
            }
            var query = iContentCategoryServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);

            if (ParentID != null)
            {
                query.Add(x => x.ParentID == ParentID);
            }
            else
            {
                query.Add(x => x.ParentID == null);
            }

            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }


            int GroupID = ddlGroup.SelectedValue.ToInt32();
            if (GroupID > 0)
            {
                query.Add(t => t.GroupID == GroupID);
            }


            base.FillManageFrom(iContentCategoryServ, query);
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
            var DataItem = iContentCategoryServ.Find(x => x.ID == id);
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
                    iContentCategoryServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iContentCategoryServ.Find(t => t.ID == ItemID);
            if (action == "down")
            {
                var t = iContentCategoryServ.GetAll(x => x.Ordering < item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentCategoryServ.SaveChanges();
                }
            }
            else
            {
                var t = iContentCategoryServ.GetAll(x => x.Ordering > item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentCategoryServ.SaveChanges();
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
                    var deletedItems = iContentCategoryServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        if (Item.Childs.Count > 0)
                        {
                            Notification.SetErrorMessage("یکی از آیتم ها داری زیرگروه می باشد.");
                            return;
                        }

                        if (!Item.ImgUrl.IsEmpty())
                        {
                            Utilities.Utilities.RemoveItemFile(Item.ImgUrl);
                        }
                        
                    }
                    iContentCategoryServ.Remove(deletedItems);
                    iContentCategoryServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iContentCategoryServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}