using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_UserPublicContent : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        //public IFeatureFormService iFeatureFormServ { get; set; }
        public ICategoryPermissionService iCategoryPermissionServ { get; set; }

        public List<int> PerCatIDs;
        public int ParCatID;
        public bool IsNull = false;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
            Container.NewItem += GoToNewItem;
        }

        protected void GoToNewItem()
        {
            RedirectTo("~/panel/cu_UserPublicContent/add?parent=" + ParCatID);
            return;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PerCatIDs = iCategoryPermissionServ.GetAll(x => x.UserID == PortalUser.ID).Select(x => x.CategoryID).ToList();
            if (ModuleParameters != null || ModuleParameters != "default")
            {
                int cid;
                if (!int.TryParse(ModuleParameters, out cid))
                {
                    RedirectTo("/panel/userpercategories");
                    return;
                }

                if (!PerCatIDs.Contains(cid))
                {
                    RedirectTo("/panel/userpercategories");
                    return;
                }

                ParCatID = cid;
            }
            else
            {
                IsNull = true;
            }

            BoundData();
        }

        protected bool HasFeature(int? CatID)
        {
            //if (CatID != null)
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
            if (IsNull)
            {
                query.Add(t => t.PortalID == PortalUser.PortalID && t.CategoryID == null);
            }
            else
            {
                query.Add(t => t.PortalID == PortalUser.PortalID && t.CategoryID == ParCatID);
            }
            
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
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
            var item = iPublicContentServ.Find(t => t.ID == ItemID);
            if (action == "up")
            {
                var qu = iPublicContentServ.ExpressionMaker();
                qu.Add(x => x.Ordering < item.Ordering);
                if (ParCatID > 0)
                {
                    qu.Add(x => x.CategoryID == ParCatID);

                    var t = iPublicContentServ.GetAll(qu).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Max(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder && x.CategoryID == ParCatID);
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
                if (ParCatID > 0)
                {
                    qu.Add(x => x.CategoryID == ParCatID);
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