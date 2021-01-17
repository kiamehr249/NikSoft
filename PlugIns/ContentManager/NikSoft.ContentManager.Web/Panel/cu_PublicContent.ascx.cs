using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_PublicContent : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        public IContentGroupService iContentGroupServ { get; set; }
        public IGroupPermissionService iGroupPermissionServ { get; set; }
        public ICategoryPermissionService iCategoryPermissionSer { get; set; }
        const string Allow_ImageTypes = "jpeg,png,gif";
        protected string SelectData1 = string.Empty;

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
            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        private void BindCombo()
        {
            var allpermits = iGroupPermissionServ.GetAll(x => x.UserID == PortalUser.ID).Select(x => x.GroupID);
            var data = iContentGroupServ.GetAll(t => t.PortalID == PortalUser.PortalID && t.Enabled && allpermits.Contains(t.ID)).ToList();
            ddlContentGroup.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }

        private bool Validate()
        {
            var groupid = ddlContentGroup.SelectedValue;
            var catid = ddlCategory.GetDropDownValue(Request.Form);
            SelectData1 = "GetContentCategory(" + groupid + "," + catid + ");\n";
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            if (fuIcon.PostedFile.ContentLength > 0)
            {
                string File1Extension = fuIcon.PostedFile.ContentType.Split('/')[1];
                int File1Size = fuIcon.PostedFile.ContentLength;
                if (!Allow_ImageTypes.Split(',').Contains(File1Extension))
                {
                    ErrorMessage.Add("فرمت آیکن ارسالی اشتباه است.(jpg, png, gif)");
                }

                int MaxValIcon = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();

                if ((MaxValIcon * 1024) < File1Size)
                {
                    ErrorMessage.Add("حجم آیکن باید کمتر از " + MaxValIcon + " KB باشد.");
                }
            }

            if (FuImg.PostedFile.ContentLength > 0)
            {
                string File2Extension = FuImg.PostedFile.ContentType.Split('/')[1];
                if (!Allow_ImageTypes.Split(',').Contains(File2Extension))
                {
                    ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                }

                int File2Size = FuImg.PostedFile.ContentLength;
                int MaxValImage = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                if ((MaxValImage * 1024) < File2Size)
                {
                    ErrorMessage.Add("حجم تصویر باید کمتر از " + MaxValImage + " KB باشد.");
                }
            }

            if (FuAttachFile.PostedFile.ContentLength > 0)
            {
                string File3Extension = FuAttachFile.PostedFile.FileName;
                if (File3Extension.EndsWith(".exe"))
                {
                    ErrorMessage.Add("فرمت فایل ارسالی آپلود نمی شود.(exe!?)");
                }
                int MaxValFile = iNikSettingServ.GetSettingValue("FileMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                int File3Size = FuAttachFile.PostedFile.ContentLength;
                if ((MaxValFile * 1024 * 1024) < File3Size)
                {
                    ErrorMessage.Add("حجم فایل باید کمتر از " + MaxValFile + " MB باشد.");
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
            var data = iPublicContentServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }

            if (data.GroupID != null && data.CategoryID != null)
            {
                SelectData1 = "GetContentCategory(" + data.GroupID + "," + data.CategoryID + ");\n";
            }


            txtTitle.Text = data.Title;
            TxtIconFont.Text = data.FontIcon;
            ddlContentGroup.SelectedValue = data.GroupID.ToString();
            TxtMiniDesc.Text = data.MiniDesc;
            TxtFullText.Text = data.FullText;
            EditFullText.Content = data.FullText;
            ImgContent.ImageUrl = "/" + data.ImgUrl;
            ImgIcon.ImageUrl = "/" + data.IconUrl;
            LblSelectFile.Text = data.AttachFile;
            chbEnbaled.Checked = data.Enabled;
            chbIsStore.Checked = data.IsStore;
        }

        private void SaveNewData()
        {
            string vpath = "CMIcons";
            string IconSrc = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref IconSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("آیکن آپلود نشد.");
                    return;
                }
            }

            string VImgPath = "ContentFiles";
            string ImgSrc = "";
            if (FuImg.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg.PostedFile, VImgPath, ref ImgSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string VFilePath = "ContentFiles";
            string FileSrc = "";
            if (FuAttachFile.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuAttachFile.PostedFile, VFilePath, ref FileSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("فایل آپلود نشد.");
                    return;
                }
            }

            var DataItem = iPublicContentServ.Create();
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.Title = txtTitle.Text.Trim();

            var GroupID = ddlContentGroup.GetDropDownValue(Request.Form);
            var CategoryID = ddlCategory.GetDropDownValue(Request.Form);

            if (GroupID > 0)
            {
                DataItem.GroupID = GroupID;
            }

            var qu = iPublicContentServ.ExpressionMaker();
            qu.Add(x => x.PortalID == PortalUser.PortalID);
            if (CategoryID > 0)
            {
                DataItem.CategoryID = CategoryID;
                qu.Add(x => x.CategoryID == CategoryID);
            }
            int maxOrder = iPublicContentServ.Count(qu);



            DataItem.FontIcon = TxtIconFont.Text;
            DataItem.MiniDesc = TxtMiniDesc.Text;
            if (hfEditor.Value == "false")
            {
                DataItem.FullText = TxtFullText.Text;
            }
            else
            {
                DataItem.FullText = EditFullText.Content;
            }
            DataItem.ImgUrl = ImgSrc;
            DataItem.IconUrl = IconSrc;
            DataItem.AttachFile = FileSrc;
            DataItem.Ordering = maxOrder + 1;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.IsStore = chbIsStore.Checked;
            DataItem.CreateDate = DateTime.Now;
            iPublicContentServ.Add(DataItem);
            iPublicContentServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            string vpath = "CMIcons";
            string IconSrc = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref IconSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("آیکن آپلود نشد.");
                    return;
                }
            }

            string VImgPath = "ContentFiles";
            string ImgSrc = "";
            if (FuImg.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg.PostedFile, VImgPath, ref ImgSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string VFilePath = "ContentFiles";
            string FileSrc = "";
            if (FuAttachFile.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuAttachFile.PostedFile, VFilePath, ref FileSrc, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("فایل آپلود نشد.");
                    return;
                }
            }

            var DataItem = iPublicContentServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();

            var GroupID = ddlContentGroup.GetDropDownValue(Request.Form);
            var CategoryID = ddlCategory.GetDropDownValue(Request.Form);

            if (GroupID > 0)
            {
                DataItem.GroupID = GroupID;
            }
            else
            {
                DataItem.GroupID = null;
            }

            if (CategoryID > 0)
            {
                DataItem.CategoryID = CategoryID;
            }
            else
            {
                DataItem.CategoryID = null;
            }

            DataItem.FontIcon = TxtIconFont.Text;
            DataItem.MiniDesc = TxtMiniDesc.Text;
            if (hfEditor.Value == "false")
            {
                DataItem.FullText = TxtFullText.Text;
            }
            else
            {
                DataItem.FullText = EditFullText.Content;
            }

            if (!IconSrc.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.IconUrl);
                DataItem.IconUrl = IconSrc;
            }
            if (!ImgSrc.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = ImgSrc;
            }
            if (!FileSrc.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.AttachFile);
                DataItem.AttachFile = FileSrc;
            }
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.IsStore = chbIsStore.Checked;
            DataItem.ModifiedDate = DateTime.Now;
            iPublicContentServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void BtnRemoveImg_Click(object sender, EventArgs e)
        {
            var DataItem = iPublicContentServ.Find(y => y.ID == ItemID);
            if (!DataItem.ImgUrl.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl))
                {
                    Notification.SetErrorMessage("تصویر حذف نشد.");
                    return;
                }
                DataItem.ImgUrl = "";
                iPublicContentServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
                ImgContent.ImageUrl = "";
            }
        }

        protected void BtnTextEditor_Click(object sender, EventArgs e)
        {
            if (hfEditor.Value == "false")
            {
                TextAreaText.Visible = false;
                EditorText.Visible = true;
                hfEditor.Value = "true";
            }
            else
            {
                TextAreaText.Visible = true;
                EditorText.Visible = false;
                hfEditor.Value = "false";
            }
        }

        protected void BtnRemoveIcon_Click(object sender, EventArgs e)
        {
            var DataItem = iPublicContentServ.Find(y => y.ID == ItemID);
            if (!DataItem.IconUrl.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.IconUrl))
                {
                    Notification.SetErrorMessage("آیکن حذف نشد.");
                    return;
                }
                DataItem.IconUrl = "";
                iPublicContentServ.SaveChanges();
                Notification.SetSuccessMessage("آیکن حذف شد.");
                ImgIcon.ImageUrl = "";
            }
        }

        protected void BtnRemovFile_Click(object sender, EventArgs e)
        {
            var DataItem = iPublicContentServ.Find(y => y.ID == ItemID);
            if (!DataItem.AttachFile.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.AttachFile))
                {
                    Notification.SetErrorMessage("فایل حذف نشد.");
                    return;
                }
                DataItem.AttachFile = "";
                iPublicContentServ.SaveChanges();
                Notification.SetSuccessMessage("فایل حذف شد.");
                LblSelectFile.Text = "";
            }
        }
    }
}