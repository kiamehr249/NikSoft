using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.ModuleEdit
{
    public partial class ShowEditableModules : WidgetUIContainer
    {

        public INikModuleService iModuleServ { get; set; }
        public INikModuleDefinitionService iNikModulesDefServ { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
            LoadData();
        }

        private void BindCombo()
        {
            var modulesList = iNikModulesDefServ.GetAll(x => true, t => new { t.ID, ModuleName = (t.Description ?? "") + "->" + t.Title + ":" + t.Version }).ToList();
            ddlModules.FillControl(modulesList, "ModuleName", "ID", false, false);
        }

        private void LoadData()
        {
            var selectedModuleID = ddlModules.SelectedValue.ToInt32();
            var modules = iModuleServ.GetAll(t => t.ModuleDefinitionID == selectedModuleID && !t.LoginRequired).OrderBy(t => t.Title);
            GV1.DataSource = modules;
            GV1.DataBind();
        }

        protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}