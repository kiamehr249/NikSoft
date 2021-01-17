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
    public partial class TextBoxControl : NikUserControl
    {
        public ITextBoxModelService iTextBoxModelServ { get; set; }
        public IFormModelService iFormModelServ { get; set; }
        public FormModel thisForm;
        public int ControlType = 0;
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

            boundTextBox();

        }

        private void ShareValidControl()
        {
            if (ddlControlType.SelectedValue == "0")
            {
                ErrorMessage.Add("نوع کنترل را وارد کنید.");
            }

            if (TxtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            if (TxtClass.Text.IsEmpty())
            {
                ErrorMessage.Add("کلاس را وارد کنید.");
            }

            if (TxtKeyWord.Text.IsEmpty())
            {
                ErrorMessage.Add("کلید واژه را وارد کنید.");
            }

            if (TxtMessage.Text.IsEmpty())
            {
                ErrorMessage.Add("متن خطا را وارد کنید.");
            }

            if (TxtPosition.Text.IsEmpty())
            {
                ErrorMessage.Add("شماره جایگاه را وارد کنید.");
            }
        }

        private void boundTextBox()
        {
            var query = iTextBoxModelServ.ExpressionMaker();
            query.Add(t => t.FormID == thisForm.ID);
            base.FillManageFrom(iTextBoxModelServ, query);
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            var DataItem = iTextBoxModelServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "editme":
                    EditTextBox(id);
                    break;
                case "deleteme":
                    DeleteTexTBox(id);
                    break;
            }
            boundTextBox();
        }

        private void DeleteTexTBox(int Id)
        {
            var deleItem = iTextBoxModelServ.Find(x => x.ID == Id);
            iTextBoxModelServ.Remove(deleItem);
            iTextBoxModelServ.SaveChanges();
        }

        private void EditTextBox(int Id)
        {
            var editItem = iTextBoxModelServ.Find(x => x.ID == Id);
            if (editItem.TextMode == TextBoxType.TextBox)
                ddlControlType.SelectedValue = "1";
            else
                ddlControlType.SelectedValue = "2";

            TxtTitle.Text = editItem.Title;
            TxtClass.Text = editItem.Class;
            TxtIdentityValue.Text = editItem.IdentityValue;
            TxtKeyWord.Text = editItem.KeyWord;
            TxtPosition.Text = editItem.Position.ToString();
            TxtMessage.Text = editItem.Message;


            ddlDataType.SelectedValue = Convert.ToInt32(editItem.DateType).ToString();
            ddlValueType.SelectedValue = Convert.ToInt32(editItem.ValueType).ToString();
            TxtLength.Text = editItem.MaxLength.ToString();
            TxtPlacehoder.Text = editItem.Placeholder;
            TxtRows.Text = editItem.Rows.ToString();
            HfTextBox.Value = editItem.ID.ToString();
            ControlType = editItem.TextMode == TextBoxType.TextBox ? 1 : 2;
        }

        private bool ValidTextBox()
        {
            ShareValidControl();

            if (TxtLength.Text.IsEmpty())
            {
                ErrorMessage.Add("طول کارکتر را وارد کنید.");
            }

            if (ddlControlType.SelectedValue == "1")
            {
                if (ddlDataType.SelectedValue == "0")
                {
                    ErrorMessage.Add("نوع داده را وارد کنید.");
                }

                if (ddlValueType.SelectedValue == "0")
                {
                    ErrorMessage.Add("نوع داده را وارد کنید.");
                }
            }

            if (ddlControlType.SelectedValue == "2")
            {
                if (TxtRows.Text.IsEmpty())
                {
                    ErrorMessage.Add("تعداد سطر را وارد کنید.");
                }
            }



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
                var editItem = iTextBoxModelServ.Find(x => x.ID == itemId);

                editItem.Title = TxtTitle.Text;
                editItem.Class = TxtClass.Text;
                editItem.IdentityValue = TxtIdentityValue.Text;
                editItem.KeyWord = TxtKeyWord.Text;
                editItem.Position = TxtPosition.Text.ToInt32();
                editItem.Message = TxtMessage.Text;
                editItem.MaxLength = TxtLength.Text.ToInt32();
                editItem.Placeholder = TxtPlacehoder.Text;
                if (ddlControlType.SelectedValue == "1")
                {
                    editItem.TextMode = TextBoxType.TextBox;
                    editItem.DateType = (FbDataType)ddlDataType.SelectedValue.ToInt32();
                    editItem.ValueType = (FbValueType)ddlValueType.SelectedValue.ToInt32();
                }
                else if (ddlControlType.SelectedValue == "2")
                {
                    editItem.TextMode = TextBoxType.TextArea;
                    editItem.Rows = TxtRows.Text.ToInt32();
                }

            }
            else
            {
                var newItem = iTextBoxModelServ.Create();
                newItem.Title = TxtTitle.Text;
                newItem.Class = TxtClass.Text;
                newItem.IdentityValue = TxtIdentityValue.Text;
                newItem.KeyWord = TxtKeyWord.Text;
                newItem.Position = TxtPosition.Text.ToInt32();
                newItem.Message = TxtMessage.Text;
                if (ddlControlType.SelectedValue == "1")
                {
                    newItem.DateType = (FbDataType)ddlDataType.SelectedValue.ToInt32();
                    newItem.ValueType = (FbValueType)ddlValueType.SelectedValue.ToInt32();
                    newItem.TextMode = TextBoxType.TextBox;
                }
                else if (ddlControlType.SelectedValue == "2")
                {
                    newItem.DateType = FbDataType.String;
                    newItem.ValueType = FbValueType.Text;
                    newItem.Rows = TxtRows.Text.ToInt32();
                    newItem.TextMode = TextBoxType.TextArea;
                }

                newItem.MaxLength = TxtLength.Text.ToInt32();
                newItem.Placeholder = TxtPlacehoder.Text;
                newItem.FormID = thisForm.ID;
                iTextBoxModelServ.Add(newItem);
            }

            iTextBoxModelServ.SaveChanges();
            ResetForm();
            boundTextBox();
        }


        private void ResetForm()
        {
            ddlControlType.SelectedValue = "0";
            ddlDataType.SelectedValue = "0";
            ddlValueType.SelectedValue = "0";

            TxtClass.Text = "";
            TxtIdentityValue.Text = "";
            TxtKeyWord.Text = "";
            TxtLength.Text = "";
            TxtMessage.Text = "";
            TxtPlacehoder.Text = "";
            TxtPosition.Text = "";
            TxtRows.Text = "";
            TxtTitle.Text = "";

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