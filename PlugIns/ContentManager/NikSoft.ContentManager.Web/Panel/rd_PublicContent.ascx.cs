using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_PublicContent : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        //public IFeatureFormService iFeatureFormServ { get; set; }

        protected string SelectData1 = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindCombo();
            BoundData();
        }

        private void BindCombo()
        {
            var data = iContentCategoryServ.GetAll(t => t.PortalID == PortalUser.PortalID && t.Enabled).ToList();
            ddlCategory.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        protected bool HasFeature(int? CatID)
        {
            //if(CatID != null)
            //{
            //    var HasForm = iFeatureFormServ.Any(x => x.CategoryID == CatID);
            //    if (HasForm)
            //    {
            //        return true;
            //    }
            //}
            return false;
        }


        protected override void BoundData()
        {
            var query = iPublicContentServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }

            int cid = ddlCategory.SelectedValue.ToInt32();
            if (cid > 0)
            {
                query.Add(x => x.CategoryID == cid);
            }

            base.FillManageFrom(iPublicContentServ, query);
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
            var DataItem = iPublicContentServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "MoveUp":
                    RearrangePriority("down", id);
                    break;
                case "MoveDown":
                    RearrangePriority("up", id);
                    break;
                case "enabled":
                    DataItem.Enabled = !DataItem.Enabled;
                    iPublicContentServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            int cid = ddlCategory.SelectedValue.ToInt32();
            var item = iPublicContentServ.Find(t => t.ID == ItemID);
            if (action == "up")
            {
                var qu = iPublicContentServ.ExpressionMaker();
                qu.Add(x => x.Ordering < item.Ordering);
                if (cid > 0)
                {
                    qu.Add(x => x.CategoryID == cid);

                    var t = iPublicContentServ.GetAll(qu).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Max(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder && x.CategoryID == cid);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iPublicContentServ.SaveChanges();
                    }
                }
                
            }
            else
            {
                var qu = iPublicContentServ.ExpressionMaker();
                qu.Add(x => x.Ordering > item.Ordering);
                if (cid > 0)
                {
                    qu.Add(x => x.CategoryID == cid);
                    var t = iPublicContentServ.GetAll(qu).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Min(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iPublicContentServ.SaveChanges();
                    }
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
                    var deletedItems = iPublicContentServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        if (!Item.ImgUrl.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.ImgUrl);
                        if (!Item.IconUrl.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.IconUrl);
                        if (!Item.AttachFile.IsEmpty())
                            Utilities.Utilities.RemoveItemFile(Item.AttachFile);
                    }
                    iPublicContentServ.Remove(deletedItems);
                    iPublicContentServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iPublicContentServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}