using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class FormListControls : NikUserControl
    {
        public IListControlModelService iListControlModelServ { get; set; }
        public IFormModelService iFormModelServ { get; set; }
        public FormModel thisForm;
        public int ControlType = 0;
        protected string SelectData1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BoundCombo();
            }
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

        private void BoundCombo()
        {
            var parent = iListControlModelServ.GetAll(x => true, y => new { y.ID, y.Title }).ToList();
            ddlParent.FillControl(parent, "Title", "ID");
        }

        private void DataBinder()
        {
            var query = iListControlModelServ.ExpressionMaker();
            query.Add(t => t.FormID == thisForm.ID);
            base.FillManageFrom(iListControlModelServ, query);
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            var DataItem = iListControlModelServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "editme":
                    EditItem(id);
                    break;
                case "deleteme":
                    DeleteItem(id);
                    break;
            }
            DataBinder();
        }

        private void DeleteItem(int Id)
        {
            var deleItem = iListControlModelServ.Find(x => x.ID == Id);
            iListControlModelServ.Remove(deleItem);
            iListControlModelServ.SaveChanges();
        }

        private void EditItem(int Id)
        {
            var editItem = iListControlModelServ.Find(x => x.ID == Id);
            ControlType = (int)editItem.ControlType;
            if (editItem.ControlType == ListControlType.DropDownList)
            {
                ddlControlType.SelectedValue = "1";
            }
            else if (editItem.ControlType == ListControlType.CheckBoxList)
            {
                ddlControlType.SelectedValue = "2";
            }
            else if (editItem.ControlType == ListControlType.RadioButtonList)
            {
                ddlControlType.SelectedValue = "3";
            }

            if (editItem.Parent != null)
                ddlParent.SelectedValue = editItem.ParentID.ToString();

            SelectData1 = "getListItems(" + editItem.ID + "," + editItem.ParentID + ");\n";

            chbIsNullable.Checked = editItem.IsNullable;
            TxtTitle.Text = editItem.Title;
            TxtClass.Text = editItem.Class;
            TxtIdentityValue.Text = editItem.IdentityValue;
            TxtKeyWord.Text = editItem.KeyWord;
            TxtPosition.Text = editItem.Position.ToString();
            TxtMessage.Text = editItem.Message;

            HfEdit.Value = editItem.ID.ToString();
            ControlType = (int)editItem.ControlType;
        }

        private bool ValidTextBox()
        {
            FormControlChecker(ddlControlType.SelectedValue, ControlCheckType.Numeric, "نوع کنترل", 3, 1);
            FormControlChecker(TxtTitle.Text, ControlCheckType.String, "عنوان");
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

        protected string GetControlType(ListControlType type)
        {
            switch (type)
            {
                case ListControlType.DropDownList:
                    return "لیست آبشاری";
                case ListControlType.CheckBoxList:
                    return "لیست چند انتخابی";
                case ListControlType.RadioButtonList:
                    return "لیست رادیویی";
            }
            return "ناشناس";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidTextBox())
            {
                return;
            }

            if (IsEdit(HfEdit))
            {
                var itemId = HfEdit.Value.ToInt32();
                var editItem = iListControlModelServ.Find(x => x.ID == itemId);

                if (!ddlParent.SelectedValue.IsEmpty() && ddlParent.SelectedValue != "0")
                    editItem.ParentID = ddlParent.SelectedValue.ToInt32();
                else
                    editItem.ParentID = null;

                editItem.Title = TxtTitle.Text;
                editItem.IsNullable = chbIsNullable.Checked;
                editItem.Class = TxtClass.Text;
                editItem.IdentityValue = TxtIdentityValue.Text;
                editItem.KeyWord = TxtKeyWord.Text;
                editItem.Position = TxtPosition.Text.ToInt32();
                editItem.Message = TxtMessage.Text;
                editItem.ControlType = (ListControlType)ddlControlType.SelectedValue.ToInt32();

            }
            else
            {
                var newItem = iListControlModelServ.Create();
                if (!ddlParent.SelectedValue.IsEmpty() && ddlParent.SelectedValue != "0")
                    newItem.ParentID = ddlParent.SelectedValue.ToInt32();
                else
                    newItem.ParentID = null;
                newItem.Title = TxtTitle.Text;
                newItem.IsNullable = chbIsNullable.Checked;
                newItem.Class = TxtClass.Text;
                newItem.IdentityValue = TxtIdentityValue.Text;
                newItem.KeyWord = TxtKeyWord.Text;
                newItem.Position = TxtPosition.Text.ToInt32();
                newItem.Message = TxtMessage.Text;
                newItem.ControlType = (ListControlType)ddlControlType.SelectedValue.ToInt32();
                newItem.FormID = thisForm.ID;
                iListControlModelServ.Add(newItem);
            }

            iListControlModelServ.SaveChanges();
            ResetForm();
            DataBinder();
        }


        private void ResetForm()
        {
            var textBoxeList = this.Controls.OfType<TextBox>();
            foreach (var item in textBoxeList)
            {
                item.Text = "";
            }

            var ddlList = this.Controls.OfType<DropDownList>();
            foreach (var item in ddlList)
            {
                item.SelectedValue = "0";
            }

            var checkList = this.Controls.OfType<CheckBox>();
            foreach (var item in checkList)
            {
                item.Checked = false;
            }

            HfEdit.Value = "0";
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