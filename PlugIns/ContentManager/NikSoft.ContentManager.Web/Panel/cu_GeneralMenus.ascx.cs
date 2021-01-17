using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_GeneralMenus : NikUserControl
    {
        public IGeneralMenuService iGeneralMenuServ { get; set; }
        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void GetEditData()
        {
            var data = iGeneralMenuServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            txtDesc.Text = data.Description;
            chbLogin.Checked = data.LoginRequired;
            chbEnbaled.Checked = data.Enabled;
        }

        private void SaveNewData()
        {
            var DataItem = iGeneralMenuServ.Create();
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = txtDesc.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.LoginRequired = chbLogin.Checked;
            iGeneralMenuServ.Add(DataItem);
            iGeneralMenuServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var DataItem = iGeneralMenuServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = txtDesc.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.LoginRequired = chbLogin.Checked;
            iGeneralMenuServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

    }
}