using NikSoft.Utilities;
using System;
using System.Globalization;
using System.Web;

namespace NikSoft.Model
{
    public class SingleSignOnService : INikSingleSignOn
    {

        private const string SEPARATOR = "Nik";
        private const string USERID = "rui";
        private const string COOKIEIDENTIFIER = "rtcookie";
        private const string CONST1 = "K_Benito_29@&";
        private const string CONST2 = "nbv8K658@h723bBSt598";
        //this must be exactly 16 ascii chars
        private const string INIT_VECTOR = "@1z0@3k$e5E6G7*J";
        private const int KEY_SIZE = 256;
        private const int PASSWORD_ITERATIONS = 4;
        private const string HASH_ALGORITHM = "SHA1";
        private const int NORMAL_LOGIN_TIME = 110;

        private const string atid = "atid";


        public bool Authenticate(ISingleSignOnService serviceInterface, string username, string pass, bool rememberMe)
        {
            if (null == serviceInterface)
            {
                return false;
            }
            var userObj = serviceInterface.GetUserByUserName(username);
            if (null == userObj)
            {
                return false;
            }
            var pPhrase = serviceInterface.GetPasswordHash(pass, userObj.UserName, userObj.LoginKey, userObj.RandomID);
            if (pPhrase != userObj.Password)
            {
                return false;
            }
            var theCookie = new HttpCookie(COOKIEIDENTIFIER);
            var exDate = DateTime.Now.AddMinutes(NORMAL_LOGIN_TIME);
            if (rememberMe)
            {
                theCookie.Expires = DateTime.Now.AddDays(2);
            }
            theCookie.Expires = exDate;
            string str1 = userObj.ID.ToString(CultureInfo.InvariantCulture);
            string str2 = userObj.UserName.CalculateMD5();
            string str3 = serviceInterface.GenerateHash(userObj, CONST1, CONST2);

            string allResult = str1 + SEPARATOR + str2 + SEPARATOR + str3 + SEPARATOR + exDate.ToString(new CultureInfo("en-US")) + SEPARATOR;

            string cipherText = NikRijndael.Encrypt(allResult, CONST1, CONST2, HASH_ALGORITHM, PASSWORD_ITERATIONS, INIT_VECTOR, KEY_SIZE);

            theCookie[USERID] = cipherText;
            HttpContext.Current.Response.Cookies.Add(theCookie);
            serviceInterface.UpdateUserStats(userObj.ID);
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceInterface"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool AuthenticateForAdmin(ISingleSignOnService serviceInterface, string username, int adminId)
        {
            if (null == serviceInterface)
            {
                return false;
            }
            var userObj = serviceInterface.GetUserByUserName(username);
            if (null == userObj)
            {
                return false;
            }
            var pPhrase = userObj.Password;
            var theCookie = new HttpCookie(COOKIEIDENTIFIER);
            var exDate = DateTime.Now.AddMinutes(NORMAL_LOGIN_TIME);

            theCookie.Expires = exDate;
            string str1 = userObj.ID.ToString(CultureInfo.InvariantCulture);
            string str2 = userObj.UserName.CalculateMD5();
            string str3 = serviceInterface.GenerateHash(userObj, CONST1, CONST2);

            string allResult = str1 + SEPARATOR + str2 + SEPARATOR + str3 + SEPARATOR + exDate.ToString(new CultureInfo("en-US")) + SEPARATOR;

            string cipherText = NikRijndael.Encrypt(allResult, CONST1, CONST2, HASH_ALGORITHM, PASSWORD_ITERATIONS, INIT_VECTOR, KEY_SIZE);

            theCookie[USERID] = cipherText;


            var theCookie2 = new HttpCookie(atid);
            theCookie2.Expires = exDate;
            theCookie2[USERID] = adminId.ToString();

            HttpContext.Current.Response.Cookies.Add(theCookie);
            HttpContext.Current.Response.Cookies.Add(theCookie2);
            serviceInterface.UpdateUserStats(userObj.ID);
            return true;
        }

        public bool Authenticate2(ISingleSignOnService serviceInterface, string oldusername, string pass, bool rememberMe)
        {
            if (null == serviceInterface)
            {
                return false;
            }
            var userObj = serviceInterface.GetUserByOldUserName(oldusername);
            if (null == userObj)
            {
                return false;
            }
            var pPhrase = serviceInterface.GetPasswordHash(pass, userObj.UserName, userObj.LoginKey, userObj.RandomID);
            if (pPhrase != userObj.Password)
            {
                return false;
            }

            return true;
        }



        public NikPortalUser AuthenticateFromContext(ISingleSignOnService serviceInterface)
        {
            try
            {
                if (null == HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER])
                {
                    return null;
                }
                var userCookie = HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER];
                if (null == userCookie[USERID])
                {
                    return null;
                }
                var cypherText = userCookie[USERID];
                string ourInfos = NikRijndael.Decrypt(cypherText, CONST1, CONST2, HASH_ALGORITHM, PASSWORD_ITERATIONS, INIT_VECTOR, KEY_SIZE);

                string[] allResult = ourInfos.Split(new[] { SEPARATOR }, StringSplitOptions.None);
                if (5 != allResult.Length)
                {
                    return null;
                }

                string ID = allResult[0];
                string str2 = allResult[1];
                string str3 = allResult[2];
                //HttpContext.Current.Response.Write("<br />" + allResult[3] + "<br />");
                var exDate_str4 = DateTime.Parse(allResult[3], new CultureInfo("en-US"));
                string str5 = allResult[4];
                if (exDate_str4 < DateTime.Parse(DateTime.Now.ToString(new CultureInfo("en-US")), new CultureInfo("en-US")))
                {
                    return null;
                }

                if ((exDate_str4 - DateTime.Parse(DateTime.Now.ToString(new CultureInfo("en-US")), new CultureInfo("en-US"))).TotalMinutes < (NORMAL_LOGIN_TIME + 1))
                {
                    exDate_str4 = DateTime.Now.AddMinutes(NORMAL_LOGIN_TIME);
                }
                else
                {
                    exDate_str4 = DateTime.Now.AddDays(2);
                }


                if (string.IsNullOrEmpty(ID))
                {
                    return null;
                }
                int uID;
                if (!int.TryParse(ID, out uID))
                {
                    return null;
                }
                var theUser = serviceInterface.GetUserbyID(uID);
                if (null == theUser)
                {
                    return null;
                }

                var pPhrase = serviceInterface.GenerateHash(theUser, CONST1, CONST2);
                var unameMd5 = theUser.UserName.CalculateMD5();
                if (str2 == unameMd5 && str3 == pPhrase)
                {
                    var rp = new NikPortalUser()
                    {
                        ID = theUser.ID,
                        PortalID = theUser.PortalID,
                        PortalFolderPath = theUser.PortalFolderPath,
                        EmailOfUser = theUser.EmailOfUser,
                        FullNameOfUser = theUser.NameOfUser
                    };
                    var theCookie = new HttpCookie(COOKIEIDENTIFIER);
                    theCookie.Expires = exDate_str4;
                    string str1 = theUser.ID.ToString(CultureInfo.InvariantCulture);
                    str2 = theUser.UserName.CalculateMD5();
                    str3 = serviceInterface.GenerateHash(theUser, CONST1, CONST2);
                    string finalResult = str1 + SEPARATOR + str2 + SEPARATOR + str3 + SEPARATOR + exDate_str4.ToString(new CultureInfo("en-US")) + SEPARATOR; ;

                    string cipherText = NikRijndael.Encrypt(finalResult, CONST1, CONST2, HASH_ALGORITHM, PASSWORD_ITERATIONS, INIT_VECTOR, KEY_SIZE);

                    theCookie[USERID] = cipherText;
                    HttpContext.Current.Response.Cookies.Add(theCookie);
                    return rp;
                }
                SignOut2();
            }
            catch (Exception ex)
            {
                //HttpContext.Current.Response.Write(ex.Message);
                //if (null != ex.InnerException) {
                //	HttpContext.Current.Response.Write("<br />1: " + ex.InnerException.Message);
                //}
            }
            return null;
        }

        public void SignOutUser()
        {
            if (null != HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER])
            {
                var myCookie = new HttpCookie(COOKIEIDENTIFIER);
                myCookie.Expires = DateTime.Now.AddDays(-10000d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }

        public bool ChangePass(ISingleSignOnService serviceInterface, string oldPass, string newpass)
        {
            var theCookie = HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER];
            if (null == theCookie)
            {
                return false;
            }
            var userIDInCookie = theCookie[USERID];
            if (string.IsNullOrEmpty(userIDInCookie))
            {
                return false;
            }
            var userID = 0;
            if (!int.TryParse(userIDInCookie, out userID))
            {
                return false;
            }
            var userobj = serviceInterface.GetUserbyID(userID);
            return null != userobj && oldPass != newpass && (1 == serviceInterface.ChangeUserPassword(userobj.ID, oldPass, newpass, newpass));
        }

        public static void SignOut2()
        {
            if (null != HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER])
            {
                var myCookie = new HttpCookie(COOKIEIDENTIFIER);
                myCookie.Expires = DateTime.Now.AddDays(-10000d);

                HttpContext.Current.Response.Cookies.Add(myCookie);


                var myCookie2 = new HttpCookie(atid);
                myCookie2.Expires = DateTime.Now.AddDays(-10000d);

                HttpContext.Current.Response.Cookies.Add(myCookie2);

            }
        }


        public static void SignOut3()
        {
            var myCookie = new HttpCookie(COOKIEIDENTIFIER);
            myCookie.Expires = DateTime.Now.AddDays(-10000d);

            //public const string BASKET_COOKIENAME = "toranjv4";
            var shopCookie = new HttpCookie("toranjv5");
            shopCookie.Expires = DateTime.Now.AddDays(-10000d);


            var myCookie2 = new HttpCookie(atid);
            myCookie2.Expires = DateTime.Now.AddDays(-10000d);

            HttpContext.Current.Response.Cookies.Add(myCookie);
            HttpContext.Current.Response.Cookies.Add(myCookie2);
            HttpContext.Current.Response.Cookies.Add(shopCookie);
        }



        public static void SignOut()
        {
            if (null != HttpContext.Current.Request.Cookies[COOKIEIDENTIFIER])
            {

                var myCookie = new HttpCookie(COOKIEIDENTIFIER);
                myCookie.Expires = DateTime.Now.AddDays(-10000d);


                //var myCookie2 = new HttpCookie(COOKIEIDENTIFIER);
                //myCookie2.Expires = DateTime.Now.AddDays(-10000d);

                var myCookie2 = new HttpCookie(atid);
                myCookie2.Expires = DateTime.Now.AddDays(-10000d);

                HttpContext.Current.Response.Cookies.Add(myCookie);
                HttpContext.Current.Response.Cookies.Add(myCookie2);
                //HttpContext.Current.Response.Cookies.Add(shopCookie);
            }
        }
    }
}