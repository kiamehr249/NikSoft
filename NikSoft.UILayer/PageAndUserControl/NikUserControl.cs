using NikSoft.Model;
using NikSoft.UILayer.WebControls;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer
{
    public class NikUserControl : WidgetUIContainer
    {

        string finalPagerID = "p_lb";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            NikGridView gv = GetGridView("gv1");
            if (null != gv)
            {
                gv.RowDataBound += gv_RowDataBound;
                gv.DataBound += gv_DataBound;
            }
        }




        protected override void OnLoad(EventArgs e)
        {
            GetItemID();
            base.OnLoad(e);
            EditFunctionBoundIfExists();
        }

        public void FormControlChecker(string cltValue, ControlCheckType cltType, string title, int maxLength = 100, int minLength = 0)
        {
            switch (cltType)
            {
                case ControlCheckType.String:
                    if (cltValue.IsEmpty())
                    {
                        ErrorMessage.Add(title + "را وارد کنید.");
                    }
                    else if (cltValue.Length > maxLength || cltValue.Length < minLength)
                    {
                        ErrorMessage.Add(" طول" + title + " باید بین " + minLength + " و " + maxLength + "باشد.");
                    }
                    break;
                case ControlCheckType.NullableNumeric:
                    if (!cltValue.IsEmpty())
                    {
                        if (!cltValue.IsNumeric())
                        {
                            ErrorMessage.Add(title + "را صحیح وارد کنید.");
                        }
                        else if (cltValue.ToInt32() > maxLength || cltValue.ToInt32() < minLength)
                        {
                            ErrorMessage.Add(" مقدار" + title + " باید بین " + minLength + " و " + maxLength + "باشد.");
                        }
                    }
                    break;
                case ControlCheckType.Numeric:
                    if (cltValue.IsEmpty())
                    {
                        ErrorMessage.Add(title + "را وارد کنید.");
                    }
                    else if (!cltValue.IsNumeric())
                    {
                        ErrorMessage.Add(title + "را صحیح وارد کنید.");
                    }
                    else if (cltValue.ToInt32() > maxLength || cltValue.ToInt32() < minLength)
                    {
                        ErrorMessage.Add(" مقدار" + title + " باید بین " + minLength + " و " + maxLength + "باشد.");
                    }
                    break;
                case ControlCheckType.NullableLongNumeric:
                    if (!cltValue.IsEmpty())
                    {
                        if (!cltValue.IsNumeric())
                        {
                            ErrorMessage.Add(title + "را صحیح وارد کنید.");
                        }
                        else if (cltValue.Length > maxLength || cltValue.Length < minLength)
                        {
                            ErrorMessage.Add(" مقدار" + title + " باید بین " + minLength + " و " + maxLength + "باشد.");
                        }
                    }
                    break;
                case ControlCheckType.LongNumeric:
                    if (cltValue.IsEmpty())
                    {
                        ErrorMessage.Add(title + "را وارد کنید.");
                    }
                    else if (!cltValue.IsNumeric())
                    {
                        ErrorMessage.Add(title + "را صحیح وارد کنید.");
                    }
                    else if (cltValue.Length > maxLength || cltValue.Length < minLength)
                    {
                        ErrorMessage.Add(" مقدار" + title + " باید بین " + minLength + " و " + maxLength + "باشد.");
                    }
                    break;

            }
        }

        #region "BaseRDform"

        private bool showFunctionButton = true;
        public bool ShowFunctionButton
        {
            get { return showFunctionButton; }
            set { showFunctionButton = value; }
        }

        private int startRecord = 0;
        private bool reOfsste = false;

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            GoToEdit();
        }

        protected void GoToEdit()
        {
            string del1, check2 = "";
            string[] check1array;
            if (null != Request.Form["ch1"])
            {
                try
                {
                    del1 = Request.Form["ch1"].ToString();
                    check1array = del1.Split(',');
                    check2 = check1array[0];
                    base.Container.gotoEditURI(check2);
                }
                catch
                {
                    Notification.SetErrorMessage("select at least one of them");
                }
                finally
                {
                }
            }
        }

        protected virtual void BoundData()
        {
        }

        protected NikGridView GetGridView(string name)
        {
            return this.FindControl(name) as NikGridView;
        }

        protected PlaceHolder GetPagingHolderControl(string name)
        {
            return this.FindControl(name) as PlaceHolder;
        }

        protected TextBox GetCountTextBox(string name)
        {
            return this.FindControl(name) as TextBox;
        }

        private void lb_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "paginationcommand")
            {
                startRecord = int.Parse(e.CommandArgument.ToString());
                BoundData();
            }
        }

        protected void FillManageFrom<T, TResult>(INikService<T> objservice, IQueryable<TResult> predicate, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount") where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.QueryResultCount(predicate);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.QueryResult(predicate, startindex, pagesize);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = t;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }

        protected void FillManageFrom<T>(INikService<T> objservice, Expression<Func<T, bool>> predicate) where T : class
        {
            var q = objservice.ExpressionMaker();
            q.Add(predicate);
            FillManageFrom(objservice, q);
        }

        protected void FillManageFrom<T>(INikService<T> objservice, List<Expression<Func<T, bool>>> allpredicates, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount") where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.Count(allpredicates);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.GetAllPaged(allpredicates, startindex, pagesize);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = t;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }


        protected void FillManageFromDesc<T, TKey>(INikService<T> objservice, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> orderKeySelector) where T : class
        {
            var q = objservice.ExpressionMaker();
            q.Add(predicate);
            FillManageFromDesc(objservice, q, orderKeySelector);
        }


        protected void FillManageFromDesc<T, TKey>(INikService<T> objservice, List<Expression<Func<T, bool>>> allpredicates, Expression<Func<T, TKey>> orderKeySelector, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount") where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.Count(allpredicates);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.GetDescending(allpredicates, x => x, orderKeySelector, startindex, pagesize);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = t;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }


        protected void FillManageFrom(object objResult, int countrecords, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount")
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = objResult;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }

        protected void FillManageFrom<T, TKey>(INikService<T> objservice, List<Expression<Func<T, bool>>> allpredicates, Expression<Func<T, TKey>> orderKeySelector, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount", bool desend = false) where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.Count(allpredicates);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.GetAllPaged(allpredicates, orderKeySelector, startindex, pagesize, desend);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = t;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }

        protected void FillManageFromd<T, TKey>(INikService<T> objservice, List<Expression<Func<T, bool>>> allpredicates, Expression<Func<T, TKey>> orderKeySelector, Repeater repeater, int pagesize, string pagingID = "pl1", string countTextID = "txtCount", bool desend = false) where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = "startrow";
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.Count(allpredicates);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.GetAllPaged(allpredicates, orderKeySelector, startindex, pagesize, desend);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            repeater.DataSource = t;
            repeater.DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }



        protected void FillManageFromDynamic<T>(INikService<T> objservice, List<Expression<Func<T, bool>>> allpredicates, string orderByString, string gridID = "gv1", string pagingID = "pl1", string countTextID = "txtCount", bool desend = false) where T : class
        {
            finalPagerID = "p_lb_" + pagingID;
            var pagingQs = this.GetGridView(gridID).PagingQueryString;
            int pagesize = this.GetGridView(gridID).PageSize;
            int startindex = 0;
            if (null != Request.QueryString[pagingQs])
            {
                if (!int.TryParse(Request.QueryString[pagingQs], out startindex))
                {
                    return;
                }
                if (startindex < 0)
                {
                    startindex = 0;
                }
            }
            if (startindex == 0)
            {
                startindex = startRecord;
            }
            var countrecords = objservice.Count(allpredicates);
            if (countrecords < startindex)
            {
                startindex = 0;
                reOfsste = true;
            }
            var t = objservice.GetAllPagedDynamic(allpredicates, orderByString, startindex, pagesize);
            if (null != GetPagingHolderControl(pagingID))
            {
                this.GetPagingHolderControl(pagingID).Controls.Clear();
                this.GetPagingHolderControl(pagingID).Controls.Add(MakePostBackPager(startindex, pagesize, countrecords, Request.QueryString, lb_Command, pagingQs));
            }
            this.GetGridView(gridID).DataSource = t;
            this.GetGridView(gridID).DataBind();
            var lc = GetCountTextBox(countTextID);
            if (null != lc)
            {
                lc.Text = countrecords.ToString();
            }
        }


        public Control MakePostBackPager(int startindex, int countPerPage, int totalRecords, NameValueCollection querystring, CommandEventHandler buttonsEventHandler, string pagingVar = "startrow")
        {
            const int rangeofLinks = 5;
            var resultedControl = new HtmlGenericControl("ul");
            resultedControl.Attributes["class"] = "pagination";
            var lbButtonsCount = 0;

            var pgCnt = (int)Math.Ceiling(((double)totalRecords / countPerPage));
            if (pgCnt > 1)
            {
                var idxBack = startindex - countPerPage;
                var idxNext = startindex + countPerPage;
                var curPage = (int)Math.Ceiling((double)(startindex + 1) / countPerPage);
                if (idxBack >= 0)
                {
                    resultedControl.Controls.Add(CreateLinkButton("<i class='fa fa-chevron-right' aria-hidden='true'></i>", finalPagerID + (++lbButtonsCount), 0, pagingVar, buttonsEventHandler, curPage == 1));
                }
                else
                {
                    resultedControl.Controls.Add(CreateLinkButton("<i class='fa fa-chevron-right' aria-hidden='true'></i>", finalPagerID + (++lbButtonsCount), 0, pagingVar, buttonsEventHandler, isDisabled: true));
                }
                var idxFst = Math.Max(curPage - rangeofLinks, 1);
                var idxLst = Math.Min(curPage + rangeofLinks, pgCnt);
                for (var i = idxFst; i <= idxLst; i++)
                {
                    var offsetPage = (i - 1) * countPerPage;
                    if (i == curPage)
                    {
                        resultedControl.Controls.Add(CreateLinkButton(i.ToString(), finalPagerID + (++lbButtonsCount), offsetPage, pagingVar, buttonsEventHandler, true));
                    }
                    else
                    {
                        resultedControl.Controls.Add(CreateLinkButton(i.ToString(), finalPagerID + (++lbButtonsCount), offsetPage, pagingVar, buttonsEventHandler));
                    }
                }
                if (idxNext < totalRecords)
                {
                    resultedControl.Controls.Add(CreateLinkButton("<i class='fa fa-chevron-left' aria-hidden='true'></i>", finalPagerID + (++lbButtonsCount), (pgCnt - 1) * countPerPage, pagingVar, buttonsEventHandler));
                }
                else
                {
                    resultedControl.Controls.Add(CreateLinkButton("<i class='fa fa-chevron-left' aria-hidden='true'></i>", finalPagerID + (++lbButtonsCount), (pgCnt - 1) * countPerPage, pagingVar, buttonsEventHandler, isDisabled: true));
                }
                var finalresultedControl = new HtmlGenericControl("div");
                finalresultedControl.Controls.Add(resultedControl);
                return finalresultedControl;
            }
            var emptyDiv = new HtmlGenericControl("div");
            return emptyDiv;
        }


        private Control CreateLinkButton(string lText, string cID, int pageNumber, string pageingvar, CommandEventHandler eventHandler, bool isActive = false, bool isDisabled = false)
        {
            var querystring = Request.QueryString;
            string allQueries = "";
            if (null != querystring)
            {
                foreach (var str in querystring.AllKeys.Where(x => null != x))
                {
                    if (str.ToLower() != pageingvar)
                    {
                        allQueries += "&" + str + "=" + querystring[str];
                    }
                }
            }
            if (allQueries != "")
            {
                allQueries = "?" + allQueries.Substring(1) + "&" + pageingvar;
            }
            else
            {
                allQueries = "?" + pageingvar;
            }

            var innerLi = new HtmlGenericControl("li");
            if (isActive)
            {
                innerLi.Attributes.Add("class", "active");
            }
            if (isDisabled)
            {
                innerLi.Attributes.Add("class", "disabled");
            }
            var lb = new LinkButton
            {
                Text = lText,
                ID = cID,
                CommandArgument = Convert.ToString(pageNumber),
                CommandName = "paginationcommand"
            };
            if (isActive || isDisabled)
            {
                lb.Enabled = false;
            }
            lb.ClientIDMode = ClientIDMode.Predictable;
            lb.PostBackUrl = "~/" + this.Level + "/" + this.ModuleName + "/" + (ModuleParameters == "start" ? "" : ModuleParameters) + allQueries + "=" + Convert.ToString(pageNumber);
            lb.Command += eventHandler;
            innerLi.Controls.Add(lb);
            return innerLi;
        }


        protected string ReturnPager(int startindex, int countPerPage, int totalRecords, string pagingVar = "startrow")
        {
            const int rangeofLinks = 5;
            var resultedPagingStr = "<nav><ul class='pagination'>";

            var pgCnt = (int)Math.Ceiling(((double)totalRecords / countPerPage));
            if (pgCnt > 1)
            {
                var idxBack = startindex - countPerPage;
                var idxNext = startindex + countPerPage;
                var curPage = (int)Math.Ceiling((double)(startindex + 1) / countPerPage);
                if (idxBack >= 0)
                {
                    resultedPagingStr += ReturnPagerLink("اول", 0, pagingVar, curPage == 1);
                }
                else
                {
                    resultedPagingStr += ReturnPagerLink("اول", 0, pagingVar, isDisabled: true);
                }
                var idxFst = Math.Max(curPage - rangeofLinks, 1);
                var idxLst = Math.Min(curPage + rangeofLinks, pgCnt);
                for (var i = idxFst; i <= idxLst; i++)
                {
                    var offsetPage = (i - 1) * countPerPage;
                    if (i == curPage)
                    {
                        resultedPagingStr += ReturnPagerLink(i.ToString(), offsetPage, pagingVar, true);
                    }
                    else
                    {
                        resultedPagingStr += ReturnPagerLink(i.ToString(), offsetPage, pagingVar);
                    }
                }
                if (idxNext < totalRecords)
                {
                    resultedPagingStr += ReturnPagerLink("آخر", (pgCnt - 1) * countPerPage, pagingVar);
                }
                else
                {
                    resultedPagingStr += ReturnPagerLink("آخر", (pgCnt - 1) * countPerPage, pagingVar, isDisabled: true);
                }
            }


            resultedPagingStr += "</ul></nav>";
            return resultedPagingStr;
        }

        private string ReturnPagerLink(string lText, int pageNumber, string pageingvar, bool isActive = false, bool isDisabled = false)
        {
            var querystring = Request.QueryString;
            string allQueries = "";
            if (null != querystring)
            {
                foreach (var str in querystring.AllKeys.Where(x => null != x))
                {
                    if (str.ToLower() != pageingvar)
                    {
                        allQueries += "&" + str + "=" + querystring[str];
                    }
                }
            }
            if (allQueries != "")
            {
                allQueries = "?" + allQueries.Substring(1) + "&" + pageingvar;
            }
            else
            {
                allQueries = "?" + pageingvar;
            }

            var classofLI = "";


            if (isActive)
            {
                classofLI = " active ";
            }
            if (isDisabled)
            {
                classofLI += " disabled ";
            }


            var hyperLink = "<a href=" + ResolveClientUrl("~/" + Level + "/" + ModuleName + "/" + ModuleParameters + allQueries + "=" + Convert.ToString(pageNumber)) + ">";
            hyperLink += lText + "</a>";
            if (isActive || isDisabled)
            {
                classofLI += " disabled ";
            }
            var innerLi = "<li class='" + classofLI + "'>";
            return innerLi + hyperLink + "<li>";
        }



        protected void gv_RowDataBound(object sender, GridViewRowEventArgs args)
        {
            var gridView = sender as NikGridView;
            if (null == gridView)
            {
                return;
            }
            var cols = gridView.Columns;
            if (args.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < cols.Count; i++)
                {
                    if (cols[i] is BoundField && !(cols[i] is CheckBoxField) && !(cols[i] is TemplateField))
                    {
                        var nItem = cols[i] as BoundField;
                        if (nItem.DataFormatString == "{0:FixLenght}")
                        {
                            var text = args.Row.Cells[i].Text;
                            if (text.Length > 21)
                            {
                                args.Row.Cells[i].Text = "<span title='" + text + "'>" + text.Substring(0, 20) + "... </span>";
                            }
                        }
                    }
                }
            }
            if (gridView.SortExpression.Length <= 0) return;
            var cellIndex = -1;
            foreach (DataControlField field in gridView.Columns)
            {
                if (field.SortExpression != gridView.SortExpression)
                {
                    continue;
                }
                cellIndex = gridView.Columns.IndexOf(field);
                break;
            }
            if (cellIndex <= -1)
            {
                return;
            }
            if (args.Row.RowType == DataControlRowType.Header)
            {
                args.Row.Cells[cellIndex].CssClass += (gridView.SortDirection == SortDirection.Ascending ? " sortasc" : " sortdesc");
            }
        }

        protected void gv_DataBound(object sender, EventArgs args)
        {
            var gridView = sender as NikGridView;
            if (null == gridView)
            {
                return;
            }
            foreach (GridViewRow item in gridView.Rows)
            {
                var lc = item.FindControl("ListCounter2") as ListCounter;
                var query = Request.QueryString[gridView.PagingQueryString];
                if (lc != null)
                {
                    if (startRecord > 0 || reOfsste)
                    {
                        lc.IndexOffset = startRecord + 1;
                    }
                    else
                    {
                        if (query.IsNumeric())
                        {
                            lc.IndexOffset = query.ToInt32() + 1;
                        }
                        else
                        {
                            lc.IndexOffset = 1;
                        }
                    }
                }
            }
        }

        #endregion "BaseRDform"

        #region "CUBaseControl"

        protected int ItemID = 0;
        protected Action GetEditingFunction;
        protected Action SaveNewFunction;
        protected Action SaveEditFunction;
        protected Func<bool> FormValidation = () => true;
        protected List<string> ErrorMessage = new List<string>();



        protected void GetItemID()
        {
            if (ModuleParameters == null)
            {
                return;
            }
            if (!ModuleParameters.Contains("|"))
            {
                return;
            }
            string[] queryID = ModuleParameters.Split('|');
            if (!queryID[1].IsNumeric())
            {
                return;
            }
            else
            {
                ItemID = queryID[1].ToInt32();
            }
        }


        protected void EditFunctionBoundIfExists()
        {
            if (!IsPostBack && ItemID > 0)
            {
                if (null != GetEditingFunction)
                {
                    GetEditingFunction();
                }
                //GetEditingFunction?.Invoke();
            }
        }

        protected void SaveClick(object sender, EventArgs e)
        {
            if (!FormValidation())
            {
                if (ErrorMessage.Count() > 0)
                {
                    Notification.SetErrorMessage(ErrorMessage);
                }
                return;
            }
            if (ItemID > 0)
            {
                SaveEditFunction();
            }
            else
            {
                SaveNewFunction();
            }
        }

        #endregion "CUBaseControl"
    }
}