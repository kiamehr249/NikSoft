using NikSoft.Model;
using NikSoft.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using NikSoft.UILayer.WebControls;
using NikSoft.Services;

namespace NikSoft.UILayer
{
    public class WidgetUIContainer : UserControl, INikContentManage, IWidget
    {

        private DBUtilities dbi = new DBUtilities();
        public IUnitOfWork uow { get; set; }
        public IUserService iUserServ { get; set; }
        public IThemeService iThemeServ { get; set; }
        public INikSettingService iNikSettingServ { get; set; }
        public string NikSiteTitle { get; private set; }

        protected bool IsAnyNotFoundIssued = false;

        public WidgetUIContainer()
        {
            ObjectFactory.BuildUp(this);
        }

        private bool addsettingpanel = false;
        public bool AddSettingPanel
        {
            get { return addsettingpanel; }
            set { addsettingpanel = value; }
        }

        public void InitHost(IEngineContainer host)
        {
            container = host;
        }

        private IEngineContainer container;
        public IEngineContainer Container
        {
            get
            {
                if (null != container)
                {
                    return container;
                }
                if (null != ((Parent) as INikContentManage))
                {
                    return ((Parent) as INikContentManage).Container;
                }
                if (null != ((NamingContainer) as INikContentManage))
                {
                    return ((NamingContainer) as INikContentManage).Container;
                }
                return null;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                var whichOne = PortalUser != null && Level.ToLower() == "panel" ? PortalUser.PortalID : CurrentPortalID;
                //NikSiteTitle = iEngineSettingService.GetSettingValue("SiteTitle", NikSettingType.Messages, whichOne);
                BuildProperties();
            }
            catch { }
            if (addsettingpanel)
            {
                BuildSettingPanel();
            }
        }

        protected void RedirectTo(string locationToGo)
        {
            Response.Redirect(locationToGo, false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void RedirectTo2(string locationToGo, bool trueorFlase = false)
        {
            Response.Redirect(locationToGo, trueorFlase);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void RedirectHome()
        {
            var locationToGo = "/";
            if (Level.ToLower() != "home" && Level.ToLower() != "panel")
            {
                locationToGo = "/" + Level;
            }
            RedirectTo(locationToGo);
        }


        protected string GetHomeURL()
        {
            if (Level.ToLower() == "home")
            {
                return "/";
            }
            return "/" + Level + "/";
        }


        protected void RedirectToModule(string moduleName)
        {
            var locationToGo = "/" + Level + "/" + moduleName;
            RedirectTo(locationToGo);
        }


        protected void RedirectToModuleWithForce(string moduleName)
        {
            var locationToGo = "/" + Level + "/" + moduleName;
            RedirectTo2(locationToGo);
        }


        public void InsertContentNotFoundAtTop()
        {
            var _404Path = Server.MapPath("~/404s/404_CNF.html");
            string text = File.ReadAllText(_404Path);
            Response.StatusCode = 404;
            var l1 = new Literal();
            l1.Text = text;
            this.Controls.AddAt(0, l1);
        }

        public NikPortalUser PortalUser
        {
            get
            {
                var rayanUserInfo = (Page) as IPortalUser;
                return rayanUserInfo != null ? rayanUserInfo.PortalUser : null;
            }
            set
            {
                ((Page) as IPortalUser).PortalUser = value;
            }
        }

        public List<string> ValidateTextBoxes()
        {
            List<string> result = new List<string>();
            var rtxt = this.Controls.GetAllControls<NikTextBox>();
            foreach (var item in rtxt)
            {
                item.Text = item.Text.Trim();
                if (item.Text.Length == 0 && item.EmptyTextIsValid)
                    continue;
                if (item.Text.Length < item.MinLength && item.MinLength > 0)
                {
                    result.Add(item.BoxTitle + " " + item.MinLengthMessage.Replace("Nik", item.MinLength.ToString()).Replace("{0}", item.MinLength.ToString()));
                }
                if (item.Text.Length > item.MaxLength && item.MaxLength > 0)
                {
                    result.Add(item.BoxTitle + " " + item.MaxLengthMessage.Replace("Nik", item.MaxLength.ToString()).Replace("{0}", item.MaxLength.ToString()));
                }
            }
            return result;
        }

        #region Routing Part


        public void Issue404()
        {
            OnError404EventHandler();
            ((Page) as INikRoutedPage).Issue404();
        }


        public void IssueContentNotFound()
        {
            OnContentNotFoundEventHandler();
        }

        protected string Language
        {
            get
            {
                return ((Page) as INikRoutedPage).Language;
            }
        }

        protected string Direction
        {
            get
            {
                return ((Page) as INikRoutedPage).Direction;
            }
        }

        protected string Level
        {
            get
            {
                return ((Page) as INikRoutedPage).Level;
            }
        }

        protected string ModuleName
        {
            get
            {
                return ((Page) as INikRoutedPage).ModuleName;
            }
        }

        protected string ModuleParameters
        {
            get
            {
                return ((Page) as INikRoutedPage).ModuleParameters;
            }
        }

        protected int CurrentPortalID
        {
            get
            {
                return ((Page) as INikRoutedPage).CurrentPortalID;
            }
        }

        protected string CurrentPortalPath
        {
            get
            {
                return ((Page) as INikRoutedPage).CurrentPortalPath;
            }
        }

        protected string Domain
        {
            get
            {
                return ((Page) as INikRoutedPage).Domain;
            }
        }

        #endregion Routing Part

        #region "UI Elements"

        protected Button saveButton = new Button();
        protected Panel settingPanel = new Panel();
        private DropDownList ddlskin = new DropDownList();

        #endregion "UI Elements"

        #region Constants

        private const string CONFIG_ATTRIBNAME = "type";
        private const string CONFIG_ATTRIBRANGE = "range";
        private const string PORTAL_ATTRIBUTE = "portal";
        private const string PANELTITLE = "options";

        #endregion Constants

        public delegate void WidgetLoadedEvent();


        public event WidgetLoadedEvent WidgetLoaded;
        //404 event
        public event Error404Event Error404;
        public event ContentNotFoundEvent ContentNotFound;


        protected virtual void OnWidgetLoadedEventHandler()
        {
            if (WidgetLoaded != null)
            {
                WidgetLoaded();
            }
        }

        protected virtual void OnError404EventHandler()
        {
            if (null != Error404)
            {
                Error404();
            }
            //Error404?.Invoke();
        }


        protected virtual void OnContentNotFoundEventHandler()
        {
            IsAnyNotFoundIssued = true;

            if (null != ContentNotFound)
            {
                ContentNotFound();
            }

            //ContentNotFound?.Invoke();
        }

        #region Helper Functions

        private string GetCaption(string configName)
        {
            return GetConfigurationattribute(configName, "caption");
        }

        private Control BuildInt(string configName)
        {
            var ddl = new DropDownList();
            ddl.CssClass = "form-control";
            ddl.ID = configName;
            string[] range = GetConfigurationattribute(configName, "range").Split(',');

            for (int i = int.Parse(range[0]); i <= int.Parse(range[1]); i++)
            {
                ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    ddl.Items.FindByValue(dval).Selected = true;
                }
                catch { }
            }
            return ddl;
        }

        private Control BuildColorCombo(string configName)
        {
            DropDownList ddl = new DropDownList();
            ddl.CssClass = "form-control";
            ddl.ID = configName;
            TextBox txtcolor = new TextBox();
            txtcolor.ID = "c2y" + configName;
            string[] range = {
                "سیاه",
                "آبی",
                "قهوه ای",
                "آبی تیره",
                "خاکستری تیره",
                "سبز تیره",
                "خاکستری",
                "سبز",
                "آبی روشن",
                "خاکستری روشن",
                "سبز روشن",
                "نارنجی",
                "قهوه ای",
                "ارغوانی - زرشکی",
                "قرمز",
                "بنفش",
                "سفید",
                "زرد" };

            string[] range2 = {
                "Black",
                "Blue",
                "Brown",
                "DarkBlue",
                "DarkGray",
                "DarkGreen",
                "Gray",
                "Green",
                "LightBlue",
                "LightGray",
                "LightGreen",
                "Orange",
                "Pink",
                "Purple",
                "Red",
                "Violet",
                "White",
                "Yellow" };
            for (int i = 0; i < range.Length; i++)
            {
                ddl.Items.Add(new ListItem(range[i], range2[i]));
            }
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    ddl.Items.FindByValue(dval).Selected = true;
                    txtcolor.Text = dval;
                    KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), dval, true);
                    txtcolor.BackColor = Color.FromKnownColor(kc);
                }
                catch { }
            }
            return ddl;
        }

        private Control BuildUploader(string configName)
        {
            FileUpload fu = new FileUpload();
            fu.CssClass = "form-control";
            fu.ID = configName;
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    HyperLink h = new HyperLink();
                    h.Text = "Show";
                    h.NavigateUrl = "~/" + dval;
                }
                catch { }
            }
            return fu;
        }

        private Control BuildComboforTableINT(string configName)
        {
            DropDownList ddl = new DropDownList();
            ddl.CssClass = "form-control";
            string table = GetConfigurationattribute(configName, "table");
            string val = GetConfigurationattribute(configName, "value");
            string text = GetConfigurationattribute(configName, "text");
            dbi.FillCombo("Select " + val + "," + text + " From " + table, ddl, text, val, false);
            ddl.ID = configName;
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    ddl.SelectedValue = dval;
                }
                catch { }
            }
            return ddl;
        }

        private Control BuildComboforTableINTbyPortal(string configName)
        {
            DropDownList ddl = new DropDownList();
            ddl.CssClass = "form-control";
            string table = GetConfigurationattribute(configName, "table");
            string val = GetConfigurationattribute(configName, "value");
            string text = GetConfigurationattribute(configName, "text");
            string anotherWhere = GetConfigurationattribute(configName, "wherepart");

            string queryWherePart = "";
            if (!string.IsNullOrWhiteSpace(anotherWhere))
            {
                queryWherePart = " where " + anotherWhere + " and PortalID=" + PortalUser.PortalID;
            }
            else
            {
                queryWherePart = " where PortalID=" + PortalUser.PortalID;
            }

            dbi.FillCombo("Select " + val + "," + text + " From " + table + queryWherePart, ddl, text, val, false);
            ddl.ID = configName;
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    ddl.SelectedValue = dval;
                    ddl.Items.FindByValue(dval).Selected = true;
                }
                catch { }
            }
            return ddl;
        }

        private Control BuildSimpleText(string configName)
        {
            TextBox txtb = new TextBox();
            txtb.CssClass = "form-control";
            txtb.ID = configName;
            string dval = GetConfigurationValue(configName);
            if (!string.IsNullOrEmpty(dval))
            {
                txtb.Text = dval;
            }
            return txtb;
        }

        private Control BuildMultiLineText(string configName)
        {
            TextBox txtb = new TextBox();
            txtb.CssClass = "form-control";
            txtb.ID = configName;
            txtb.TextMode = TextBoxMode.MultiLine;
            txtb.Rows = 5;
            txtb.Columns = 40;
            string dval = GetConfigurationValue(configName);
            if (!string.IsNullOrEmpty(dval))
            {
                txtb.Text = dval;
            }
            return txtb;
        }

        private Control BuildBoolean(string configName)
        {
            CheckBox ck = new CheckBox();
            ck.ID = configName;
            ck.Text = "";
            string dval = GetConfigurationValue(configName);
            if (!string.IsNullOrEmpty(dval) && dval == "true")
            {
                ck.Checked = true;
            }
            else
            {
                ck.Checked = false;
            }
            return ck;
        }


        private Control BuildRadioList(string configName)
        {
            DropDownList ddl = new DropDownList();
            ddl.CssClass = "form-control";
            string range = GetConfigurationattribute(configName, "range");
            var radioItems = range.Split(new[] { "]-[" }, System.StringSplitOptions.None);
            if (1 == radioItems.Length)
                return ddl;
            radioItems[0] = radioItems[0].Replace("[", "");
            radioItems[radioItems.Length - 1] = radioItems[radioItems.Length - 1].Replace("]", "");
            foreach (var item in radioItems)
            {
                ListItem li = new ListItem();
                li.Text = item;
                li.Value = item;
                ddl.Items.Add(li);
            }
            ddl.ID = configName;
            string dval = GetConfigurationValue(configName);
            if (string.Empty != dval)
            {
                try
                {
                    ddl.SelectedValue = dval;
                    //ddl.Items.FindByValue(dval).Selected = true;
                }
                catch { }
            }
            return ddl;
        }


        public string GetConfigurationValue(string configurationName)
        {
            configurationName = configurationName.ToLower();
            if (element.ContainsKey(configurationName))
            {
                return element[configurationName].InnerText;
            }
            else
            {
                return null;
            }
        }

        protected string GetConfigurationattribute(string configurationName, string attributeName)
        {
            if (null == element[configurationName])
            {
                return "";
            }
            return element[configurationName].Attributes[attributeName] == null ? "" : element[configurationName].Attributes[attributeName].Value;
        }

        protected void setConfigurationValue(string configurationName, string nodevalue)
        {
            XmlNode xnode = element[configurationName];
            xnode.InnerText = nodevalue;
            WriteConfigurationValue();
        }

        private void WriteConfigurationValue()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("state");
                foreach (XmlNode xnode in element.Values)
                {
                    writer.WriteStartElement(xnode.Name);
                    foreach (XmlAttribute xattrib in xnode.Attributes)
                    {
                        writer.WriteAttributeString(xattrib.Name, xattrib.Value);
                    }
                    writer.WriteValue(xnode.InnerText);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Flush();
                State = sb.ToString();
            }
        }

        #endregion Helper Functions

        protected IWidgetHost host;

        private string state;
        protected string State
        {
            get
            {
                if (state == null)
                {
                    state = this.host.GetState();
                }
                return state;
            }
            set
            {
                state = value;
            }
        }

        private Dictionary<string, XmlNode> element = new Dictionary<string, XmlNode>();

        protected void saveButton_Click(object sender, EventArgs e)
        {
            (this as IWidget).HideSettings();
        }

        protected virtual void BuildSettingPanel()
        {
            settingPanel.GroupingText = PANELTITLE;
            saveButton.Click += new EventHandler(saveButton_Click);
            saveButton.CssClass = "btn btn-default btn-save-nik";
            saveButton.Text = "save";

            foreach (XmlNode xnode in element.Values)
            {
                string nodetype = GetConfigurationattribute(xnode.Name, CONFIG_ATTRIBNAME);
                string isportalBased = GetConfigurationattribute(xnode.Name, PORTAL_ATTRIBUTE);
                switch (nodetype.ToLower())
                {
                    case "int":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildInt(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "table,int":
                        {
                            if (isportalBased == "true")
                            {
                                settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                                settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                                settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                                settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                                settingPanel.Controls.Add(BuildComboforTableINTbyPortal(xnode.Name));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                            }
                            else
                            {
                                settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                                settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                                settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                                settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                                settingPanel.Controls.Add(BuildComboforTableINT(xnode.Name));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                                settingPanel.Controls.Add(new LiteralControl("</div>"));
                            }
                            break;
                        }
                    case "color":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildColorCombo(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "text":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildSimpleText(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "multitext":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildMultiLineText(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "fileupload":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildUploader(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "bool":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            var ct = BuildBoolean(xnode.Name);
                            settingPanel.Controls.Add(ct);
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label' for='" + ct.UniqueID + "'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "radio":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildBoolean(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    case "radiolist":
                        {
                            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
                            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
                            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>" + GetCaption(xnode.Name) + "</label>"));
                            settingPanel.Controls.Add(BuildRadioList(xnode.Name));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            settingPanel.Controls.Add(new LiteralControl("</div>"));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            var ddl = new DropDownList();
            ddl.ID = "ddskin";
            var skinList = GetBlocks();

            ddl.FillControl(skinList, "Text", "Value", defaultText: "Free Frame", defaultValue: string.Empty);
            ddl.SelectedValue = host.SkinIDofWidget.ToString();
            ddl.CssClass = "form-control";
            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
            settingPanel.Controls.Add(new LiteralControl("<div class='form-group'>"));
            settingPanel.Controls.Add(new LiteralControl("<label class='control-label'>change skin</label>"));
            settingPanel.Controls.Add(ddl);
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Controls.Add(new LiteralControl("<div class='row'>"));
            settingPanel.Controls.Add(new LiteralControl("<div class='col-md-12'>"));
            settingPanel.Controls.Add(new LiteralControl("<hr /'>"));
            settingPanel.Controls.Add(new LiteralControl("<div class='form-group text-center'>"));
            settingPanel.Controls.Add(saveButton);
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Controls.Add(new LiteralControl("</div>"));
            settingPanel.Visible = false;
            this.Controls.AddAt(0, settingPanel);
        }

        private List<ListItem> GetBlocks()
        {
            var list = new List<ListItem>();
            var themes = iThemeServ.GetAll(t => true);
            foreach (var theme in themes)
            {
                var files = new DirectoryInfo(Server.MapPath("~/" + theme.ThemePath)).GetFiles("*.ascx", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var cntrl = LoadControl(Utilities.Utilities.PhysicalToVirtual(file.FullName)) as BlockTemplate;
                    if (cntrl != null)
                    {
                        var skinTitle = cntrl.Controls.GetAllChilds<SkinTitle>().FirstOrDefault();
                        if (skinTitle == null)
                        {
                            continue;
                        }
                        list.Add(new ListItem(theme.Title + " - " + skinTitle.Title, Utilities.Utilities.PhysicalToVirtual(file.FullName).Replace("~/", string.Empty)));
                    }
                }
            }
            return list.OrderBy(t => t.Text).ToList();
        }

        virtual public void HideSettingPanel()
        {
            settingPanel.Visible = false;
        }

        virtual public void ShowSettingPanel()
        {
            settingPanel.Visible = settingPanel.Visible ? false : true;
        }

        protected virtual void BuildProperties()
        {
            if (null == host)
                return;


            string xstate = host.GetState().ToLower();

            var doc = new XmlDocument();
            doc.LoadXml(State.ToLower());
            XmlNode xnod = doc.GetElementsByTagName("state")[0];
            XmlNodeList docNodes = xnod.ChildNodes;
            foreach (XmlNode node in docNodes)
            {
                element.Add(node.Name, node);
            }
        }

        protected virtual void SaveSettingPanel()
        {
            foreach (XmlNode xnode in element.Values)
            {
                string nodetype = GetConfigurationattribute(xnode.Name, CONFIG_ATTRIBNAME);
                string nodevalue = "";
                switch (nodetype.ToLower())
                {
                    case "int":
                        {
                            nodevalue = (settingPanel.FindControl(xnode.Name) as DropDownList).SelectedValue;
                            setConfigurationValue(xnode.Name, nodevalue);
                            break;
                        }
                    case "table,int":
                        {
                            var ddl = settingPanel.FindControl(xnode.Name) as DropDownList;
                            if (ddl.Items.Count > 0)
                            {
                                nodevalue = ddl.SelectedValue;
                                setConfigurationValue(xnode.Name, nodevalue);
                            }
                            else
                            {
                                setConfigurationValue(xnode.Name, "-1");
                            }
                            break;
                        }
                    case "color":
                        {
                            var ddc = settingPanel.FindControl(xnode.Name) as DropDownList;
                            nodevalue = ddc.SelectedValue;
                            setConfigurationValue(xnode.Name, nodevalue);
                            break;
                        }
                    case "fileupload":
                        {
                            string advertiseFile = "";
                            var fu = settingPanel.FindControl(xnode.Name) as FileUpload;
                            if (advertiseFile == "")
                            {
                                break;
                            }
                            nodevalue = advertiseFile;
                            setConfigurationValue(xnode.Name, nodevalue);
                            break;
                        }
                    case "text":
                    case "multitext":
                        {
                            nodevalue = (settingPanel.FindControl(xnode.Name) as TextBox).Text;
                            setConfigurationValue(xnode.Name, nodevalue);
                            break;
                        }
                    case "bool":
                        {
                            nodevalue = (settingPanel.FindControl(xnode.Name) as CheckBox).Checked ? "true" : "false";
                            setConfigurationValue(xnode.Name, nodevalue);
                            break;
                        }
                    case "radiolist":
                        {
                            var ddl = settingPanel.FindControl(xnode.Name) as DropDownList;
                            if (ddl.Items.Count > 0)
                            {
                                nodevalue = ddl.SelectedValue;
                                setConfigurationValue(xnode.Name, nodevalue);
                            }
                            else
                            {
                                setConfigurationValue(xnode.Name, "-1");
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void SaveState()
        {
            var xml = this.State;
            this.host.SaveState(xml);
        }

        public new void Init(IWidgetHost host)
        {
            this.host = host;
        }

        public void ShowSettings()
        {
            ShowSettingPanel();
        }

        public void HideSettings()
        {
            HideSettingPanel();
            SaveSkinChanges();
            SaveSettingPanel();
            SaveState();
            OnWidgetLoadedEventHandler();
        }

        private void SaveSkinChanges()
        {
            var ddl = FindControl("ddskin") as DropDownList;
            if (null != ddl)
            {
                host.SkinIDofWidget = ddl.SelectedValue == "0" ? string.Empty : ddl.SelectedValue;
                host.SkinChanged();
            }
        }

        public void Minimized()
        {
        }

        public void Maximized()
        {
        }

        public void Closed()
        {
        }

        public INotification Notification
        {
            get { return (Page) as INotification; }
        }
    }
}