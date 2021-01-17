using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Portal
{
    public partial class cu_Portal : NikUserControl
    {
        public IPortalService iPortalServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        private void BindCombo()
        {
            ddlPortalParent.FillControl(iPortalServ.GetAll(x => true, x => new { x.ID, x.Title }).ToList(), "Title", "ID");
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

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد کنید.");
            }
            if (txtAlias.Text.IsEmpty())
            {
                ErrorMessage.Add("Alias را وارد کنید.");
            }

            if (!txtQouta.Text.IsNumeric())
            {
                ErrorMessage.Add("حداکثر حجم عدد صحیح می باشد.");
            }

            if (ddlDirection.SelectedIndex == 0)
            {
                ErrorMessage.Add("جهت را انتخاب کنید.");
            }
            if (ddlLanguage.SelectedIndex == 0)
            {
                ErrorMessage.Add("زبان پرتال را انتخاب کنید.");
            }

            if (txtFolderAlias.Text.IsEmpty())
            {
                ErrorMessage.Add("نام پوشه را وارد کنید.");
            }
            var selectedFolder = txtFolderAlias.Text.Trim();
            var isAnyFolderWithThisName = iPortalServ.Find(x => selectedFolder == x.AliasFolder && x.ID != ItemID);
            if (null != isAnyFolderWithThisName && 0 == ItemID)
            {
                ErrorMessage.Add("این پوشه وجود دارد، لطفا از نام دیگری استفاده کنید.");
            }

            if (fuFav.HasFile)
            {
                if (!Utilities.Utilities.CheckFileFormat(fuFav.FileName, "png"))
                {
                    ErrorMessage.Add("فرمت FAV آیکن باید png باشد.");
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
            var data = iPortalServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            txtAlias.Text = data.Alias;
            txtDesc.Text = data.Description;
            txtMeta.Text = data.MetaDescription ?? "";
            txtFolderAlias.Text = data.AliasFolder;
            chbHerOwnLogo.Checked = data.ShowOwnLogo;
            if (!data.Favicon.IsEmpty())
            {
                ImgFav.ImageUrl = "~/" + data.Favicon;
            }
            txtQouta.Text = data.MaxVol.ToString();
            ddlDirection.SelectedValue = data.Direction;
            ddlLanguage.SelectedValue = Convert.ToInt32(data.PortalLanguage).ToString();
            if (data.ParentID != null)
            {
                ddlPortalParent.SelectedValue = data.ParentID.ToString();
            }
        }

        private void SaveNewData()
        {
            var fileName = string.Empty;
            var upath = "Fav";
            if (fuFav.HasFile)
            {
                var isok = Utilities.Utilities.UploadFile(fuFav.PostedFile, upath, ref fileName, PortalUser.PortalFolderPath);
                if (!isok)
                {
                    Notification.SetErrorMessage("fav آیکن آپلود نشد.");
                    return;
                }
            }
            var portal = iPortalServ.Create();
            portal.Title = txtTitle.Text.Trim();
            portal.ParentID = ddlPortalParent.SelectedIndex > 0 ? ddlPortalParent.SelectedValue.ToInt32() : (int?)null;
            portal.Description = txtDesc.Text.Trim();
            portal.Alias = txtAlias.Text.Trim();
            portal.Direction = ddlDirection.SelectedValue;
            portal.PortalLanguage = (PortalLanguage)Convert.ToInt32(ddlLanguage.SelectedValue);
            portal.MaxVol = txtQouta.Text.ToInt32();
            portal.ShowOwnLogo = chbHerOwnLogo.Checked;

            portal.AliasFolder = txtFolderAlias.Text.Trim();
            portal.MetaDescription = txtMeta.Text.Trim();
            portal.Favicon = fileName;
            iPortalServ.Add(portal);
            uow.SaveChanges(PortalUser.ID);
            AddSetting(portal.ID);
            Container.gotoList();
        }

        public void AddSetting(int PortalID)
        {
            if (PortalID != 1)
            {
                var Settings = iNikSettingServ.GetAll(t => t.PortalID == 1);
                var SettingNamesP = iNikSettingServ.GetAll(t => t.PortalID == PortalID, t => t.SettingName);
                foreach (var item in Settings)
                {
                    if (!SettingNamesP.Contains(item.SettingName))
                    {
                        var setting = iNikSettingServ.Create();
                        setting.PortalID = PortalID;
                        setting.SettingName = item.SettingName;
                        setting.SettingValue = item.SettingValue;
                        setting.SettingLabel = item.SettingLabel;
                        setting.MinAllowed = item.MinAllowed;
                        setting.MaxAllowed = item.MaxAllowed;
                        setting.FieldType = item.FieldType;
                        setting.UseEditor = item.UseEditor;

                        setting.SettingModule = item.SettingModule;
                        iNikSettingServ.Add(setting);
                    }
                }
                iNikSettingServ.SaveChanges();
            }
        }

        private void UpdateData()
        {
            var fileName = string.Empty;
            var upath = "Fav";
            if (fuFav.HasFile)
            {
                var isok = Utilities.Utilities.UploadFile(fuFav.PostedFile, upath, ref fileName, PortalUser.PortalFolderPath);
                if (!isok)
                {
                    Notification.SetErrorMessage("fav آیکن آپلود نشد.");
                    return;
                }
            }
            var portal = iPortalServ.Find(y => y.ID == ItemID);
            portal.Title = txtTitle.Text.Trim();
            portal.ParentID = ddlPortalParent.SelectedIndex > 0 ? ddlPortalParent.SelectedValue.ToInt32() : (int?)null;
            portal.Description = txtDesc.Text.Trim();
            portal.Alias = txtAlias.Text.Trim();
            portal.Direction = ddlDirection.SelectedValue;
            portal.PortalLanguage = (PortalLanguage)Convert.ToInt32(ddlLanguage.SelectedValue);
            portal.MaxVol = txtQouta.Text.ToInt32();
            portal.ShowOwnLogo = chbHerOwnLogo.Checked;

            portal.AliasFolder = txtFolderAlias.Text.Trim();
            portal.MetaDescription = txtMeta.Text.Trim();
            if (!fileName.IsEmpty())
            {
                Utilities.Utilities.RemoveItemFile(portal.Favicon);
                portal.Favicon = fileName;
            }
            uow.SaveChanges(PortalUser.ID);
            AddSetting(portal.ID);
            Container.gotoList();
        }

        private string GetCleanDomain(string domain)
        {
            if (domain.StartsWith("http://"))
            {
                domain = domain.Substring(0, "http://".Length);
            }
            if (domain.StartsWith("https://"))
            {
                domain = domain.Substring(0, "https://".Length);
            }
            if (domain.StartsWith("www."))
            {
                domain = domain.Substring(0, "www.".Length);
            }
            if (domain.EndsWith("/"))
            {
                domain = domain.Replace("/", string.Empty);
            }
            return domain;
        }
    }
}