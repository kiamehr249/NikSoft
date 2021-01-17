using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using System;
using System.Web.UI.HtmlControls;

namespace NikSoft.Web.Modules.BaseModules.Template
{
    public partial class PanelWidgetContainer : ContainerBase, IWidgetHost
    {
        public IWidgetService iwidgetservice { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AddModuletoPageAndBuildSkin(WidgetBodyPanel, AdminorUi.AdminPart);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ltTitle.Text = this.WidgetInstance.WidgetDefinitionUI.Title;
            UpdateTexts();
        }


        private void UpdateTexts()
        {
            lt_Pub.Text = this.WidgetInstance.Widget.Published ? "پنهان کردن ویجت " : "نشان دادن ویجت";
            lt_title.Text = this.WidgetInstance.Widget.ShowTitle ? "پنهان کردن عنوان " : "نشان دادن عنوان";
            lt_panel.Text = this.WidgetInstance.Widget.Expanded ? "مشاهده تنظیمات " : "‌بستن تنظیمات";
            WidgetTitle.Text = this.WidgetInstance.Widget.Title;
            widgetEdit.NavigateUrl = "~/panel/EditWidget/" + WidgetInstance.Widget.ID;
        }


        public override void Close()
        {
            var e = new WidgetDeletionEventArgs(WidgetInstance.Widget.ID.ToString());
            WidgetInstance.WidgetDefinitionUI.OnWidgetDeletionEventHandler(WidgetInstance, e);
        }

        public override void SkinChanged()
        {
            WidgetInstance.WidgetDefinitionUI.OnSkinChangedEventHandler();
        }

        public override void SaveState(string state)
        {
            WidgetInstance.UpdateState(state);
        }

        public override string GetState()
        {
            return this.WidgetInstance.Widget.State;
        }

        public override string SkinIDofWidget
        {
            get
            {
                return this.WidgetInstance.Widget.WidgetSkinPath;
            }
            set
            {
                var w = iwidgetservice.Find(x => x.ID == WidgetInstance.Widget.ID);
                w.WidgetSkinPath = value;
                iwidgetservice.SaveChanges();
                this.WidgetInstance.Widget.WidgetSkinPath = value;
            }
        }

        protected void ShowTitle_Click(object sender, EventArgs e)
        {
            try
            {
                var w = iwidgetservice.Find(x => x.ID == WidgetInstance.Widget.ID);
                w.ShowTitle = !WidgetInstance.Widget.ShowTitle;
                iwidgetservice.SaveChanges();
                UpdateTexts();
                WidgetHeaderUpdatePanel.Update();
            }
            catch { return; }
        }

        protected void SaveWidgetTitle_Click(object sender, EventArgs e)
        {
            WidgetTitleTextBox.Visible = SaveWidgetTitle.Visible = lw1.Visible = widget_tlink.Visible = false;
            WidgetTitle.Visible = true;
            if (string.Empty != WidgetTitleTextBox.Text.Trim())
            {
                var w = iwidgetservice.Find(x => x.ID == WidgetInstance.Widget.ID);
                w.Title = WidgetTitleTextBox.Text;
                w.TitleLink = widget_tlink.Text;
                iwidgetservice.SaveChanges();
                UpdateTexts();
                WidgetHeaderUpdatePanel.Update();
            }
        }

        protected void WidgetTitle_Click(object sender, EventArgs e)
        {
            WidgetTitleTextBox.Text = this.WidgetInstance.Widget.Title;
            widget_tlink.Text = this.WidgetInstance.Widget.TitleLink;
            WidgetTitleTextBox.Visible = SaveWidgetTitle.Visible = lw1.Visible = widget_tlink.Visible = true;
            WidgetTitle.Visible = false;
        }

        protected void ShowSetting_Click(object sender, EventArgs e)
        {
            try
            {
                this._WidgetRef.ShowSettings();
                WidgetBodyUpdatePanel.Update();
                UpdateTexts();
            }
            catch { return; }
        }

        protected override void AddWID(int wiD)
        {
            var cntrl = new HtmlGenericControl("span");
            cntrl.Style.Add("display", "none");
            cntrl.Attributes.Add("wid", wiD.ToString());
            cntrl.Attributes.Add("class", "widgetclass");
            plcWid.Controls.Add(cntrl);
        }

        protected void lb_delWid_Click(object sender, EventArgs e)
        {
            try
            {
                var w = iwidgetservice.Find(x => x.ID == WidgetInstance.Widget.ID);
                w.Published = !WidgetInstance.Widget.Published;
                iwidgetservice.SaveChanges();
                UpdateTexts();
                WidgetHeaderUpdatePanel.Update();

            }
            catch { return; }
        }
    }
}