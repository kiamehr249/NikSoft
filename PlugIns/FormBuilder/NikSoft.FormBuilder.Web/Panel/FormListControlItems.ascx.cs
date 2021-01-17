using NikSoft.FormBuilder.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.FormBuilder.Web.Panel
{
    public partial class FormListControlItems : NikUserControl
    {
        public IListControlModelService iListControlModelServ { get; set; }
        public IFormModelService iFormModelServ { get; set; }
        public IListControlItemModelService iListControlItemModelServ { get; set; }
        public FormModel thisForm;
        public ListControlModel thisListControl;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            InitDate();
            if (!IsPostBack)
            {
                BoundCombo();
            }
            ddlControl.SelectedValue = thisListControl.ID.ToString();
            DataBinder();
        }

        private void InitDate()
        {
            int controlId;
            if (!int.TryParse(ModuleParameters, out controlId))
            {
                RedirectTo("/" + Level + "/rd_forms");
                return;
            }

            thisListControl = iListControlModelServ.Find(x => x.ID == controlId);
            thisForm = iFormModelServ.Find(x => x.ID == thisListControl.FormID);

            if (thisForm == null)
            {
                RedirectTo("/" + Level + "/rd_forms");
                return;
            }

            if (thisListControl.ParentID != null)
            {
                childCell.Visible = true;
            }
            

        }

        private void BoundCombo()
        {
            var query = iListControlModelServ.ExpressionMaker();
            var query2 = iListControlItemModelServ.ExpressionMaker();
            if (thisListControl.ControlType == ListControlType.DropDownList)
            {
                query.Add(x => x.ControlType == ListControlType.DropDownList);
                if (thisListControl.ParentID != null)
                {
                    query2.Add(x => x.ListControlID == thisListControl.ParentID);
                    var parentItems = iListControlItemModelServ.GetAll(query2, y => new { y.ID, y.Title }).ToList();
                    ddlParentItems.FillControl(parentItems, "Title", "ID");
                }
            }
            else if (thisListControl.ControlType == ListControlType.CheckBoxList)
            {
                query.Add(x => x.ControlType == ListControlType.CheckBoxList);
            }
            else if (thisListControl.ControlType == ListControlType.RadioButtonList)
            {
                query.Add(x => x.ControlType == ListControlType.RadioButtonList);
            }

            var controls = iListControlModelServ.GetAll(query, y => new { y.ID, y.Title }).ToList();
            ddlControl.FillControl(controls, "Title", "ID");
            
        }

        private void DataBinder()
        {
            var query = iListControlItemModelServ.ExpressionMaker();
            query.Add(t => t.FormID == thisForm.ID && t.ListControlID == thisListControl.ID);
            base.FillManageFrom(iListControlItemModelServ, query);
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            var DataItem = iListControlItemModelServ.Find(x => x.ID == id);
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
            var deleItem = iListControlItemModelServ.Find(x => x.ID == Id);
            iListControlItemModelServ.Remove(deleItem);
            iListControlItemModelServ.SaveChanges();
        }

        private void EditItem(int Id)
        {
            var editItem = iListControlItemModelServ.Find(x => x.ID == Id);

            if (editItem.ParentID != null)
            {
                ddlParentItems.SelectedValue = editItem.ParentID.ToString();
            }
            ddlControl.SelectedValue = editItem.ListControlID.ToString();
            TxtTitle.Text = editItem.Title;
            HfEdit.Value = editItem.ID.ToString();
            ddlControl.Enabled = true;
        }

        private bool ValidForm()
        {
            FormControlChecker(TxtTitle.Text, ControlCheckType.String, "عنوان");
            FormControlChecker(ddlControl.SelectedValue, ControlCheckType.Numeric, "لیست کنترول");
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
            if (!ValidForm())
            {
                return;
            }

            if (IsEdit(HfEdit))
            {
                var itemId = HfEdit.Value.ToInt32();
                var editItem = iListControlItemModelServ.Find(x => x.ID == itemId);

                editItem.Title = TxtTitle.Text;
                editItem.ListControlID = ddlControl.SelectedValue.ToInt32();
                if (CheckDropDownValue(ddlParentItems))
                {
                    editItem.ParentID = ddlParentItems.SelectedValue.ToInt32();
                }
            }
            else
            {
                var newItem = iListControlItemModelServ.Create();
                newItem.Title = TxtTitle.Text;
                newItem.ListControlID = ddlControl.SelectedValue.ToInt32();
                newItem.FormID = thisForm.ID;
                if (CheckDropDownValue(ddlParentItems))
                {
                    newItem.ParentID = ddlParentItems.SelectedValue.ToInt32();
                }
                iListControlItemModelServ.Add(newItem);
            }

            iListControlItemModelServ.SaveChanges();
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

        private bool CheckDropDownValue(DropDownList clt)
        {
            if (clt.Items.Count > 0)
            {
                if (clt.SelectedValue == "0")
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        
    }
}