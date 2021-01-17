using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.UILayer;
using NikSoft.Utilities;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NikSoft.Web.Modules.BaseModules.EngineBase
{
    public partial class PanelModuleLoader : WidgetUIContainer, IEngineContainer
    {
        private string controlPath = "";
        protected string PageHeadingText = "";
        private bool isCrudModule;
        private bool userCreateAndUpdatePermission, userReadAndDeletePermission;
        private bool userViewPermission;
        private string pureModuleKey = "";
        private INikContentManage widget = null;
        private const string PAGELOCK = "~/modules/BaseModules/EngineBase/LockPage.ascx";
        private bool isModuleOk = true;

        public INikModuleService iNikModuleService { get; set; }
        public IUserRoleModuleService iUserRoleModuleService { get; set; }
        public IUserTypeGroupService iUserTypeGroupServ { get; set; }


        //What those this property really do?
        public string ModuleHeader
        {
            get { return PageHeadingText; }
            set { PageHeadingText = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (null == PortalUser)
            {
                isModuleOk = false;
                RedirectTo("~/home/login");
                return;
            }

            showHideButtons(false, false, false, false, false);

            if (ModuleName.IsEmpty() || ModuleName.Length <= 3 || ModuleName.Length > 31)
            {
                isModuleOk = false;
                ph1.Controls.Add(LoadControl("~/Modules/BaseModules/EngineBase/LockPage.ascx"));
                return;
            }
            var requiredModule = iNikModuleService.GetAll(x => x.ModuleKey == ModuleName, x => new { x.ModuleKey, x.ModuleFile, x.IsXMLBase, x.Title, x.LoginRequired, x.SecondTitle });

            if (1 != requiredModule.Count())
            {
                showHideButtons(false, false, false, false, false);
                isModuleOk = false;
                ph1.Controls.Add(LoadControl("~/Modules/BaseModules/EngineBase/LockPage.ascx"));
                return;
            }

            var moduleInfo = requiredModule.First();

            PageHeadingText = moduleInfo.Title;
            Page.Title = moduleInfo.Title;

            var othertest = ModuleName.Substring(0, 2).ToLower();
            pureModuleKey = ModuleName.Substring(3);
            isCrudModule = othertest == "cu" || othertest == "rd" || othertest == "vd" || othertest == "rp";

            if (!moduleInfo.LoginRequired)
            {
                //go ahaed
            }
            else if (isCrudModule && ReadCrudPermission(ModuleName.Substring(3)))
            {
                if (!SetAdministrativePageImages(othertest))
                {
                    return;
                }
            }
            else if (!ReadNormalPermission())
            {
                ph1.Controls.Add(LoadControl(PAGELOCK));
                return;
            }

            controlPath = "~/" + moduleInfo.ModuleFile;
            if (moduleInfo.IsXMLBase)
            {
                if (isCrudModule)
                {
                    switch (othertest)
                    {
                        case "rd":
                            {
                                controlPath = "~/modules/xmldata/rd_ui.ascx";
                                break;
                            }
                        case "cu":
                            {
                                controlPath = "~/modules/xmldata/cu_ui.ascx";
                                break;
                            }
                        case "vd":
                            {
                                controlPath = "~/modules/xmldata/rd_ui.ascx";
                                break;
                            }
                        case "rp":
                            {
                                controlPath = "~/modules/xmldata/rd_ui.ascx";
                                break;
                            }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(controlPath.Trim()))
            {
                return;
            }
            try
            {
                var ctl = LoadControl(controlPath);
                widget = ctl as INikContentManage;
                if (widget != null)
                {
                    widget.InitHost(this);
                }
                else
                {
                    showHideButtons(false, false, false, false, false);
                }
                ph1.Controls.Add(ctl);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }

        protected void showHideButtons(bool ListBtn, bool NewItemBtn, bool searchBtn, bool edit, bool remove)
        {
            imgBtnList.Visible = ListBtn;
            imgBtnNew.Visible = NewItemBtn;
            searchbtn.Visible = searchBtn;
            be.Visible = edit;
            bd.Visible = remove;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!isModuleOk)
            {
                return;
            }
            if (widget == null)
            {
                return;
            }
            if (!(widget is NikUserControl))
            {
                be.Visible = false;
                bd.Visible = false;
                return;
            }
            if (isCrudModule)
            {
                be.Visible = hasCUPermissionCurrentUser() && (widget as NikUserControl).ShowFunctionButton;
                bd.Visible = hasRDPermissionCurrentUser() && (widget as NikUserControl).ShowFunctionButton;
            }
            else
            {
                showHideButtons(false, false, false, false, false);
                //be.Visible = (widget as NikUserControl).ShowFunctionButton;
                //bd.Visible = (widget as NikUserControl).ShowFunctionButton;
            }
        }

        private bool SetAdministrativePageImages(string othertest)
        {

            switch (othertest)
            {
                case "rd":
                    {
                        showHideButtons(false, true, true, true, true);
                        if (!hasCUPermissionCurrentUser())
                        {
                            imgBtnNew.Visible = false;
                        }
                        else
                        {
                            //imgBtnNew.ImageUrl = "~/images/rayan/NewEditUp.jpg";
                        }
                        if (!hasRDPermissionCurrentUser())
                        {
                            ph1.Controls.Add(LoadControl(PAGELOCK));
                            return false;
                        }
                        imgBtnList.CssClass = "btn btn-default";
                    }
                    break;
                case "cu":
                    {
                        showHideButtons(true, false, false, false, false);
                        if (ModuleParameters.Contains("|"))
                        {
                            imgBtnNew.Text = "<i class='fa fa-pencil-square-o' aria-hidden='true'></i>ویرایش";
                            imgBtnNew.CssClass = "btn btn-warning";
                            if (!hasRDPermissionCurrentUser())
                            {
                                gotoList();
                                return false;
                            }
                        }
                        else
                        {
                            if (!hasCUPermissionCurrentUser())
                            {
                                gotoList();
                                return false;
                            }
                            imgBtnNew.Text = "<i class='fa fa-pencil-square-o' aria-hidden='true'></i>جدید";
                            imgBtnNew.CssClass = "btn btn-success";
                        }
                    }
                    break;
                default:
                    {
                        searchbtn.Visible = false;
                    }
                    break;
            }
            Page.Title = "نیک سافت-" + PageHeadingText;
            return true;
        }

        private bool ReadCrudPermission(string pureModuleKey)
        {
            string mname1 = "rd_" + pureModuleKey;
            string mname2 = "cu_" + pureModuleKey;
            var userGroups = iUserServ.GetUserGroup(PortalUser.ID);

            var moduleIDs = iNikModuleService.Entity.Where(rm => rm.ModuleKey == mname1 || rm.ModuleKey == mname2).Select(x => x.ID).ToList();
            var permissions = iUserRoleModuleService.Entity.Where(x => moduleIDs.Contains(x.NikModuleID) && userGroups.Contains(x.UserTypeGroupID)).Select(x => new { x.NikModuleID, x.PermissionType }).ToList();

            foreach (var item in permissions)
            {
                if (item.PermissionType == UserGroupPermissionType.CreateAndUpdate)
                {
                    userCreateAndUpdatePermission = true;
                    continue;
                }
                if (item.PermissionType == UserGroupPermissionType.ReadAndDelete)
                {
                    userReadAndDeletePermission = true;
                    continue;
                }
            }
            return userCreateAndUpdatePermission || userReadAndDeletePermission;
        }

        private bool ReadNormalPermission()
        {
            var userGroups = iUserServ.GetUserGroup(PortalUser.ID);

            var moduleIDs = iNikModuleService.GetAll(rm => rm.ModuleKey == ModuleName, x => x.ID);
            var permissions = iUserRoleModuleService.GetAll(x => moduleIDs.Contains(x.NikModuleID) && userGroups.Contains(x.UserTypeGroupID), x => new { x.NikModuleID, x.PermissionType });

            foreach (var item in permissions.Where(item => item.PermissionType == UserGroupPermissionType.None))
            {
                userViewPermission = true;
                break;
            }
            return userViewPermission;
        }

        protected void imgBtnNew_Click(object sender, EventArgs e)
        {
            if (!HandleNewEvent())
            {
                gotonewItem();
            }
        }

        public void gotonewItem()
        {
            if (pureModuleKey.Length >= KEY_MINIMUM_LENGTH && hasCUPermissionCurrentUser())
            {
                RedirectTo("~/panel/" + "cu_" + pureModuleKey + "/" + AddNewKey);
                return;
            }
        }

        protected void imgBtnList_Click(object sender, ImageClickEventArgs e)
        {
            gotoList();
        }

        private const int KEY_MINIMUM_LENGTH = 3;

        public void gotoList()
        {
            if (!HandleListEvent())
            {
                if (pureModuleKey.Length >= KEY_MINIMUM_LENGTH)
                {
                    RedirectTo("~/panel/" + "rd_" + pureModuleKey + "/view");
                    return;
                }
            }
        }

        public void gotoEditURI(string ItemID)
        {
            if (pureModuleKey.Length >= KEY_MINIMUM_LENGTH && hasCUPermissionCurrentUser())
            {
                RedirectTo("~/panel/" + "cu_" + pureModuleKey + "/" + EditItemKey + "|" + ItemID);
                return;
            }
        }

        public string getEditURI(string iID)
        {
            if (pureModuleKey.Length >= KEY_MINIMUM_LENGTH && hasCUPermissionCurrentUser())
            {
                return "~/panel/" + "cu_" + pureModuleKey + "/" + EditItemKey + "|" + iID;
            }
            return "";
        }

        public bool hasCUPermissionCurrentUser()
        {
            return userCreateAndUpdatePermission;
        }

        public bool hasRDPermissionCurrentUser()
        {
            return userReadAndDeletePermission;
        }

        public LinkButton btnDelete
        {
            get { return bd; }
        }

        private string _editKey = "edit";
        public string EditItemKey
        {
            get
            {
                return _editKey;
            }

            set
            {
                _editKey = value;
            }
        }


        private string _newKey = "add";
        public string AddNewKey
        {
            get
            {
                return _newKey;
            }

            set
            {
                _newKey = value;
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string del1;
            try
            {
                if (null != Request.Form["ch1"])
                {
                    del1 = Request.Form["ch1"].Split(',')[0];
                    gotoEditURI(del1);
                }
                else
                {
                    Notification.SetErrorMessage("حداقل یک آیتم را انتخاب کنید.");
                }
            }
            catch
            {
                Notification.SetErrorMessage("حداقل یک آیتم را انتخاب کنید.");
            }
            finally
            {
            }
        }

        public event NewItemEventHandler NewItem;

        private bool HandleNewEvent()
        {
            if (null != NewItem)
            {
                NewItem();
                return true;
            }
            return false;
        }

        public event GotoListEventHandler GotoList;

        private bool HandleListEvent()
        {
            if (null != GotoList)
            {
                GotoList();
                return true;
            }
            return false;
        }

        protected void imgBtnList_Click1(object sender, EventArgs e)
        {
            gotoList();
        }


        public void ShowSearch()
        {
            searchbtn.Visible = true;
        }

        public void HideSearch()
        {
            searchbtn.Visible = false;
        }

    }
}