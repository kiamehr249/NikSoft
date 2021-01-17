using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Portal
{
    public partial class PortalDomain : WidgetUIContainer
    {

        public IPortalService iportalServ { get; set; }
        public IPortalAddressService iportalAddressServ { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }
        protected int selectedPortalID = 0;

        protected string portalTitle = "";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ModuleParameters.IsNumeric())
            {
                selectedPortalID = ModuleParameters.ToInt32();
            }
            if (selectedPortalID == 0)
            {
                RedirectTo("~/panel/rd_portal/view");
                return;
            }

            var ThisPortal = iportalServ.Find(x => x.ID == selectedPortalID);
            portalTitle = ThisPortal.Title;

            if (!IsPostBack)
            {
                BoundData();
            }
        }

        protected void BoundData()
        {
            var query = iportalAddressServ.ExpressionMaker();
            query.Add(t => t.PortalID == selectedPortalID);
            GV1.DataSource = iportalAddressServ.GetAll(query);
            GV1.DataBind();
        }

        private void SaveNewData()
        {

            if (txtDomainAddress.Text.Trim().IsEmpty())
            {
                Notification.SetErrorMessage("دامنه را وارد کنید.");
                return;
            }

            var selectedPortal = iportalAddressServ.Find(t => t.DomainAddress == txtDomainAddress.Text.Trim()
            && t.PortalID == selectedPortalID);
            if (null != selectedPortal)
            {
                Notification.SetErrorMessage("این دامنه قبلا استفاده شده است.");
                return;
            }
            var newAddress = iportalAddressServ.Create();
            newAddress.DomainAddress = txtDomainAddress.Text.Trim();
            newAddress.Desciption = TxtDesc.Text;
            newAddress.PortalID = selectedPortalID;
            iportalAddressServ.Add(newAddress);
            iportalAddressServ.SaveChanges();
            this.ClearForm();
            BoundData();
        }

        protected void bd_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].ToString();
                    List<int> l = del1.Split(',').ToList().ConvertAll(x => int.Parse(x));
                    var deletedItems = iportalAddressServ.GetAll(x => l.Contains(x.ID)).ToList();
                    foreach (var item in deletedItems)
                    {
                        iportalAddressServ.Remove(item);
                    }
                    iportalAddressServ.SaveChanges(PortalUser.ID);
                    Notification.SetSuccessMessage("آیتم یا آیتم ها حذف شدند.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iUserServ.Reaload();
            }
            finally
            {
                this.ClearForm();
                BoundData();
            }
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            RedirectTo("~/panel/rd_portal/view");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveNewData();
        }
    }

}