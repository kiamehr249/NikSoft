using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_UserPublicContent : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        public IContentGroupService iContentGroupServ { get; set; }
        //public IGroupPermissionService iGroupPermissionServ { get; set; }
        //public ICategoryPermissionService iCategoryPermissionSer { get; set; }
        public int? ParCatid;
        public int groupID;

        const string Allow_ImageTypes = "jpeg,png,gif";

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string cat = Request.QueryString["parent"];
            int catid;
            if (cat != null)
            {
                if (int.TryParse(cat, out catid))
                {
                    ParCatid = catid;
                    var thiscat = iContentCategoryServ.Find(x => x.ID == ParCatid);
                    groupID = thiscat.GroupID;
                }
            }
            else
            {
                if (ItemID > 0)
                {
                    ParCatid = iPublicContentServ.Find(t => t.ID == ItemID).CategoryID;
                }
            }

            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        private bool Validate()
        {
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
            var thisContent = iPublicContentServ.Find(t => t.ID == ItemID);
            if (thisContent == null)
            {
                RedirectTo("/panel/rd_UserPublicContent/" + ParCatid);
                return;
            }

            txtTitle.Text = thisContent.Title;
            TxtIconFont.Text = thisContent.FontIcon;
            TxtMiniDesc.Text = thisContent.MiniDesc;
            TxtFullText.Text = thisContent.FullText;
            EditFullText.Content = thisContent.FullText;
            ImgContent.ImageUrl = "/" + thisContent.ImgUrl;
            ImgIcon.ImageUrl = "/" + thisContent.IconUrl;
            LblSelectFile.Text = thisContent.AttachFile;
            chbEnbaled.Checked = thisContent.Enabled;
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

            int maxOrder = 1;
            var qu = iPublicContentServ.ExpressionMaker();
            qu.Add(x => x.PortalID == PortalUser.PortalID);
            if (ParCatid > 0)
            {
                DataItem.CategoryID = ParCatid;
                qu.Add(x => x.CategoryID == ParCatid);
                maxOrder = iPublicContentServ.Count(qu);
            }

            if (groupID > 0)
            {
                DataItem.GroupID = groupID;
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
            DataItem.ImgUrl = ImgSrc;
            DataItem.IconUrl = IconSrc;
            DataItem.AttachFile = FileSrc;
            DataItem.Ordering = maxOrder + 1;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.IsStore = false;
            DataItem.CreateDate = DateTime.Now;
            iPublicContentServ.Add(DataItem);
            iPublicContentServ.SaveChanges(PortalUser.ID);
            RedirectTo("/panel/rd_UserPublicContent/" + ParCatid);
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
            DataItem.CreateDate = DateTime.Now;
            iPublicContentServ.SaveChanges(PortalUser.ID);
            RedirectTo("/panel/rd_UserPublicContent/" + ParCatid);
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