using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.Menu
{
    public partial class cu_Menu : NikUserControl
    {
        public INikMenuService iNikMenuService { get; set; }

        protected override void OnInit(EventArgs e)
        {
            ShowFunctionButton = false;
            base.OnInit(e);
        }

        private void BindCombo()
        {
            var data = iNikMenuService.GetAll(t => t.PortalID == PortalUser.PortalID).OrderBy(t => t.ParentID).ThenBy(t => t.Ordering).ToList();
            if (ItemID != 0)
            {
                data = data.Where(t => t.ID != ItemID).ToList();
            }
            ddlParent.FillControl(data.Select(t => new { t.ID, t.Title }).ToList(), "Title", "ID");
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
                ErrorMessage.Add("Pleas insert Title");
            }
            if (txtLink.Text.IsEmpty())
            {
                ErrorMessage.Add("Pleas insert Link");
            }
            if (!txtOrder.Text.IsNumeric())
            {
                ErrorMessage.Add("Oredr Number is wrong!");
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
            var data = iNikMenuService.Find(t => t.ID == ItemID);
            if (data == null)
            {
                Container.gotoList();
                return;
            }
            txtTitle.Text = data.Title;
            if (data.ParentID != null)
            {
                ddlParent.SelectedValue = data.ParentID.ToString();
            }
            txtAweSome.Text = data.AweSomeFontClass ?? "";
            txtOrder.Text = data.Ordering.ToString();
            chbEnbaled.Checked = data.Enabled;
            chbShowInDashboard.Checked = data.ShowInPanel;
            if (!data.ModuleLinkTitle.IsEmpty())
            {
                hfLink.Value = "panel/" + data.Link;
                txtLink.Text = data.ModuleLinkTitle;
            }
            else
            {
                txtLink.Text = data.Link;
            }
        }

        private void SaveNewData()
        {
            var isOk = false;
            var VPath = "PanelMenu";
            string fileName = string.Empty;
            string ImgDashboard = string.Empty;
            if (fuIcon.HasFile)
            {
                isOk = Utilities.Utilities.UploadFile(fuIcon.PostedFile, VPath, ref fileName, PortalUser.PortalFolderPath);
            }

            int SelectedParent = ddlParent.SelectedValue.ToInt32();
            int? IsParent;
            if (SelectedParent == 0)
            {
                IsParent = null;
            }
            else
            {
                IsParent = SelectedParent;
            }

            var menu = iNikMenuService.Create();
            menu.PortalID = PortalUser.PortalID;
            menu.ParentID = IsParent;
            menu.Title = txtTitle.Text.Trim();
            menu.ModuleLinkTitle = "";
            menu.Link = hfLink.Value.IsEmpty() ? txtLink.Text.Trim() : "panel/" + hfLink.Value.Trim();
            menu.ModuleLinkTitle = hfLink.Value.IsEmpty() ? string.Empty : txtLink.Text;
            menu.Ordering = txtOrder.Text.ToInt32();
            menu.Enabled = chbEnbaled.Checked;
            menu.ImageURI = fileName;
            menu.AweSomeFontClass = txtAweSome.Text.Trim();
            menu.ShowInPanel = chbShowInDashboard.Checked;
            iNikMenuService.Add(menu);
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }

        private void UpdateData()
        {
            var isOk = false;
            string MenuImage = string.Empty;
            string ImgDashboard = string.Empty;
            if (fuIcon.HasFile)
            {
                isOk = Utilities.Utilities.UploadFile(fuIcon.PostedFile, "Menu", ref MenuImage, PortalUser.PortalFolderPath);
            }
            var menu = iNikMenuService.Find(y => y.ID == ItemID);

            int SelectedParent = ddlParent.SelectedValue.ToInt32();
            int? IsParent;
            if (SelectedParent == 0)
            {
                IsParent = null;
            }
            else
            {
                IsParent = SelectedParent;
            }

            menu.ParentID = IsParent;
            menu.Title = txtTitle.Text.Trim();

            menu.Link = hfLink.Value.IsEmpty() ? txtLink.Text.Trim() : "panel/" + hfLink.Value.Trim().Replace("panel/", "");
            menu.ModuleLinkTitle = hfLink.Value.IsEmpty() ? string.Empty : txtLink.Text;
            menu.Ordering = txtOrder.Text.ToInt32();
            menu.Enabled = chbEnbaled.Checked;
            menu.AweSomeFontClass = txtAweSome.Text.Trim();
            menu.ShowInPanel = chbShowInDashboard.Checked;
            if (!string.IsNullOrEmpty(MenuImage))
            {
                Utilities.Utilities.RemoveItemFile(menu.ImageURI);
                menu.ImageURI = MenuImage;
            }
            uow.SaveChanges(PortalUser.ID);
            Container.gotoList();
        }
    }
}