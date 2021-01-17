using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class FeatureItems : NikUserControl
    {
        public IFeatureFormService iFeatureFormServ { get; set; }
        public IFTextBoxService iFTextBoxServ { get; set; }
        public IFDropDownListService iFDropDownListServ { get; set; }
        public IFCheckBoxListService iFCheckBoxListServ { get; set; }

        protected List<FeatuerModel> Thisfeatuers = new List<FeatuerModel>();

        public int FeatureID;
        public int CategoryID;
        public string FeatureName;

        protected void Page_Load(object sender, EventArgs e)
        {
            int FID;
            if (!int.TryParse(ModuleParameters, out FID))
            {
                Notification.SetErrorMessage("404 page note found");
                return;
            }
            FeatureID = FID;

            var thisFeature = iFeatureFormServ.Find(x => x.ID == FeatureID);
            if (thisFeature == null)
            {
                Notification.SetErrorMessage("404 page note found");
                return;
            }

            CategoryID = thisFeature.CategoryID;
            FeatureName = thisFeature.Title;
            HpBack.NavigateUrl = "/panel/ContentFeatures/" + CategoryID;
            HpCancel.NavigateUrl = "/panel/FeatureItems/" + FeatureID;
            BoundData();
        }


        protected override void BoundData()
        {
            Thisfeatuers.Clear();
            var textBoxes = iFTextBoxServ.GetAll(x => x.FeatureFormID == FeatureID);
            if (textBoxes.Count > 0)
            {
                foreach (var TB in textBoxes)
                {
                    var TxBx = new FeatuerModel();
                    TxBx.ItemID = TB.ID;
                    TxBx.Title = TB.Title;
                    TxBx.FeatureKey = TB.FeatureKey;
                    TxBx.Description = TB.Description;
                    TxBx.Type = TB.IsLarge ? FeatureType.TextArea : FeatureType.TextBox;
                    TxBx.Enabled = TB.Enabled;
                    TxBx.FormID = FeatureID;
                    Thisfeatuers.Add(TxBx);
                }
            }


            var ddlLists = iFDropDownListServ.GetAll(x => x.FeatureFormID == FeatureID && x.ParentID == null);
            if (ddlLists.Count > 0)
            {
                foreach (var DDL in ddlLists)
                {
                    FeatuerModel newDdl = new FeatuerModel();
                    newDdl.ItemID = DDL.ID;
                    newDdl.Title = DDL.Title;
                    newDdl.FeatureKey = DDL.FeatureKey;
                    newDdl.Type = FeatureType.DropDown;
                    newDdl.Enabled = DDL.Enabled;
                    newDdl.FormID = FeatureID;
                    Thisfeatuers.Add(newDdl);
                }
            }


            var chbLists = iFCheckBoxListServ.GetAll(x => x.FeatureFormID == FeatureID);
            if (chbLists.Count > 0)
            {
                foreach (var CHB in chbLists)
                {
                    FeatuerModel newChb = new FeatuerModel();
                    newChb.ItemID = CHB.ID;
                    newChb.Title = CHB.Title;
                    newChb.FeatureKey = CHB.FeatureKey;
                    newChb.Type = FeatureType.CheckBoxList;
                    newChb.Enabled = CHB.Enabled;
                    newChb.FormID = FeatureID;
                    Thisfeatuers.Add(newChb);
                }
            }


            GV1.DataSource = Thisfeatuers;
            GV1.DataBind();
        }


        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                //case "MoveUp":
                //    RearrangePriority("up", id);
                //    break;
                //case "MoveDown":
                //    RearrangePriority("down", id);
                //    break;
                case "enabled":
                    SetEnabled(id);
                    break;
                case "EditMe":
                    EditItem(id);
                    break;
                case "DeleteMe":
                    DeleteItem(id);
                    break;
                    //case "iscover":
                    //    SetCoverItem(id);
                    //    break;
            }
            BoundData();
        }

        protected void EditItem(int Id)
        {
            var item = Thisfeatuers.Find(x => x.ItemID == Id);
            if (item == null)
            {
                return;
            }

            HfEdit.Value = Id.ToString();

            SetBtns();

            txtTitle.Text = item.Title;
            TxtFeatureKey.Text = item.FeatureKey;
            TxtDesc.Text = item.Description;
            DdlType.SelectedValue = Convert.ToInt32(item.Type).ToString();
        }

        protected void DeleteItem(int Id)
        {
            var item = Thisfeatuers.Find(x => x.ItemID == Id);
            if (item == null)
            {
                return;
            }

            if (item.Type == FeatureType.TextBox || item.Type == FeatureType.TextArea)
            {
                var dbTBox = iFTextBoxServ.Find(x => x.ID == item.ItemID);
                iFTextBoxServ.Remove(dbTBox);
                iFTextBoxServ.SaveChanges();
            }

            if (item.Type == FeatureType.DropDown)
            {
                var dbDdl = iFDropDownListServ.Find(x => x.ID == item.ItemID);
                if (dbDdl.Childs.Count > 0)
                {
                    Notification.SetErrorMessage("لیست آبشاری داری آتم می باشد.");
                    return;
                }
                iFDropDownListServ.Remove(dbDdl);
                iFDropDownListServ.SaveChanges();
            }

            if (item.Type == FeatureType.CheckBoxList)
            {
                var dbChbox = iFCheckBoxListServ.Find(x => x.ID == item.ItemID);
                if (dbChbox.FCheckItems.Count > 0)
                {
                    Notification.SetErrorMessage("لیست انتخابی داری آتم می باشد.");
                    return;
                }
                iFCheckBoxListServ.Remove(dbChbox);
                iFCheckBoxListServ.SaveChanges();
            }

            BoundData();

        }

        protected void SetEnabled(int Id)
        {
            var item = Thisfeatuers.Find(x => x.ItemID == Id);
            if (item == null)
            {
                return;
            }

            if (item.Type == FeatureType.TextBox || item.Type == FeatureType.TextArea)
            {
                var dbTBox = iFTextBoxServ.Find(x => x.ID == item.ItemID);
                dbTBox.Enabled = !dbTBox.Enabled;
                iFTextBoxServ.SaveChanges();
            }

            if (item.Type == FeatureType.DropDown)
            {
                var dbDdl = iFDropDownListServ.Find(x => x.ID == item.ItemID);
                dbDdl.Enabled = !dbDdl.Enabled;
                iFDropDownListServ.SaveChanges();
            }

            if (item.Type == FeatureType.CheckBoxList)
            {
                var dbChbox = iFCheckBoxListServ.Find(x => x.ID == item.ItemID);
                dbChbox.Enabled = !dbChbox.Enabled;
                iFCheckBoxListServ.SaveChanges();
            }

            BoundData();

        }

        protected string GetTpye(FeatureType featureType)
        {
            switch (featureType)
            {
                case FeatureType.TextBox:
                    return "کادر متنی";
                case FeatureType.TextArea:
                    return "کادر متنی بزرگ";
                case FeatureType.DropDown:
                    return "لیست آبشاری";
                case FeatureType.CheckBoxList:
                    return "لیست انتخابی";
            }

            return "";
        }

        protected void SetBtns()
        {
            if (HfEdit.Value == "0")
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                DdlType.Enabled = true;
                HpCancel.Visible = false;
                HpBack.Visible = true;
            }
            else
            {
                DdlType.Enabled = false;
                btnUpdate.Visible = true;
                btnSave.Visible = false;
                HpCancel.Visible = true;
                HpBack.Visible = false;
            }

            txtTitle.Text = "";
            TxtDesc.Text = "";
            TxtFeatureKey.Text = "";
            DdlType.SelectedValue = "0";
        }

        protected bool ValidForm()
        {
            if (txtTitle.Text.IsEmpty())
                ErrorMessage.Add("عنوان را وارد کنید");
            if (DdlType.SelectedValue == "0")
                ErrorMessage.Add("نوع را وارد کنید");
            if(TxtFeatureKey.Text.IsEmpty())
                ErrorMessage.Add("کلید را وارد کنید");

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            Notification.SetErrorMessage(ErrorMessage);
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidForm())
            {
                return;
            }

            FeatureType featureType = (FeatureType)DdlType.SelectedValue.ToInt32();

            if (featureType == FeatureType.TextBox)
            {
                var txBox = iFTextBoxServ.Create();
                txBox.Title = txtTitle.Text;
                txBox.FeatureKey = TxtFeatureKey.Text;
                txBox.Description = TxtDesc.Text;
                txBox.Enabled = true;
                txBox.IsLarge = false;
                txBox.FeatureFormID = FeatureID;
                iFTextBoxServ.Add(txBox);
                iFTextBoxServ.SaveChanges();
            }

            if (featureType == FeatureType.TextArea)
            {
                var txBox = iFTextBoxServ.Create();
                txBox.Title = txtTitle.Text;
                txBox.FeatureKey = TxtFeatureKey.Text;
                txBox.Description = TxtDesc.Text;
                txBox.Enabled = true;
                txBox.IsLarge = true;
                txBox.FeatureFormID = FeatureID;
                iFTextBoxServ.Add(txBox);
                iFTextBoxServ.SaveChanges();
            }

            if (featureType == FeatureType.DropDown)
            {
                var Ddl = iFDropDownListServ.Create();
                Ddl.Title = txtTitle.Text;
                Ddl.FeatureKey = TxtFeatureKey.Text;
                Ddl.Description = TxtDesc.Text;
                Ddl.Enabled = true;
                Ddl.Ordering = 1;
                Ddl.ParentID = null;
                Ddl.FeatureFormID = FeatureID;
                iFDropDownListServ.Add(Ddl);
                iFDropDownListServ.SaveChanges();
            }

            if (featureType == FeatureType.CheckBoxList)
            {
                var Chb = iFCheckBoxListServ.Create();
                Chb.Title = txtTitle.Text;
                Chb.FeatureKey = TxtFeatureKey.Text;
                Chb.Description = TxtDesc.Text;
                Chb.Enabled = true;
                Chb.FeatureFormID = FeatureID;
                iFCheckBoxListServ.Add(Chb);
                iFCheckBoxListServ.SaveChanges();
            }

            SetBtns();
            BoundData();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidForm())
            {
                return;
            }

            int Id = HfEdit.Value.ToInt32();
            var ItemType = DdlType.SelectedValue.ToInt32();

            if ((FeatureType)ItemType == FeatureType.TextBox)
            {
                var TB = iFTextBoxServ.Find(x => x.ID == Id);
                TB.Title = txtTitle.Text;
                TB.FeatureKey = TxtFeatureKey.Text;
                TB.Description = TxtDesc.Text;
                TB.IsLarge = false;
                iFTextBoxServ.SaveChanges();
            }

            if ((FeatureType)ItemType == FeatureType.TextArea)
            {
                var TA = iFTextBoxServ.Find(x => x.ID == Id);
                TA.Title = txtTitle.Text;
                TA.FeatureKey = TxtFeatureKey.Text;
                TA.Description = TxtDesc.Text;
                TA.IsLarge = true;
                iFTextBoxServ.SaveChanges();
            }

            if ((FeatureType)ItemType == FeatureType.DropDown)
            {
                var DDL = iFDropDownListServ.Find(x => x.ID == Id);
                DDL.Title = txtTitle.Text;
                DDL.FeatureKey = TxtFeatureKey.Text;
                DDL.Description = TxtDesc.Text;
                iFDropDownListServ.SaveChanges();
            }

            if ((FeatureType)ItemType == FeatureType.CheckBoxList)
            {
                var CHB = iFCheckBoxListServ.Find(x => x.ID == Id);
                CHB.Title = txtTitle.Text;
                CHB.FeatureKey = TxtFeatureKey.Text;
                CHB.Description = TxtDesc.Text;
                iFCheckBoxListServ.SaveChanges();
            }

            HfEdit.Value = "0";
            SetBtns();
            BoundData();
        }
    }
}