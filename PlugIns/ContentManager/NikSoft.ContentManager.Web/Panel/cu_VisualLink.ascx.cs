using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_VisualLink : NikUserControl
    {
        public IVisualLinkService iVisualLinkServ { get; set; }

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
            var data = iVisualLinkServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }


            txtTitle.Text = data.Title;
            TxtDesc.Text = data.Description;
        }

        private void SaveNewData()
        {
            int items = iVisualLinkServ.GetAll(x => x.PortalID == PortalUser.PortalID, y => new { y.ID }).ToList().Count;
            var DataItem = iVisualLinkServ.Create();
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.Enabled = true;
            iVisualLinkServ.Add(DataItem);
            iVisualLinkServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var DataItem = iVisualLinkServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            iVisualLinkServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }
    }
}