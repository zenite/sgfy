using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Net;

namespace javascripttest
{
    public class cookieHelper
    {
        public bool check(CookieContainer cookieContainer,string url)
        {
            CookieCollection cookies = new CookieCollection();
            cookies = cookieContainer.GetCookies(new Uri(url));
            bool result = false;
            foreach (var item in cookies)
            {
                //if((Cookie)item)
            }
            return false;
        }

        public object GetSsid(ref AccountModel account, string url)
        {
            Uri uri;
            CookieCollection cookies;
            Exception exception2;
            account.hasMulti = "0";
            try
            {

                uri = new Uri(url + "/index.php");

                cookies = account.cookies.GetCookies(uri);
                string str = "";
                IEnumerator enumerator = cookies.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        Cookie current = (Cookie)enumerator.Current;
                        if (current.Name.Contains("game_user_") & !str.Contains("game_user_"))
                        {
                            str = str + current.ToString() + ";";
                            account.user_id = current.Name.Substring(current.Name.LastIndexOf("_")+1);
                            if (string.IsNullOrWhiteSpace(account.user_id))
                            { }
                            account.hasMulti = "1";
                        }
                        else
                        {
                            if ((current.Name == "broadcast_ids") & !str.Contains("broadcast_ids"))
                            {
                                str = str + current.ToString() + ";";
                                continue;
                            }
                            if ((current.Name == "PHPSESSID") & !str.Contains("PHPSESSID"))
                            {
                                str = str + current.ToString() + ";";
                                continue;
                            }
                            if ((current.Name == "__ut") & !str.Contains("__ut"))
                            {
                                str = str + current.ToString() + ";";
                                continue;
                            }
                            if (current.Name.Contains("logout_") & !str.Contains("logout_"))
                            {
                                str = str + current.ToString() + ";";
                                continue;
                            }
                            if ((current.Name == "KL_UTMP") & !str.Contains("KL_UTMP"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "KL_SSO") & !str.Contains("KL_SSO"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "KL_PERSON") & !str.Contains("KL_PERSON"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "equiplist_order") & !str.Contains("equiplist_order"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "xl_validUname") & !str.Contains("xl_validUname"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "VERIFY_KEY") & !str.Contains("VERIFY_KEY"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "xlwg_usrid") & !str.Contains("xlwg_usrid"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "_loginedSvrs") & !str.Contains("_loginedSvrs"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "aqLevel") & !str.Contains("aqLevel"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "aqLevel") & !str.Contains("aqLevel"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "_xltj") & !str.Contains("_xltj"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "__xltjbr") & !str.Contains("__xltjbr"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "logout_") & !str.Contains("logout_"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "_de") & !str.Contains("_de"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "first_login_flag") & !str.Contains("first_login_flag"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "societyguester") & !str.Contains("societyguester"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "id") & !str.Contains("id"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "xnsid") & !str.Contains("xnsid"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "loginfrom") & !str.Contains("loginfrom"))
                            {
                                str = str + current.ToString() + ";";
                            }
                            if ((current.Name == "_urm_") & !str.Contains("_urm_"))
                            {
                                str = str + current.ToString() + ";";
                            }
                        }
                    }
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    account.cookieStr = str;                    
                }
            }
            catch (Exception exception)
            {
                exception2 = exception;             
            }            
            return string.Empty;
        }



        /// <summary>
        /// 转换cookie
        /// </summary>
        /// <param name="account"></param>
        public void setCookies(ref AccountModel account)
        {
            CookieContainer cookies = new CookieContainer();
            if (!string.IsNullOrEmpty(account.cookieStr))
            {
                Regex regex = new Regex("(?<_name>.*?)=(?<_cook>.*?);", RegexOptions.None);
                MatchCollection matchs = regex.Matches(account.cookieStr.Replace(" ",""));
              
                    foreach (Match match in matchs)
                    {                        
                        try
                        {
                            if (!match.Groups["_name"].Value.Contains("ut"))
                            {
                                Cookie cookie = new Cookie(match.Groups["_name"].Value, match.Groups["_cook"].Value)
                                {
                                    Expires = DateTime.Now.AddDays(125.0)
                                };
                                MainLogic.InternetSetCookie("http://" + account.Server_url + "/", cookie.Name, cookie.Value);
                                cookies.Add(new Uri("http://" + account.Server_url + "/"), cookie);
                            }
                           
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }                         
            account.cookies = cookies;
        }


        /// <summary>
        /// 重设cookie
        /// </summary>
        public void ClearIECookie()
        {
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "broadcast_ids", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "PHPSESSID", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "KL_UTMP", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "KL_SSO", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "KL_PERSON", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "equiplist_order", "");
            MainLogic.InternetSetCookie("http://" + Constant.Server_Url + "/", "loginfrom", "");
        }



        public string StoreAccount(ref AccountModel account)
        {

            return string.Empty;
        }
    }
}
