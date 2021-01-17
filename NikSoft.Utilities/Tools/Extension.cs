using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace NikSoft.Utilities
{
    public static class Extension
    {
        public static string GetFriendlyURL(this string instance)
        {
            var t = instance.Replace(" ", "_").Replace("(", "").Replace(")", "").Replace("`", "")
                .Replace("/", "").Replace("\\", "").Replace("|", "").Replace("}", "").Replace("'", "")
                .Replace("*", "").Replace("&", "").Replace(")", "").Replace("{", "")
                .Replace("#", "").Replace("!", "").Replace("~", "").Replace("[", "").Replace("]", "")
                .Replace("%", "").Replace("$", "").Replace(".", "").Replace(",", "").Replace("،", "").Replace("-", "")
                .Replace(":", "").Trim().Trim('_');
            return t.Length <= 200 ? t : t.Substring(0, 199);
        }



        public static string GetFarsiText(this string instance)
        {
            return instance.Replace("ي", "ی").Replace("ك", "ک").Replace("ي", "ی");
        }

        public static string InputEncode(this string st)
        {
            string temp = st;
            string temp2 = st.ToLower();
            temp2 = temp2.Replace("'", "").Replace("`", "");
            temp2 = temp2.Replace(";", "");
            temp2 = temp2.Replace("#", "");
            temp2 = temp2.Replace("\"", "");
            temp2 = temp2.Replace("%", "");
            temp2 = temp2.Replace("--", "");
            temp2 = temp2.Replace("=", "");
            temp2 = temp2.Replace(" ", "");
            temp2 = temp2.Replace("+", "");
            temp2 = temp2.Replace("(", "");
            temp2 = temp2.Replace(")", "");
            temp2 = temp2.Replace("update ", "");
            temp2 = temp2.Replace("delete ", "");
            temp2 = temp2.Replace("drop ", "");
            temp2 = temp2.Replace("alter ", "");
            temp2 = temp2.Replace("dbo ", " ");
            //st = st.Replace("go", " ");
            temp2 = temp2.Replace("select", "");
            temp2 = temp2.Replace("update", "");
            temp2 = temp2.Replace("insert", "");
            temp2 = temp2.Replace("delete", "");
            temp2 = temp2.Replace("drop", "");
            temp2 = temp2.Replace("alter", "");
            //temp2 = temp2.Replace("xp", " ");
            temp2 = temp2.Replace("dbo", "");
            //st = st.Replace("sa", " ");
            temp2 = temp2.Replace("schema", "");

            if (temp2 == st.ToLower())
            {
                return temp;
            }
            return temp2;
        }

        private static bool ObjectIsNumeric(object Obj)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Obj), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool IsGuid(this string str)
        {
            bool isGuid = false;
            if (str.IsEmpty() == false)
            {
                Guid guid = Guid.Empty;
                isGuid = Guid.TryParse(str.Trim(), out guid);
            }
            return isGuid;
        }

        public static bool IsNumeric(this object str)
        {
            return ObjectIsNumeric(str);
        }

        public static bool IsNumeric(this string str)
        {
            if (str.IsEmpty() == true)
            {
                return false;
            }
            return ObjectIsNumeric(str.Trim());
        }

        public static bool IsInteger(this string str)
        {
            if (str.IsEmpty() == true)
            {
                return false;
            }
            bool isNum;
            int retNum;
            isNum = int.TryParse(Convert.ToString(str), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static int GetPositiveInteger(this string str)
        {
            if (str.IsEmpty() == true)
            {
                return -1;
            }
            int retNum = -1;
            int.TryParse(Convert.ToString(str), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return retNum;
        }

        public static bool IsEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                return true;
            }
            return false;
        }

        public static int ToInt32(this string str)
        {
            if (str.IsEmpty())
            {
                throw new ArgumentNullException();
            }
            if (!str.IsNumeric())
            {
                throw new InvalidCastException();
            }
            return Convert.ToInt32(str.Trim());
        }

        public static string ToPersianNumeric(this string str)
        {
            var loopCount = str.Length;
            var result = "";
            for (int i = 0; i < loopCount; i++)
            {
                if (str[i] >= 48 && str[i] <= 57)
                {
                    if (str[i] == 48)
                    {
                        result += "۰";
                    }
                    else if (str[i] == 49)
                    {
                        result += "۱";
                    }
                    else if (str[i] == 50)
                    {
                        result += "۲";
                    }
                    else if (str[i] == 51)
                    {
                        result += "۳";
                    }
                    else if (str[i] == 52)
                    {
                        result += "۴";
                    }
                    else if (str[i] == 53)
                    {
                        result += "۵";
                    }
                    else if (str[i] == 54)
                    {
                        result += "۶";
                    }
                    else if (str[i] == 55)
                    {
                        result += "۷";
                    }
                    else if (str[i] == 56)
                    {
                        result += "۸";
                    }
                    else if (str[i] == 57)
                    {
                        result += "۹";
                    }
                }
                else
                {
                    result += str[i];
                }
            }

            return result;
        }

        public static int HexToInt32(this string str)
        {
            if (str.IsEmpty())
            {
                throw new ArgumentNullException();
            }

            bool isNum;
            int retNum;
            isNum = int.TryParse(str, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out retNum);
            if (!isNum)
                throw new ArgumentNullException();

            return retNum;
        }

        public static string ScrubHtml(this string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        public static string ScrubHtml(this string value, int maxLength)
        {
            var result = value.ScrubHtml();
            if (result.Length > maxLength)
            {
                result = result.Substring(0, maxLength - 1);
            }
            return result;
        }

        public static void FillControl(this ListControl lstControl, IList datalist, string title, string value, bool addDefault = true, bool selectDefault = true, string defaultText = "", string defaultValue = "")
        {
            lstControl.DataSource = datalist;
            lstControl.DataTextField = title;
            lstControl.DataValueField = value;
            lstControl.DataBind();

            if (addDefault)
            {
                var li = new ListItem(NikSoft.Portal_Select, "0");
                if (!defaultText.IsEmpty())
                {
                    li.Text = defaultText;
                }
                if (!defaultValue.IsEmpty())
                {
                    li.Value = defaultValue;
                }
                lstControl.Items.Insert(0, li);
                if (selectDefault)
                {
                    lstControl.Items[0].Selected = true;
                }
            }
        }

        public static List<ListItem> GetSelectedItems(this ListControl lst)
        {
            return lst.Items.OfType<ListItem>().Where(i => i.Selected).ToList();
        }

        public static bool IsValidEmail(this string emailaddress)
        {
            try
            {
                var m = new MailAddress(emailaddress);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ClearForm(this Control uc)
        {
            var u = uc.Controls.OfType<TextBox>();
            foreach (var item in u)
            {
                item.Text = string.Empty;
            }
            var s = uc.Controls.OfType<DropDownList>();
            foreach (var item in s)
            {
                item.ClearSelection();
                item.SelectedIndex = 0;
            }
            var d = uc.Controls.OfType<RadioButtonList>();
            foreach (var item in d)
            {
                item.SelectedIndex = -1;
            }
            var c = uc.Controls.OfType<CheckBox>();
            foreach (var item in c)
            {
                item.Checked = false;
            }
            var l = uc.Controls.OfType<RadioButton>();
            foreach (var item in l)
            {
                item.Checked = false;
            }
            var w = uc.Controls.OfType<CheckBoxList>();
            foreach (var item in w)
            {
                item.SelectedIndex = -1;
            }
            foreach (Control item in uc.Controls)
            {
                ClearForm(item);
            }
        }

        public static List<T> GetAllControls<T>(this ControlCollection controls) where T : Control
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

        public static List<T> GetAllChilds<T>(this ControlCollection controls)
        {
            var list = new List<T>();
            foreach (var item in controls)
            {
                if (item is T)
                {
                    list.Add((T)item);
                }
                list.AddRange(GetAllChilds<T>((item as Control).Controls));
            }
            return list;
        }

        public static List<Control> GetAllControls<T1, T2>(this ControlCollection controls)
            where T1 : Control
            where T2 : Control
        {
            var list = new List<Control>();
            foreach (Control item in controls)
            {
                if (item is T1 || item is T2)
                {
                    list.Add(item);
                }
                list.AddRange(GetAllControls<T1, T2>(item.Controls));
            }
            return list;
        }

        public static List<Control> GetAllControls<T1, T2, T3>(this ControlCollection controls)
            where T1 : Control
            where T2 : Control
            where T3 : Control
        {
            var list = new List<Control>();
            foreach (Control item in controls)
            {
                if (item is T1 || item is T2 || item is T3)
                {
                    list.Add(item);
                }
                list.AddRange(GetAllControls<T1, T2>(item.Controls));
            }
            return list;
        }

        public static List<Control> GetAllControls<T1, T2, T3, T4>(this ControlCollection controls)
            where T1 : Control
            where T2 : Control
            where T3 : Control
            where T4 : Control
        {
            var list = new List<Control>();
            foreach (Control item in controls)
            {
                if (item is T1 || item is T2 || item is T3 || item is T4)
                {
                    list.Add(item);
                }
                list.AddRange(GetAllControls<T1, T2>(item.Controls));
            }
            return list;
        }

        public static List<Control> GetAllControls<T1, T2, T3, T4, T5>(this ControlCollection controls)
            where T1 : Control
            where T2 : Control
            where T3 : Control
            where T4 : Control
            where T5 : Control
        {
            var list = new List<Control>();
            foreach (Control item in controls)
            {
                if (item is T1 || item is T2 || item is T3 || item is T4 || item is T5)
                {
                    list.Add(item);
                }
                list.AddRange(GetAllControls<T1, T2, T3, T4, T5>(item.Controls));
            }
            return list;
        }

        public static List<Control> GetAllControls<T1, T2, T3, T4, T5, T6>(this ControlCollection controls)
            where T1 : Control
            where T2 : Control
            where T3 : Control
            where T4 : Control
            where T5 : Control
            where T6 : Control
        {
            var list = new List<Control>();
            foreach (Control item in controls)
            {
                if (item is T1 || item is T2 || item is T3 || item is T4 || item is T5 || item is T6)
                {
                    list.Add(item);
                }
                list.AddRange(GetAllControls<T1, T2, T3, T4, T5, T6>(item.Controls));
            }
            return list;
        }

        public static List<T> GetAllControls<T>(this ControlCollection controls, string name) where T : Control
        {
            var list = new List<T>();
            foreach (Control item in controls)
            {
                if (item is T && item.ID.Contains(name))
                {
                    list.Add((T)item);
                }
                list.AddRange(GetAllControls<T>(item.Controls, name));
            }
            return list;
        }

        public static string CalculateMD5(this string strToEncript)
        {
            var md5 = new MD5CryptoServiceProvider();
            var byteuser = System.Text.Encoding.UTF8.GetBytes(strToEncript);
            var hashedvalue = md5.ComputeHash(byteuser);
            return Convert.ToBase64String(hashedvalue);
        }

        public static string CalculateMD5String(this string strToEncript)
        {
            var md5 = new MD5CryptoServiceProvider();
            var byteuser = System.Text.Encoding.UTF8.GetBytes(strToEncript);
            var hashedvalue = md5.ComputeHash(byteuser);
            return BitConverter.ToString(hashedvalue).Replace("-", "");
        }

        public static bool StringIsNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr.Trim(), @"^\d+$");
        }

        public static bool StringContainNumber(this string checkStr)
        {
            return Regex.IsMatch(checkStr.Trim(), @"^\d+$");
        }

        public static int ToInteger(this TimeSpan time)
        {
            return int.Parse(time.Hours.ToString() + time.Minutes.ToString().PadLeft(2, '0') + time.Seconds.ToString().PadLeft(2, '0'));
        }

        public static short ToShort(this TimeSpan time)
        {
            return short.Parse(time.Hours.ToString() + time.Minutes.ToString().PadLeft(2, '0'));
        }

        public static string ToHHMM(this TimeSpan time)
        {
            return time.ToString("hh\\:mm");
        }

        public static string ToHHMMSS(this TimeSpan time)
        {
            return time.ToString("hh\\:mm\\:ss");
        }

        public static int GetDropDownValue(this DropDownList ddl, NameValueCollection form)
        {
            if (null == form[ddl.UniqueID])
            {
                return 0;
            }
            var data = form[ddl.UniqueID];
            if (!data.IsNumeric())
            {
                return 0;
            }
            return data.ToInt32();
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static bool StringIsEnglish(this string checkStr, bool space = false)
        {
            if (space)
            {
                return Regex.IsMatch(checkStr.Trim(), @"^[a-zA-Z0-9 ]*$");
            }
            else
            {
                return Regex.IsMatch(checkStr.Trim(), @"^[a-zA-Z0-9]*$");
            }
        }

        public static string ConvertToXmlString<T>(this T item) where T : class
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            string xml;

            using (MemoryStream m = new MemoryStream())
            {
                ser.Serialize(m, item);
                m.Position = 0;
                xml = new StreamReader(m).ReadToEnd();
            }
            return xml;
        }

        public static string GetFullNameSpace(this Type type)
        {
            var classFullName = type.FullName;
            var assemblyName = type.Module.Name;
            return classFullName + "," + assemblyName.Replace(".dll", "");
        }

        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src);
        }

        public static bool HasProperty(this object src, string propertyName)
        {
            return src.GetType().GetProperty(propertyName) != null;
        }

        public static void SetPropValue(this object src, string propertyName, object value)
        {
            var pInfo = src.GetType().GetProperty(propertyName);
            pInfo.SetValue(src, Convert.ChangeType(value, pInfo.PropertyType), null);
        }

        public static Tuple<int, bool> ParseandGetint(this string inputparam)
        {
            var temp = -1;
            var parseResult = false;
            parseResult = int.TryParse(inputparam, out temp);
            return new Tuple<int, bool>(temp, parseResult);
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

        public static Tuple<long, bool> ParseandGetLong(this string inputparam)
        {
            long temp = -1;
            var parseResult = false;
            parseResult = long.TryParse(inputparam, out temp);
            return new Tuple<long, bool>(temp, parseResult);
        }

        public static string GetDate10Digit(this DateTime dateTimeVar)
        {
            var pcal = new PersianCalendar();
            var year = pcal.GetYear(dateTimeVar).ToString(CultureInfo.InvariantCulture);
            var month = pcal.GetMonth(dateTimeVar).ToString(CultureInfo.InvariantCulture);
            var day = pcal.GetDayOfMonth(dateTimeVar).ToString(CultureInfo.InvariantCulture);
            day = day.PadLeft(2, '0');
            month = month.PadLeft(2, '0');
            return year + "/" + month + "/" + day;
        }

        public static bool IsPersianDateTime(this string str)
        {
            PersianDateTime dt;
            return PersianDateTime.TryParse(str, out dt);
        }

        public static bool IsValidIranianNationalCode(this string input)
        {
            input = input.PadLeft(10, '0');

            if (!Regex.IsMatch(input, @"^(?!(\d)\1{9})\d{10}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(9, 1));
            var sum = Enumerable.Range(0, 9)
                .Select(x => Convert.ToInt32(input.Substring(x, 1)) * (10 - x))
                .Sum() % 11;

            return sum < 2 && check == sum || sum >= 2 && check + sum == 11;
        }

        public static string GetPostBackControlId(this Page page)
        {
            if (!page.IsPostBack)
            {
                return string.Empty;
            }
            Control control = null;
            string controlName = page.Request.Params["__EVENTTARGET"];
            if (controlName.IsEmpty() == false)
            {
                control = page.FindControl(controlName);
            }
            else
            {
                string controlId;
                Control foundControl;
                foreach (string ctl in page.Request.Form)
                {
                    if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                    {
                        controlId = ctl.Substring(0, ctl.Length - 2);
                        foundControl = page.FindControl(controlId);
                    }
                    else
                    {
                        foundControl = page.FindControl(ctl);
                    }
                    if (!(foundControl is IButtonControl))
                    {
                        continue;
                    }
                    control = foundControl;
                    break;
                }
            }
            return control == null ? string.Empty : control.ID;
        }

        public static bool IsValidIranianLegalCode(this string input)
        {
            //input has 11 digits that all of them are not equal
            if (!Regex.IsMatch(input, @"^(?!(\d)\1{10})\d{11}$"))
                return false;

            var check = Convert.ToInt32(input.Substring(10, 1));
            int dec = Convert.ToInt32(input.Substring(9, 1)) + 2;
            int[] Coef = new int[10] { 29, 27, 23, 19, 17, 29, 27, 23, 19, 17 };

            var sum = Enumerable.Range(0, 10)
                          .Select(x => (Convert.ToInt32(input.Substring(x, 1)) + dec) * Coef[x])
                          .Sum() % 11;

            return sum == check;
        }


    }
}