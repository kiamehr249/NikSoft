using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_VisualLinkItem : NikUserControl
    {
        public IVisualLinkService iVisualLinkServ { get; set; }
        public IVisualLinkItemService iVisualLinkItemServ { get; set; }
        const string Allow_ImageTypes = "jpeg,png,gif";
        protected int parentID;

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            Container.GotoList += GoToList;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ItemID > 0)
            {
                parentID = iVisualLinkItemServ.Find(x => x.ID == ItemID).VisualLinkID;
            }
            else
            {
                parentID = Request.QueryString["ParentID"].ToInt32();
            }
            
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
            var data = iVisualLinkServ.GetAll(t => t.PortalID == PortalUser.PortalID && t.Enabled).ToList();
            ddlCategory.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
            ddlCategory.SelectedValue = parentID.ToString();
            ddlCategory.Enabled = false;
        }

        protected void GoToEditItem()
        {

        }

        protected void GoToList()
        {
            RedirectTo("~/Panel/rd_visuallinkitem/" + parentID);
        }

        private bool Validate()
        {
            var groupid = ddlCategory.SelectedValue.ToInt32();
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            if(groupid < 1)
            {
                ErrorMessage.Add("گروه پیوند را وارد کنید.");
            }

            if (FuImg1.PostedFile.ContentLength > 0)
            {
                string FileExtension = FuImg1.PostedFile.ContentType.Split('/')[1];
                int FileSize = FuImg1.PostedFile.ContentLength;
                if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                {
                    ErrorMessage.Add("فرمت آیکن ارسالی اشتباه است.(jpg, png, gif)");
                }

                int MaxValIcon = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();

                if ((MaxValIcon * 1024) < FileSize)
                {
                    ErrorMessage.Add("حجم آیکن باید کمتر از " + MaxValIcon + " KB باشد.");
                }
            }

            if (FuImg2.PostedFile.ContentLength > 0)
            {
                string FileExtension = FuImg2.PostedFile.ContentType.Split('/')[1];
                if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                {
                    ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                }

                int File2Size = FuImg2.PostedFile.ContentLength;
                int MaxValImage = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                if ((MaxValImage * 1024) < File2Size)
                {
                    ErrorMessage.Add("حجم تصویر باید کمتر از " + MaxValImage + " KB باشد.");
                }
            }

            if (FuImg3.PostedFile.ContentLength > 0)
            {
                string FileExtension = FuImg3.PostedFile.ContentType.Split('/')[1];
                if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                {
                    ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                }

                int FileSize = FuImg2.PostedFile.ContentLength;
                int MaxValImage = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                if ((MaxValImage * 1024) < FileSize)
                {
                    ErrorMessage.Add("حجم تصویر باید کمتر از " + MaxValImage + " KB باشد.");
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
            var data = iVisualLinkItemServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }

            ddlCategory.Enabled = true;

            txtTitle.Text = data.Title;
            TxtDesc.Text = data.Descroption;
            ddlCategory.SelectedValue = data.VisualLinkID.ToString();
            TxtLink1.Text = data.Link1;
            TxtLink2.Text = data.Link2;
            TxtBtnText1.Text = data.Btn1Text;
            TxtBtnText2.Text = data.Btn2Text;
            Img1Prev.ImageUrl = "/" + data.Img1;
            Img2Prev.ImageUrl = "/" + data.Img2;
            Img3Prev.ImageUrl = "/" + data.Img3;
            Img4Prev.ImageUrl = "/" + data.Img4;
            chbEnbaled.Checked = data.Enabled;
        }

        private void SaveNewData()
        {
            string vpath = "VisualLinks";
            string img1src = "";
            if (FuImg1.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg1.PostedFile, vpath, ref img1src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }
            string img2src = "";
            if (FuImg2.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg2.PostedFile, vpath, ref img2src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string img3src = "";
            if (FuImg3.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg3.PostedFile, vpath, ref img3src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string img4src = "";
            if (FuImg4.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg4.PostedFile, vpath, ref img4src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            int items = iVisualLinkItemServ.Count(x => x.VisualLinkID == parentID);
            var DataItem = iVisualLinkItemServ.Create();
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Descroption = TxtDesc.Text;
            DataItem.Link1 = TxtLink1.Text;
            DataItem.Link2 = TxtLink2.Text;
            DataItem.Btn1Text = TxtBtnText1.Text;
            DataItem.Btn2Text = TxtBtnText2.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.Img1 = img1src;
            DataItem.Img2 = img2src;
            DataItem.Img3 = img3src;
            DataItem.Img4 = img4src;
            DataItem.VisualLinkID = parentID;
            DataItem.Ordering = items + 1;
            DataItem.Enabled = chbEnbaled.Checked;
            iVisualLinkItemServ.Add(DataItem);
            iVisualLinkItemServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            if (!Validate())
                return;

            string vpath = "VisualLinks";
            string img1src = "";
            if (FuImg1.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg1.PostedFile, vpath, ref img1src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }
            string img2src = "";
            if (FuImg2.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg2.PostedFile, vpath, ref img2src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string img3src = "";
            if (FuImg3.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg3.PostedFile, vpath, ref img3src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            string img4src = "";
            if (FuImg4.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuImg4.PostedFile, vpath, ref img4src, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            var DataItem = iVisualLinkItemServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.VisualLinkID = ddlCategory.SelectedValue.ToInt32();
            DataItem.Descroption = TxtDesc.Text;
            DataItem.Link1 = TxtLink1.Text;
            DataItem.Link1 = TxtLink1.Text;
            DataItem.Link2 = TxtLink2.Text;
            DataItem.Btn1Text = TxtBtnText1.Text;
            DataItem.Btn2Text = TxtBtnText2.Text;
            DataItem.Enabled = chbEnbaled.Checked;

            if (!img1src.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.Img1);
                DataItem.Img1 = img1src;
            }
            if (!img2src.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.Img2);
                DataItem.Img2 = img2src;
            }
            if (!img3src.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.Img3);
                DataItem.Img3 = img3src;
            }
            if (!img4src.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.Img4);
                DataItem.Img4 = img4src;
            }
            DataItem.Enabled = chbEnbaled.Checked;
            iVisualLinkItemServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void BtnRemoveImg1_Click(object sender, EventArgs e)
        {
            var DataItem = iVisualLinkItemServ.Find(y => y.ID == ItemID);
            if (!DataItem.Img1.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.Img1))
                {
                    Notification.SetErrorMessage("تصویر حذف نشد.");
                    return;
                }
                DataItem.Img1 = "";
                iVisualLinkItemServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
                Img1Prev.ImageUrl = "";
            }
        }

        protected void BtnRemoveImg2_Click(object sender, EventArgs e)
        {
            var DataItem = iVisualLinkItemServ.Find(y => y.ID == ItemID);
            if (!DataItem.Img2.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.Img2))
                {
                    Notification.SetErrorMessage("تصویر حذف نشد.");
                    return;
                }
                DataItem.Img2 = "";
                iVisualLinkItemServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
                Img2Prev.ImageUrl = "";
            }
        }

        protected void BtnRemoveImg3_Click(object sender, EventArgs e)
        {
            var DataItem = iVisualLinkItemServ.Find(y => y.ID == ItemID);
            if (!DataItem.Img3.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.Img3))
                {
                    Notification.SetErrorMessage("تصویر حذف نشد.");
                    return;
                }
                DataItem.Img3 = "";
                iVisualLinkItemServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
                Img3Prev.ImageUrl = "";
            }
        }

        protected void BtnRemoveImg4_Click(object sender, EventArgs e)
        {
            var DataItem = iVisualLinkItemServ.Find(y => y.ID == ItemID);
            if (!DataItem.Img4.IsEmpty())
            {
                if (!Utilities.Utilities.RemoveItemFile(DataItem.Img4))
                {
                    Notification.SetErrorMessage("تصویر حذف نشد.");
                    return;
                }
                DataItem.Img4 = "";
                iVisualLinkItemServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
                Img4Prev.ImageUrl = "";
            }
        }
    }
}