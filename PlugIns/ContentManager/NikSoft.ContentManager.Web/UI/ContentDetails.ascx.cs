using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.ContentManager.Web.UI
{
    public partial class ContentDetails : NikUserControl
    {
        public IPublicContentService iPublicContentServ { get; set; }
        public IContentFileService iContentFileServ { get; set; }
        protected PublicContent myConetnt;
        public PublicContent NextItem;
        public PublicContent PrevItem;
        public List<PublicContent> LastItems;
        public string CreateDate;
        protected void Page_Load(object sender, EventArgs e)
        {
            GetContent();
        }

        protected void GetContent()
        {
            int ContID;
            if (!int.TryParse(ModuleParameters, out ContID))
            {
                return;
            }

            myConetnt = iPublicContentServ.Find(x => x.ID == ContID);
            Page.Title = myConetnt.Title;
            if (RepAlbums != null)
            {
                RepAlbums.DataSource = myConetnt.ContentFiles.ToList();
                RepAlbums.DataBind();
            }

            
            var next = iPublicContentServ.GetRange(x => x.ID > ContID && x.CategoryID == myConetnt.CategoryID && x.PortalID == myConetnt.PortalID, x => x.ID, 1, false);
            if (next != null)
                NextItem = next.FirstOrDefault();
            var prev = iPublicContentServ.GetRange(x => x.ID < ContID && x.CategoryID == myConetnt.CategoryID && x.PortalID == myConetnt.PortalID, x => x.ID, 1, true);
            if (prev != null)
                PrevItem = prev.FirstOrDefault();

            if(RepLast != null)
            {
                LastItems = iPublicContentServ.GetAll(x => x.CategoryID == myConetnt.CategoryID && x.PortalID == CurrentPortalID && x.ID != myConetnt.ID).OrderByDescending(x => x.ID).Take(6).ToList();
                RepLast.DataSource = LastItems;
                RepLast.DataBind();
            }
                
        }

    }
}