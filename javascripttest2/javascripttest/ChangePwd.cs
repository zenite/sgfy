using javascripttest.BLL;
using javascripttest.DbHelper;
using javascripttest.entity;
using mshtml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class ChangePwd : Form
    {
        private Regular.SetConfig mainConfig;
        private UrlCommand urlCommand;
        private bool? recruiteType;
        private ScriptEngine scriptEngine;
        private System.Windows.Forms.NumericUpDown numericUpDownTimeout;
        private MainLogic mainHelper;
        private cookieHelper cookieHelper;
        private DBUti DbHelper;
        private List<AccountModel> CurrenList = new List<AccountModel>();
        private List<AccountModel> accountList = new List<AccountModel>();
        public Dictionary<string, AccountModel> accoutDics;
        private int accountId;
        //private Dictionary<string, AccountTask> tickAccountList;
        private Queue<target> targets;
        private ThreadWorkers TraderThr;
        private ThreadWorkers defenseThr;
        private int traderAccountIndex;
        private int defenseAccountIndex;
        private commonurl commonUrl;
        private string Imgusername;
        private string Imgpwd;
        //private static Mutex corMutex;
        private object obj = new object();
        private static object MonitorObj = new object();
        private static object MonitorObj1 = new object();
        private int threadCount = 0;
        private accountants form;
  
        public ChangePwd()
        {
            InitializeComponent();
            scriptEngine = new ScriptEngine();
            
            scriptEngine.Language = (ScriptLanguage)Enum.Parse(typeof(ScriptLanguage), "JavaScript");
            //this.scriptEngine.Timeout = (int)this.numericUpDownTimeout.Value;
            accoutDics = new Dictionary<string, AccountModel>();
            mainConfig = new Regular.SetConfig(this.Name, "controls");
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {
             backgroundWorker1.RunWorkerAsync();
        }

        private void changebtn_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < accountList.Count; i++)
            {
                AccountModel account = accountList[i];
                string url = @"http://www.kunlun.com/?act=passport.do_reset_password";
                string hostUrl = @"www.kunlun.com";
                string referenceUrl = @"http://www.kunlun.com/?act=passport.changepwd";
                string OriginUrl = @"http://www.kunlun.com";
                string form_str = string.Format("old_password={0}&password={1}&password_retype={1}", account.password, pwdTxt.Text.ToString());
               
                string result = urlCommand.PostForm1(url, hostUrl, OriginUrl, referenceUrl, account, form_str);
                var log = string.Empty;
                try
                {
                    if (result.Contains("新密码修改成功！"))
                    {
                        log = string.Format("账号：{0} {1}修改成功,新密码为{2}", account.username, account.password, pwdTxt.Text.ToString());
                        refreshLog(log, Constant.pwdIndex);
                        LogHelper.Info(log);
                    }
                    else
                    {
                        log = string.Format("账号：{0} {1}修改失败", account.username, account.password);
                        refreshLog(log, Constant.pwdIndex);
                        LogHelper.Info(log);
                    }
                }
                catch (Exception ex)
                {

                }
              
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Constant.Server_Url))
                {
                    MessageBox.Show("请设置要登录的主机");
                    return;
                }
                else
                {
                    CurrenList = new List<AccountModel>();
                    accountList = new List<AccountModel>();
                    accoutDics = new Dictionary<string, AccountModel>();
                    DataSet ds = DbHelper.getAllAccount();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AccountModel account = new AccountModel();
                        account.username = dr["username"].ToString();
                        account.password = dr["password"].ToString();
                        account.cookieStr = dr["cookieStr"].ToString();
                        account.hasMulti = dr["hasMulti"].ToString();
                        account.Initial_Status = dr["Initial_Status"].ToString();
                        account.user_id = dr["user_id"].ToString();
                        account.Server_url = Constant.Server_Url;
                        account.cookies = new CookieContainer();
                        //cookieHelper.setCookies(ref account);
                        CurrenList.Add(account);
                    }
                    refreshLog("账号循环开始", 0);

                    foreach (var account in CurrenList)
                    {
                        Interlocked.Increment(ref threadCount);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(loginEntrance), account);
                    }
                    //等待线程全部完成
                    while (threadCount > 0)
                    {
                        Thread.Sleep(1000);
                    }
                    refreshLog("账号登陆完毕", 0);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshLog("所有账号登陆已结束，可以做其他操作啦", 0);
        }

        public void refreshLog(string log, int logindex = 1)
        {
            if (logindex == 0)
            {
                CommonDelegate.recordLog HandleRef = new CommonDelegate.recordLog(StartRefreshLog);
                HandleRef.Invoke(log, logindex);
            }
            else
            {
                StartRefreshLog(log, logindex);
            }
        }

        public void StartRefreshLog(string log, int logindex = 1)
        {

                if (log0.InvokeRequired || this.InvokeRequired)
                {
                    log0.BeginInvoke(new MethodInvoker(() =>
                    {
                        log0.Items.Add(log);
                        log0.Refresh();

                    }));
                }
                else
                {
                    int count = log0.Items.Add(log);
                    if (count >= 1000)
                    {
                        Monitor.Enter(MonitorObj1);
                        log0.Items.Clear();
                        log0.Refresh();
                        Monitor.Exit(MonitorObj1);
                        log0.Refresh();
                    }
                }
            }

        public void loginEntrance(object obj)
        {
            AccountModel account = obj as AccountModel;
            loginKunLunServer(account);
        }

        public void loginKunLunServer(AccountModel account)
        {
            try
            {
                string destination_url = string.Empty;
                string userid = account.user_id;
                //int countBlank = 0;            
                string login_serverurl = account.Server_url;
                string server_url = "http://" + login_serverurl;
                //登陆官网，获取相应cookie
                string serverurl = server_url + "/index.php?act=login.form";
                //account.hasMulti = "0";
            
                //获取验证码图片                
                Login_Start:
                AccountModel newaccount = new AccountModel();
                newaccount.username = account.username;
                newaccount.password = account.password;
                newaccount.Server_url = account.Server_url;
                newaccount.Initial_Status = account.Initial_Status;
                newaccount.cookies = new CookieContainer();
                account = newaccount;

                try
                {
                    Monitor.Enter(MonitorObj);
                    string code = string.Empty;

                    var p_token = urlCommand.Html_get3("https://login.kunlun.com/?act=user.gettoken", ref account, "https://go.kunlun.com/?ref=http://sg.kunlun.com").Split('\'')[1];
                    byte[] imageBytes = urlCommand.getnewImage(account, "login.kunlun.com", "http://sg.kunlun.com/");
                    var base64str = Convert.ToBase64String(imageBytes);
                    var coderesult = urlCommand.PostData("http://localhost:8021/b", "1.jpg", imageBytes, null);
                    var codeentity = (HttpCodeEntity)JsonConvert.DeserializeObject(coderesult, typeof(HttpCodeEntity));
                    code = codeentity.value;
                    //code = recognise(image_path);
                    if (code.Length != 4)
                    {
                        Monitor.Exit(MonitorObj);
                        goto Login_Start;
                    }
                   
                    string hostUrl = "login.kunlun.com";
                    string referenceUrl = "https://go.kunlun.com/?ref="  +HttpUtility.UrlEncode("http://www.kunlun.com/?act=passport.changepwd");
                    var userpass1 = getToken(account.password, p_token);
                    string form_str = "username=" + account.username + "&userpass=password&userpass1=" + userpass1 + "&returl=" + referenceUrl + "&usercode=" + code;
                    string OriginUrl = "http://sg.kunlun.com";
                    string login_url = "https://login.kunlun.com/?act=user.webloginr";
                    string result = urlCommand.PostForm(login_url, hostUrl, referenceUrl, OriginUrl, ref account, form_str);
                    if (result.Contains("密码有误"))
                    {
                        refreshLog("账号：" + account.username + "密码有误，请确认", 0);
                        DbHelper.UpdateError(account, "1");//账号密码错误
                        Monitor.Exit(MonitorObj);
                        //Interlocked.Decrement(ref threadCount);
                        return;
                    }
                    else if (result.Contains("用户不存在"))
                    {
                        refreshLog("账号:" + account.username + "用户不存在", 0);
                        DbHelper.UpdateError(account, "2");//用户不存在
                        Monitor.Exit(MonitorObj);
                        //Interlocked.Decrement(ref threadCount);
                        return;
                    }
                    else if (!result.Contains("success"))
                    {
                        Monitor.Exit(MonitorObj);
                        goto Login_Start;
                    }
                    else
                    {
                        //urlCommand.Html_get3("http://sg.kunlun.com/2009/0309/article_497.html", ref account, "http://sg.kunlun.com/2009/0309/article_497.html");
                        //urlCommand.Html_get3("http://item.kunlun.com/login/sg/login.php", ref account, "");
                        //urlCommand.Html_get3("http://lasts.item.kunlun.com/index.php?act=login.slist&game=sg&uid=144243425&fmt=js&sign=61d8a", ref account, "http://item.kunlun.com/login/sg/login.php");

                        printCookie("http://www.kunlun.com/?act=passport.changepwd", ref account, "http://sg.kunlun.com/2009/0309/article_497.html");
                        lock (accountList)
                        {
                            var exsit = accountList.Find(item => { if (item.username == account.username) return true; else return false; });
                            if (exsit != null)
                            {
                                accountList.Remove(exsit);
                            }
                            accountList.Add(account);
                        }

                        refreshLog(string.Format("账号:{0}登录成功",account.username), Constant.pwdIndex);
                        //Interlocked.Decrement(ref threadCount);
                        Monitor.Exit(MonitorObj);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        Monitor.Exit(MonitorObj);
                    }
                    catch
                    { }
                    goto Login_Start;
                }

                //urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
                ////登陆相应服务器，判断路程
                //urlCommand.urlPost(server_url, ref account, login_serverurl, "http://game.sg.kunlun.com/?sid=" + Constant.Server_Url.Split(new char[] { '.' })[0]);
                //login_into:
                //string dddd = urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
                //string returnhtml = urlCommand.Html_get("http://" + Constant.Server_Url + "/", "http://" + Constant.Server_Url + "/", account, true);
                //if (mainHelper.checkLoginout(ref account))
                //{
                //    account.cookieStr = null;
                //    goto Login_Start;
                //}

                //string refreshhtml = string.Empty;
                //if (returnhtml.Contains("Loading...") && !string.IsNullOrEmpty(account.user_id))
                //    goto Login_Start;
                //else if (returnhtml.Contains("Loading...") && string.IsNullOrEmpty(account.user_id))
                //    refreshhtml = refreshLogin(returnhtml, ref account, login_serverurl, serverurl, server_url);
                //else
                //    refreshhtml = returnhtml;
                //if (!refreshhtml.Contains("login.rolelist"))
                //{
                //    // if (mainHelper.checkAccountLoginOut(account))     
                //    cookieHelper.GetSsid(ref account, server_url);
                //    if (string.IsNullOrEmpty(account.user_id))
                //    {
                //        refreshLogin(refreshhtml, ref account, login_serverurl, serverurl, server_url);
                //        goto Login_Start;
                //    }
                //    if (mainHelper.checkLoginout(ref account))
                //    {
                //        account.cookieStr = null;
                //        goto Login_Start;
                //    }
                //    refreshLog(string.Format("0,userID:{0},userName:{1}", account.user_id, account.username), 0);
                //    StoreAccount(account);
                //}
                //else
                //{
                //    List<string> user_ids = new List<string>();
                //    getrole:
                //    destination_url = server_url + "/index.php?act=login.rolelist";
                //    urlCommand.urlPost(destination_url, ref account, login_serverurl, serverurl, true);
                //    if (account.extreHtml.Contains("Loading..."))
                //    {
                //        if (account.extreHtml.Contains("Loading..."))
                //            goto getrole;
                //    }
                //    RegexHtml.getList(SGEnum.RegexType.User_id, account.extreHtml, out user_ids);
                //    List<string> LoginInList = DbHelper.checkAccount(account);
                //    if (string.IsNullOrEmpty(account.user_id) && string.IsNullOrEmpty(userid))
                //    {
                //        foreach (var user_id in user_ids)
                //        {
                //            if ((from item in LoginInList where item == user_id select item).Count() == 0)
                //            {
                //                account.user_id = user_id;
                //                goto Login_Server;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if (!string.IsNullOrEmpty(userid))
                //            account.user_id = userid;
                //        goto Login_Server;
                //    }
                //    Login_Server:
                //    refreshLog(string.Format("1,userID:{0}", account.user_id), 0);
                //    string role_loginUrl = server_url + "/index.php?act=login.role&user_id=" + account.user_id;
                //    string role_loginrefernce = destination_url;
                //    urlCommand.urlPost(role_loginUrl, ref account, login_serverurl, role_loginrefernce, true);
                //    cookieHelper.GetSsid(ref account, server_url);
                //    if (mainHelper.checkLoginout(ref account))
                //    {
                //        account.cookieStr = null;
                //        goto Login_Start;
                //    }
                //    refreshLog(string.Format("2,userID:{0},userName:{1}", account.user_id, account.username), 0);
                //    StoreAccount(account);
                //    LoginInList = DbHelper.checkAccount(account);
                //    if (LoginInList.Count < user_ids.Count())
                //    {
                //        userid = string.Empty;
                //        goto Login_Start;
                //    }
                //}
            }
            catch (Exception ex)
            {
                refreshLog(ex.Message, 0);
            }
            finally
            {
                Interlocked.Decrement(ref threadCount);
            }
        }

        public void printCookie(string url, ref AccountModel account,string refer)
        {
            var getCookie = urlCommand.Html_get2(url, ref account,  refer);
        }

        private void ChangePwd_Load(object sender, EventArgs e)
        {
            try
            {

                webBrowser1.Navigate(new Uri("https://www.baidu.com/"));

                string config_ServerUrl = xmlHelper.getConfig("Server_Url");
                Constant.Server_Url = config_ServerUrl;

                urlCommand = new UrlCommand();
                urlCommand.relogin += new CommonDelegate.reLogin(relogin);
                cookieHelper = new cookieHelper();
                DbHelper = new DBUti();
                commonUrl = new commonurl();
            }
            catch (Exception ex)
            {


            }
            //ListBox li=(ListBox)((TabPage)this.report.Controls[0]).Controls[0];
            //mainHelper = new MainLogic(commonUrl, urlCommand, refreshLog, refreshgrid);
            //softReg = new SoftReg();
            //eventregular = new EventRegular();
            //checkRegister();

            //if (!DbHelper.autoUpdateDatabase())
            //{
            //    show("数据库初始化失败");
            //}
        }

        private void relogin(AccountModel account)
        {
            new Thread(delegate () {
                loginKunLunServer(account);
                //AccountModel outAccount = new AccountModel();
                //accoutDics.TryGetValue(account.user_id, out outAccount);
                //if (outAccount != null)
                //    refreshFormsAccount(outAccount);
            }).Start();
        }

        public string getToken(string pwd,string p_token)
        {
            string code = File.ReadAllText(@"E:\微软云\OneDrive\新建文件夹\sgfy\javascripttest2\javascripttest\jsscript\kl_login.js");
            var paras = new List<string>() { pwd, "10001", p_token }.ConvertAll(t=>(Object)t).ToArray();
            return scriptEngine.Run("getToken", paras,code).ToString();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            IHTMLDocument2 window = webBrowser1.Document.DomDocument as  IHTMLDocument2;
            scriptEngine.msc.AddObject("window", window.parentWindow, true);
        }
    }
}
