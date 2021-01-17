using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class cu_GeneralMenuItem : NikUserControl
    {
        public IGeneralMenuItemService iGeneralMenuItemServ { get; set; }
        const string Allow_ImageTypes = "jpeg,png,gif";
        public int? parentID;
        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            Container.GotoList += GoToList;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindCombo();

            GetEditingFunction = GetEditData;
            SaveNewFunction = SaveNewData;
            SaveEditFunction = UpdateData;
            btnSave.Click += SaveClick;
            FormValidation = Validate;
        }

        private void BindCombo()
        {
            int _ID = 0;
            var query = iGeneralMenuItemServ.ExpressionMaker();
            query.Add(x => x.PortalID == PortalUser.PortalID);
            if (Request.QueryString["MainID"] != null)
            {
                _ID = int.Parse(Request.QueryString["MainID"].ToString());
                query.Add(x => x.GeneralMenuID == _ID);
            }
            int _pID = 0;
            if (Request.QueryString["ParentID"] != null)
            {
                _pID = int.Parse(Request.QueryString["ParentID"].ToString());
                parentID = _pID;
            }

            if (ItemID > 0)
            {
                query.Add(x => x.ID != ItemID);
            }

            ddlParentMenu.FillControl(iGeneralMenuItemServ.GetAll(query, x => new { x.Title, x.ID }).ToList(), "Title", "ID", true, true);

            if (parentID != null && parentID > 0)
            {
                ddlParentMenu.SelectedValue = parentID.ToString();
            }
            ddlParentMenu.Enabled = false;
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }

            if (fuIcon.PostedFile.ContentLength > 0)
            {
                string FileExtension = fuIcon.PostedFile.ContentType.Split('/')[1];
                int FileSize = fuIcon.PostedFile.ContentLength;
                int MaxVal = iNikSettingServ.GetSettingValue("ImageMaxVol", NikSettingType.SystemSetting, PortalUser.PortalID).ToInt32();

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
            ddlParentMenu.Enabled = true;
            var data = iGeneralMenuItemServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            if (data.ParentID != null)
            {
                ddlParentMenu.SelectedValue = data.ParentID.ToString();
            }
            TxtLink.Text = data.Link;
            TxtClassName.Text = data.ClassName;
            TxtFontIcon.Text = data.Font;
            MenuImg.ImageUrl = "/" + data.ImgUrl;
            txtDesc.Text = data.Description;
            chbLogin.Checked = data.LoginRequired;
            chbEnbaled.Checked = data.Enabled;
        }

        private void SaveNewData()
        {
            string vpath = "MenuImage";
            string IconUrl = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref IconUrl, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            int items = iGeneralMenuItemServ.Count(x => x.PortalID == PortalUser.PortalID);
            var DataItem = iGeneralMenuItemServ.Create();
            DataItem.PortalID = PortalUser.PortalID;
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Link = TxtLink.Text;
            DataItem.Description = txtDesc.Text;
            DataItem.ImgUrl = IconUrl;
            DataItem.Font = TxtFontIcon.Text;
            DataItem.ClassName = TxtClassName.Text;
            DataItem.GeneralMenuID = int.Parse(Request.QueryString["MainID"].ToString());
            if (Request.QueryString["ParentID"] != null)
            {
                DataItem.ParentID = int.Parse(Request.QueryString["ParentID"].ToString());
            }

            DataItem.Ordering = items + 1;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.LoginRequired = chbLogin.Checked;
            iGeneralMenuItemServ.Add(DataItem);
            iGeneralMenuItemServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            string vpath = "MenuImage";
            string IconUrl = "";
            if (fuIcon.PostedFile.ContentLength > 0)
            {
                var isokImmage = Utilities.Utilities.UploadFile(fuIcon.PostedFile, vpath, ref IconUrl, PortalUser.PortalFolderPath);
                if (!isokImmage)
                {
                    Notification.SetErrorMessage("تصویر آپلود نشد.");
                    return;
                }
            }

            var DataItem = iGeneralMenuItemServ.Find(y => y.ID == ItemID);
            DataItem.Title = txtTitle.Text.Trim();
            DataItem.Link = TxtLink.Text;
            int MPID = ddlParentMenu.SelectedValue.ToInt32();
            if (MPID == 0)
            {
                DataItem.ParentID = null;
            }
            else
            {
                DataItem.ParentID = MPID;
            }

            DataItem.Description = txtDesc.Text;
            if (!IconUrl.IsEmpty())
                DataItem.ImgUrl = IconUrl;
            DataItem.Font = TxtFontIcon.Text;
            DataItem.ClassName = TxtClassName.Text;
            DataItem.Enabled = chbEnbaled.Checked;
            DataItem.LoginRequired = chbLogin.Checked;
            iGeneralMenuItemServ.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        protected void BtnRemoveImg_Click(object sender, EventArgs e)
        {
            var DataItem = iGeneralMenuItemServ.Find(y => y.ID == ItemID);
            if (!DataItem.ImgUrl.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(DataItem.ImgUrl);
                DataItem.ImgUrl = "";
                iGeneralMenuItemServ.SaveChanges();
                Notification.SetSuccessMessage("تصویر حذف شد.");
            }
        }

        protected void GoToList()
        {
            if (Request.QueryString["MainID"] != null && Request.QueryString["ParentID"] != null)
            {
                RedirectTo("~/panel/rd_GeneralMenuItem/default?MainID=" + Request.QueryString["MainID"].ToString() + "&ParentID=" + Request.QueryString["ParentID"].ToString());
            }
            else if (Request.QueryString["MainID"] != null && Request.QueryString["ParentID"] == null)
            {
                RedirectTo("~/panel/rd_GeneralMenuItem/default?MainID=" + Request.QueryString["MainID"].ToString());
            }
            else if (Request.QueryString["MainID"] == null && Request.QueryString["ParentID"] == null)
            {
                var l = iGeneralMenuItemServ.Find(t => t.ID == ItemID);
                if (l.ParentID != null)
                {
                    RedirectTo("~/panel/rd_GeneralMenuItem/default?MainID=" + l.GeneralMenuID + "&ParentID=" + l.ParentID);
                }
                else
                {
                    RedirectTo("~/panel/rd_GeneralMenuItem/default?MainID=" + l.GeneralMenuID);
                }
            }
            return;
        }

    }
}