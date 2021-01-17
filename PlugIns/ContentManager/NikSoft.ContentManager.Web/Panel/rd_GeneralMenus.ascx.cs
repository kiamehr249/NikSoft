using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class rd_GeneralMenus : NikUserControl
    {
        public IGeneralMenuService iGeneralMenuServ { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Container.btnDelete.Click += BtnDelete_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BoundData();
        }


        protected override void BoundData()
        {
            var query = iGeneralMenuServ.ExpressionMaker();
            query.Add(t => t.PortalID == PortalUser.PortalID);
            if (!txtTitle.Text.IsEmpty())
            {
                query.Add(t => t.Title.Contains(txtTitle.Text.Trim()));
            }
            base.FillManageFrom(iGeneralMenuServ, query);
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
            var DataItem = iGeneralMenuServ.Find(x => x.ID == id);
            switch (e.CommandName)
            {
                case "enabled":
                    DataItem.Enabled = !DataItem.Enabled;
                    iGeneralMenuServ.SaveChanges();
                    break;
            }
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
                    var deletedItems = iGeneralMenuServ.GetAll(x => l.Contains(x.ID)).ToList();
                    iGeneralMenuServ.Remove(deletedItems);
                    iGeneralMenuServ.SaveChanges();
                    Notification.SetSuccessMessage("حذف با موفقیت انجام شد.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حذف انجام نشد.");
                iGeneralMenuServ.Reaload();
            }
            finally
            {
                this.BoundData();
            }
        }
    }
}