using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class ContentFeatures : NikUserControl
    {
        public IFeatureFormService iFeatureFormServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        public int categoryID;
        //public bool HasForm = false;
        public bool IsStore = false;
        public string CatName = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }

        protected override void BoundData()
        {
            if (!int.TryParse(ModuleParameters, out categoryID))
            {
                RedirectTo("~/panel/rd_ContentCategory");
                return;
            }

            var ThisCat = iContentCategoryServ.Find(x => x.ID == categoryID && x.PortalID == PortalUser.PortalID);
            if (ThisCat == null)
            {
                RedirectTo("~/panel/rd_ContentCategory");
                return;
            }

            HypBackLevel.NavigateUrl = "/panel/rd_ContentCategory/" + ThisCat.ParentID;

            IsStore = ThisCat.IsStore;
            CatName = (ThisCat.ParentID != null ? ThisCat.Parent.Title + " - " : "") + ThisCat.Title;
            HypBackToList.NavigateUrl = "/panel/rd_ContentCategory/" + ThisCat.ParentID;
            HypCancel.NavigateUrl = "/panel/ContentFeatures/" + ThisCat.ID;
            var ThisForm = iFeatureFormServ.Find(x => x.CategoryID == categoryID);

            var query = iFeatureFormServ.ExpressionMaker();
            query.Add(t => t.CategoryID == categoryID);

            base.FillManageFrom(iFeatureFormServ, query);
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "enabled":
                    SetItemEnabled(id);
                    break;
                case "EditMe":
                    EditItem(id);
                    break;
                case "DeleteMe":
                    DeleteItem(id);
                    break;
            }
        }

        protected void SetItemEnabled(int Id)
        {
            var ThisItem = iFeatureFormServ.Find(x => x.ID == Id);
            ThisItem.Enabled = !ThisItem.Enabled;
            iFeatureFormServ.SaveChanges();
            BoundData();
        }

        protected void EditItem(int Id)
        {
            var EditItem = iFeatureFormServ.Find(x => x.ID == Id);
            hfEditItemID.Value = EditItem.ID.ToString();
            CheckBtnControl();
            txtTitle.Text = EditItem.Title;
            TxtDesc.Text = EditItem.Description;
            chbEnbaled.Checked = EditItem.Enabled;
            BoundData();
        }

        protected void DeleteItem(int Id)
        {
            var delItem = iFeatureFormServ.Find(x => x.ID == Id);
            iFeatureFormServ.Remove(delItem);
            iFeatureFormServ.SaveChanges();
            BoundData();
        }

        protected void CheckBtnControl()
        {
            if (hfEditItemID.Value != "0")
            {
                BtnSave.Visible = false;
                BtnUpdate.Visible = true;
                HypBackToList.Visible = false;
                HypCancel.Visible = true;
            }
            else
            {
                BtnSave.Visible = true;
                BtnUpdate.Visible = false;
                HypBackToList.Visible = true;
                HypCancel.Visible = false;
            }

        }

        protected bool ValidateForm()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان نمی تواند خالی باشد.");
            }


            ErrorMessage.AddRange(this.ValidateTextBoxes());
            Notification.SetErrorMessage(ErrorMessage);
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            var newForm = iFeatureFormServ.Create();
            newForm.Title = txtTitle.Text;
            newForm.Description = TxtDesc.Text;
            newForm.Enabled = chbEnbaled.Checked;
            newForm.IsStore = IsStore;
            newForm.PortalID = PortalUser.PortalID;
            newForm.CategoryID = categoryID;
            iFeatureFormServ.Add(newForm);
            iFeatureFormServ.SaveChanges();
            BoundData();
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            int ThisItemID;
            if (!int.TryParse(hfEditItemID.Value, out ThisItemID))
            {
                Notification.SetErrorMessage("مشکل فنی به مسئول فنی اطلاع دهید");
                return;
            }

            var ThisItem = iFeatureFormServ.Find(x => x.ID == ThisItemID);
            ThisItem.Title = txtTitle.Text;
            ThisItem.Description = TxtDesc.Text;
            ThisItem.Enabled = chbEnbaled.Checked;
            iFeatureFormServ.SaveChanges();
            hfEditItemID.Value = "0";
            CheckBtnControl();
            BoundData();
        }
    }
}