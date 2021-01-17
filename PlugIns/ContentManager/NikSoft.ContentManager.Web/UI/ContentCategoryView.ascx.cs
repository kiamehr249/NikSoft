using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.UI
{
    public partial class ContentCategoryView : NikUserControl
    {
        public IContentCategoryService iContentCategoryServ { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            int gid;
            if(!int.TryParse(ModuleParameters, out gid))
            {
                IssueContentNotFound();
                Issue404();
            }

            var query = iContentCategoryServ.ExpressionMaker();
            query.Add(x => x.PortalID == CurrentPortalID && x.Enabled && x.GroupID == gid && x.ParentID == null);
            if (!TxtSearch.Text.IsEmpty())
            {
                query.Add(x => x.Title.Contains(TxtSearch.Text) || x.Description.Contains(TxtSearch.Text));
            }
            var AllCats = iContentCategoryServ.GetAll(query).ToList();
            RepGroup.DataSource = AllCats;
            RepGroup.DataBind();
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}