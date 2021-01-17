using NikSoft.Model;
using NikSoft.NikModel;
using NikSoft.Services;
using NikSoft.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace NikSoft.WebService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PortalWebService : System.Web.Services.WebService
    {
        private readonly JavaScriptSerializer serializer = new JavaScriptSerializer();
        private NikPortalUser portalUser;
        private IUserService iUserServ;

        public PortalWebService()
        {
            SetCurrentUser();
        }

        public void SetCurrentUser()
        {
            portalUser = null;
            iUserServ = ObjectFactory.GetInstance<IUserService>();
            var sbl = new SingleSignOnService();
            var theUser = sbl.AuthenticateFromContext(iUserServ);
            if (null != theUser && theUser.ID > 0)
            {
                portalUser = theUser;
                return;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetGroupByUserType(int userTypeID, int userID)
        {
            if (null == portalUser)
            {
                return string.Empty;
            }
            var userGroup = iUserServ.GetUserGroup(userID);
            var iUserTypeGroupServ = ObjectFactory.GetInstance<IUserTypeGroupService>();
            var data = iUserTypeGroupServ.GetAll(t => t.UserTypeID == userTypeID && !userGroup.Contains(t.ID), t => new JsonResult { ID = t.ID, Title = t.Title });
            if (true)
            {
                data.Insert(0, new JsonResult { Title = "انتخاب کنید", ID = 0 });
            }
            return serializer.Serialize(data);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetUserGroup(int userTypeID)
        {
            if (null == portalUser)
            {
                return string.Empty;
            }

            var iUserTypeGroupServ = ObjectFactory.GetInstance<IUserTypeGroupService>();
            var data = iUserTypeGroupServ.GetAll(t => t.UserTypeID == userTypeID, t => new JsonResult { ID = t.ID, Title = t.Title });
            if (true)
            {
                data.Insert(0, new JsonResult { Title = "انتخاب کنید", ID = 0 });
            }
            return serializer.Serialize(data);
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string LoadFileList(string path)
        {
            var iPortalServ = ObjectFactory.GetInstance<IPortalService>();
            if (null == portalUser)
            {
                return string.Empty;
            }
            if (portalUser.PortalID != 1)
            {
                var portal = iPortalServ.Find(t => t.ID == portalUser.PortalID);
                if (null == portal)
                {
                    return string.Empty;
                }
                else
                {
                }
            }
            var list = new List<FileManager>();
            string topPath = "files";
            bool isRoot = false;
            if (topPath.ToLower() == path.ToLower())
            {
                isRoot = true;
            }
            var pPath = "";
            var p = iPortalServ.Find(t => t.ID == portalUser.PortalID);

            if (null == p)
            {
                return string.Empty;
            }

            if (!path.Contains("files/" + p.AliasFolder))
            {
                pPath = "~/" + path + "/" + p.AliasFolder;
            }
            else
            {
                //do nothing, we have already does the work
                pPath = "~/" + path;
            }

            try
            {
                pPath = Server.MapPath(pPath);
            }
            catch { return string.Empty; }
            if (!Directory.Exists(pPath))
            {
                return string.Empty;
            }
            list.AddRange(new DirectoryInfo(pPath).GetDirectories().Select(t => new FileManager(t)).ToList());
            list.AddRange(new DirectoryInfo(pPath).GetFiles().Select(t => new FileManager(t)).ToList());
            if (!isRoot)
            {
                list.Insert(0, new FileManager("up", pPath.Substring(0, path.LastIndexOf("/")), path.Substring(0, path.LastIndexOf("/"))));
                list.Insert(0, new FileManager("Root", topPath, Server.MapPath("~/" + topPath)));
            }
            return serializer.Serialize(list);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool RemoveFiles(List<string> items)
        {
            if (null == portalUser)
            {
                return false;
            }
            foreach (var item in items)
            {
                var path = "~/" + item;
                path = Server.MapPath(path);
                if (Utilities.Utilities.PathIsDirectory(path))
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int AddMenuAccess(int MenuID, int UserGroupID)
        {
            var iMenuAccessSev = ObjectFactory.GetInstance<IUserRoleMenuService>();
            var menuItem = iMenuAccessSev.Find(x => x.NikMenuID == MenuID && x.UserTypeGroupID == UserGroupID);
            if (menuItem != null)
            {
                return 0;
            }
            else
            {
                var newMenu = iMenuAccessSev.Create();
                newMenu.NikMenuID = MenuID;
                newMenu.UserTypeGroupID = UserGroupID;
                newMenu.PermissionType = UserGroupPermissionType.MenuView;
                iMenuAccessSev.Add(newMenu);
                iMenuAccessSev.SaveChanges();
                return 1;
            }

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int RemoveMenuAccess(int MenuID, int UserGroupID)
        {
            var iMenuAccessSev = ObjectFactory.GetInstance<IUserRoleMenuService>();
            var menuItem = iMenuAccessSev.Find(x => x.NikMenuID == MenuID && x.UserTypeGroupID == UserGroupID);
            if (menuItem == null)
            {
                return 0;
            }
            else
            {
                iMenuAccessSev.Remove(menuItem);
                iMenuAccessSev.SaveChanges();
                return 1;
            }

        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int CreateNewFolder(string folderName, string path)
        {
            var iPortalServ = ObjectFactory.GetInstance<IPortalService>();
            if (null == portalUser)
            {
                return 2;
            }

            var pPath = "";
            var p = iPortalServ.Find(t => t.ID == portalUser.PortalID);

            if (null == p)
            {
                return 2;
            }

            if (!path.Contains("files/" + p.AliasFolder))
            {
                pPath = "~/" + path + "/" + p.AliasFolder;
            }
            else
            {
                //do nothing, we have already does the work
                pPath = "~/" + path;
            }

            path = Server.MapPath(pPath);
            if (Directory.Exists(Path.Combine(path, folderName)))
            {
                return 1;
            }
            try
            {
                Directory.CreateDirectory(Path.Combine(path, folderName));
                return 0;
            }
            catch
            {
                return 2;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool PasteFiles(List<string> items, string path, string action)
        {
            if (null == portalUser)
            {
                return false;
            }
            path = Server.MapPath("~/" + path);
            if (action.ToLower() == "copy".ToLower())
            {
                foreach (var item in items)
                {
                    var pPath = Server.MapPath("~/" + item);
                    if (Utilities.Utilities.PathIsDirectory(Server.MapPath("~/" + item)))
                    {
                        CopyFolder(pPath, path);
                    }
                    else
                    {
                        if (File.Exists(pPath))
                        {
                            File.Copy(pPath, path + "\\" + Path.GetFileName(item));
                        }
                    }
                }
            }
            else if (action.ToLower() == "cut".ToLower())
            {
                foreach (var item in items)
                {
                    var pPath = Server.MapPath("~/" + item);
                    if (Utilities.Utilities.PathIsDirectory(pPath))
                    {
                        if (Directory.Exists(pPath))
                        {
                            Directory.Move(pPath, path);
                        }
                    }
                    else
                    {
                        if (File.Exists(pPath))
                        {
                            File.Move(pPath, path + "\\" + Path.GetFileName(item));
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UploadFile()
        {
            var iPortalServ = ObjectFactory.GetInstance<IPortalService>();

            if (null == portalUser)
            {
                return string.Empty;
            }
            try
            {
                var request = HttpContext.Current.Request;
                var path = request.Headers["Path"];
                if (path.IsEmpty())
                {
                    return "file path not found";
                }

                var pPath = "";
                var p = iPortalServ.Find(t => t.ID == portalUser.PortalID);

                if (null == p)
                {
                    return "";
                }

                if (!path.Contains("files/" + p.AliasFolder))
                {
                    pPath = "~/" + path + "/" + p.AliasFolder;
                }
                else
                {
                    //do nothing, we have already does the work
                    pPath = "~/" + path;
                }

                path = Server.MapPath(pPath);
                var files = request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    files[i].SaveAs(path + "/" + files[i].FileName);
                }
                return "done";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void CopyFolder(string source, string destination)
        {
            if (null == portalUser)
                return;

            if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
            {
                destination += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(destination)) Directory.CreateDirectory(destination);
            string[] files = Directory.GetFileSystemEntries(source);
            foreach (var element in files)
            {
                if (Directory.Exists(element))
                {
                    CopyFolder(element, destination + Path.GetFileName(element));
                }
                else
                {
                    File.Copy(element, destination + Path.GetFileName(element), true);
                }
            }
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveWidgetPosition(string result)
        {
            if (portalUser == null)
            {
                return string.Empty;
            }
            //TODO : check for url or more security
            if (result.IsEmpty())
            {
                return string.Empty;
            }
            try
            {
                var res = result;
                var iWidgetServ = ObjectFactory.GetInstance<IWidgetService>();
                var iWidgetDefinitionServ = ObjectFactory.GetInstance<IWidgetDefinitionService>();
                string[] eachcolumn = res.Split('_');
                int wID = 0;
                int panelNo = 0;
                foreach (var item in eachcolumn)
                {
                    if (item.Contains('-'))
                    {
                        string[] panelID = item.Split('-');
                        panelNo = int.Parse(panelID[0]);
                        if (panelID.Count() > 1 && !string.IsNullOrWhiteSpace(panelID[1]))
                        {
                            string[] itemIDs = panelID[1].Split(',');
                            int i = 0;
                            foreach (var widgetID in itemIDs)
                            {
                                wID = int.Parse(widgetID);
                                i++;
                                var w = iWidgetServ.Find(x => x.ID == wID);
                                if (null == w)
                                {
                                    continue;
                                }
                                w.PanelNo = panelNo;
                                w.OrderNo = i;
                            }
                        }
                    }
                }
                iWidgetServ.SaveChanges();
            }
            catch
            {
                return "error";
            }
            return "done";
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SaveNewWidget(int widgetID, int panelNo, int pageID)
        {
            if (portalUser == null)
            {
                return string.Empty;
            }
            //TODO : check for url or more security
            try
            {
                var iWidgetServ = ObjectFactory.GetInstance<IWidgetService>();
                var iWidgetDefinitionServ = ObjectFactory.GetInstance<IWidgetDefinitionService>();
                var widgetprops = iWidgetDefinitionServ.Find(x => x.ID == widgetID);
                var w = iWidgetServ.Create();
                w.TemplateID = pageID;
                w.WidgetDefinitionID = widgetprops.ID;
                w.WidgetSkinPath = string.Empty;
                w.PanelNo = panelNo;
                var newOrder = 1;
                var lastOrder = iWidgetServ.GetAll(x => x.TemplateID == pageID && x.PanelNo == panelNo);
                if (lastOrder.Count > 0)
                {
                    newOrder = lastOrder.Max(x => x.OrderNo) + 1;
                }
                w.OrderNo = newOrder;
                w.Expanded = true;
                w.State = widgetprops.DefaultState;
                w.ShowTitle = false;
                w.Title = widgetprops.Title;
                w.TitleLink = "";
                iWidgetServ.Add(w);
                iWidgetServ.SaveChanges();
            }
            catch
            {
                return "error";
            }
            return "done";
        }

        [WebMethod(EnableSession = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string RemoveWidget(int widgetID)
        {
            if (portalUser == null)
            {
                return string.Empty;
            }
            //TODO : check for url or more security
            try
            {
                var iWidgetServ = ObjectFactory.GetInstance<IWidgetService>();
                var w = iWidgetServ.Find(x => x.ID == widgetID);
                iWidgetServ.Remove(w);
                iWidgetServ.SaveChanges();
            }
            catch
            {
                return "error";
            }
            return "done";
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void setSomeIds(int id1, int id2, bool yes)
        {
            //id1 is groupID, id2 is menuID
            if (null == portalUser)
            {
                SerializeIt(new { Yes = false });
                return;
            }

            var iUserTypeGroupRoleMenuServ = ObjectFactory.GetInstance<IUserRoleMenuService>();
            var iUserGroupModuleServ = ObjectFactory.GetInstance<IUserRoleModuleService>();

            //single perm on menupermission
            if (!iUserGroupModuleServ.SinglePermission("menupermission", portalUser.ID, UserGroupPermissionType.None))
            {
                //SerializeIt(new { Res = false, Message = "چنین سرویسی ارائه نمی‌شود" });
                SerializeIt(new { Yes = false });
                return;
            }


            //TODO: check if current user has enough permissions for doing this?

            var per = iUserTypeGroupRoleMenuServ.Find(t => t.UserTypeGroupID == id1 && t.NikMenuID == id2);
            if (yes)
            {
                if (per == null)
                {
                    per = iUserTypeGroupRoleMenuServ.Create();
                    per.NikMenuID = id2;
                    per.UserTypeGroupID = id1;
                    per.PermissionType = UserGroupPermissionType.MenuView;
                    iUserTypeGroupRoleMenuServ.Add(per);
                    iUserTypeGroupRoleMenuServ.SaveChanges();
                }
            }
            else
            {
                if (per != null)
                {
                    iUserTypeGroupRoleMenuServ.Remove(per);
                    iUserTypeGroupRoleMenuServ.SaveChanges();
                }
            }

            SerializeIt(new { Yes = true });
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetModulesByCategory(int id)
        {
            var iNikModuleServ = ObjectFactory.GetInstance<INikModuleService>();
            var iNikModuleDefinitionServ = ObjectFactory.GetInstance<INikModuleDefinitionService>();
            var modules = iNikModuleServ.GetAll(x => x.ModuleDefinitionID == id && x.Editable, y => new JsonResult { ID = y.ID, Title = y.Title + "-" + y.ModuleKey }).ToList();
            modules.Insert(0, new JsonResult { ID = 0, Title = "انتخاب کنید" });
            return serializer.Serialize(modules);
        }



        private void SerializeIt(object obj)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            HttpContext.Current.Response.Write(serializer.Serialize(obj));
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}