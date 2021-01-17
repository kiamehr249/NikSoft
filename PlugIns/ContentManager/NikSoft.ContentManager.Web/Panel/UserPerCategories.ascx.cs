using NikSoft.ContentManager.Service;
using NikSoft.NikModel;
using NikSoft.UILayer;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.Panel
{
    public partial class UserPerCategories : NikUserControl
    {
        public IContentCategoryService iContentCategoryServ { get; set; }
        public ICategoryPermissionService iCategoryPermissionServ { get; set; }
        public User MyUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadMe();
        }

        protected void LoadMe()
        {
            MyUser = iUserServ.Find(x => x.ID == PortalUser.ID);
            var perIDs = iCategoryPermissionServ.GetAll(x => x.UserID == PortalUser.ID, y => new { y.CategoryID }).Select(x => x.CategoryID).ToList();
            var query = iContentCategoryServ.ExpressionMaker();
            query.Add(x => perIDs.Contains(x.ID));
            base.FillManageFromd(iContentCategoryServ, query, x => x.ID, RepCategories, 20);
        }
    }
}