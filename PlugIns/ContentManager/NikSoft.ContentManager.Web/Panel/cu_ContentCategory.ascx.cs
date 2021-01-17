using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_ContentCategory : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        public IGroupPermissionService iGroupPermissionServ { get; set; }
        public ICategoryPermissionService iCategoryPermissionServ { get; set; }

        const string Allow_ImageTypes = "jpeg,png,gif";
        protected string SelectData = string.Empty;
        protected int? ParentID;
        protected int GroupID;
        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            Container.GotoList += GoToList;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }

            ddlContentGroup.Enabled = false;
            ddlParent.Enabled = false;

            if (Request.QueryString["ParentID"] != null)
            {
                ParentID = int.Parse(Request.QueryString["ParentID"].ToString());
                var XParent = iContentCategoryServ.Find(x => x.ID == ParentID);
                GroupID = XParent.GroupID;
                ddlContentGroup.SelectedValue = GroupID.ToString();
                ddlParent.SelectedValue = ParentID.ToString();
            }
            else
            {
                ParentID = null;
                ddlContentGroup.Enabled = true;
            }

            

            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        protected void GoToList()
        {
            int? PID;
            if (ItemID > 0)
            {
                PID = iContentCategoryServ.Find(x => x.ID == ItemID, y => new { y.ParentID }).ParentID;
            }
            else
            {
                PID = ParentID;
            }
            RedirectTo("~/Panel/rd_ContentCategory/" + PID);
        }

        private void BindCombo()
        {
            var AllowGroups = iGroupPermissionServ.GetAll(x => x.UserID == PortalUser.ID).Select(x => x.GroupID).ToList();
            var Groups = iContentGroupServ.GetAll(t => t.PortalID == PortalUser.PortalID && AllowGroups.Contains(t.ID)).ToList();
            ddlContentGroup.FillControl(Groups.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");

            var AllowCategories = iCategoryPermissionServ.GetAll(x => x.UserID == PortalUser.ID).Select(x => x.CategoryID).ToList();
            var query1 = iContentCategoryServ.ExpressionMaker();
            query1.Add(x => x.PortalID == PortalUser.PortalID && AllowCategories.Contains(x.ID));

            if (ItemID > 0)
            {
                query1.Add(x => x.ID != ItemID);
                var ThisItem = iContentCategoryServ.Find(x => x.ID == ItemID);
                if (ThisItem.ParentID == null)
                {
                    query1.Add(x => x.ParentID == null);
                }
                
            }
            var Gategories = iContentCategoryServ.GetAll(query1).ToList();
            ddlParent.FillControl(Gategories.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            if (ddlContentGroup.SelectedValue.ToInt32() == 0)
            {
                ErrorMessage.Add("گروه محتوایی را انتخاب کنید.");
            }

            string FileExtension = fuIcon.PostedFile.ContentType.Split('/')[1];
            int FileSize = fuIcon.PostedFile.ContentLength;
            int MaxVal = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
            if (FileSize > 0)
            {
                if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                {
                    ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                }

                if ((MaxVal * 1024) < FileSize)
                {
                    ErrorMessage.Add("حجم تصویر باید کمتر از " + MaxVal + " KB باشد.");
                }
            }

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void GetEditData()
        {
            ddlContentGroup.Enabled = false;
            if (ParentID != null)
            {
                ddlParent.Enabled = true;
            }
            else
            {
                ddlParent.Enabled = true;
            }

            var data = iContentCategoryServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }

            txtTitle.Text = data.Title;
            TxtDesc.Text = data.Description;
            TxtKey.Text = data.ModuleKey;
            ddlContentGroup.SelectedValue = data.GroupID.ToString();
            if (data.ParentID != null)
            {
                ddlParent.SelectedValue = data.ParentID.ToString();
            }

            CatImage.ImageUrl = "/" + data.ImgUrl;
            TxtFontIcon.Text = data.IconFont;
            chbEnbaled.Checked = data.Enabled;
            chbHasFeature.Checked = data.HasFeature;
            chbIsStore.Checked = data.IsStore;


        }

        private void SaveNewData()
        {
            string vpath = "CMIcons";
            string ImageSrc = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref ImageSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            int items = iContentCategoryServ.Count(x => x.PortalID == PortalUser.PortalID);

            var DataItem = iContentCategoryServ.Create();
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.ModuleKey = TxtKey.Text;
            DataItem.GroupID = ddlContentGroup.SelectedValue.ToInt32();
            DataItem.Ordering = items + 1;
            DataItem.ImgUrl = ImageSrc;
            DataItem.IconFont = TxtFontIcon.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.IsStore = chbIsStore.Checked;
            DataItem.HasFeature = chbHasFeature.Checked;

            DataItem.ParentID = ParentID;
            DataItem.CreatedBy = PortalUser.ID;
            iContentCategoryServ.Add(DataItem);
            iContentCategoryServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            string vpath = "CMIcons";
            string ImageSrc = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref ImageSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }
            var DataItem = iContentCategoryServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.ModuleKey = TxtKey.Text;

            DataItem.GroupID = ddlContentGroup.SelectedValue.ToInt32();

            if (!ImageSrc.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = ImageSrc;
            }
            DataItem.IconFont = TxtFontIcon.Text;
            DataItem.IsStore = chbIsStore.Checked;
            DataItem.HasFeature = chbHasFeature.Checked;
            DataItem.Enabled = chbEnbaled.Checked;

            int pid = ddlParent.SelectedValue.ToInt32();
            if (pid == 0)
                DataItem.ParentID = null;
            else
                DataItem.ParentID = pid;

            DataItem.ModifiedBy = PortalUser.ID;
            iContentCategoryServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void BtnRemoveImg_Click(object sender, EventArgs e)
        {
            var DataItem = iContentCategoryServ.Find(y => y.ID == ItemID);
            if (!DataItem.ImgUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = "";
                iContentCategoryServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
            }
        }
    }
}