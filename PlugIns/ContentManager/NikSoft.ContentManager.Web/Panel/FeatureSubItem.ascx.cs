using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class FeatureSubItem : NikUserControl
    {
        public IFeatureFormService iFeatureFormServ { get; set; }
        public IFDropDownListService iFDropDownListServ { get; set; }
        public IFCheckBoxListService iFCheckBoxListServ { get; set; }
        public IFCheckItemService iFCheckItemServ { get; set; }

        protected List<FeatureListModel> ListModels = new List<FeatureListModel>();

        public int FeatureItemID;
        public int FormID;
        public int TypeID;
        public string FeatureItemName;
        public int ParentID;

        protected void Page_Load(object sender, EventArgs e)
        {
            int FID;
            if (!int.TryParse(ModuleParameters, out FID))
            {
                Notification.SetErrorMessage("404 page note found");
                return;
            }

            FormID = FID;

            var TID = Request.QueryString["type"];
            int typeID;
            if (!int.TryParse(TID, out typeID))
            {
                Notification.SetErrorMessage("404 page note found");
                return;
            }

            TypeID = typeID;

            string ParentRequest = Request.QueryString["parent"];
            if (ParentRequest != null)
            {
                int ParentDropID;
                if (!int.TryParse(ParentRequest, out ParentDropID))
                {
                    Notification.SetErrorMessage("404 page note found");
                    return;
                }

                ParentID = ParentDropID;
                var parentItem = iFDropDownListServ.Find(x => x.ID == ParentID);
                if (parentItem.ParentID == null)
                {
                    HpCancel.NavigateUrl = "/panel/FeatureSubItem/" + FormID + "?type=" + TypeID;
                    HpBack.NavigateUrl = "/panel/FeatureItems/" + FormID;
                }
                else
                {
                    HpCancel.NavigateUrl = "/panel/FeatureSubItem/" + FormID + "?type=" + TypeID;
                    HpBack.NavigateUrl = "/panel/FeatureSubItem/" + FormID + "?type=" + TypeID + "&parent=" + parentItem.ParentID;
                }

                
            }
            else
            {
                HpCancel.NavigateUrl = "/panel/FeatureSubItem/" + FormID + "?type=" + TypeID;
                HpBack.NavigateUrl = "/panel/FeatureItems/" + FormID;
            }

            BoundData();

        }


        protected override void BoundData()
        {
            ListModels.Clear();

            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                keyWrap.Visible = true;
                DescWrap.Visible = true;
                var ParentItem = iFDropDownListServ.Find(x => x.ID == ParentID);
                FormID = ParentItem.FeatureFormID;
                FeatureItemName = ParentItem.Title;

                var itemList = iFDropDownListServ.GetAll(x => x.ParentID == ParentID);
                if (itemList.Count > 0)
                {
                    foreach (var DDL in itemList)
                    {
                        FeatureListModel newDdl = new FeatureListModel();
                        newDdl.ID = DDL.ID;
                        newDdl.Title = DDL.Title;
                        newDdl.Enabled = DDL.Enabled;
                        newDdl.Ordering = DDL.Ordering;
                        newDdl.Description = DDL.Description;
                        newDdl.FeatureKey = DDL.FeatureKey;
                        newDdl.FeatureFormID = DDL.FeatureFormID;
                        ListModels.Add(newDdl);
                    }
                }
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                keyWrap.Visible = true;
                DescWrap.Visible = true;

                var FreatureItem = iFCheckBoxListServ.Find(x => x.ID == ParentID);
                FeatureItemName = FreatureItem.Title;
                FormID = FreatureItem.FeatureFormID;

                var itemList = iFCheckItemServ.GetAll(x => x.FCheckBoxListID == FeatureItemID);
                if (itemList.Count > 0)
                {
                    foreach (var CHB in itemList)
                    {
                        FeatureListModel newCHB = new FeatureListModel();
                        newCHB.ID = CHB.ID;
                        newCHB.Title = CHB.Title;
                        newCHB.Enabled = CHB.Enabled;
                        newCHB.Ordering = CHB.Ordering;
                        ListModels.Add(newCHB);
                    }
                }
            }

            GV1.DataSource = ListModels.OrderByDescending(x => x.Ordering);
            GV1.DataBind();
        }


        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "MoveUp":
                    RearrangePriority("up", id);
                    break;
                case "MoveDown":
                    RearrangePriority("down", id);
                    break;
                case "enabled":
                    SetEnabled(id);
                    break;
                case "EditMe":
                    EditItem(id);
                    break;
                case "DeleteMe":
                    DeleteItem(id);
                    break;
            }
            BoundData();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                var item = iFDropDownListServ.Find(x => x.ID == ItemID);
                if (action == "down")
                {
                    var t = iFDropDownListServ.GetAll(x => x.ParentID == FeatureItemID && x.Ordering < item.Ordering).ToList();
                    if (t.Count > 0)
                    {
                        int newOrder = t.Max(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iFDropDownListServ.SaveChanges();
                    }
                }
                else
                {
                    var t = iFDropDownListServ.GetAll(x => x.ParentID == FeatureItemID && x.Ordering > item.Ordering).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Min(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iFDropDownListServ.SaveChanges();
                    }
                }
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                var item = iFCheckItemServ.Find(x => x.ID == ItemID);
                if (action == "down")
                {
                    var t = iFCheckItemServ.GetAll(x => x.FCheckBoxListID == FeatureItemID && x.Ordering < item.Ordering).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Max(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iFCheckItemServ.SaveChanges();
                    }
                }
                else
                {
                    var t = iFCheckItemServ.GetAll(x => x.FCheckBoxListID == FeatureItemID && x.Ordering > item.Ordering).ToList();

                    if (t.Count > 0)
                    {
                        int newOrder = t.Min(x => x.Ordering);
                        var item2 = t.Find(x => x.Ordering == newOrder);
                        item2.Ordering = item.Ordering;
                        item.Ordering = newOrder;
                        iFCheckItemServ.SaveChanges();
                    }
                }
            }
        }

        protected void EditItem(int Id)
        {
            var item = ListModels.Find(x => x.ID == Id);
            if (item == null)
            {
                return;
            }

            HfEdit.Value = Id.ToString();
            SetBtns();

            txtTitle.Text = item.Title;
            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                TxtDesc.Text = item.Description;
                TxtKeyFeature.Text = item.FeatureKey;
            }

        }

        protected void DeleteItem(int Id)
        {
            var item = ListModels.Find(x => x.ID == Id);
            if (item == null)
            {
                return;
            }

            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                var dbDdl = iFDropDownListServ.Find(x => x.ID == item.ID);
                iFDropDownListServ.Remove(dbDdl);
                iFDropDownListServ.SaveChanges();
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                var dbChbox = iFCheckItemServ.Find(x => x.ID == item.ID);
                iFCheckItemServ.Remove(dbChbox);
                iFCheckItemServ.SaveChanges();
            }

            BoundData();

        }

        protected void SetEnabled(int Id)
        {
            var item = ListModels.Find(x => x.ID == Id);
            if (item == null)
            {
                return;
            }

            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                var dbDdl = iFDropDownListServ.Find(x => x.ID == item.ID);
                dbDdl.Enabled = !dbDdl.Enabled;
                iFDropDownListServ.SaveChanges();
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                var dbChbox = iFCheckItemServ.Find(x => x.ID == item.ID);
                dbChbox.Enabled = !dbChbox.Enabled;
                iFCheckItemServ.SaveChanges();
            }

            BoundData();

        }

        protected void SetBtns()
        {
            if (HfEdit.Value == "0")
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                HpCancel.Visible = false;
                HpBack.Visible = true;
                txtTitle.Text = "";
            }
            else
            {
                btnUpdate.Visible = true;
                btnSave.Visible = false;
                HpCancel.Visible = true;
                HpBack.Visible = false;
            }
        }

        protected bool ValidForm()
        {
            if (txtTitle.Text.IsEmpty())
                ErrorMessage.Add("عنوان را وارد کنید");
            if ((FeatureType)TypeID == FeatureType.DropDown && TxtKeyFeature.Text.IsEmpty())
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


            int MaxOrder = 0;
            if (ListModels.Count > 0)
            {
                MaxOrder = ListModels.Max(x => x.Ordering);
            }

            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                var Ddl = iFDropDownListServ.Create();
                Ddl.Title = txtTitle.Text;
                Ddl.Enabled = true;
                Ddl.ParentID = ParentID;
                Ddl.Ordering = MaxOrder + 1;
                Ddl.FeatureFormID = FormID;
                Ddl.FeatureKey = TxtKeyFeature.Text;
                iFDropDownListServ.Add(Ddl);
                iFDropDownListServ.SaveChanges();
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                var Chb = iFCheckItemServ.Create();
                Chb.Title = txtTitle.Text;
                Chb.Enabled = true;
                Chb.FCheckBoxListID = FeatureItemID;
                iFCheckItemServ.Add(Chb);
                iFCheckItemServ.SaveChanges();
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

            if ((FeatureType)TypeID == FeatureType.DropDown)
            {
                var DDL = iFDropDownListServ.Find(x => x.ID == Id);
                DDL.Title = txtTitle.Text;
                iFDropDownListServ.SaveChanges();
            }

            if ((FeatureType)TypeID == FeatureType.CheckBoxList)
            {
                var CHB = iFCheckItemServ.Find(x => x.ID == Id);
                CHB.Title = txtTitle.Text;
                iFCheckItemServ.SaveChanges();
            }

            HfEdit.Value = "0";
            SetBtns();
            BoundData();
        }
    }
}