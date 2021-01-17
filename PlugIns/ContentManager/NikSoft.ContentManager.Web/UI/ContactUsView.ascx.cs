using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.UI
{
    public partial class ContactUsView : NikUserControl
    {
        public IContactUsService iContactUsServ { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool ValidateForm()
        {
            if (!TxtFname.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtFname.Validate());
            }

            if (!TxtLname.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtLname.Validate());
            }

            if (!TxtPhone.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtPhone.Validate());
            }

            if (!TxtPhone.Text.IsEmpty())
            {
                if (!TxtPhone.Text.IsNumeric())
                {
                    ErrorMessage.Add(TxtPhone.PublicMessage);
                }
            }

            if (!TxtEmail.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtEmail.Validate());
            }

            if (!TxtEmail.Text.IsEmpty())
            {
                bool isEmail = Regex.IsMatch(TxtEmail.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    ErrorMessage.Add(TxtEmail.PublicMessage);
                }
            }

            if (!TxtSubject.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtSubject.Validate());
            }

            if (!TxtMessage.Validate().IsEmpty())
            {
                ErrorMessage.Add(TxtMessage.Validate());
            }

            if (ErrorMessage.Count > 0)
            {
                Notification.SetErrorMessage(ErrorMessage);
                return false;
            }
            return true;
        }

        protected void resetForm()
        {
            TxtFname.Text = "";
            TxtLname.Text = "";
            TxtPhone.Text = "";
            TxtEmail.Text = "";
            TxtSubject.Text = "";
            TxtCompany.Text = "";
            TxtMessage.Text = "";
        }

        protected void BtnSend_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                return;
            }

            var myContact = iContactUsServ.Create();
            myContact.FirstName = TxtFname.Text;
            myContact.LastName = TxtLname.Text;
            myContact.Phone = TxtPhone.Text;
            myContact.Email = TxtEmail.Text;
            myContact.Company = TxtCompany.Text;
            myContact.Title = TxtSubject.Text;
            myContact.Message = TxtMessage.Text;
            if (PortalUser != null)
            {
                myContact.UserID = PortalUser.ID;
            }
            myContact.PortalID = CurrentPortalID;
            myContact.CreateDate = DateTime.Now;
            iContactUsServ.Add(myContact);
            iContactUsServ.SaveChanges();
            resetForm();
            Notification.SetSuccessMessage(HfSuccess.Value);
        }
    }
}