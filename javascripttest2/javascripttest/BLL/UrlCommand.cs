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
using System.Net.Cache;

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

        //public object getImage(ref AccountModel account, string hostUlr,string referenceUlr)
        //{
        //    string imageUlr = "http://login.kunlun.com/?act=index.captcha&r=0.7004946304950863";
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(imageUlr);
        //    CookieContainer cookies = new CookieContainer();
        //    account.cookies = cookies;
        //    req.CookieContainer = account.cookies;
        //    req.Accept = "image/webp,image/*,*/*;q=0.8";
        //    req.Headers.Add("Accept-Language", "en-US,en;q=0.8");            
        //    req.Referer = "http://sg.kunlun.com/";
        //    req.Host = "login.kunlun.com";
        //    req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";

        //    string path = string.Empty;
        //    try
        //    {
        //        path = "TmpBmp.bmp";
        //        using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
        //        {
        //            Stream imageStream = response.GetResponseStream();
        //            account.cookies.Add(response.Cookies);
        //            Bitmap streamImage = new Bitmap(imageStream);
        //            if (File.Exists(path)) File.Delete(path);
        //            streamImage.Save(path);
        //            imageStream.Dispose();
        //        }
        //    }
        //    catch (Exception ex)
        //    {                
        //        return null;
        //    }
        //    return path;
        //}

        public byte[] getImage(ref AccountModel account, string hostUlr, string referenceUlr)
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
                    //Bitmap streamImage = new Bitmap(imageStream);
                    return StreamToBytes(imageStream);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public byte[] getnewImage( AccountModel account, string hostUlr, string referenceUlr)
        {
            string imageUlr = "http://login.kunlun.com/?act=index.captcha&r=0.7004946304950863";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(imageUlr);
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
                    //account.cookies.Add(response.Cookies);
                    //Bitmap streamImage = new Bitmap(imageStream);
                    return StreamToBytes(imageStream);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public byte[] StreamToBytes(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public string PostUrl(string url, string postData)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.Timeout = 5000;//设置请求超时时间，单位为毫秒
            req.ContentType = "application/json";
            byte[] data = Encoding.UTF8.GetBytes(postData);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }

            return result;
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
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    account.cookies.Add(response.Cookies);
                    if (extreHtml)
                    {
                        account.extreHtml = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    }
                    html = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                }                
            }
            catch (Exception ex)
            {
                
            }
            return html;
        }

        public string PostData(string url, string fileName, byte[] file, Dictionary<string, string> dict)
        {
            try
            {
                var httpUpload = new HttpUpload();
                if (dict != null)
                {
                    foreach (var item in dict)
                    {
                        httpUpload.SetFieldValue(item.Key, item.Value);
                    }
                }
                httpUpload.SetFieldValue("image_file", fileName, "image/jpeg", file);
                string responStr = httpUpload.Upload(url);
                return responStr;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
                    //string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    //req.Abort();
                    //return getGb2312(str5);
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
                    string result = getGb2312(response.ResponseUri.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally { }
            }
            return string.Empty;
        }
        public string PostForm1(string url, string hostUrl,string originUrl, string refernceUrl, AccountModel account, string form_string)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.CookieContainer = account.cookies;
                req.Method = "Post";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                //req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
                req.KeepAlive = true;
                req.Referer = refernceUrl;
                req.Host = hostUrl;
                // req.CachePolicy=new RequestCachePolicy(RequestCacheLevel.)

                //account.cookies.GetCookies(new Uri(".sg.kunlun.com"));
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("Cache-Control", "max-age=0");
                req.Headers.Add("Origin", originUrl);
                req.Headers.Add("Upgrade-Insecure-Requests", "1");
                byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(form_string);
                req.ContentLength = bytes.Length;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    // account.cookies.Add(response.Cookies);
                    try
                    {
                        string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                        req.Abort();
                        string result = getGb2312(str5);
                        if (result.ToLower().Contains("login.logout"))
                            relogin(account);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
           
            
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

        public  string Html_get(string url,string referUrl,AccountModel account,bool direct=false)
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
                    string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    if (result.ToLower().Contains("login.logout")&&!direct)
                        relogin(account);
                    return result;
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
                throw ex;
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
                    try
                    {
                        string result = getGb2312(str5);
                        if (result.ToLower().Contains("login.logout"))
                            relogin(account);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { }
                   
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

        public string Html_get(string url,ref AccountModel account)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = account.cookies;
                request.Method = "GET";
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 1.1.4322)";
                response = (HttpWebResponse)request.GetResponse();
                try
                {
                    string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    try
                    {
                        request.CookieContainer.Add(response.Cookies);
                        string result = getGb2312(str5);
                        if (result.ToLower().Contains("login.logout"))
                            relogin(account);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { }

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

        public string Html_get2(string url, ref AccountModel account, string refer)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = account.cookies;
                request.Method = "GET";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.87 Safari/537.36";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                //request.IfModifiedSince = "Sat, 09 Nov 2019 09:24:56 GMT";
                
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
                if(!string.IsNullOrEmpty(refer))
                 request.Referer = refer;
                request.Headers.Add("Upgrade-Insecure-Requests", "1");
                //request.Headers.Add("Sec-Fetch-Mode", "navigate");
                //request.Headers.Add("Sec-Fetch-Site", "same-site");
                request.Host = "www.kunlun.com";
             
                response = (HttpWebResponse)request.GetResponse();
                try
                {
                    string str5 = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    try
                    {
                        account.cookies.Add(response.Cookies);
                        string result = getGb2312(str5);
                        if (result.ToLower().Contains("login.logout"))
                            relogin(account);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { }

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

        public string Html_get3(string url, ref AccountModel account, string refer)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = new CookieContainer();
                request.CookieContainer = account.cookies;
                request.Method = "GET";
                request.KeepAlive = true;
                request.UserAgent = "*/*";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3";
                //request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.9");
                if (!string.IsNullOrEmpty(refer))
                    request.Referer = refer;
                request.Host = "login.kunlun.com";

                response = (HttpWebResponse)request.GetResponse();
                try
                {
                    string result = new StreamReader(response.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                    request.Abort();
                    try
                    {
                        account.cookies.Add(response.Cookies);
                        
                        if (result.ToLower().Contains("login.logout"))
                            relogin(account);
                        return result;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally { }

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
