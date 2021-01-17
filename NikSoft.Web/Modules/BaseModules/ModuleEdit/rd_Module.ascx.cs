using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.ModuleEdit
{
    public partial class rd_Module : NikUserControl
    {
        public INikModuleService iNikModuleServ { get; set; }
        public INikModuleDefinitionService iNikModuleDefinitionServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
            }
            BoundData();
        }

        private void BindCombo()
        {
            var data = iNikModuleDefinitionServ.GetAll(x => true).ToList();
            ddlCategory.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
        }


        protected override void BoundData()
        {
            var query = iNikModuleServ.ExpressionMaker();
            query.Add(t => t.IsExternal);

            if (PortalUser.ID != 1)
            {
                query.Add(x => x.PortalID == PortalUser.PortalID);
            }

            var catid = ddlCategory.SelectedValue.ToInt32();
            if (catid > 0)
            {
                query.Add(x => x.ModuleDefinitionID == catid);
            }

            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }

            if (!TxtModuleKey.Text.IsEmpty())
            {
                query.Add(t => t.ModuleKey.Contains(TxtModuleKey.Text.Trim()));
            }

            base.FillManageFrom(iNikModuleServ, query);
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

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].ToString();
                    List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                    var deletedItems = iNikModuleServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var Item in deletedItems)
                    {
                        if (Item.IsExternal)
                        {
                            if (!Item.ModuleFile.IsEmpty())
                            {
                                Utilities.Utilities.RemoveItemFile(Item.ModuleFile);
                            }
                        }
                    }
                    iNikModuleServ.Remove(deletedItems);
                    iNikModuleServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iNikModuleServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}