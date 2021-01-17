using NikSoft.ContentManager.Service;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NikSoft.ContentManager.Web.UI
{
    public partial class ContentInCategory : NikUserControl
    {
        public IContentCategoryService iContentCategoryServ { get; set; }
        public IPublicContentService iPublicContentServ { get; set; }
        public ContentCategory category;
        public int TotalData;
        public int currentPart;
        public List<Pagination> PageParts;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void LoadData()
        {
            int Cid;
            if (!int.TryParse(ModuleParameters, out Cid))
            {
                if (ModuleParameters != null)
                {
                    category = iContentCategoryServ.Find(x => x.ModuleKey == ModuleParameters && x.PortalID == CurrentPortalID);
                    if (category != null)
                    {
                        boundPaging(true, category.ID);
                    }

                }
            }
            else
            {
                boundPaging(false, Cid);
            }
        }

        protected void boundPaging(bool HasModuleKey, int CatID)
        {
            int size = 20;
            string _part = Request.QueryString["part"];
            int part = 0;
            if (!_part.IsEmpty())
                part = _part.ToInt32();

            currentPart = part;
            int startItem = part * size;

            List<PublicContent> Datas = new List<PublicContent>();
            if (HasModuleKey)
            {
                var query1 = iPublicContentServ.ExpressionMaker();
                query1.Add(x => x.Enabled && x.CategoryID == CatID);
                if (!TxtSearch.Text.IsEmpty())
                {
                    query1.Add(x => x.Title.Contains(TxtSearch.Text) || x.MiniDesc.Contains(TxtSearch.Text));
                }
                TotalData = iPublicContentServ.Count(query1);
                Datas = iPublicContentServ.GetAllPaged(query1, x=> x.Ordering, startItem, size, true).ToList();
            }
            else
            {
                var query2 = iPublicContentServ.ExpressionMaker();
                query2.Add(x => x.Enabled && x.CategoryID == CatID && x.PortalID == CurrentPortalID);
                if (!TxtSearch.Text.IsEmpty())
                {
                    query2.Add(x => x.Title.Contains(TxtSearch.Text) || x.MiniDesc.Contains(TxtSearch.Text));
                }
                TotalData = iPublicContentServ.Count(query2);
                Datas = iPublicContentServ.GetAllPaged(query2, x => x.Ordering, startItem, size, true).ToList();
            }

           // PageParts = BoundPageRep(TotalData, size, part);
            RepPaging.DataSource = BoundPageRep(TotalData, size, part);
            RepPaging.DataBind();
            RepContents.DataSource = Datas;
            RepContents.DataBind();


        }

        protected List<Pagination> BoundPageRep(int max , int size, int part)
        {
            var pageDatas = new List<Pagination>();
            double pr = max / size;
            int val;
            if (max % size != 0)
                val = (int)Math.Floor(pr) + 1;
            else
                val = (int)Math.Floor(pr);


            int i = 0;
            while ( i < val )
            {
                if(part == i)
                {
                    pageDatas.Add(new Pagination
                    {
                        Id = i,
                        IsCurrent = true
                    });
                }
                else
                {
                    pageDatas.Add(new Pagination
                    {
                        Id = i,
                        IsCurrent = false
                    });
                }

                i++;
            }
            return pageDatas;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}