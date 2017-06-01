using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing;
using Microsoft.VisualBasic;
using javascripttest.entity;
using System.Threading.Tasks;

namespace javascripttest
{
    public class UrlCommand
    {
        public event CommonDelegate.reLogin relogin;
        public void getTokenCookie(out AccountModel account)
        {
            AccountModel account1 = new AccountModel();
            string url = "http://login.kunlun.com/?act=index.captcha&r=" + new Random(); 
            //string host="login.kunlun.com";
            //string refer="http://sg.kunlun.com/";
            account1.cookies = new CookieContainer();
            urlPost(url, ref account1, "", "");
            account = account1;
        }

        public object getImage(ref AccountModel account, string hostUlr,string referenceUlr)
        {
            string imageUlr = "http://login.kunlun.com/?act=index.captcha&r=0.7004946304950863";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(imageUlr);
            CookieContainer cookies = new CookieContainer();
            account.cookies = cookies;
            req.CookieContainer = account.cookies;
            req.Accept = "image/webp,image/*,*/*;q=0.8";
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8");            
            req.Referer = "http://sg.kunlun.com/";
            req.Host = "login.kunlun.com";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";

            string path = string.Empty;
            try
            {
                path = "TmpBmp.bmp";
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    Stream imageStream = response.GetResponseStream();
                    account.cookies.Add(response.Cookies);
                    Bitmap streamImage = new Bitmap(imageStream);
                    if (File.Exists(path)) File.Delete(path);
                    streamImage.Save(path);
                    imageStream.Dispose();
                }
            }
            catch (Exception ex)
            {                
                return null;
            }
            return path;
        }


        public string  urlPost(string url,ref AccountModel account,string hostUrl,string referenceUrl,bool extreHtml=false)
        {
            string html=string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = account.cookies;
            req.Method = "Post";            
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            req.KeepAlive = true;            
            req.Referer = referenceUrl;
            req.Host = hostUrl;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                account.cookies.Add(response.Cookies);
                if (extreHtml)
                {
                    account.extreHtml = new StreamReader(response.GetResponseStream(),Encoding.UTF8).ReadToEnd();
                }   
                html=new StreamReader(response.GetResponseStream(),Encoding.UTF8).ReadToEnd();
            }
            return html;
        }
     
    
        public string PostForm(string url,string hostUrl,string refernceUrl,string OriginUrl,ref AccountModel account,string form_string)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = account.cookies;
            req.Method = "Post";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.Headers.Add("Accept-Encoding", "gzip, deflate");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            req.KeepAlive = true;
            req.Referer = refernceUrl;
            req.Host = hostUrl;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Headers.Add("Origin", OriginUrl);
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(form_string);
            req.ContentLength = bytes.Length;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();                
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                account.cookies.Add(response.Cookies);
                try
                {
                    return getGb2312(response.ResponseUri.ToString());
                }
                catch (Exception ex)
                {
 
                }
                finally { }
            }
            return string.Empty;
        }

        public string PostForm(string url, string hostUrl, string refernceUrl, AccountModel account, string form_string)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = account.cookies;
            req.Method = "Post";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            req.Headers.Add("Accept-Encoding", "gzip, deflate");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.8");
            req.KeepAlive = true;
            req.Referer = refernceUrl;
            req.Host = hostUrl;
            req.ContentType = "application/x-www-form-urlencoded";
            //req.Headers.Add("Origin", OriginUrl);
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(form_string);
            req.ContentLength = bytes.Length;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            Stream requestStream = req.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                account.cookies.Add(response.Cookies);
                try
                {
                    string result= getGb2312(response.ResponseUri.ToString());
                    if(result.ToLower().Contains("login.logout"))
                        relogin(account);
                    return result;
                }
                catch (Exception ex)
                {

                }
                finally { }
            }
            return string.Empty;
        }

        public static string getGb2312(string big5)
        {
            string str = "";
            if ((big5 != null) && (big5 != string.Empty))
            {
                big5 = big5.Trim();
                str = Strings.StrConv(big5, VbStrConv.SimplifiedChinese, 0);
            }
            return str;
        }

        public  string Html_get(string url,string referUrl,AccountModel account)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = account.cookies;
                request.Referer = referUrl;
                request.Method = "GET";                
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322)";
                response = (HttpWebResponse)request.GetResponse();
                try
                {                  
                    string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    return getGb2312(str5);
                }
                finally
                {
                    try
                    {
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {                
            }
            return string.Empty;
        }

        public string Html_get1(string url, string host, AccountModel account)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = account.cookies;
                request.Host = host;
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322)";
                response = (HttpWebResponse)request.GetResponse();
                try
                {
                    string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    return getGb2312(str5);
                }
                finally
                {
                    try
                    {
                        if (response != null)
                        {
                            response.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
        }
    }
}
