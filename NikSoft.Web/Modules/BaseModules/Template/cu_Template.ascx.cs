using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.UILayer.WebControls;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Template
{
    public partial class cu_Template : NikUserControl
    {
        public ITemplateService iPageTemplateServ { get; set; }
        public INikModuleService iNikModulesServ { get; set; }
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
            ddlTemplateType.FillControl(iPageTemplateServ.GetTemplates(), "Text", "Value", true, true, "انتخاب کنید", "0");
            ddlUI.FillControl(GetUIs(), "Text", "Value");
            ddlModule.FillControl(iNikModulesServ.GetAll(t => t.LoginRequired == false, t => new { t.ModuleKey, t.Title }).ToList(), "Title", "ModuleKey", defaultValue: "");
        }

        private List<ListItem> GetUIs()
        {
            var list = new List<ListItem>();
            var themes = iThemeServ.GetAll(t => t.PortalID == PortalUser.PortalID);
            foreach (var theme in themes)
            {
                var files = new DirectoryInfo(Server.MapPath("~/" + theme.ThemePath)).GetFiles("*.ascx", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(file.FullName)) as NikSkinTemplate;
                    if (cntrl != null)
                    {
                        var skinTitle = cntrl.Controls.GetAllChilds<SkinTitle>().FirstOrDefault();
                        if (skinTitle == null)
                        {
                            continue;
                        }
                        list.Add(new ListItem(string.Format("{0} - {1}", theme.Title, skinTitle.Title), Utilities.Utilities.PhysicalToVirtual(file.FullName).Replace("~/", string.Empty)));
                    }
                }
            }
            return list.OrderBy(t => t.Text).ToList();
        }

        private bool Validate()
        {
            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد نمایید");
            }
            if (ddlTemplateType.SelectedIndex == 0)
            {
                ErrorMessage.Add("نوع صفحه را انتخاب نمایید");
            }
            if (ddlUI.SelectedIndex == 0)
            {
                ErrorMessage.Add("چیدمان صفحه را انتخاب نمایید");
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
            var data = iPageTemplateServ.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            ddlTemplateType.SelectedValue = Convert.ToInt32(data.Type).ToString();
            txtDesc.Text = data.Description;
            ddlUI.SelectedValue = data.TemplateName;
            if (!data.ModuleKey.IsEmpty())
            {
                ddlModule.SelectedValue = data.ModuleKey;
            }
            txtModuleParameter.Text = data.ModuleParameter;
        }

        private void SaveNewData()
        {
            var tType = (TemplateType)(Convert.ToInt32(ddlTemplateType.SelectedValue));
            var modulekey = ddlModule.SelectedIndex > 0 ? ddlModule.SelectedValue : "";

            var data = iPageTemplateServ.Create();
            data.Title = txtTitle.Text.Trim();
            data.Description = txtDesc.Text.Trim();
            data.PortalID = PortalUser.PortalID;
            data.Type = tType;
            data.TemplateName = ddlUI.SelectedValue;
            data.Direction = "ltr";
            if (tType == TemplateType.InnerPage)
            {
                data.ModuleKey = modulekey;
            }
            else
            {
                data.ModuleKey = string.Empty;
            }
            data.ModuleParameter = txtModuleParameter.Text.Trim();
            data.Culture = null;
            data.UserMustLogin = false;
            data.IsSelected = false;
            iPageTemplateServ.Add(data);
            uow.SaveChanges(PortalUser.ID);
            Cache.Remove("Templates");
            Container.gotoList();
        }

        private void UpdateData()
        {
            var tType = (TemplateType)(Convert.ToInt32(ddlTemplateType.SelectedValue));
            var modulekey = ddlModule.SelectedIndex > 0 ? ddlModule.SelectedValue : "";
            if (tType == TemplateType.InnerPage)
            {
                if (iPageTemplateServ.Any(t => t.Type == tType && t.ModuleKey == modulekey && t.ID != ItemID && t.PortalID == PortalUser.PortalID))
                {
                    Notification.SetErrorMessage("صفحه ای با تنظیمات مورد نظر وجود دارد");
                    return;
                }
            }
            var data = iPageTemplateServ.Find(y => y.ID == ItemID);
            data.Title = txtTitle.Text.Trim();
            data.Description = txtDesc.Text.Trim();
            data.Type = (TemplateType)(Convert.ToInt32(ddlTemplateType.SelectedValue));
            if (tType == TemplateType.InnerPage)
            {
                data.ModuleKey = modulekey;
            }
            else
            {
                data.ModuleKey = string.Empty;
            }
            data.ModuleParameter = txtModuleParameter.Text.Trim();
            data.TemplateName = ddlUI.SelectedValue;
            uow.SaveChanges(PortalUser.ID);
            Cache.Remove("Templates");
            Container.gotoList();
        }
    }
}