using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class cu_UserType : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }

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
            btnSubmit.Click += SaveClick;
            FormValidation = Validate;
        }

        protected void GetEditData()
        {
            var ut = iUserTypeServ.Find(t => t.ID == ItemID);
            if (ut == null)
            {
                Container.gotoList();
            }
            txtTitle.Text = ut.Title;
        }

        private void SaveNewData()
        {
            var ut = iUserTypeServ.Create();
            ut.Title = txtTitle.Text.Trim();
            ut.PortalID = PortalUser.PortalID;
            iUserTypeServ.Add(ut);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var ut = iUserTypeServ.Find(y => y.ID == ItemID);
            ut.Title = txtTitle.Text.Trim();
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("insert title text");
            }
            if (txtTitle.Text.StringIsNumber())
            {
                ErrorMessage.Add("title can not numberic");
            }
            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }
    }
}