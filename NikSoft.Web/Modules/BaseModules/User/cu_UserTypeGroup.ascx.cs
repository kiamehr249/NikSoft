using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class cu_UserTypeGroup : NikUserControl
    {
        public IUserTypeService iUserTypeServ { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            FormValidation = ValidateForm;
            btnSubmit.Click += SaveClick;
            base.OnLoad(e);
        }

        protected void GetEditData()
        {
            var utg = iUserTypeGroupServ.Find(t => t.ID == ItemID);
            if (utg == null)
            {
                Container.gotoList();
            }
            txtTitle.Text = utg.Title;
            ddlUserType.SelectedValue = utg.UserTypeID.ToString();
        }

        private void SaveNewData()
        {
            var utg = iUserTypeGroupServ.Create();
            utg.Title = txtTitle.Text.Trim();
            utg.UserTypeID = ddlUserType.SelectedValue.ToInt32();
            iUserTypeGroupServ.Add(utg);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var utg = iUserTypeGroupServ.Find(y => y.ID == ItemID);
            utg.Title = txtTitle.Text.Trim();
            utg.UserTypeID = ddlUserType.SelectedValue.ToInt32();
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private bool ValidateForm()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("insert title pleas");
            }
            if (txtTitle.Text.StringIsNumber())
            {
                ErrorMessage.Add("title can not be numberic");
            }
            if (ddlUserType.SelectedIndex == 0)
            {
                ErrorMessage.Add("select the user group pleas");
            }
            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        private void BindCombo()
        {
            ddlUserType.FillControl(iUserTypeServ.GetAll(t => t.PortalID == PortalUser.PortalID, t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }
    }
}