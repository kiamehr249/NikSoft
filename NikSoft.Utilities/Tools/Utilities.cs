using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NikSoft.Utilities
{
    public class Utilities
    {
        public static bool RemoveItemFile(string ItemFilePath)
        {
            HttpContext context = HttpContext.Current;
            ItemFilePath = context.Server.MapPath("~/" + ItemFilePath);
            if (File.Exists(ItemFilePath))
            {
                File.Delete(ItemFilePath);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static string ResolveUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return originalUrl;
            }

            if (IsAbsolutePath(originalUrl))
            {
                return originalUrl;
            }

            if (!originalUrl.StartsWith("~"))
            {
                return originalUrl;
            }

            int queryStringStartIndex = originalUrl.IndexOf('?');
            if (queryStringStartIndex != -1)
            {
                string queryString = originalUrl.Substring(queryStringStartIndex);
                string baseUrl = originalUrl.Substring(0, queryStringStartIndex);

                return string.Concat(VirtualPathUtility.ToAbsolute(baseUrl), queryString);
            }
            else
            {
                return VirtualPathUtility.ToAbsolute(originalUrl);
            }
        }

        public static string ResolveUrl2(string originalUrl, string level)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                return originalUrl;
            }
            originalUrl = originalUrl.ToLower();
            if (IsAbsolutePath(originalUrl) || originalUrl.StartsWith("#"))
            {
                return originalUrl;
            }
            else if (originalUrl.StartsWith("www."))
            {
                return "http://" + originalUrl;
            }

            if (!originalUrl.StartsWith("~"))
            {
                if (originalUrl == "/")
                {
                    return (HttpContext.Current.Handler as Page).ResolveUrl("~/");
                }
                return (HttpContext.Current.Handler as Page).ResolveUrl("~/" + level + "/" + originalUrl);
            }
            else
            {
                return (HttpContext.Current.Handler as Page).ResolveUrl(originalUrl);
            }
        }

        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (string.IsNullOrEmpty(serverUrl))
            {
                return serverUrl;
            }

            if (IsAbsolutePath(serverUrl))
            {
                return serverUrl;
            }

            string newServerUrl = ResolveUrl(serverUrl);
            Uri result = new Uri(HttpContext.Current.Request.Url, newServerUrl);

            if (!forceHttps)
            {
                return result.ToString();
            }
            else
            {
                return ForceUriToHttps(result).ToString();
            }
        }

        public static string ResolveServerUrl(string serverUrl)
        {
            return ResolveServerUrl(serverUrl, false);
        }

        private static Uri ForceUriToHttps(Uri uri)
        {
            UriBuilder builder = new UriBuilder(uri);
            builder.Scheme = Uri.UriSchemeHttps;
            builder.Port = 443;

            return builder.Uri;
        }

        private static bool IsAbsolutePath(string originalUrl)
        {
            int IndexOfSlashes = originalUrl.IndexOf("://");
            int IndexOfQuestionMarks = originalUrl.IndexOf("?");

            if (IndexOfSlashes > -1 && (IndexOfQuestionMarks < 0 || (IndexOfQuestionMarks > -1 && IndexOfQuestionMarks > IndexOfSlashes)))
            {
                return true;
            }
            return false;
        }

        public static string PhysicalToVirtual(string path)
        {
            if (!path.StartsWith(HttpContext.Current.Request.PhysicalApplicationPath))
            {
                throw new InvalidOperationException("Physical path is not within the application root");
            }
            return "~/" + path.Substring(HttpContext.Current.Request.PhysicalApplicationPath.Length).Replace("\\", "/");
        }

        public static void CopyFile(string source, string dest)
        {
            if (!Directory.Exists(source))
            {
                return;
            }
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            string[] files = Directory.GetFiles(source);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string destFile = Path.Combine(dest, name);
                File.Copy(file, destFile, true);
            }
            var folders = Directory.GetDirectories(source).ToList();
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string newDest = Path.Combine(dest, name);
                CopyFile(folder, newDest);
            }
        }

        public static byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)
        {
            byte[] Ret;
            try
            {
                using (var ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        public static byte[] ConvertFileToByteArray(Stream file)
        {
            byte[] Ret;
            try
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    Ret = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return Ret;
        }

        public static Tuple<byte[], ImageFormat> GetFileByteAndType(FileUpload theFileUpload)
        {
            if (!theFileUpload.HasFile)
            {
                return null;
            }
            if (theFileUpload.PostedFile.ContentLength <= 0)
            {
                return null;
            }
            var fName = theFileUpload.FileName;
            var imagetype = ImageFormat.Jpeg;
            var extension = System.IO.Path.GetExtension(fName);
            if (extension == null)
            {
                return null;
            }
            fName = extension.ToLower();
            if (fName == ".bmp")
            {
                imagetype = ImageFormat.Bmp;
            }
            else if (fName == ".gif")
            {
                imagetype = ImageFormat.Gif;
            }
            else if (fName == ".png")
            {
                imagetype = ImageFormat.Png;
            }
            else if (fName == ".jpg" || fName == ".jpeg")
            {
                imagetype = ImageFormat.Jpeg;
            }
            else
            {
                return null;
            }
            var imgSent = System.Drawing.Image.FromStream(theFileUpload.PostedFile.InputStream);
            var imgBytes = ConvertImageToByteArray(imgSent, imagetype);
            return new Tuple<byte[], ImageFormat>(imgBytes, imagetype);
        }

        public static string RandomStringNumber()
        {
            var r = new Random((int)DateTime.Now.Ticks);
            var pr1 = r.Next(1000, 9999).ToString();
            var pr2 = r.Next(1000, 9999).ToString();
            var prLast = r.Next(1000, 9999).ToString();
            var pr3 = Convert.ToChar(r.Next(65, 90)).ToString();
            var pr4 = Convert.ToChar(r.Next(97, 122)).ToString();
            var pr5 = Convert.ToChar(r.Next(97, 122)).ToString();
            var pr6 = Convert.ToChar(r.Next(65, 90)).ToString();
            return pr4 + pr1 + pr3 + pr6 + pr2 + pr5 + prLast;
        }

        public static string CalculateMD5(string strToEncript)
        {
            var md5 = new MD5CryptoServiceProvider();
            var byteuser = System.Text.Encoding.UTF8.GetBytes(strToEncript);
            var hashedvalue = md5.ComputeHash(byteuser);
            return Convert.ToBase64String(hashedvalue);
        }

        public static int RandomNumber()
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(10000000, 999999999);
        }

        public static int RandomNumber(int start, int end)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(start, end);
        }

        public static bool ImageRatio(int widthRatio, int heightRatio, Stream stream)
        {
            try
            {
                var image = System.Drawing.Image.FromStream(stream);
                var ratio = (double)((double)widthRatio / (double)heightRatio);
                var imageRation = (double)((double)image.Width / (double)image.Height);
                if (imageRation == ratio)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckImageSize(Stream stream, int checkSize)
        {
            try
            {
                var image = System.Drawing.Image.FromStream(stream);
                double jpegByteSize;
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    jpegByteSize = ms.Length / 1024;
                    if (jpegByteSize > checkSize)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ensure directory is exist
        /// </summary>
        /// <param name="path">Full path of directory</param>
        public static bool EnsureDirectory(string path)
        {
            try
            {
                if (!System.IO.Directory.Exists(path))
                {
                    var dir = System.IO.Directory.CreateDirectory(path);
                    if (dir == null)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool UploadFile(HttpPostedFile file, string PathConfigName, ref string FileName, string portalFolderAlias = "")
        {
            if (string.Empty == PathConfigName)
            {
                FileName = "";
                return false;
            }
            System.Web.HttpContext context;
            context = System.Web.HttpContext.Current;
            string VirtualPath = "~/" + WebConfigurationManager.AppSettings[PathConfigName].ToString();

            if (!string.IsNullOrWhiteSpace(portalFolderAlias))
            {
                var DirectoryItems = VirtualPath.Split('/');
                VirtualPath = "";
                foreach (var item in DirectoryItems)
                {
                    VirtualPath += item + "/";
                    if (item.ToLower() == "files")
                    {
                        VirtualPath += portalFolderAlias + "/";
                    }
                }
                VirtualPath = VirtualPath.TrimEnd('/');
            }

            string PathToSave = context.Server.MapPath(VirtualPath);
            var ed = EnsureDirectory(PathToSave);
            if (!ed)
            {
                FileName = "";
                return false;
            }
            FileName = "";
            if (file.ContentLength > 0)
            {
                FileName = file.FileName;
                FileName = FileName.Substring(FileName.LastIndexOf("\\") + 1);
                Random r = new Random();
                string pre = r.Next(1, 1999999999).ToString();
                FileName = pre + FileName;
                try
                {
                    file.SaveAs(PathToSave + "\\" + FileName);
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            FileName = VirtualPath.Replace("~/", "") + "/" + FileName;
            return true;
        }

        public static string GetRealPath(string PathConfigName, string portalFolderAlias = "")
        {
            if (string.Empty == PathConfigName)
            {
                return "";
            }
            System.Web.HttpContext context;
            context = System.Web.HttpContext.Current;
            string VirtualPath = "~/" + WebConfigurationManager.AppSettings[PathConfigName].ToString();

            if (!string.IsNullOrWhiteSpace(portalFolderAlias))
            {
                var allDs = VirtualPath.Split('/');
                VirtualPath = "";
                foreach (var item in allDs)
                {
                    VirtualPath += item + "/";
                    if (item.ToLower() == "files")
                    {
                        VirtualPath += portalFolderAlias + "/";
                    }
                }
                VirtualPath = VirtualPath.TrimEnd('/');
            }

            string PathToSave = context.Server.MapPath(VirtualPath);
            var ed = EnsureDirectory(PathToSave);
            if (!ed)
            {
                return "";
            }

            return VirtualPath;
        }

        public static string FullyQualifiedApplicationPath()
        {
            var appPath = string.Empty;
            var context = HttpContext.Current;
            if (context != null)
            {
                appPath = string.Format("{0}://{1}{2}{3}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                                        context.Request.ApplicationPath);
            }
            if (!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            return appPath;
        }

        public static string DateIsInRange(string fromdate, string todate)
        {
            var errormsg = "";
            PersianDateTime pdFromDate = null;
            PersianDateTime pdToDate = null;
            try
            {
                pdFromDate = PersianDateTime.Parse(fromdate);
            }
            catch
            {
                errormsg += "تاریخ شروع نادرست می باشد\n";
            }
            try
            {
                pdToDate = PersianDateTime.Parse(todate);
            }
            catch
            {
                errormsg += "تاریخ پایان نادرست می باشد\n";
            }

            if (errormsg == "")
            {
                if (null != pdFromDate && null != pdToDate)
                {
                    if (pdToDate < pdFromDate)
                    {
                        errormsg += "تاریخ پایان باید بعد از تاریخ شروع باشد\n";
                    }
                }
            }
            return errormsg;
        }

        public static bool CheckFileFormat(string fileName, params string[] ext)
        {
            var fileExt = Path.GetExtension(fileName).ToLower().Replace(".", "");
            foreach (var item in ext)
            {
                if (item.ToLower() == fileExt)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
            {
                return false;
            }
            var accessRules = accessControlList.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
            {
                return false;
            }
            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                {
                    continue;
                }
                if (rule.AccessControlType == AccessControlType.Allow)
                {
                    writeAllow = true;
                }
                else if (rule.AccessControlType == AccessControlType.Deny)
                {
                    writeDeny = true;
                }
            }
            return writeAllow && !writeDeny;
        }

        public static bool PathIsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void MetaDescription(Page page, string value)
        {
            if (value.IsEmpty())
            {
                return;
            }
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "description";
            meta.Content = value;
            page.Header.Controls.Add(meta);
        }

        public static void MetaKeywords(Page page, string value)
        {
            if (value.IsEmpty())
            {
                return;
            }
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "keywords";
            meta.Content = value;
            page.Header.Controls.Add(meta);
        }


        public static void AddMetaKeys(Page page, string title, string image, string Description, string publisher, string identifier)
        {
            HtmlMeta meta2 = new HtmlMeta();
            meta2.Name = "og:title";
            meta2.Content = title;
            page.Header.Controls.Add(meta2);

            HtmlMeta meta3 = new HtmlMeta();
            meta3.Name = "og:description";
            meta3.Content = Description;
            page.Header.Controls.Add(meta3);

            HtmlMeta meta = new HtmlMeta();
            meta.Name = "og:image";
            meta.Content = image;
            page.Header.Controls.Add(meta);

            HtmlMeta meta_twitter = new HtmlMeta();
            meta_twitter.Name = "twitter:card";
            meta_twitter.Content = image;
            page.Header.Controls.Add(meta_twitter);

            HtmlMeta meta4 = new HtmlMeta();
            meta4.Name = "dc.publisher";
            meta4.Content = publisher;
            page.Header.Controls.Add(meta4);

            HtmlMeta meta5 = new HtmlMeta();
            meta5.Name = "dc.identifier";
            meta5.Content = identifier;
            page.Header.Controls.Add(meta5);
        }



        public static void AddMetaKeys(Page page, string title, string image, string Description, string publisher, string identifier, string canonicaurl)
        {
            HtmlMeta meta2 = new HtmlMeta();
            meta2.Name = "og:title";
            meta2.Content = title;
            page.Header.Controls.Add(meta2);

            HtmlMeta meta3 = new HtmlMeta();
            meta3.Name = "og:description";
            meta3.Content = Description;
            page.Header.Controls.Add(meta3);

            HtmlMeta meta = new HtmlMeta();
            meta.Name = "og:image";
            meta.Content = image;
            page.Header.Controls.Add(meta);

            HtmlMeta meta_url = new HtmlMeta();
            meta_url.Name = "og:url";
            meta_url.Content = canonicaurl;
            page.Header.Controls.Add(meta_url);


            HtmlMeta meta_twitter = new HtmlMeta();
            meta_twitter.Name = "twitter:card";
            meta_twitter.Content = image;
            page.Header.Controls.Add(meta_twitter);

            HtmlMeta meta4 = new HtmlMeta();
            meta4.Name = "dc.publisher";
            meta4.Content = publisher;
            page.Header.Controls.Add(meta4);

            HtmlMeta meta5 = new HtmlMeta();
            meta5.Name = "dc.identifier";
            meta5.Content = identifier;
            page.Header.Controls.Add(meta5);
        }

        public static HtmlLink CreateCSSLink(string cssFilePath, string media)
        {
            var link = new HtmlLink();
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");
            link.Href = cssFilePath;
            if (string.IsNullOrEmpty(media))
            {
                media = "all";
            }
            link.Attributes.Add("media", media);
            return link;
        }

        public static HtmlGenericControl CreateJSLink(string scriptFilePath)
        {
            var script = new HtmlGenericControl { TagName = "script" };
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", scriptFilePath);
            return script;
        }

        public static void CreateCSSLink(Page page, string cssFilePath, string media, bool ExplodesAdmin = false)
        {
            if (ExplodesAdmin)
            {
                var uri = HttpContext.Current.Request.Url.ToString().ToLower();
                if (uri.Contains("panel"))
                    return;
            }

            foreach (var item in page.Header.Controls)
            {
                var c = item as HtmlLink;
                if (null != c)
                {
                    if (cssFilePath == c.Href)
                    {
                        return;
                    }
                }
            }

            var css = new HtmlLink();
            css.Attributes.Add("type", "text/css");
            css.Attributes.Add("rel", "stylesheet");
            css.Href = cssFilePath;
            if (string.IsNullOrEmpty(media))
            {
                media = "all";
            }
            css.Attributes.Add("media", media);
            page.Header.Controls.Add(css);
        }

        public static void CreateJSLink(Page page, string scriptFilePath, bool ExplodesAdmin = false, bool top = false)
        {
            if (ExplodesAdmin)
            {
                var uri = HttpContext.Current.Request.Url.ToString().ToLower();
                if (uri.Contains("panel"))
                    return;
            }

            foreach (var item in page.Header.Controls)
            {
                var c = item as HtmlGenericControl;
                if (null != c && c.Attributes["src"] != null)
                {
                    if (scriptFilePath == c.Attributes["src"])
                    {
                        return;
                    }
                }
            }

            var script = new HtmlGenericControl { TagName = "script" };
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", scriptFilePath);

            Literal jsList;
            if (top)
            {
                jsList = page.FindControl("litScriptTop") as Literal;
            }
            else
            {
                jsList = page.FindControl("litScriptBottom") as Literal;
            }
            if (null != jsList && !jsList.Text.Contains(scriptFilePath))
            {
                jsList.Text += "<script type=\"text/javascript\" src=\"" + scriptFilePath + "\"></script>";
            }
        }

        public static void UnZip(string path, string outputPath)
        {
            if (!EnsureDirectory(HttpContext.Current.Server.MapPath(Path.Combine(outputPath))))
            {
                return;
            }
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(HttpContext.Current.Server.MapPath(path))))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(Path.Combine(outputPath, directoryName)));
                    }
                    if (!fileName.IsEmpty())
                    {
                        using (FileStream streamWriter = File.Create(HttpContext.Current.Server.MapPath(outputPath) + "/" + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static bool IsFarsi(string str)
        {
            foreach (char item in str)
            {
                int i = Convert.ToInt32(item);
                switch (i)
                {
                    case 1632:
                        return true;

                    case 1633:
                        return true;

                    case 1634:
                        return true;

                    case 1635:
                        return true;

                    case 1636:
                        return true;

                    case 1781:
                        return true;

                    case 1638:
                        return true;

                    case 1639:
                        return true;

                    case 1640:
                        return true;

                    case 1641:
                        return true;

                    case 1570:
                        return true;

                    case 1575:
                        return true;

                    case 1576:
                        return true;

                    case 1662:
                        return true;

                    case 1578:
                        return true;

                    case 1579:
                        return true;

                    case 1580:
                        return true;

                    case 1670:
                        return true;

                    case 1581:
                        return true;

                    case 1582:
                        return true;

                    case 1583:
                        return true;

                    case 1584:
                        return true;

                    case 1585:
                        return true;

                    case 1586:
                        return true;

                    case 1688:
                        return true;

                    case 1587:
                        return true;

                    case 1588:
                        return true;

                    case 1589:
                        return true;

                    case 1590:
                        return true;

                    case 1591:
                        return true;

                    case 1592:
                        return true;

                    case 1593:
                        return true;

                    case 1594:
                        return true;

                    case 1601:
                        return true;

                    case 1602:
                        return true;

                    case 1711:
                        return true;

                    case 1705:
                        return true;

                    case 1604:
                        return true;

                    case 1606:
                        return true;

                    case 1605:
                        return true;

                    case 1608:
                        return true;

                    case 1607:
                        return true;

                    case 1740:
                        return true;

                    case 1574:
                        return true;

                    default:
                        break;
                }
            }
            return false;
        }

        public static string ConvertToHtml(string Text, int Length)
        {
            string Result = Regex.Replace(HttpUtility.HtmlDecode(Text), @"<[^>]*>", String.Empty);
            if (Result.Length > Length)
                Result = Result.Substring(0, Length) + "...";
            return Result;
        }

        public static string ConvertToHtml(string Text)
        {
            return Regex.Replace(HttpUtility.HtmlDecode(Text), @"<[^>]*>", String.Empty);
        }

        public static string GetTextLimited(string Text, int Length)
        {
            if (null == Text)
                return "";

            if (Text.Length > Length)
                return Text.Substring(0, Length) + "...";
            return Text;
        }

        public static string GetEnabledImage(bool enabled)
        {
            if (enabled)
            {
                return "<span class='glyphicon glyphicon-ok'></span>";
            }
            else
            {
                return "<span class='glyphicon glyphicon-remove'></span>";
            }
        }

        public static string GetEnabledImageAndText(bool enabled)
        {
            if (enabled)
            {
                return "<span class='glyphicon glyphicon-ok'></span>فعال";
            }
            else
            {
                return "<span class='glyphicon glyphicon-remove'></span>غیر فعال";
            }
        }

        public static DateTime DatePersian(string Date)
        {
            string[] date = Date.Split('/');
            int Year = Convert.ToInt32(date[0]);
            int month = Convert.ToInt32(date[1]);
            int Day = Convert.ToInt32(date[2]);
            PersianCalendar x = new PersianCalendar();
            DateTime DatePersian = x.ToDateTime(Year, month, Day, 0, 0, 0, 0);
            return DatePersian;
        }

        public static string ShowPersianDate(DateTime input, int mode)
        {
            var pd = new PersianDateTime(input);
           return pd.ToString((PersianDateTimeFormat)mode);
        }

        public static string GetTime5Digit()
        {
            DateTime curDT = DateTime.Now;
            string hour = curDT.Hour.ToString();
            string minute = curDT.Minute.ToString();

            hour = hour.PadLeft(2, '0');
            minute = minute.PadLeft(2, '0');
            return hour + ":" + minute;
        }

        public static bool IsExcelFile(string FileName)
        {
            var extension = Path.GetExtension(FileName);
            if (extension != null)
            {
                var fileExtension = extension.ToLower();
                if (fileExtension == ".xlsx" || fileExtension == ".xls") return true;
            }
            return false;
        }

        public static string GetDate(DateTime? dt)
        {
            if (dt == null)
            {
                return "";
            }
            return Extension.GetDate10Digit(dt.Value) + " " + dt.Value.ToString("hh:mm:ss");
        }

        public static string GetDate(DateTime? dt, string timeFormat, string seprator)
        {
            if (dt == null)
            {
                return "";
            }
            return Extension.GetDate10Digit(dt.Value) + (seprator.IsEmpty() ? " " : seprator) + dt.Value.ToString(timeFormat.IsEmpty() ? "hh:mm:ss" : timeFormat);
        }

        public static string RandomString(Random r, int length, bool onlyAlphabet = false, string chars = null)
        {
            if (chars.IsEmpty())
            {
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                if (onlyAlphabet)
                {
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                }
            }
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        public static List<T> GetAllControls<T>(ControlCollection controls) where T : Control
        {
            var list = new List<T>();
            foreach (Control item in controls)
            {
                if (item is T)
                {
                    list.Add((T)item);
                }
                list.AddRange(GetAllControls<T>(item.Controls));
            }
            return list;
        }

        public static List<T> GetAllControls<T>(ControlCollection controls, string contain) where T : Control
        {
            var list = new List<T>();
            foreach (Control item in controls)
            {
                if (item is T && item.ID.Contains(contain))
                {
                    list.Add((T)item);
                }
                list.AddRange(GetAllControls<T>(item.Controls, contain));
            }
            return list;
        }

        public static string CheckIfDateIsInRange(string fromdate, string todate)
        {
            var errormsg = "";
            int DateFrom2 = 0, DateTo2 = 0;
            string DateFrom, DateTo;
            if ("0" != fromdate)
            {
                if (!CheckFormatDateFrom(fromdate))
                {
                    errormsg += "تاریخ شروع نادرست می باشد\n";
                }
            }
            if ("0" != todate)
            {
                if (!CheckFormatDateFrom(todate))
                {
                    errormsg += "تاریخ پایان نادرست می باشد\n";
                }
            }
            if ("" == errormsg)
            {
                if (fromdate != "0" && todate != "0")
                {
                    DateFrom = fromdate.Replace("/", "");
                    DateFrom2 = int.Parse(DateFrom);
                    DateTo = todate.Replace("/", "");
                    DateTo2 = int.Parse(DateTo);
                    if (DateTo2 < DateFrom2)
                    {
                        errormsg += "تاریخ پایان باید بعد از تاریخ شروع باشد\n";
                    }
                }
            }
            return errormsg;
        }

        public static bool CheckFormatDateFrom(string inDate)
        {
            var dateshow = inDate.Split('/');
            if (3 != dateshow.Count())
            {
                return false;
            }
            else
            {
                var testnumber1 = ParseandGetint(dateshow[0]);
                var testnumber2 = ParseandGetint(dateshow[1]);
                var testnumber3 = ParseandGetint(dateshow[2]);
                if (!testnumber1.Item2 || !testnumber2.Item2 || !testnumber3.Item2)
                {
                    return false;
                }
                if (testnumber1.Item1 <= 1000 || testnumber1.Item1 >= 9999)
                {
                    return false;
                }
                if (testnumber2.Item1 < 1 || testnumber2.Item1 > 12)
                {
                    return false;
                }
                if (testnumber3.Item1 < 1 || testnumber3.Item1 > 31)
                {
                    return false;
                }
                return true;
            }
        }

        public static Tuple<int, bool> ParseandGetint(string inputparam)
        {
            var temp = -1;
            var parseResult = false;
            parseResult = int.TryParse(inputparam, out temp);
            return new Tuple<int, bool>(temp, parseResult);
        }
    }
}