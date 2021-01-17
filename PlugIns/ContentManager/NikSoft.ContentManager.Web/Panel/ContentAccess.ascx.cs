using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class ContentAccess : NikUserControl
    {
        public IContentGroupService iContentGroupServ { get; set; }
        public IContentCategoryService iContentCategoryServ { get; set; }
        public IGroupPermissionService iGroupPermissionServ { get; set; }
        public ICategoryPermissionService iCategoryPermissionServ { get; set; }
        protected string SelectData1 = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCombo();
                BoundData();
            }
                
        }

        protected override void BoundData()
        {
            var query = iGroupPermissionServ.ExpressionMaker();
            query.Add(x => x.PortalID == PortalUser.PortalID);
            base.FillManageFrom(iGroupPermissionServ, query);

            var query2 = iCategoryPermissionServ.ExpressionMaker();
            query2.Add(x => x.PortalID == PortalUser.PortalID);
            base.FillManageFrom(iCategoryPermissionServ, query2, "GV2", "pl2");
        }

        protected void BindCombo()
        {
            var myUsers = iUserServ.GetAll(x => x.PortalID == PortalUser.PortalID && x.UserType == NikUserType.NikUser, y => new ListControlModel { ID = y.ID, Title = y.FirstName + " " + y.LastName }).ToList();
            ddlUsers.FillControl(myUsers, "Title", "ID");
            var groups = iContentGroupServ.GetAll(x => x.PortalID == PortalUser.PortalID, y => new ListControlModel { ID = y.ID, Title = y.Title }).ToList();
            ddlGroup.FillControl(groups, "Title", "ID", true, true, "انتخاب همه", "0");
        }

        protected string GetUserName(int id)
        {
            var thisUser = iUserServ.Find(x => x.ID == id);
            string fullname = thisUser.FirstName + " " + thisUser.LastName;
            return fullname;
        }

        protected void DeleteItem(int id, int type)
        {
            if (type == 1)
            {
                var delitem = iGroupPermissionServ.Find(x => x.ID == id);
                iGroupPermissionServ.Remove(delitem);
                iGroupPermissionServ.SaveChanges(PortalUser.ID);
            }
            else
            {
                var delitem = iCategoryPermissionServ.Find(x => x.ID == id);
                iCategoryPermissionServ.Remove(delitem);
                iCategoryPermissionServ.SaveChanges();
            }

            Notification.SetSuccessMessage("دسترسی مورد نظر گرفته شد.");
        }

        protected void GV1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "DeleteMe":
                    DeleteItem(id, 1);
                    break;
            }
            BoundData();
        }

        protected void GV2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            switch (e.CommandName)
            {
                case "DeleteMe":
                    DeleteItem(id, 2);
                    break;
            }
            BoundData();
        }

        protected bool IsValidForm()
        {
            var groupid = ddlGroup.SelectedValue.ToInt32();
            var CategoryID = ddlCategory.GetDropDownValue(Request.Form);
            SelectData1 = "GetContentCategory2(" + groupid + "," + CategoryID + ");\n";
            if (ddlUsers.SelectedValue == "0")
            {
                ErrorMessage.Add("کاربر مورد نظر را انتخاب کنید.");
            }

            ErrorMessage.AddRange(this.ValidateTextBoxes());
            if (ErrorMessage.Count > 0)
            {
                Notification.SetErrorMessage(ErrorMessage);
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValidForm())
                return;

            var groupid = ddlGroup.SelectedValue.ToInt32();
            int userid = ddlUsers.SelectedValue.ToInt32();
            var CategoryID = ddlCategory.GetDropDownValue(Request.Form);
            
            if (groupid == 0)
            {
                var allgroups = iContentGroupServ.GetAll(x => x.PortalID == PortalUser.PortalID);
                var allperm = iGroupPermissionServ.GetAll(x => x.PortalID == PortalUser.PortalID && x.UserID == userid).Select(x => x.GroupID).ToList();
                foreach (var gr in allgroups)
                {
                    if (!allperm.Contains(gr.ID))
                    {
                        var gpitem = iGroupPermissionServ.Create();
                        gpitem.GroupID = gr.ID;
                        gpitem.UserID = userid;
                        gpitem.PortalID = PortalUser.PortalID;
                        gpitem.CreatorID = PortalUser.ID;
                        gpitem.CreateDate = DateTime.Now;
                        iGroupPermissionServ.Add(gpitem);
                    }
                    
                }

                iGroupPermissionServ.SaveChanges();
                Notification.SetSuccessMessage("گروه های مورد نظر دسترسی داده شد");
            }
            else
            {
                if (CategoryID == 0)
                {
                    var allCategories = iContentCategoryServ.GetAll(x => x.PortalID == PortalUser.PortalID);
                    var allPem = iCategoryPermissionServ.GetAll(x => x.PortalID == PortalUser.ID && x.UserID == userid).Select(x => x.CategoryID).ToList();
                    
                    foreach (var cat in allCategories)
                    {
                        if (!allPem.Contains(cat.ID))
                        {
                            var cpitem = iCategoryPermissionServ.Create();
                            cpitem.UserID = userid;
                            cpitem.CategoryID = cat.ID;
                            cpitem.PortalID = PortalUser.PortalID;
                            cpitem.CreatorID = PortalUser.ID;
                            cpitem.CreateDate = DateTime.Now;
                            iCategoryPermissionServ.Add(cpitem);
                        }
                        
                    }

                    iCategoryPermissionServ.SaveChanges();
                    Notification.SetSuccessMessage("دسته بندی های مورد نظر دسترسی داده شد");
                }
            }

            if (groupid != 0 && CategoryID != 0)
            {
                var oldGroup = iGroupPermissionServ.Find(x => x.UserID == userid && x.GroupID == groupid);
                if(oldGroup == null)
                {
                    var newItem = iGroupPermissionServ.Create();
                    newItem.UserID = userid;
                    newItem.GroupID = groupid;
                    newItem.PortalID = PortalUser.PortalID;
                    newItem.CreatorID = PortalUser.ID;
                    newItem.CreateDate = DateTime.Now;
                    iGroupPermissionServ.Add(newItem);
                    iGroupPermissionServ.SaveChanges();
                    Notification.SetSuccessMessage("گروه مورد نظر دسترسی داده شد");
                }
                else
                {
                    Notification.SetErrorMessage("گروه مورد نظر قبلا سترسی داده شده است");
                }

                var oldCat = iCategoryPermissionServ.Find(x => x.UserID == userid && x.CategoryID == CategoryID);
                if (oldCat == null)
                {
                    var newItem = iCategoryPermissionServ.Create();
                    newItem.UserID = userid;
                    newItem.CategoryID = CategoryID;
                    newItem.PortalID = PortalUser.PortalID;
                    newItem.CreatorID = PortalUser.ID;
                    newItem.CreateDate = DateTime.Now;
                    iCategoryPermissionServ.Add(newItem);
                    iCategoryPermissionServ.SaveChanges();
                    Notification.SetSuccessMessage("دسته بندی مورد نظر دسترسی داده شد.");
                }
                else
                {
                    Notification.SetErrorMessage("دسته بندی مورد نظر قبلا سترسی داده شده است");
                }
            }

            
            BoundData();

        }
    }
}