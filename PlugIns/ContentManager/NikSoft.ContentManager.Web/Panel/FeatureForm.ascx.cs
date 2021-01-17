using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class FeatureForm : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IFeatureFormService iFeatureFormServ { get; set; }
        public IFTextBoxService iFTextBoxServ { get; set; }
        public IFDropDownListService iFDropDownListServ { get; set; }
        public IFDropDownItemService iFDropDownItemServ { get; set; }
        public IFCheckBoxListService iFCheckBoxListServ { get; set; }
        public IFCheckItemService iFCheckItemServ { get; set; }
        public IFTextBoxAnswerService iFTextBoxAnswerServ { get; set; }
        public IFDropDownAnswerService iFDropDownAnswerServ { get; set; }
        public IFCheckBoxAnswerService iFCheckBoxAnswerServ { get; set; }

        public bool IsStore = false;
        public int ContentID;
        public int CategoryID;
        protected void Page_Load(object sender, EventArgs e)
        {
            string CT = Request.QueryString["type"];
            int ContentType;
            if (!int.TryParse(CT, out ContentType))
            {
                return;
            }

            if (ContentType == 1)
                IsStore = false;
            else
                IsStore = true;

            int CID;
            if (!int.TryParse(ModuleParameters, out CID))
            {
                return;
            }

            ContentID = CID;

            if (!IsStore)
            {
                var ThisContent = iPublicContentServ.Find(x => x.ID == ContentID);
                CategoryID = ThisContent.CategoryID.Value;
            }
            else
            {

            }
            

            FormGenerat();
        }

        protected void FormGenerat()
        {
            var ThisForm = iFeatureFormServ.GetAll(x => x.CategoryID == CategoryID).FirstOrDefault();
            var TxtBoxes = iFTextBoxServ.GetAll(x => x.FeatureFormID == ThisForm.ID && !x.IsLarge);

            string startCol4 = "<div class='col-sm-4'><div class='form-group'>";
            string startCol12 = "<div class='col-sm-12'><div class='form-group'>";
            string endCol = "</div></div>";
            

            foreach (var TxBox in TxtBoxes)
            {
                PlaceHolder TextBoxHolder = new PlaceHolder();

                Literal StartTxt = new Literal();
                StartTxt.Text = startCol4;
                TextBoxHolder.Controls.Add(StartTxt);

                Label TxtBoxLabel = new Label();
                TxtBoxLabel.Text = TxBox.Title;
                TxtBoxLabel.AssociatedControlID = "FTxt" + TxBox.ID;
                TextBoxHolder.Controls.Add(TxtBoxLabel);

                TextBox TextBoxItem = new TextBox();
                TextBoxItem.ID = "FTxt" + TxBox.ID;
                TextBoxItem.CssClass = "form-control";
                TextBoxItem.ClientIDMode = ClientIDMode.Static;
                TextBoxHolder.Controls.Add(TextBoxItem);

                Literal EndTxt = new Literal();
                EndTxt.Text = endCol;
                TextBoxHolder.Controls.Add(EndTxt);

                PlcTextBox.Controls.Add(TextBoxHolder);
            }

            var DropDowns = iFDropDownListServ.GetAll(x => x.FeatureFormID == ThisForm.ID);
            foreach (var Ddl in DropDowns)
            {
                PlaceHolder DropDownHolder = new PlaceHolder();

                Literal StartDDl = new Literal();
                StartDDl.Text = startCol4;
                DropDownHolder.Controls.Add(StartDDl);

                Label DDlLabel = new Label();
                DDlLabel.Text = Ddl.Title;
                DDlLabel.AssociatedControlID = "FDdl" + Ddl.ID;
                DropDownHolder.Controls.Add(DDlLabel);

                DropDownList DropDownItem = new DropDownList();
                DropDownItem.ID = "FDdl" + Ddl.ID;
                DropDownItem.CssClass = "form-control select2";
                DropDownItem.ClientIDMode = ClientIDMode.Static;

                var DropItems = iFDropDownItemServ.GetAll(x => x.FDDLID == Ddl.ID).OrderBy(x => x.Ordering).Select(x => new ListControlModel { ID = x.ID, Title = x.Title }).ToList();
                DropDownItem.DataSource = DropItems;
                DropDownItem.DataTextField = "Title";
                DropDownItem.DataValueField = "ID";
                DropDownItem.DataBind();
                DropDownHolder.Controls.Add(DropDownItem);

                Literal EndDDl = new Literal();
                EndDDl.Text = endCol;
                DropDownHolder.Controls.Add(EndDDl);

                PlcDropDown.Controls.Add(DropDownHolder);

            }

            var CheckBoxes = iFCheckBoxListServ.GetAll(x => x.FeatureFormID == ThisForm.ID);
            foreach (var CheckBox in CheckBoxes)
            {
                PlaceHolder CheckBoxHolder = new PlaceHolder();

                Literal StartCheck = new Literal();
                StartCheck.Text = startCol4;
                CheckBoxHolder.Controls.Add(StartCheck);

                Label CheckLabel = new Label();
                CheckLabel.Text = CheckBox.Title;
                CheckBoxHolder.Controls.Add(CheckLabel);

                CheckBoxList CheckBoxItem = new CheckBoxList();
                CheckBoxItem.ID = "FChb" + CheckBox.ID;
                CheckBoxItem.CssClass = "checkbox checkboxlist";
                CheckBoxItem.ClientIDMode = ClientIDMode.Static;
                CheckBoxItem.RepeatDirection = RepeatDirection.Vertical;
                CheckBoxItem.RepeatLayout = RepeatLayout.Flow;

                var CheckBoxItems = iFCheckItemServ.GetAll(x => x.FCheckBoxListID == CheckBox.ID).OrderBy(x => x.Ordering).Select(x => new ListControlModel { ID = x.ID, Title = x.Title }).ToList();
                CheckBoxItem.DataSource = CheckBoxItems;
                CheckBoxItem.DataTextField = "Title";
                CheckBoxItem.DataValueField = "ID";
                CheckBoxItem.DataBind();
                CheckBoxHolder.Controls.Add(CheckBoxItem);

                Literal EndCheck = new Literal();
                EndCheck.Text = endCol;
                CheckBoxHolder.Controls.Add(EndCheck);

                PlcCheckBoxList.Controls.Add(CheckBoxHolder);
            }

            var TextAreas = iFTextBoxServ.GetAll(x => x.FeatureFormID == ThisForm.ID && x.IsLarge);
            foreach (var TxArea in TextAreas)
            {
                PlaceHolder TextAreaHolder = new PlaceHolder();

                Literal StartArea = new Literal();
                StartArea.Text = startCol12;
                TextAreaHolder.Controls.Add(StartArea);

                Label TxtAreaLabel = new Label();
                TxtAreaLabel.Text = TxArea.Title;
                TxtAreaLabel.AssociatedControlID = "FTxtArea" + TxArea.ID;
                TextAreaHolder.Controls.Add(TxtAreaLabel);

                TextBox TextAreaItem = new TextBox();
                TextAreaItem.ID = "FTxtArea" + TxArea.ID;
                TextAreaItem.CssClass = "form-control";
                TextAreaItem.ClientIDMode = ClientIDMode.Static;
                TextAreaItem.TextMode = TextBoxMode.MultiLine;
                TextAreaItem.Rows = 3;
                TextAreaHolder.Controls.Add(TextAreaItem);

                Literal EndArea = new Literal();
                EndArea.Text = endCol;
                TextAreaHolder.Controls.Add(EndArea);

                PlcTextArea.Controls.Add(TextAreaHolder);
            }


        }
    }
}