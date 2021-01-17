using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class FileUploadControl : NikUserControl
    {
        public IFileUploadModelService iFileUploadModelServ { get; set; }
        public IFormModelService iFormModelServ { get; set; }
        public FormModel thisForm;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitDate();
        }

        private void InitDate()
        {
            int formId;
            if (!int.TryParse(ModuleParameters, out formId))
            {
                RedirectTo("/" + Level + "/rd_forms");
                return;
            }

            thisForm = iFormModelServ.Find(x => x.ID == formId);
            if (thisForm == null)
            {
                RedirectTo("/" + Level + "/rd_forms");
                return;
            }

            DataBinder();

        }

        private void DataBinder()
        {
            var query = iFileUploadModelServ.ExpressionMaker();
            query.Add(t => t.FormID == thisForm.ID);
            base.FillManageFrom(iFileUploadModelServ, query);
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            var DataItem = iFileUploadModelServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "editme":
                    EditTextBox(id);
                    break;
                case "deleteme":
                    DeleteTexTBox(id);
                    break;
            }
            DataBinder();
        }

        private void DeleteTexTBox(int Id)
        {
            var deleItem = iFileUploadModelServ.Find(x => x.ID == Id);
            iFileUploadModelServ.Remove(deleItem);
            iFileUploadModelServ.SaveChanges();
        }

        private void EditTextBox(int Id)
        {
            var editItem = iFileUploadModelServ.Find(x => x.ID == Id);

            TxtTitle.Text = editItem.Title;
            TxtPath.Text = editItem.Path;
            TxtClass.Text = editItem.Class;
            TxtIdentityValue.Text = editItem.IdentityValue;
            TxtKeyWord.Text = editItem.KeyWord;
            TxtPosition.Text = editItem.Position.ToString();
            TxtMessage.Text = editItem.Message;
            chbNullable.Checked = editItem.IsNullable;
            HfTextBox.Value = editItem.ID.ToString();
        }

        private bool ValidTextBox()
        {
            FormControlChecker(TxtTitle.Text, ControlCheckType.String, "عنوان");
            FormControlChecker(TxtTitle.Text, ControlCheckType.String, "مسیر آپلود");
            FormControlChecker(TxtClass.Text, ControlCheckType.String, "کلاس");
            FormControlChecker(TxtKeyWord.Text, ControlCheckType.String, "کلید واژه");
            FormControlChecker(TxtMessage.Text, ControlCheckType.String, "متن خطا");
            FormControlChecker(TxtPosition.Text, ControlCheckType.String, "شماره جایگاه");

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                Notification.SetErrorMessage(ErrorMessage);
                return false;
            }
            return true;
        }

        protected string GetValueType(FbValueType type)
        {
            switch (type)
            {
                case FbValueType.Email:
                    return "ایمیل";
                case FbValueType.Numeric:
                    return "عدد";
                case FbValueType.Password:
                    return "پسورد";
                case FbValueType.Text:
                    return "متن";
            }
            return "ناشناس";
        }

        protected void btnSaveTextbox_Click(object sender, EventArgs e)
        {
            if (!ValidTextBox())
            {
                return;
            }

            if (IsEdit(HfTextBox))
            {
                var itemId = HfTextBox.Value.ToInt32();
                var editItem = iFileUploadModelServ.Find(x => x.ID == itemId);

                editItem.Title = TxtTitle.Text;
                editItem.Path = TxtPath.Text;
                editItem.Class = TxtClass.Text;
                editItem.IdentityValue = TxtIdentityValue.Text;
                editItem.KeyWord = TxtKeyWord.Text;
                editItem.Position = TxtPosition.Text.ToInt32();
                editItem.Message = TxtMessage.Text;
                editItem.IsNullable = chbNullable.Checked;
            }
            else
            {
                var newItem = iFileUploadModelServ.Create();
                newItem.Title = TxtTitle.Text;
                newItem.Path = TxtPath.Text;
                newItem.Class = TxtClass.Text;
                newItem.IdentityValue = TxtIdentityValue.Text;
                newItem.KeyWord = TxtKeyWord.Text;
                newItem.Position = TxtPosition.Text.ToInt32();
                newItem.Message = TxtMessage.Text;
                newItem.FormID = thisForm.ID;
                newItem.IsNullable = chbNullable.Checked;
                iFileUploadModelServ.Add(newItem);
            }

            iFileUploadModelServ.SaveChanges();
            ResetForm();
            DataBinder();
        }


        private void ResetForm()
        {
            TxtClass.Text = "";
            TxtIdentityValue.Text = "";
            TxtKeyWord.Text = "";
            TxtMessage.Text = "";
            TxtPosition.Text = "";
            TxtTitle.Text = "";
            chbNullable.Checked = false;
            HfTextBox.Value = "0";
        }


        private bool IsEdit(HiddenField clt)
        {
            if (clt.Value == "0")
            {
                return false;
            }

            return true;
        }
    }
}