using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class rd_Forms : NikUserControl
    {
        public IFormModelService iFormModelServ { get; set; }
        public ITextBoxModelService iTextBoxModelServ { get; set; }
        public IListControlModelService iListControlModelServ { get; set; }
        public ICheckBoxModelService iCheckBoxModelServ { get; set; }
        public IFileUploadModelService iFileUploadModelServ { get; set; }
        public int parentId;
        public int textBoxCount;
        public int listControlCount;
        public int checkBoxCount;
        public int fileUploadCount;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BoundCombo();
            }
            BoundData();
        }

        protected void BoundCombo() {
            var data = iFormModelServ.GetAll(t => t.PortalID == PortalUser.PortalID).ToList();
            ddlParent.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        protected override void BoundData()
        {
            var query = iFormModelServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            base.FillManageFrom(iFormModelServ, query);

        }
        protected int GetItemCount(int Id)
        {
            textBoxCount = iTextBoxModelServ.Count(x => x.FormID == Id);
            listControlCount = iListControlModelServ.Count(x => x.FormID == Id);
            checkBoxCount = iCheckBoxModelServ.Count(x => x.FormID == Id);
            fileUploadCount = iFileUploadModelServ.Count(x => x.FormID == Id);
            
            return textBoxCount + listControlCount + checkBoxCount + fileUploadCount;
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
            var DataItem = iFormModelServ.Find(x => x.ID == id);
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
                    iFormModelServ.SaveChanges();
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iFormModelServ.Find(t => t.ID == ItemID);
            if (action == "up")
            {
                var t = iFormModelServ.GetAll(x => x.Ordering < item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iFormModelServ.SaveChanges();
                }
            }
            else
            {
                var t = iFormModelServ.GetAll(x => x.Ordering > item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iFormModelServ.SaveChanges();
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
                    var deletedItems = iFormModelServ.GetAll(x => l.Contains(x.ID)).ToList();
                    iFormModelServ.Remove(deletedItems);
                    iFormModelServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iFormModelServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}