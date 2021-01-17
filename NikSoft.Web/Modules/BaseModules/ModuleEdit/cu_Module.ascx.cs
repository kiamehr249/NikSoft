using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.ModuleEdit
{
    public partial class cu_Module : NikUserControl
    {
        public INikModuleService iNikModuleServ { get; set; }
        public INikModuleDefinitionService iNikModuleDefinitionServ { get; set; }
        protected string SelectData1 = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        private void BindCombo()
        {
            var data = iNikModuleDefinitionServ.GetAll(t => true).ToList();
            if (ItemID != 0)
            {
                data = data.Where(t => t.ID != ItemID).ToList();
            }
            ddlCategory.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
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
           var editid = ddlEditable.GetDropDownValue(Request.Form);
            var catid = ddlCategory.SelectedValue.ToInt32();
            SelectData1 = "GetContentCategory(" + editid + "," + catid + ");\n";

            if (txtTitle.Text.IsEmpty())
            {
                ErrorMessage.Add("عنوان را وارد نمایید.");
            }

            if (TxtModuleKey.Text.IsEmpty())
            {
                ErrorMessage.Add("کلید ماژول را وارد نمایید.");
            }
            else
            {
                var findit = iNikModuleServ.Find(x => x.ModuleKey == TxtModuleKey.Text);
                if (findit != null)
                    ErrorMessage.Add("کلید ماژول تکراری می باشد.");
            }

            if (catid < 1)
            {
                ErrorMessage.Add("دسته بندی را وارد نمایید.");
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
            var moduleItem = iNikModuleServ.Find(t => t.ID == ItemID);
            if (moduleItem == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = moduleItem.Title;
            TxtModuleKey.Text = moduleItem.ModuleKey;
            ddlCategory.SelectedValue = moduleItem.ModuleDefinitionID.ToString();
            ddlCategory.Enabled = false;
            ddlEditable.Enabled = false;
            //SelectData1 = "GetModulesByCategory(" + moduleItem.ModuleDefinitionID + "," + moduleItem.ID + ");\n";
        }

        private void SaveNewData()
        {
            var baseid = ddlEditable.GetDropDownValue(Request.Form);
            int catid = ddlCategory.SelectedValue.ToInt32();

            var baseModule = iNikModuleServ.Find(x => x.ID == baseid);
            if (baseModule == null)
            {
                Notification.SetErrorMessage("ماژول پایه چک شود.");
                return;
            }

            var modulePath = SaveModule(baseModule, TxtModuleKey.Text);
            if(modulePath.IsEmpty())
            {
                Notification.SetErrorMessage("ذخیره انجام نشد.");
                return;
            }

            var newModule = iNikModuleServ.Create();
            newModule.Title = txtTitle.Text;
            newModule.ModuleKey = TxtModuleKey.Text;
            newModule.CreatedBy = PortalUser.ID;
            newModule.ModuleFile = modulePath;
            newModule.ModuleKey = TxtModuleKey.Text;
            newModule.ModuleDefinitionID = catid;
            newModule.LoginRequired = baseModule.LoginRequired;
            newModule.IsXMLBase = false;
            newModule.ShowAsModule = true;
            newModule.SecondTitle = baseModule.SecondTitle;
            newModule.IsGeneralModule = false;
            newModule.PortalID = PortalUser.PortalID;
            newModule.Editable = false;
            newModule.IsExternal = true;
            iNikModuleServ.Add(newModule);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private string SaveModule(NikModule editableModule, string newKey)
        {
            var basePath = Server.MapPath("~/" + editableModule.ModuleFile);
            var newPath = "files/ShareModules/";
            if (!Utilities.Utilities.EnsureDirectory(Server.MapPath("~/" + newPath)))
            {
                Notification.SetErrorMessage("دسترسی به این ماژول امکان ندارد.");
                return string.Empty;
            }

            WidgetUIContainer cntrl = null;
            try
            {
                cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(basePath)) as WidgetUIContainer;
            }
            catch
            {
                Notification.SetErrorMessage("این ماژول قابل لود شدن نیست.");
            }

            if (cntrl == null)
            {
                Notification.SetErrorMessage("متن اشتباه است.");
                return string.Empty;
            }

            string content = string.Empty;

            if (File.Exists(basePath))
            {
                using (StreamReader reader = new StreamReader(basePath, Encoding.UTF8))
                {
                    content = reader.ReadToEnd();
                }
            }
            else
            {
                Notification.SetErrorMessage("مسیر ماژول پایه اشتباه است.");
                return string.Empty;
            }



            string newFullPath = newPath + newKey + ".ascx";
            using (StreamWriter outfile = new StreamWriter(Server.MapPath("~/" + newFullPath), false, Encoding.UTF8))
            {
                outfile.Write(content);
            }
            return newFullPath;
        }

        private void UpdateData()
        {
            if (txtTitle.Text.IsEmpty())
            {
                Notification.SetErrorMessage("عنوان را وارد نمایید");
                return;
            }

            if(TxtModuleKey.Text.IsEmpty())
            {
                Notification.SetErrorMessage("کلید ماژول را وارد نمایید");
            }

            var moduleItem = iNikModuleServ.Find(y => y.ID == ItemID);
            moduleItem.Title = txtTitle.Text;
            moduleItem.ModuleKey = TxtModuleKey.Text;
            moduleItem.ModifiedBy = PortalUser.ID;
            moduleItem.LastModifiedDateTime = DateTime.Now;
            iNikModuleServ.SaveChanges();
            Container.gotoList();
        }
    }
}