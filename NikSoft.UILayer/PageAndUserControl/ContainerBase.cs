using NikSoft.NikModel;
using NikSoft.UILayer.WebControls;
using NikSoft.Utilities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.UILayer
{
    public class ContainerBase : NikBaseUserControl, IWidgetHost
    {
        private const string WIDGET_BASE_DIRECTORY = "~/";
        protected IWidget _WidgetRef;
        private const string ADMIN_DEFAULT_WIDGET = @"~/Modules/BaseModules/Template/NullWidget.ascx";
        public WidgetUI WidgetInstance { get; set; }
        public string PageDirection { get; set; }
        protected ImageUtility imu;
        protected int headerHeight = 0;

        protected void AddModuletoPageAndBuildSkin(Control addtoCtrl, AdminorUi typeofUI)
        {
            if (null == WidgetInstance)
                return;
            try
            {
                string pathofControlToLoad = string.Empty;
                //TODO, here is where we must hook into
                if (typeofUI == AdminorUi.UiPart)
                {
                    if (!string.IsNullOrEmpty(this.WidgetInstance.Widget.NewUrl))
                    {
                        pathofControlToLoad = WIDGET_BASE_DIRECTORY + this.WidgetInstance.Widget.NewUrl;
                    }
                    else
                    {
                        pathofControlToLoad = WIDGET_BASE_DIRECTORY + this.WidgetInstance.WidgetDefinitionUI.Url;
                    }

                }
                else if (typeofUI == AdminorUi.AdminPart)
                {
                    pathofControlToLoad = ADMIN_DEFAULT_WIDGET;
                }
                var widget = LoadControl(pathofControlToLoad) as WidgetUIContainer;
                if (widget == null)
                {
                    return;
                }
                if (typeofUI == AdminorUi.AdminPart)
                {
                    widget.AddSettingPanel = true;
                    AddWID(WidgetInstance.Widget.ID);
                }
                WidgetInstance.WidgetDefinitionUI.SkinChanged += WidgetDef_SkinChanged;
                this._WidgetRef = widget as IWidget;
                this._WidgetRef.Init(this);
                if (typeofUI == AdminorUi.AdminPart)
                {
                    this.Controls.Add(widget);
                }
                else
                {
                    if (this.WidgetInstance.Widget.WidgetSkinPath.IsEmpty())
                    {
                        this.Controls.Add(widget);
                    }
                    else
                    {
                        var block = LoadControl(WIDGET_BASE_DIRECTORY + this.WidgetInstance.Widget.WidgetSkinPath) as BlockTemplate;
                        if (block == null)
                        {
                            this.Controls.Add(widget);
                        }
                        else
                        {
                            if (this.WidgetInstance.Widget.ShowTitle)
                            {
                                block.AddTitle(this.WidgetInstance.Widget.Title, !this.WidgetInstance.Widget.TitleLink.IsEmpty(), this.WidgetInstance.Widget.TitleLink);
                            }
                            block.AddControl(widget);
                            this.Controls.Add(block);
                        }
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Response.Write("Additional Info: NOP");
            }
            catch (Exception ex)
            {
                Response.Write("Engine skin builder : " + ex.Message);
                if (null != ex.InnerException)
                {
                    Response.Write("Engine skin builder2 : " + ex.InnerException.Message);
                    if (null != ex.InnerException.InnerException)
                    {
                        Response.Write("Engine skin builder3 : " + ex.InnerException.InnerException.Message);
                    }
                }

            }
        }

        private void AddCell(string picUri, byte[] picContent, string pictype, IList<TableCell> celllist, TableRow itr, HorizontalAlign ha, ImageAlign ia)
        {
            if (celllist == null)
            {
                return;
                //thrownewArgumentNullException celllist is null
            }
            celllist.Add(LoadControl(typeof(TableCell), null) as TableCell);
            int whichCell = celllist.Count - 1;
            var img = LoadControl(typeof(DynamicImage), null) as DynamicImage;
            int tempimagewidth = 0;
            if (!string.IsNullOrEmpty(picUri))
            {
                imu = new ImageUtility(picUri);
                tempimagewidth = imu.width();
                string x = ResolveClientUrl(picUri);
                string y = ResolveUrl(picUri);
                string z = Page.ResolveUrl(picUri);
                string t = GetRoutedClientURI(picUri);
                celllist[whichCell].Width = new Unit(tempimagewidth + "px");
                img.ImageFile = t;
            }
            else if (null != picContent && picContent.Length > 10)
            {
                img.ImageBytes = picContent;
            }

            img.ImageAlign = ia;
            celllist[whichCell].Controls.Add(img);
            celllist[whichCell].HorizontalAlign = ha;
            celllist[whichCell].Attributes.Add("background-position", ha.ToString().ToLower());
            if (!string.IsNullOrEmpty(picUri))
            {
                celllist[whichCell].Attributes.Add("width", tempimagewidth + "px");
            }
            //celllist[cellcount].Attributes.Add("class", "imgleft");
            //celllist[cellcount].BorderWidth = new Unit(2);
            itr.Cells.Add(celllist[whichCell]);
            //cellcount++;
        }

        public virtual void Close()
        {
        }

        public virtual void SkinChanged()
        {
        }

        public virtual void SaveState(string state)
        {
            WidgetInstance.UpdateState(state);
        }

        public virtual string GetState()
        {
            return this.WidgetInstance.Widget.State;
        }

        private void WidgetDef_SkinChanged()
        {
            this.WidgetInstance.UpdateSkin(SkinIDofWidget);
        }

        public virtual string SkinIDofWidget
        {
            get
            {
                return this.WidgetInstance.Widget.WidgetSkinPath;
            }
            set
            {
                this.WidgetInstance.Widget.WidgetSkinPath = value;
            }
        }

        protected virtual void AddWID(int wiD)
        {
            return;
        }
    }
}