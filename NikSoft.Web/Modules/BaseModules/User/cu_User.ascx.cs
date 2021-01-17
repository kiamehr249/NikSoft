using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.User
{
    public partial class cu_User : NikUserControl
    {

        public IPortalService iPortalServ { get; set; }

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
            if (PortalUser.PortalID != 1)
            {
                portalcont.Visible = false;
            }
            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSubmit.Click += SaveClick;
            FormValidation = Validate;
        }

        protected void GetEditData()
        {

            if (ItemID == PortalUser.ID)
            {
                logoutmsg.Visible = true;
            }
            else
            {
                logoutmsg.Visible = false;
            }


            NikModel.User u = null;
            if (PortalUser.PortalID == 1)
            {
                u = iUserServ.GetAll(x => x.ID == ItemID).Select(x => new NikModel.User() { FirstName = x.FirstName, UserName = x.UserName, LastName = x.LastName, Email = x.Email, PassExpireDate = x.PassExpireDate, UnExpire = x.UnExpire, IsLock = x.IsLock, PortalID = x.PortalID }).First();
            }
            else
            {
                u = iUserServ.GetAll(x => x.ID == ItemID && x.PortalID == PortalUser.PortalID).Select(x => new NikModel.User() { FirstName = x.FirstName, UserName = x.UserName, LastName = x.LastName, Email = x.Email, PassExpireDate = x.PassExpireDate, UnExpire = x.UnExpire, IsLock = x.IsLock, PortalID = x.PortalID }).First();
            }
            if (null != u)
            {
                txtLastName.Text = u.LastName;
                txtFirstName.Text = u.FirstName;
                txtUsername.Text = u.UserName;
                txtUsername.ReadOnly = true;
                txtEmail.Text = u.Email;
                dateExpire.Text = new Utilities.PersianDateTime(u.PassExpireDate).ToString(PersianDateTimeFormat.Date);
                chbUnExpire.Checked = u.UnExpire;
                chbIsLock.Checked = u.IsLock;
                ddlPortal.SelectedValue = u.PortalID.ToString();
            }
            else
            {
                Container.gotoList();
                return;
            }
        }

        private void SaveNewData()
        {
            var userLoginKey = Utilities.Utilities.RandomStringNumber();
            var UserRandomID = Utilities.Utilities.RandomNumber();
            var userPassFullHashed = iUserServ.GetPasswordHash(txtPassword.Text, txtUsername.Text, userLoginKey, UserRandomID);
            var u = new NikModel.User
            {
                Email = txtEmail.Text,
                LastName = txtLastName.Text,
                LoginKey = userLoginKey,
                FirstName = txtFirstName.Text,
                RandomID = UserRandomID,
                UserName = txtUsername.Text,
                Password = userPassFullHashed,
                PortalID = PortalUser.PortalID == 1 ? Convert.ToInt32(ddlPortal.SelectedValue) : PortalUser.PortalID,
                PassExpireDate = chbUnExpire.Checked ? DateTime.Now : Utilities.PersianDateTime.Parse(dateExpire.Text).ToDateTime(),
                UnExpire = chbUnExpire.Checked,
                IsLock = chbIsLock.Checked,
                UserType = NikUserType.NikUser
            };
            iUserServ.Add(u);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var u = iUserServ.Find(y => y.ID == ItemID);
            var Lock1 = u.IsLock;
            u.Email = txtEmail.Text;
            u.LastName = txtLastName.Text;
            u.FirstName = txtFirstName.Text;
            u.PassExpireDate = chbUnExpire.Checked ? DateTime.Now : Utilities.PersianDateTime.Parse(dateExpire.Text).ToDateTime();
            u.UnExpire = chbUnExpire.Checked;
            u.IsLock = chbIsLock.Checked;
            if (txtPassword.Text.Trim() != "")
            {
                if (txtPassword.Text.Length > 5)
                {
                    var userLoginKey = Utilities.Utilities.RandomStringNumber();
                    var userRandomID = Utilities.Utilities.RandomNumber();
                    var userPassFullHashed = iUserServ.GetPasswordHash(txtPassword.Text, u.UserName, userLoginKey, userRandomID);
                    u.RandomID = userRandomID;
                    u.LoginKey = userLoginKey;
                    u.Password = userPassFullHashed;
                }
            }
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void btnCheckuser_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.IsEmpty())
            {
                Notification.SetErrorMessage("insert username please");
                return;
            }
            if (ExistUser(txtUsername.Text.Trim()))
            {
                Notification.SetErrorMessage("Username is not valid\n");
            }
            else
            {
                Notification.SetSuccessMessage("Username is not valid\n");
            }
        }

        private bool ExistUser(string uname)
        {
            var u = iUserServ.GetAll(x => x.UserName == uname, x => x.ID);
            return null != u && u.Any();
        }

        private void BindCombo()
        {
            ddlPortal.FillControl(iPortalServ.GetAll(t => true).ToList(), "Title", "ID", true, true);
        }

        private bool Validate()
        {
            if (txtFirstName.Text.StringContainNumber())
            {
                ErrorMessage.Add("first name cont be numeric");
            }
            if (txtLastName.Text.StringContainNumber())
            {
                ErrorMessage.Add("last name cont be numeric");
            }
            if (ItemID == 0)
            {
                if (ExistUser(txtUsername.Text))
                {
                    ErrorMessage.Add("username is not valid");
                }
            }
            if (ItemID == 0)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ErrorMessage.Add("password cont be empty");
                }
                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ErrorMessage.Add("password not match with confirm");
                }
            }
            if (!txtEmail.Text.IsValidEmail())
            {
                ErrorMessage.Add("your email is not valid");
            }
            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (!chbUnExpire.Checked && string.IsNullOrWhiteSpace(dateExpire.Text))
            {
                ErrorMessage.Add("select the expire password date time");
            }
            if (PortalUser.PortalID == 1)
            {
                if (ddlPortal.SelectedIndex == 0)
                {
                    ErrorMessage.Add("select the user portal");
                }
            }
            if (!chbUnExpire.Checked)
            {
                var today = Utilities.PersianDateTime.Now.ToString(PersianDateTimeFormat.Date);
                if (today.CompareTo(dateExpire.Text) >= 0)
                {
                    ErrorMessage.Add("expire date time must be start tomarow");
                }
            }
            if (ErrorMessage.Count > 0)
            {
                Notification.SetErrorMessage(ErrorMessage);
                return false;
            }
            return true;
        }
    }
}