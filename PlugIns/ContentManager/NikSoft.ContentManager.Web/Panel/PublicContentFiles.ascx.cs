using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class PublicContentFiles : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentFileService iContentFileServ { get; set; }
        protected int ContentID;
        const string Allow_ImageTypes = "jpeg,png,gif";
        const string Allow_VideoType = "mp4";
        const string Allow_SoundType = "mpeg";
        protected FileType SelectedType;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }


        protected override void BoundData()
        {
            if (!int.TryParse(ModuleParameters, out ContentID))
            {
                Notification.SetErrorMessage("مسیر اشتباه است");
                IssueContentNotFound();
                Response.StatusCode = 404;
                return;
            }

            var ContentItem = iPublicContentServ.Find(x => x.ID == ContentID && x.PortalID == PortalUser.PortalID);
            if (ContentItem == null)
            {
                Notification.SetErrorMessage("مسیر اشتباه است");
                IssueContentNotFound();
                Response.StatusCode = 404;
                return;
            }

            if (ContentItem.ContentCategory != null)
            {
                if (ContentItem.ContentCategory.IsStore)
                {
                    HypBackToList.NavigateUrl = "/panel/rd_StoreContent";
                }
                else
                {
                    HypBackToList.NavigateUrl = "/panel/rd_PublicContent";
                }
            }
            else
            {
                HypBackToList.NavigateUrl = "/panel/rd_PublicContent";
            }


            var query = iContentFileServ.ExpressionMaker();
            query.Add(t => t.PublicContentID == ContentID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }

            int ItemTypeID = ddlSearchType.SelectedValue.ToInt32();
            if (ItemTypeID > 0)
            {
                query.Add(x => x.ItemType == (FileType)ItemTypeID);
            }

            base.FillManageFrom(iContentFileServ, query);
        }

        protected void breset_Click(object sender, EventArgs e)
        {
            this.ClearForm();
            BoundData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BoundData();
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
                    SetItemEnabled(id);
                    break;
                case "EditMe":
                    EditItem(id);
                    break;
                case "DeleteMe":
                    DeleteItem(id);
                    break;
                case "iscover":
                    SetCoverItem(id);
                    break;
            }
            BoundData();
        }

        protected void SetItemEnabled(int Id)
        {
            var NewItem = iContentFileServ.Find(x => x.ID == Id);
            NewItem.Enabled = !NewItem.Enabled;
            iContentFileServ.SaveChanges();
        }

        protected void SetCoverItem(int Id)
        {
            var NewItem = iContentFileServ.Find(x => x.ID == Id);
            var OldCover = iContentFileServ.GetAll(x => x.PublicContentID == NewItem.PublicContentID && x.IsCover).FirstOrDefault();
            if (OldCover != null)
            {
                OldCover.IsCover = false;
            }

            NewItem.IsCover = true;
            iContentFileServ.SaveChanges();
        }

        public void RearrangePriority(string action, int ItemID)
        {
            var item = iContentFileServ.Find(t => t.ID == ItemID);
            if (action == "down")
            {
                var t = iContentFileServ.GetAll(x => x.Ordering < item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Max(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentFileServ.SaveChanges();
                }
            }
            else
            {
                var t = iContentFileServ.GetAll(x => x.Ordering > item.Ordering).ToList();

                if (t.Count > 0)
                {
                    int newOrder = t.Min(x => x.Ordering);
                    var item2 = t.Find(x => x.Ordering == newOrder);
                    item2.Ordering = item.Ordering;
                    item.Ordering = newOrder;
                    iContentFileServ.SaveChanges();
                }
            }
        }

        protected int GetMaxVolume(FileType FileType)
        {
            int MaxVal = 0;
            switch (FileType)
            {
                case FileType.Image:
                    MaxVal = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                    return MaxVal * 1024;
                case FileType.Video:
                    MaxVal = iNikSettingServ.GetSettingValue("VideoMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                    return MaxVal * 1024 * 1024;
                case FileType.Sound:
                    MaxVal = iNikSettingServ.GetSettingValue("SoundMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                    return MaxVal * 1024 * 1024;
                case FileType.Other:
                    MaxVal = iNikSettingServ.GetSettingValue("FileMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();
                    return MaxVal * 1024 * 1024;
                default:
                    return MaxVal;
            }
        }

        protected string GetFileType(FileType itemType)
        {
            switch (itemType)
            {
                case FileType.Image:
                    return "تصویر";
                case FileType.Video:
                    return "ویدیو";
                case FileType.Sound:
                    return "صدا";
                case FileType.Other:
                    return "فایل";
                default:
                    return string.Empty;
            }
        }

        protected void ResetForm()
        {
            TxtItemTitle.Text = "";
            ddlItemType.SelectedValue = "0";
            TxtDesc.Text = "";
            chbIsCover.Checked = false;
        }

        protected void EditItem(int EditItemID)
        {
            var DataItem = iContentFileServ.Find(x => x.ID == EditItemID);
            TxtItemTitle.Text = DataItem.Title;
            ddlItemType.SelectedValue = Convert.ToInt32(DataItem.ItemType).ToString();
            chbEnbaled.Checked = DataItem.Enabled;
            chbIsCover.Checked = DataItem.IsCover;
            BtnSave.Visible = false;
            BtnUpdate.Visible = true;
            HypBackToList.Visible = false;
            HypCancel.NavigateUrl = "/" + Level + "/PublicContentFiles/" + ContentID;
            HypCancel.Visible = true;
            hfEditItemID.Value = EditItemID.ToString();
            if (DataItem.ItemType == FileType.Image)
            {
                ImgFile.ImageUrl = "/" + DataItem.FileUrl;
                fileImg.Visible = true;
            }
            else if (DataItem.ItemType == FileType.Video)
            {
                ImgFile.ImageUrl = "/" + DataItem.CoverImage;
                fileImg.Visible = true;
            }
            else
            {
                fileImg.Visible = false;
            }
        }

        protected void DeleteItem(int DeleteItemID)
        {
            var DeleteItem = iContentFileServ.Find(x => x.ID == DeleteItemID);
            if (!DeleteItem.FileUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DeleteItem.FileUrl);
                if (DeleteItem.ItemType == FileType.Video && !DeleteItem.CoverImage.IsEmpty())
                {
                    Utilities.Utilities.RemoveItemFile(DeleteItem.CoverImage);
                }
            }
            iContentFileServ.Remove(DeleteItem);
            iContentFileServ.SaveChanges();
        }

        protected bool ValidatSave()
        {
            if (TxtItemTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد نمایید.");
            }
            int TypeID = ddlItemType.SelectedValue.ToInt32();
            if (TypeID < 1 || TypeID > 4)
            {
                ErrorMessage.Add("نوع را وارد انتخاب کنید.");
            }
            if (hfEditItemID.Value == "0")
            {
                if (FuFile.PostedFile.ContentLength < 1)
                {
                    ErrorMessage.Add("فایل مورد نظر را انتخاب کنید.");
                }
            }

            string FileExtension = FuFile.PostedFile.ContentType.Split('/')[1];
            int FileSize = FuFile.PostedFile.ContentLength;

            switch (TypeID)
            {
                case 1:
                    if (!Allow_ImageTypes.Split(',').Contains(FileExtension))
                    {
                        ErrorMessage.Add("فرمت تصویر ارسالی اشتباه است.(jpg, png, gif)");
                    }
                    SelectedType = FileType.Image;
                    if (GetMaxVolume(SelectedType) < FileSize)
                    {
                        ErrorMessage.Add("حجم تصویر باید کمتر از " + (GetMaxVolume(SelectedType) / 1024) + " KB باشد.");
                    }
                    break;
                case 2:
                    if (chbIsCover.Checked)
                    {
                        ErrorMessage.Add("ویدیو را نمی تواند به عنوان کاور انتخاب کرد.");
                    }
                    if (Allow_VideoType != FileExtension)
                    {
                        ErrorMessage.Add("فرمت ویدئو باید mp4 باشد.");
                    }
                    SelectedType = FileType.Video;
                    if (GetMaxVolume(SelectedType) < FileSize)
                    {
                        ErrorMessage.Add("حجم ویدئو باید کمتر از " + ((GetMaxVolume(SelectedType) / 1024) / 1024) + " MB باشد.");
                    }
                    break;
                case 3:
                    if (chbIsCover.Checked)
                    {
                        ErrorMessage.Add("صوت را نمی تواند به عنوان کاور انتخاب کرد.");
                    }
                    if (Allow_SoundType != FileExtension)
                    {
                        ErrorMessage.Add("فرمت صوت باید mp3 باشد.");
                        ErrorMessage.Add(FileExtension);
                    }
                    SelectedType = FileType.Sound;
                    if (GetMaxVolume(SelectedType) < FileSize)
                    {
                        ErrorMessage.Add("حجم صوت باید کمتر از " + ((GetMaxVolume(SelectedType) / 1024) / 1024) + " MB باشد.");
                    }
                    break;
                case 4:
                    if (chbIsCover.Checked)
                    {
                        ErrorMessage.Add("فایل را نمی تواند به عنوان کاور انتخاب کرد.");
                    }
                    SelectedType = FileType.Other;
                    if (GetMaxVolume(SelectedType) < FileSize)
                    {
                        ErrorMessage.Add("حجم فایل باید کمتر از " + ((GetMaxVolume(SelectedType) / 1024) / 1024) + " MB باشد.");
                    }
                    break;
            }

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            Notification.SetErrorMessage(ErrorMessage);
            if (ErrorMessage.Count > 0)
            {
                return false;
            }
            return true;
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidatSave())
            {
                return;
            }

            string vpath = "ContentFiles";
            string FileUrl = "";
            if (FuFile.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuFile.PostedFile, vpath, ref FileUrl, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("فایل آپلود نشد.");
                    return;
                }
            }

            var DataItem = iContentFileServ.Create();

            if (SelectedType == FileType.Video && !FileUrl.IsEmpty())
            {
                var fullpathToVideo = Server.MapPath("~/" + FileUrl);
                var basePath = Server.MapPath("~/files/" + PortalUser.PortalFolderPath + "/ContentFiles/");
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                ffMpeg.GetVideoThumbnail(fullpathToVideo, basePath + Path.GetFileNameWithoutExtension(fullpathToVideo) + "_cover.jpg");
                DataItem.CoverImage = "files/" + PortalUser.PortalFolderPath + "/ContentFiles/" + Path.GetFileNameWithoutExtension(fullpathToVideo) + "_cover.jpg";
            }


            var Albums = iContentFileServ.GetAll(x => x.PublicContentID == ContentID);
            int ItemCount = Albums.Count;
            var IsOldCover = Albums.Where(x => x.IsCover).FirstOrDefault();
            DataItem.Title = TxtItemTitle.Text;
            DataItem.ItemType = (FileType)ddlItemType.SelectedValue.ToInt32();
            DataItem.FileUrl = FileUrl;

            if (chbIsCover.Checked)
            {
                if (IsOldCover != null)
                {
                    IsOldCover.IsCover = false;
                }
                DataItem.IsCover = true;
            }
            DataItem.IsCover = chbIsCover.Checked;
            DataItem.Description = TxtDesc.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.PublicContentID = ContentID;
            DataItem.Ordering = ItemCount + 1;
            iContentFileServ.Add(DataItem);
            iContentFileServ.SaveChanges();
            BoundData();
            ResetForm();
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidatSave())
            {
                return;
            }

            string vpath = "ContentFiles";
            string FileUrl = "";
            if (FuFile.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(FuFile.PostedFile, vpath, ref FileUrl, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("فایل آپلود نشد.");
                    return;
                }
            }



            var IsOldCover = iContentFileServ.GetAll(x => x.PublicContentID == ContentID && x.IsCover).FirstOrDefault();
            int ItemIDEdit = hfEditItemID.Value.ToInt32();
            var DataItem = iContentFileServ.Find(x => x.ID == ItemIDEdit);

            if (SelectedType == FileType.Video && !FileUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.CoverImage);
                var fullpathToVideo = Server.MapPath("~/" + FileUrl);
                var basePath = Server.MapPath("~/files/" + PortalUser.PortalFolderPath + "/");

                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                ffMpeg.GetVideoThumbnail(fullpathToVideo, basePath + Path.GetFileNameWithoutExtension(fullpathToVideo) + "_cover.jpg");
                DataItem.CoverImage = "files/" + PortalUser.PortalFolderPath + "/" + Path.GetFileNameWithoutExtension(fullpathToVideo) + "_cover.jpg";
            }

            DataItem.Title = TxtItemTitle.Text;
            DataItem.ItemType = (FileType)ddlItemType.SelectedValue.ToInt32();
            if (!FileUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.FileUrl);
                DataItem.FileUrl = FileUrl;
            }

            if (chbIsCover.Checked)
            {
                if (IsOldCover != null)
                {
                    IsOldCover.IsCover = false;
                }
                DataItem.IsCover = true;
            }
            DataItem.IsCover = chbIsCover.Checked;
            DataItem.Description = TxtDesc.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.PublicContentID = ContentID;
            iContentFileServ.SaveChanges();
            BoundData();
            ResetForm();
        }

    }
}