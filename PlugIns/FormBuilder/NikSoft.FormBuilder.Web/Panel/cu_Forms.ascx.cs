using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class cu_Forms : NikUserControl
    {
        public IFormModelService iFormModelServ { get; set; }

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
            var data = iFormModelServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            TxtDesc.Text = data.Description;
            TxtMessage.Text = data.Message;
            chbEnbaled.Checked = data.Enabled;
            chbIpRecord.Checked = data.RecordIP;
            chbLogin.Checked = data.LoginRequired;
        }

        private void SaveNewData()
        {

            int items = iFormModelServ.Count(x => x.PortalID == PortalUser.PortalID);

            var DataItem = iFormModelServ.Create();
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.Message = TxtMessage.Text;
            DataItem.Ordering = items + 1;
            DataItem.LoginRequired = chbLogin.Checked;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.PortalID = PortalUser.PortalID;
            iFormModelServ.Add(DataItem);
            iFormModelServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var DataItem = iFormModelServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.Message = TxtMessage.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.LoginRequired = chbLogin.Checked;
            DataItem.RecordIP = chbIpRecord.Checked;
            iFormModelServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }
    }
}