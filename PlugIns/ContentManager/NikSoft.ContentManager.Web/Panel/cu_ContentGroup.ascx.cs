using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_ContentGroup : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }
        const string Allow_ImageTypes = "jpeg,png,gif";

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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

            string FileExtension = fuIcon.PostedFile.ContentType.Split('/')[1];
            int FileSize = fuIcon.PostedFile.ContentLength;
            int MaxVal = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();

            if (FileSize > 0)
            {
                if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                {
                    ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                }
            }
            

            if ((MaxVal * 1024) < FileSize)
            {
                ErrorMessage.Add("حجم تصویر باید کمتر از " + MaxVal + " KB باشد.");
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
            var data = iContentGroupServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            TxtDesc.Text = data.Description;
            TxtFontIcon.Text = data.FontIcon;
            CatImage.ImageUrl = "/" + data.ImgUrl;
            chbEnbaled.Checked = data.Enabled;
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

            int items = iContentGroupServ.Count(x => x.PortalID == PortalUser.PortalID);

            var DataItem = iContentGroupServ.Create();
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.FontIcon = TxtFontIcon.Text;
            DataItem.Ordering = items + 1;
            DataItem.ImgUrl = ImageSrc;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.CreatedBy = PortalUser.ID;
            iContentGroupServ.Add(DataItem);
            iContentGroupServ.SaveChanges(PortalUser.ID);
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
            var DataItem = iContentGroupServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Description = TxtDesc.Text;
            DataItem.FontIcon = TxtFontIcon.Text;
            if (!ImageSrc.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = ImageSrc;
            }
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.ModifiedBy = PortalUser.ID;
            DataItem.LastModifiedDateTime = DateTime.Now;
            iContentGroupServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void BtnRemoveImg_Click(object sender, EventArgs e)
        {
            var DataItem = iContentGroupServ.Find(y => y.ID == ItemID);
            if (!DataItem.ImgUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = "";
                iContentGroupServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
            }
        }
    }
}