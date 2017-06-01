using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using LZ.Event.Log;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using javascripttest.Properties;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSScriptControl;
using Microsoft.VisualBasic;
//using Microsoft.JScript;
using Microsoft.Win32;
using javascripttest.DbHelper;
using javascripttest.entity;
using javascripttest.BLL;
using System.Runtime.InteropServices;
//using javascripttest.msscript;

namespace javascripttest
{
    public partial class Main : Form
    {       
        public Main()
        {
            InitializeComponent();
            InitReport();
            accoutDics = new Dictionary<string, AccountModel>();
        }
        private UrlCommand urlCommand;
        private bool? recruiteType;
        private MainLogic mainHelper;
        private cookieHelper cookieHelper;
        private DBUti DbHelper;
        private List<AccountModel> CurrenList = new List<AccountModel>();
        private List<AccountModel> accountList = new List<AccountModel>();
        public Dictionary<string, AccountModel> accoutDics;
        private int accountId;
        //private Dictionary<string, AccountTask> tickAccountList;
        private Queue<target> targets;
        private Thread TraderThr;
        private Thread HeavenThr;
        private Thread SearchItemThr;
        private Thread PearlThr;
        private Thread depotItemThr;
        Thread t;
        private Thread AttackThr;
        private SoftReg softReg;
        private EventRegular eventregular;
        private commonurl commonUrl;
        private string Imgusername;
        private string Imgpwd;
        //private static Mutex corMutex;
        private object obj = new object();
        private static object MonitorObj=new object();
        private static object MonitorObj1 = new object();
        private int threadCount = 0;
        private accountants form;
        private void Main_Load(object sender, EventArgs e)
        {
            Initial_urlBox();
            Initial_EnableAuto();
            urlCommand = new UrlCommand();
            urlCommand.relogin += new CommonDelegate.reLogin(relogin);
            cookieHelper = new cookieHelper();
            DbHelper = new DBUti();
            commonUrl = new commonurl();
            //ListBox li=(ListBox)((TabPage)this.report.Controls[0]).Controls[0];
            logs = new List<ListBox> { log1, log2, log3, log4,log5};
            mainHelper = new MainLogic(commonUrl, urlCommand, refreshLog, refreshgrid);            
            softReg=new SoftReg();
            eventregular = new EventRegular();                
            checkRegister();
            WarQueue = new Queue<AccountModel>();
            if (!DbHelper.autoUpdateDatabase())
            {
                show("数据库初始化失败");
            }
        }
        /// <summary>
        /// 账号管理入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void account_manager_Click(object sender, EventArgs e)
        {
            if (form == null || form.IsDisposed)
            {
                form = new accountants(accoutDics);
                form.mainHelper = this.mainHelper;
                form.Show();
            }
            else
            {
                form.accoutDics = accoutDics;
                form.mainHelper = this.mainHelper;
                form.Show();
            }
          
        }

        /// <summary>
        /// 打码登陆入口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Start_Click(object sender, EventArgs e)
        {
            Imgusername = this.txtqUser.Text.Trim();
            Imgpwd = this.txtqPwd.Text.Trim();
            if (string.IsNullOrEmpty(Imgusername) || string.IsNullOrEmpty(Imgpwd))
            {
                MessageBox.Show("请填写您自动验证的用户名和密码");
                return;
            }
            if (!ImgValid())
            {
                MessageBox.Show("请确保您的自动验证用户名和密码是正确的");
                return;
            }
            DataSet ds = DbHelper.getAllAccount();
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("请先录入账号在进行此操作");

                //DbHelper.test();            
                openFileDialog1.Filter = "excel文档|*.xlsx;*.csv;*.xls;*.txt";
                openFileDialog1.InitialDirectory = @"C:\Users\liuhao\Desktop";
                DataSet excelds = new DataSet();
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string excel_path = openFileDialog1.FileName;
                    string File_type = Path.GetExtension(excel_path);
                    if (!string.IsNullOrEmpty(excel_path) && (File_type == ".xlsx" || File_type == ".xls" || File_type == ".csv"||File_type==".txt"))
                    {
                        ExcelHelper excelHelper = new ExcelHelper();
                        excelds = excelHelper.getExeclData(excel_path);
                        if (excelds == null || excelds.Tables.Count == 0)
                        {
                            show("没有数据");
                            return;
                        }
                    }
                    else
                    {
                        show("请选择正确的文件");
                        return;
                    }
                }
                DataView dv = excelds.Tables[0].DefaultView;
                DataTable dt = dv.ToTable(true, "name", "password");

                DataSet DbDs = new DataSet();
                DbDs = DbHelper.getAllAccount();
                DataTable DbDt = DbDs.Tables[0];
                List<DataRow> rows = new List<DataRow>();
                foreach (DataRow item in dt.Rows)
                {
                    if (DbDt.AsEnumerable().Where(a => a.Field<string>("username").ToString() == item["name"].ToString()).Count() == 0)
                    {
                        rows.Add(item);
                    }
                }
                if (rows.Count() > 0)
                {
                    AccountModel account = new AccountModel();
                    foreach (var item in rows)
                    {
                        account.username = item["name"].ToString();
                        account.password = item["password"].ToString();
                        account.Server_url = Constant.Server_Url;
                        DbHelper.insertAccount(account);
                    }
                }
                show("账号生成成功");
                return;
            }
            else
            {
                backgroundWorker1.RunWorkerAsync();     
            }                   
        }

        private void show(string message)
        {
            MessageBox.Show(message);
        }

     /// <summary>
     /// 设置服务器地址
     /// </summary>
     /// <param name="sender"></param>
     /// <param name="e"></param>
        private void setServer_Click(object sender, EventArgs e)
        {
             string url = string.Empty;
             if (!this.selectServer.Visible)
                {
                    this.setServer.Text = "保存";
                    this.selectServer.Visible = true;
                    this.address.Visible = false;
                }
                else
                {
                    url = this.selectServer.Text;                    
                    this.setServer.Text = "设置";
                    this.selectServer.Visible = false;
                    this.address.Visible = true;
                }
                if (!string.IsNullOrEmpty(url))
                {
                    Constant.Server_Url = url;
                    this.address.Text = url;
                    xmlHelper.saveConfig("Server_Url", url);
                }          
        }
        
        private void Initial_EnableAuto()
        {
            string config_ServerUrl = xmlHelper.getConfig("enableAuto");
            string txtqUser = xmlHelper.getConfig("txtqUser");
            string txtqPwd = xmlHelper.getConfig("txtqPwd");
            enabledAuto.Checked = config_ServerUrl == "true" ? true : false;
            this.txtqUser.Text = txtqUser;
            this.txtqPwd.Text = txtqPwd;
        }
        private void Initial_urlBox()
        {
            DataSet urlDs= xmlHelper.getXmlData();
            this.selectServer.DisplayMember = "url";
            this.selectServer.DataSource = urlDs.Tables[0];
            string config_ServerUrl = xmlHelper.getConfig("Server_Url");
            if (string.IsNullOrEmpty(config_ServerUrl))
            {
                this.selectServer.Visible = true;
                this.setServer.Text = "保存";
                this.address.Visible = false;                
            }
            else
            {
                this.selectServer.Visible = false;
                this.address.Text = config_ServerUrl;
                this.setServer.Text = "设置";
                this.address.Visible = true;
                Constant.Server_Url = config_ServerUrl;
            }
        }
        /// <summary>
        /// 招兵设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoldiersType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SoldiersType.Text=="攻击")
            {
                this.defenseSetting.Visible = false;
                this.attackSetting.Visible = true;
                recruiteType = true;
            }
            if (SoldiersType.Text == "防御")
            {
                this.defenseSetting.Visible = true;
                this.attackSetting.Visible = false;
                recruiteType = false;
            }
        }


        private void relogin(object o)
        {
            new Thread(delegate() {
                loginKunLunServer(o);
                AccountModel outAccount=new AccountModel();
                accoutDics.TryGetValue((o as AccountModel).user_id,out outAccount);
                if (outAccount != null)
                    refreshFormsAccount(outAccount);
            }).Start();
        }
        private void refreshFormsAccount(AccountModel account)
        {
            if (form!=null&&form.IsDisposed)
                form.refreshAccounts(account);
        }
        public void loginKunLunServer(object obj)
        {
            AccountModel account = obj as AccountModel;       
            string destination_url = string.Empty;
            string userid = account.user_id;
            //int countBlank = 0;            
            string login_serverurl = account.Server_url;
            string server_url = "http://" + login_serverurl;
            //登陆官网，获取相应cookie
            string serverurl = server_url + "/index.php?act=login.form";
            account.hasMulti = "0";
            if (!string.IsNullOrEmpty(account.user_id))
            {
                goto login_into;
            }
        //获取验证码图片                
        Login_Start:
            AccountModel newaccount = new AccountModel();
            newaccount.username = account.username;
            newaccount.password = account.password;
            newaccount.Server_url = account.Server_url;
            newaccount.Initial_Status = account.Initial_Status;
            account = newaccount;
            
            //Mutex corMutex = new Mutex(false, "login");
            try
            {
                Monitor.Enter(MonitorObj);
                string code = string.Empty;
                string image_path = (string)urlCommand.getImage(ref account, "login.kunlun.com", "http://sg.kunlun.com/");
                //dialogbox dialog = new dialogbox();
                //Image tmp_img = Image.FromFile(image_path);
                //Image img_copy = new Bitmap(tmp_img);
                //dialog.CodePicture.Image = img_copy;
                //tmp_img.Dispose();              
                //dialog.StartPosition = FormStartPosition.CenterScreen;
                //if (dialog.ShowDialog() == DialogResult.OK)
                //{
                //    code = dialog.CodeTextBox.Text;
                //    if (string.IsNullOrEmpty(code) || code.Length != 4)
                //    {
                //        Monitor.Exit(MonitorObj);
                //        goto Login_Start;
                //    }
                //}
                //else//record error
                //{
                //    Monitor.Exit(MonitorObj);
                //    goto Login_Start;
                //}
                code = recognise(image_path);
                if (code.Length != 4)
                {
                    Monitor.Exit(MonitorObj);
                    goto Login_Start;
                }                    
                string form_str = "username=" + account.username + "&userpass=" + account.password + "&returl=" + HttpUtility.UrlEncode("http://static.kunlun.com/v3/login_sso_js_v1/callback.html") + "&usercode=" + code;
                string hostUrl = "login.kunlun.com";
                string referenceUrl = "http://sg.kunlun.com/";
                string OriginUrl = "http://sg.kunlun.com";
                string login_url = "http://login.kunlun.com/index.php?act=user.webLogin";
                string result = urlCommand.PostForm(login_url, hostUrl, referenceUrl, OriginUrl, ref account, form_str);
                if (result.Contains("密码有误"))
                {
                    refreshLog("账号：" + account.username + "密码有误，请确认", 0);
                    DbHelper.UpdateError(account, "1");//账号密码错误
                    Monitor.Exit(MonitorObj);
                    Interlocked.Decrement(ref threadCount);
                    return;
                }
                else if (result.Contains("用户不存在"))
                {
                    refreshLog("账号:" + account.username + "用户不存在", 0);
                    DbHelper.UpdateError(account, "2");//用户不存在
                    Monitor.Exit(MonitorObj);
                    Interlocked.Decrement(ref threadCount);
                    return;
                }
                else if (!result.Contains("success"))
                {
                    Monitor.Exit(MonitorObj);
                    goto Login_Start;
                }
                else {
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

            //finally
            //{
            //    corMutex.ReleaseMutex();
            //}
            urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
            //登陆相应服务器，判断路程
            urlCommand.urlPost(server_url, ref account, login_serverurl, "http://game.sg.kunlun.com/?sid=" + Constant.Server_Url.Split(new char[] { '.' })[0]);
        login_into:
            string dddd=urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
            string returnhtml = urlCommand.Html_get("http://" + Constant.Server_Url + "/", "http://" + Constant.Server_Url + "/", account);
            string refreshhtml = string.Empty;
            if (returnhtml.Contains("Loading...") && !string.IsNullOrEmpty(account.user_id))
                goto Login_Start;
            else if (returnhtml.Contains("Loading...") && string.IsNullOrEmpty(account.user_id))
                refreshhtml = refreshLogin(returnhtml, ref account, login_serverurl, serverurl, server_url);
            else
                refreshhtml = returnhtml;
            if (!refreshhtml.Contains("login.rolelist"))
            {
                //if (mainHelper.checkAccountLoginOut(account))     
                cookieHelper.GetSsid(ref account, server_url);
                if (string.IsNullOrEmpty(account.user_id))
                {
                    refreshLogin(refreshhtml, ref account, login_serverurl, serverurl, server_url);
                }
                if (mainHelper.checkLoginout(ref account))
                    goto Login_Start;
                StoreAccount(account);
            }
            else
            {
                List<string> user_ids = new List<string>();
            getrole:
                destination_url = server_url + "/index.php?act=login.rolelist";
                urlCommand.urlPost(destination_url, ref account, login_serverurl, serverurl, true);
                if (account.extreHtml.Contains("Loading..."))
                {
                    if (account.extreHtml.Contains("Loading..."))
                        goto getrole;
                }
                RegexHtml.getList(SGEnum.RegexType.User_id, account.extreHtml, out user_ids);
                List<string> LoginInList = DbHelper.checkAccount(account);
                if (string.IsNullOrEmpty(account.user_id) && string.IsNullOrEmpty(userid))
                {
                    foreach (var user_id in user_ids)
                    {
                        if ((from item in LoginInList where item == user_id select item).Count() == 0)
                        {
                            account.user_id = user_id;
                            //countBlank++;
                            goto Login_Server;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(userid))
                        account.user_id = userid;
                    goto Login_Server;
                }
            Login_Server:
                string role_loginUrl = server_url + "/index.php?act=login.role&user_id=" + account.user_id;
                string role_loginrefernce = destination_url;
                urlCommand.urlPost(role_loginUrl, ref account, login_serverurl, role_loginrefernce, true);
                cookieHelper.GetSsid(ref account, server_url);
                if (mainHelper.checkLoginout(ref account))
                    goto Login_Start;
                StoreAccount(account);
                LoginInList = DbHelper.checkAccount(account);
                if (LoginInList.Count < user_ids.Count())
                {
                    userid = string.Empty;
                    goto Login_Start;
                }                
            }
            Interlocked.Decrement(ref threadCount);
        }

        
        
        /// <summary>
        /// 存储账号
        /// </summary>
        /// <param name="account"></param>
        private void StoreAccount(AccountModel account)
        {
            string html = urlCommand.Html_get("http://" + Constant.Server_Url, "http://" + Constant.Server_Url + "/", account);            
            if (!string.IsNullOrEmpty(html))
            {
                if (html.Contains("Loading..."))
                {
                    html = refreshLogin(html, ref account, Constant.Server_Url, "http://" + Constant.Server_Url, "http://" + Constant.Server_Url);
                    if (html.Contains("Loading..."))
                    {
                        html = refreshLogin(html, ref account, Constant.Server_Url, "http://" + Constant.Server_Url, "http://" + Constant.Server_Url);
                        if(html.Contains("Loading..."))
                            html = refreshLogin(html, ref account, Constant.Server_Url, "http://" + Constant.Server_Url, "http://" + Constant.Server_Url);
                    }
                }
                if (html.Contains("createmonarch"))
                {
                    string logger = "   账号："+account.username+"  在 "+Constant.Server_Url+"  中没有账号";
                    DbHelper.UpdateError(account,"2");
                    refreshLog(logger, 0);
                    return;
                }
                InfomationMining(ref account, html);
                //this.accountId++;
                //AccountTask task = new AccountTask() { account=account,accountId=this.accountId.ToString()};
                //tickAccountList.Add(accountId.ToString(), task);
                try
                {
                    if (DbHelper.InsertOrUpdate(account))
                        DbHelper.insertAccount(account,"insert");
                    else
                        DbHelper.updateAccount(account);
                }
                catch (Exception ex)//记录错误
                {
                    throw ex;
                }
                //Thread t = new Thread(delegate() { });
                //t.Start();                            
                InitialThread(account); 
            }          
        }

        
        


        private string refreshLogin(string html,ref AccountModel account, string login_serverurl, string serverurl, string server_url)
        {            
            if(html.Contains("Loading..."))
            {
                string chanelUrl = mainHelper.getParaValue("(?<=(function nOnLoad\\(\\) { window.location=\")).+?(?=\"; };)", html);
                urlCommand.urlPost(server_url + "/" + chanelUrl, ref account, login_serverurl, serverurl, true);   
            }
            return account.extreHtml;
        }
        private void InfomationMining(ref AccountModel account,string html)
        {            
            account.rankOfNobility = mainHelper.getParaValue("(?<=(emperor.court_list'\\);\">【)).+?(?=】)", html);
            account.chief = mainHelper.getParaValue("(?<=(<li><span class=\"title\">君主:</span><span class=\"value\">)).+?(?=<)", html);
            account.villageid = mainHelper.getParaValue(@"(?<=(w\.villageid=')).+?(?=';)", html);
            string typeOfCountry = mainHelper.getParaValue(@"(?<=(<img src=.*?/images/ui/)).+?(?=(\.gif.*?country))", html);

            if (!string.IsNullOrEmpty(typeOfCountry))
            {
                switch (typeOfCountry)
                {
                    case "shu":
                        account.typeOfCountry = "蜀";
                        break;
                    case "wei":
                        account.typeOfCountry = "魏";
                        break;
                    case "wu":
                        account.typeOfCountry = "吴";
                        break;
                }
            }
            string logger = "读取cookie成功：" + account.chief;
            logger += "   爵位：" + account.rankOfNobility + "   国家：" + account.typeOfCountry + "。 正在读取账号信息，请稍后操作";
            refreshLog(logger, 0);          
        }

        //异步获取账号信息
        private void InitialThread(AccountModel account)
        {
            //AccountModel account = task.account;
            mainHelper.InitialAccountInfo(ref account);
            DbHelper.updateAccount("city_num", account.village.Count().ToString(), account.user_id);
            try
            {
                lock (accountList)
                    accountList.Add(account);
                lock (accoutDics)
                {
                    AccountModel outAccount;
                    accoutDics.TryGetValue(account.user_id, out outAccount);
                    if (outAccount != null)
                    {
                        accoutDics[account.user_id] = account;
                    }
                    else
                        accoutDics.Add(account.user_id, account);
                }
                    
                string logg = "账号：" + account.chief + "   加载成功";
                refreshLog(logg, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            //tickAccountList.Remove(task.accountId);           
        }

       

      

       

        private void rapidLoginBtn_Click(object sender, EventArgs e)
        {
            //rapidForm = new RapidLogin();
            //rapidForm.Show();
        }
        /// <summary>
        /// 开始招兵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecruitStart_Click(object sender, EventArgs e)
        {
            int nCountry = 0;
            List<string> recruite = new List<string>();
            int soldierAmount =0;
            int.TryParse(SoldiersAmount.Text.Trim(), out soldierAmount);
            if (soldierAmount > 0)
            {
                if (recruiteType != null)
                {
                    if (recruiteType == true)
                    {
                        foreach (var item in attackSetting.Controls)
                        {                            
                            if ((item as CheckBox).Checked)
                            {
                                recruite.Add((item as CheckBox).Text);
                                
                            }
                        }
                        if (recruite.Count == 0)
                        {
                            show("请勾选");
                            return;
                        }
                        else
                        {
                            foreach (var item in accountList)
                            {
                                nCountry = Constant.getNCountry(item.typeOfCountry);
                                foreach (var village in item.village)
                                {
                                    string cityFilter = filterrecruit.Text;
                                    Thread recruiteThr = new Thread(delegate() { recrite(item, nCountry, recruite, soldierAmount, cityFilter); });
                                    recruiteThr.Start();   
                                }
                            }
                        }
                    }
                    else
                    {
                        //defenseSetting
                        foreach (var item in defenseSetting.Controls)
                        {
                            if ((item as CheckBox).Checked)
                            {
                                recruite.Add((item as CheckBox).Text);

                            }
                        }
                        if (recruite.Count == 0)
                        {
                            show("请勾选");
                            return;
                        }
                        else
                        {
                            foreach (var item in accountList)
                            {
                                nCountry = Constant.getNCountry(item.typeOfCountry);
                                string cityFilter = filterrecruit.Text;
                                //Thread recruiteThr = new Thread(delegate() { });
                                //recruiteThr.Start();     
                                recrite(item, nCountry, recruite, soldierAmount, cityFilter); 
                            }                           
                        }                       
                    }
                    
                }
                else
                {
                    show("请选择要招募的兵种");
                    return;
                }
            }
            else
            {
                show("请输入要招兵的数量");
            }           
        }


        private void recrite(AccountModel account, int nCountry, List<string> recruite, int soldierAmount, string cityFilter)
        {
            foreach (var village in account.village)
            {
                foreach (var soldierName in recruite)
                {
                    if (Constant.GetSldTypeByName(nCountry, soldierName) != -1)//必须获取兵种type
                    {
                        string type = Constant.GetSldTypeByName(nCountry, soldierName).ToString();                        
                        recruiteSoldier(soldierAmount.ToString(), type, soldierName, nCountry, village, account, cityFilter);
                    }
                }
            }
        }
        //招兵
        private void recruiteSoldier(string num, string type, string soldierName,int nCountry,village village,AccountModel account,string cityFilter)
        {
            if (cityFilter == "仅主陪")
            {
                if (village.Ismain != "1" && village.IsAssistant != "1")
                    return;
            }
            else if (cityFilter == "不包括主陪")
            {
                if (village.Ismain == "1" || village.IsAssistant == "1")
                    return;
            }
            string btid = Constant.getSoldierBtidByName(soldierName).ToString();
            string bid = string.Empty;
            if (village.buildings.Where(k => Constant.GetBldTypeByName(k.buildingName).ToString() == type).Count() > 0)//保证招兵的建筑存在
            {
                string recruitMessage = mainHelper.RecruitSolider(nCountry, village, account, type, num, Constant.recruiteIndex, btid);
                if (recruitMessage.Contains("招募数量错误"))
                {
                    return;
                }
            }
        }

        
        
        /// <summary>
        /// 注册按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void registerBtn_Click(object sender, EventArgs e)
        {
            string register = this.register.Text;
            string registerCode = this.registerCode.Text;
            this.eventregular.SaveToRegistry("RegCodefinQ", registerCode);
            string str3 = string.Empty;
            if (register.Length > 8)
            {
                str3 = softReg.Decrypt(registerCode, "register").Replace(softReg.Decrypt(register, "register"), "*");
            }
            if (str3.Split(new char[] { '*' }).Length > 1)
            {
                this.method_32(str3);
            }
            else
            {
                MessageBox.Show("注册失败!请获取正确的注册码", "注册结果");
            }
        }
        private void method_32(string string_0)
        {
            string str = string_0.Split(new char[] { '*' })[0];
            string str2 = string_0.Split(new char[] { '*' })[1];
            str = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
            time_valid.Text =Convert.ToDateTime(str).ToString();           
        }       


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetSystemDirectory(StringBuilder stringBuilder_0, int int_17);
        
        private void checkRegister()
        {
            long hardNum =softReg.GetHardNum("c");
            this.register.Text = softReg.Encrypt("|" + hardNum.ToString() + "|2016|", "register");
            this.registerCode.Text = this.eventregular.LoadFromRegistry("RegCodefinQ").ToString();
        }

        private Queue<AccountModel> WarQueue;
        private bool defensePause=false;
        private int defenseUpperAmount=0;
        private string[] soldierType;
        private string[] attackSoldierType;

        /// <summary>
        /// 支援
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void defenseBattleStart_Click(object sender, EventArgs e)
        {
            soldierType=new string[3];
            Battle battle = new Battle();
            if (WarQueue == null || WarQueue.Count == 0)
            {
                foreach (AccountModel ccaccount in accountList)
                {
                    WarQueue.Enqueue(ccaccount);
                }
            }
            if (WarQueue == null || WarQueue.Count == 0)
            {
                MessageBox.Show("请检查你的账号");
                return;
            }            
            if (Direct_city.Checked)
            {
                battle.directType = "city";
                battle.x = x_Coord.Text;
                battle.y = y_Coord.Text;
                if (string.IsNullOrEmpty(battle.x))
                {
                    show("请输入x坐标");
                    return;
                }
                if (string.IsNullOrEmpty(battle.y))
                {
                    show("请输入y坐标");
                    return;
                }
                if (!string.IsNullOrEmpty(DefenseUpper.Text))
                {
                    defenseUpperAmount = Convert.ToInt32(DefenseUpper.Text);
                }
                battle.type = Battle_type.SelectedIndex;
            }
            else if (Direct_graincentre.Checked)
            {
                battle.directType = "graincentre";
                if (string.IsNullOrEmpty(Battle_1type.Text))
                {
                    show("请选择出征方式");
                    return;
                }
                else
                {
                    battle.type = Battle_1type.SelectedIndex;
                }
            }
            else
            {
                show("请确认选择出征目标");
                return;
            }
            if (!string.IsNullOrEmpty(DefensePattern.Text))
            {
                battle.Pattern = DefensePattern.SelectedIndex;
            }
            else
            {
                DefensePattern.SelectedIndex = 1;
                battle.Pattern = 1;
            }
            if (!string.IsNullOrEmpty(DefenseNum.Text))
            {
                battle.BattleNum = DefenseNum.Text;
            }
            if (!string.IsNullOrEmpty(cityFilter.Text))
            {
                battle.CitySelected = cityFilter.SelectedIndex;
            }
            else
            {
                cityFilter.SelectedIndex = 0;
                battle.CitySelected = 0;
            }

            foreach (Control soldier in defensePanel.Controls)
            {
                if (soldier is CheckBox)
                {
                    if ((soldier as CheckBox).Checked)
                    {
                        string checkboxName = (soldier as CheckBox).Name;
                        string soldierName = (soldier as CheckBox).Text;
                        for (int i = 0; i < 3; i++)
                        {
                            if (checkboxName.Contains(Constant.m_strCountryNames[i].ToLower()))
                            {
                                soldierType[i] += Constant.GetSldTypeByName(i, soldierName) + ",";
                            }
                        }
                        if (checkboxName == "chihou")
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                soldierType[i] += 9 + ",";
                            }
                        }
                    }
                }
            }
            battle.DefensePattern = DefensePattern.Text;

            while (!defensePause)
            {
                AccountModel account = WarQueue.Dequeue();
                Battle(account, battle); 
                //Thread BattleThr = new Thread(delegate() {});
                //BattleThr.Start();               
                if (WarQueue == null || WarQueue.Count() == 0)
                {
                    show("出征完毕");
                    return;
                }
            }               
        }

        private void Battle(AccountModel account,Battle battle)        
        {
            if (battle.directType == "city")
            {
                DefenseBattle(account, battle);
            }
            else
            {
                tuntianBattle(account, battle);
            }       
        }
        /// <summary>
        /// 向屯田所派兵或召回
        /// </summary>
        /// <param name="account"></param>
        /// <param name="battle"></param>
        private void tuntianBattle(AccountModel account,Battle battle)
        {
             mainHelper.RefreshTuntian(ref account);
             int surplus = account.MaxTuntianNum-account.CurTuntianNum;
             int num0 = 0, num1 = 0,
                 //num2 = 0,
                 num3 = 0, num4 = 0
                 //,num5 = 0, num6 = 0
                 ;
            string data=string.Empty;

            //屯兵
            if (battle.type == 0)
            {               
                foreach(var village in account.village)
                {
                     mainHelper.RefreshTuntian(ref account);
                    string[] soldiers=village.soldierss;                    
                     if (account.typeOfCountry == "魏")
                     {
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[0]) > surplus)
                         {
                             num0 = surplus; surplus = 0;
                         }
                         else
                         {
                             num0 = Convert.ToInt32(soldiers[0]); surplus -= num0;
                         }
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[1]) > surplus)
                         {
                             num1 = surplus; surplus = 0;
                         }
                         else
                         {
                             num1 = Convert.ToInt32(soldiers[1]); surplus -= num1;
                         }
                     }
                     else if (account.typeOfCountry == "蜀")
                     {
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[1]) > surplus )
                         {
                             num1 = surplus; surplus = 0;
                         }
                         else
                         {
                             num1 = Convert.ToInt32(soldiers[1]); surplus -= num1;
                         }
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[3]) > surplus)
                         {
                             num3 = surplus; surplus = 0;
                         }
                         else
                         {
                             num3 = Convert.ToInt32(soldiers[3]); surplus -= num3;
                         }
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[4]) > surplus)
                         {
                             num4 = surplus; surplus = 0;
                         }
                         else
                         {
                             num4 = Convert.ToInt32(soldiers[4]); surplus -= num4;
                         }
                     }
                     else
                     {
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[0]) > surplus)
                         {
                             num0 = surplus; surplus = 0;
                         }
                         else
                         {
                             num0 = Convert.ToInt32(soldiers[0]); surplus -= num0;
                         }
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[3]) > surplus)
                         {
                             num3 = surplus; surplus = 0;
                         }
                         else
                         {
                             num3 = Convert.ToInt32(soldiers[3]); surplus -= num3;
                         }
                         if (surplus > 0)
                         if (Convert.ToInt32(soldiers[4]) > surplus)
                         {
                             num4 = surplus; surplus = 0;
                         }
                         else
                         {
                             num4 = Convert.ToInt32(soldiers[4]); surplus -= num4;
                         }
                     }
                     if (num0 != 0 & num1 != 0 & num3 != 0 & num4 != 0)
                     {
                         if (num0 != 0)
                         {
                             data += "_uintSoldier_num0=" + num0;
                         }
                         else
                         {
                             data += "_uintSoldier_num0=";
                         }
                         if (num1 != 0)
                         {
                             data += "&_uintSoldier_num1=" + num1;
                         }
                         else
                         {
                             data += "&_uintSoldier_num1=";
                         }
                         if (num3 != 3)
                         {
                             data += "&_uintSoldier_num3=" + num3;
                         }
                         else
                         {
                             data += "&_uintSoldier_num3=";
                         }
                         if (num4 != 0)
                         {
                             data += "&_uintSoldier_num4=" + num4;
                         }
                         else
                         {
                             data += "&_uintSoldier_num4=";
                         }
                         data += "&_uintSoldier_num2=&_uintSoldier_num9=&_uintSoldier_num10=&_uintSoldier_num11=&_uintSoldier_num30=&_uintSoldier_num31=&_uintSoldier_num32=&_uintSoldier_num33=";
                         mainHelper.SendTuntianArmy(account, Constant.defenseIndex, village.VillageID, data); 
                     }                     
                }
            }
            else//召回
            {
                if (account.TuntianList.Count() > 0)
                {
                    mainHelper.CallbackTuntianArmy(account,Constant.defenseIndex);
                }
            }
        }
        private void DefenseBattle(AccountModel account, Battle battle)
        {
            int countryId = Constant.getNCountry(account.typeOfCountry);
            if (battle.type ==4)
            {
                callbackArmy(account,battle,countryId);
            }
            else
            {
                if (!string.IsNullOrEmpty(soldierType[countryId]))
                {
                    foreach (village village in account.village)
                    {
                        if (battle.CitySelected == 1)//仅主陪
                        {
                            if (village.Ismain != "1" && village.IsAssistant != "1")
                                continue;
                        }
                        if (battle.CitySelected == 2)//不包括主陪
                        {
                            if (village.Ismain == "1" || village.IsAssistant == "1")
                                continue;
                        }
                        if (battle.type == 0 || battle.type == 1)//攻击或掠夺
                        {
                            string[] soldierTypes = soldierType[countryId].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            string soldierlist = string.Empty;
                            for (var i = 0; i < soldierTypes.Length; i++)
                            {
                                if (battle.Pattern != 4)
                                {
                                    //double percent = Math.Round((float)Convert.ToInt32(battle.DefensePattern.Replace("%", "")) / 100, 2);
                                    //string soldiernum=(Convert.ToInt32(village.soldierss[i])*percent).ToString().Split(new char[]{'.'})[0];
                                    string soldiernum = mainHelper.doubleous(village.soldierss[i], battle.DefensePattern);
                                    if (Convert.ToInt32(soldiernum) > 0 && Convert.ToInt32(soldiernum)<defenseUpperAmount)
                                        soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(soldiernum) + "&";
                                    else
                                        soldierlist += "soldier[" + i + "]=" + defenseUpperAmount + "&";
                                }
                                else
                                {
                                    string num = battle.BattleNum;
                                    if (Convert.ToInt32(num) > 0 && Convert.ToInt32(village.soldierss[i])>0)
                                    {
                                        if ((Convert.ToInt32(village.soldierss[i]) > Convert.ToInt32(num) || Convert.ToInt32(village.soldierss[i]) > defenseUpperAmount))
                                        {
                                            int plancount = Convert.ToInt32(num) > defenseUpperAmount ? defenseUpperAmount : Convert.ToInt32(num);
                                            soldierlist += "soldier[" + i + "]=" + plancount + "&";
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(Convert.ToInt32(village.soldierss[i])) > 0)
                                                soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(village.soldierss[i]) + "&";
                                        }
                                    }
                                }
                            }
                            string str = mainHelper.defensebattle(battle.x, battle.y, village, account, soldierlist, Constant.defenseIndex, battle.type.ToString());
                            if (str.Contains("目标不正确") || str.Contains("不可以发兵攻打") || str.Contains("不允许增援")) return;
                            if (str.Contains("搬迁24小时内不允许出兵")) continue;
                        }
                        else if (battle.type == 3)//支援模式
                        {
                            string[] soldierTypes = soldierType[countryId].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            string soldierlist = string.Empty;
                            for (var i = 0; i < soldierTypes.Length; i++)
                            {
                                if (battle.Pattern != 4)
                                {
                                    //double percent = Math.Round((float)Convert.ToInt32(battle.DefensePattern.Replace("%", "")) / 100, 2);
                                    //string soldiernum=(Convert.ToInt32(village.soldierss[i])*percent).ToString().Split(new char[]{'.'})[0];
                                    string soldiernum = mainHelper.doubleous(village.soldierss[i], battle.DefensePattern);
                                    if (Convert.ToInt32(soldiernum) >=0 && Convert.ToInt32(soldiernum) < defenseUpperAmount)
                                        soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(soldiernum) + "&";
                                    else
                                        soldierlist += "soldier[" + i + "]=" + defenseUpperAmount+ "&";
                                }
                                else
                                {
                                    string num = battle.BattleNum;
                                    if (Convert.ToInt32(num) > 0)
                                    {
                                        if (Convert.ToInt32(village.soldierss[i]) > Convert.ToInt32(num) || (Convert.ToInt32(village.soldierss[i]) > defenseUpperAmount) && defenseUpperAmount > 0)
                                        {
                                            if (Convert.ToInt32(num)<defenseUpperAmount)
                                                soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(num) + "&";
                                            else
                                                soldierlist += "soldier[" + i + "]=" + defenseUpperAmount + "&";
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(Convert.ToInt32(village.soldierss[i])) > 0)
                                                soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(village.soldierss[i]) + "&";
                                        }
                                    }
                                }
                            }
                            string str = mainHelper.defensebattle(battle.x, battle.y, village, account, soldierlist, Constant.defenseIndex, battle.type.ToString());
                            if (str.Contains("目标不正确") || str.Contains("不可以发兵攻打") || str.Contains("不允许增援")) return;
                            if (str.Contains("搬迁24小时内不允许出兵")) continue;
                        }                       
                        else if (battle.type == 5)//展示
                        {
                            string soldierbang = string.Empty;
                            Type type = village.soldiers.GetType();
                            PropertyInfo[] properties = type.GetProperties();
                            foreach (var item in properties)
                            {
                                string value = item.GetValue(village.soldiers).ToString();
                                if (string.IsNullOrWhiteSpace(value))
                                    continue;
                                soldierbang += Constant.m_strSoldierNames[countryId, Convert.ToInt32(item.Name.Replace("soldier", ""))] + ":" + value + ",";
                            }
                            refreshLog(account.chief + "  的城镇 " + village.VillageName, Constant.defenseIndex);
                        }
                    }
                } 
            }            
        }
        /// <summary>
        /// 召回驻军
        /// </summary>
        /// <param name="account"></param>
        /// <param name="battle"></param>
        /// <param name="countryId"></param>
        private void callbackArmy(AccountModel account, Battle battle,int countryId)
        {
            string bid=string.Empty;
            foreach (var village in account.village)
            { 
                bid=string.Empty;
                Building building=village.buildings.Where(build=>build.buildingName=="中军帐").FirstOrDefault();
                if(building!=null)
                {
                    mainHelper.CallbackLocSoldiers(village, bid, battle.x, battle.y, account, Constant.defenseIndex);
                }                
            }
        }
        //private void displayArmy()
        //{ }
        //private void investArmy()
        //{ }

       
        private void defenseBattlePause_Click(object sender, EventArgs e)
        {
            defensePause = true;
        }

        private void DefenseBattleStop_Click(object sender, EventArgs e)
        {
            WarQueue.Clear();
        }

        //continue/break
        /// <summary>
        /// 资源升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Construct_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> buildingList = new List<string>();
                foreach (Control upgradeBuilding in buildingsCollection.Controls)
                {
                    if (upgradeBuilding is CheckBox)
                    {
                        if ((upgradeBuilding as CheckBox).Checked)
                        {
                            buildingList.Add((upgradeBuilding as CheckBox).Text);
                        }
                    }
                }

                List<string> resourceList = new List<string>();
                foreach (Control upgradeResource in resourseCollection.Controls)
                {
                    if (upgradeResource is CheckBox)
                    {
                        resourceList.Add((upgradeResource as CheckBox).Text);
                    }
                }
                string createBuildingText=CreateBuildingName.Text;
                Thread upGradeThr = new Thread(delegate() { Upgrade(buildingList, createBuildingText, resourceList); });
                upGradeThr.Start();
            //    foreach (AccountModel account in accountList)
            //    {
            //        foreach (village village in account.village)
            //        {
            //            var spaceBuiding = village.buildings.Where(building => (!string.IsNullOrEmpty(building.buildingName)));
            //            int createbtid = Constant.GetBldTypeByName(CreateBuildingName.Text);
            //            if (spaceBuiding.Count() > 0 && createbtid != -1)
            //            {
            //                string createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
            //            }
            //            foreach (Control upgradeBuilding in buildingsCollection.Controls)
            //            {
            //                if (upgradeBuilding is CheckBox)
            //                {
            //                    if ((upgradeBuilding as CheckBox).Checked)
            //                    {
            //                        string objBuildingName = (upgradeBuilding as CheckBox).Text;
            //                        string upgradebtid = Constant.GetBldTypeByName(objBuildingName).ToString();
            //                        var compatiableBuilding = village.buildings.Where(building => building.buildingName == objBuildingName && building.buidinglevel<20);
            //                        if (upgradebtid != "-1" && compatiableBuilding.Count() > 0)
            //                        {
            //                            Building building = compatiableBuilding.FirstOrDefault();
            //                            string upgradMessage = mainHelper.BuildingUpgrade(village, building.buildingId, account, 0);
            //                            if(upgradMessage.Contains("已有建筑任务"))
            //                            break;                                         
            //                        }
            //                    }
            //                }
            //            }
            //            foreach (Control upgradeResource in resourseCollection.Controls)
            //            {
            //                if (upgradeResource is CheckBox)
            //                {
            //                    string objResourceName = (upgradeResource as CheckBox).Text;
            //                    var compatiableResource = village.resources.Where(resource => resource.resourceName == objResourceName);
            //                    if (compatiableResource.Count() > 0)
            //                    {
            //                        resourceInfo objResource = compatiableResource.FirstOrDefault();
            //                        if (Convert.ToInt32(objResource.resourceNum) < 20)
            //                        {
            //                            string objrid = objResource.resourceId;
            //                            string upgradeResourceMessage = mainHelper.UpgradeResources(village, objrid, account, 0);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            }
            catch (Exception ex)
            {

            }            
        }
        //建筑和资源升级
        private void Upgrade(List<string> buildings, string createBuildingText, List<string> resources)
        {
            string upgradMessage = string.Empty;
            foreach (AccountModel account in accountList)
            {
                foreach (village village in account.village)
                {
                    //创建并升级建筑，智能建设
                    #region
                    if (village.IsStateVillage == "1")
                        continue;
                    //创建建筑
                    if (string.IsNullOrEmpty(createBuildingText))
                    {
                        var spaceBuiding = village.buildings.Where(building => (!string.IsNullOrEmpty(building.buildingName)));                        
                        var existBuilding = village.buildings.Where(building => building.buildingName == createBuildingText);
                        int createbtid = Constant.GetBldTypeByName(createBuildingText);                        
                        string createMessage = string.Empty;
                        if (spaceBuiding.Count() > 0 && createbtid != -1)
                        {
                            if (existBuilding.Count() == 0)
                            {
                                var buildingBingshe = village.buildings.Where(building => building.buildingName == "兵舍");
                                var buildingBingsheLevel3 = village.buildings.Where(building => building.buildingName == "兵舍" && building.buidinglevel > 2);

                                var buildingZhongjun = village.buildings.Where(building => building.buildingName == "中军帐");
                                var buildingZhongjunlevel5 = village.buildings.Where(building => building.buildingName == "中军帐" && building.buidinglevel > 4);
                                var buildingZhongjunlevel20 = village.buildings.Where(building => building.buildingName == "中军帐" && building.buidinglevel == 20);

                                var buildingJiaochang = village.buildings.Where(building => building.buildingName == "校场");
                                var buildingJiaochanglevel3 = village.buildings.Where(building => building.buildingName == "校场" && building.buidinglevel > 2);
                                var buildingJiaochanglevel5 = village.buildings.Where(building => building.buildingName == "校场" && building.buidinglevel > 4);
                                var buildingJiaochanglevel10 = village.buildings.Where(building => building.buildingName == "校场" && building.buidinglevel > 9);
                                var buildingJiaochanglevle20 = village.buildings.Where(building => building.buildingName == "校场" && building.buidinglevel == 20);

                                var buildingBingqisi=village.buildings.Where(building=>building.buildingName=="兵器司");
                                var buildingBingqisilevel3 = village.buildings.Where(building => building.buildingName == "兵器司" && building.buidinglevel > 2);

                                var buildingGuandi = village.buildings.Where(building => building.buildingName == "官邸");
                                var buildingBieyuanlevel5 = village.buildings.Where(building => building.buildingName == "别院"&&building.buidinglevel>4);
                                var buildingGuandilevel5 = village.buildings.Where(building => building.buildingName == "官邸" && building.buidinglevel > 4);

                                var buildingXueguan=village.buildings.Where(building=>building.buildingName=="学馆");
                                var buildingXueguanlevel5=village.buildings.Where(building=>building.buildingName=="学馆"&&building.buidinglevel>4);
                                


                            兵舍:
                                if (createBuildingText == "兵舍")
                                {
                                    if (buildingZhongjun.Count() == 0)
                                    {
                                        createBuildingText = "中军帐";
                                    }
                                    createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }
                            伤兵营:
                                if (createBuildingText == "伤兵营")
                                {
                                    if (buildingZhongjun.Count() == 0)
                                    {
                                        createBuildingText = "中军帐";
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                    }                                    
                                    else if (buildingBingshe.Count() == 0)
                                    {
                                        createBuildingText = "兵舍";
                                        goto 兵舍;
                                    }
                                    else 
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                    
                                }
                            校场:
                                if (createBuildingText == "校场")
                                {
                                    if (buildingBingshe.Count() == 0)
                                    {
                                        createBuildingText = "兵舍";
                                        goto 兵舍;
                                    }
                                    else if (buildingBingqisilevel3.Count() == 0)
                                    {
                                        upgradMessage = UpdateBuilding(buildings, "兵舍", village, account);
                                        if (upgradMessage.Contains("已有建筑任务"))
                                            continue;
                                    }
                                    else
                                    {
                                        if (spaceBuiding.Count() == 0)
                                        {

                                        }
                                        else
                                        {
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                    }
                                         
                                }
                            别院:
                                if (createBuildingText == "别院")
                                {
                                    if (village.Ismain != "1")
                                    {
                                        if (buildingZhongjun.Count() == 0)
                                        {
                                            createBuildingText = "中军帐";
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                        else if (buildingZhongjunlevel5.Count() == 0)
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "中军帐", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                        else
                                        {
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }   
                                    }
                                    
                                }
                            官邸:
                                if (createBuildingText == "官邸")
                                {
                                    if (village.Ismain == "1")
                                    {
                                        if (buildingZhongjun.Count() == 0)
                                        {
                                            createBuildingText = "中军帐";
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                        else if (buildingZhongjunlevel5.Count() == 0)
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "中军帐", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                        else
                                        {
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                    }
                                }
                            兵器司:
                                if (createBuildingText == "兵器司")
                                {
                                    if (buildingJiaochang.Count() == 0)
                                    {
                                        createBuildingText = "校场";
                                        goto 校场;
                                    }
                                    else if (buildingJiaochanglevel3.Count() == 0)
                                    {
                                        upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                        if (upgradMessage.Contains("已有建筑任务"))
                                            continue;
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }
                            土木司:
                                if (createBuildingText == "土木司"||createBuildingText == "斥候营")
                                {
                                    if (buildingJiaochang.Count() == 0)
                                    {
                                        createBuildingText = "校场";
                                        goto 校场;
                                    }
                                    else if (buildingJiaochanglevel5.Count() == 0)
                                    {
                                        upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                        if (upgradMessage.Contains("已有建筑任务"))
                                            continue;
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }                          
                            马场:
                                if (createBuildingText == "马场")
                                {
                                    if (!(buildingBingqisilevel3.Count() != 0 && buildingJiaochanglevel5.Count() != 0))
                                    {
                                        if (buildingBingqisi.Count() == 0)
                                        {
                                            createBuildingText = "兵器司";
                                            goto 兵器司;
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "兵器司", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                        if (buildingJiaochang.Count() == 0)
                                        {
                                            createBuildingText = "校场";
                                            goto 校场;
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;

                                        }
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }

                            学馆:
                                if (createBuildingText == "学馆")
                                {
                                    if (buildingJiaochang.Count() == 0)
                                    {
                                        createBuildingText = "校场";
                                        goto 校场;
                                    }
                                    else if (buildingJiaochanglevel10.Count() == 0)
                                    {
                                        upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                        if (upgradMessage.Contains("已有建筑任务"))
                                            continue;
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }
                            工匠坊:
                                if (createBuildingText == "工匠坊")
                                {
                                    if (buildingJiaochang.Count() == 0)
                                    {
                                        createBuildingText = "校场";
                                        goto 校场;
                                    }
                                    else if (buildingJiaochanglevel10.Count() == 0)
                                    {
                                        upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                        if (upgradMessage.Contains("已有建筑任务"))
                                            continue;
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }
                            虎贲营:
                                if (createBuildingText == "虎贲营")
                                {
                                    if (!(buildingZhongjunlevel20.Count() != 0 && buildingJiaochanglevle20.Count() != 0))
                                    {

                                        if (buildingZhongjun.Count() == 0)
                                        {
                                            createBuildingText = "中军帐";
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "中军帐", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                        if (buildingJiaochang.Count() == 0)
                                        {
                                            createBuildingText = "校场";
                                            goto 校场;
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "校场", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;

                                        }
                                    }
                                    else
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                }
                            督造司:
                                if (createBuildingText == "督造司")
                                {
                                    if (village.Ismain == "1")
                                    {
                                        if (!(buildingGuandi.Count() != 0 && buildingXueguanlevel5.Count() != 0))
                                        {
                                            if (buildingGuandi.Count() == 0)
                                            {
                                                createBuildingText = "官邸";
                                                goto 官邸;
                                            }
                                            if (buildingXueguan.Count() == 0)
                                            {
                                                createBuildingText = "学馆";
                                                goto 学馆;
                                            }
                                            else
                                            {
                                                upgradMessage = UpdateBuilding(buildings, "学馆", village, account);
                                                if (upgradMessage.Contains("已有建筑任务"))
                                                    continue;
                                            }
                                        }
                                        else
                                        {
                                            createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                        }
                                    }
                                }
                            盟旗:
                                if (createBuildingText == "盟旗")
                                {
                                    if (village.Ismain == "1" && buildingGuandilevel5.Count() == 0)
                                    {
                                        if (buildingGuandi.Count() == 0)
                                        {
                                            createBuildingText = "官邸";
                                            goto 官邸;
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "官邸", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                    }
                                    else if (village.Ismain == "0" && buildingBieyuanlevel5.Count() == 0)
                                    {
                                        if (buildingGuandi.Count() == 0)
                                        {
                                            createBuildingText = "别院";
                                            goto 别院;
                                        }
                                        else
                                        {
                                            upgradMessage = UpdateBuilding(buildings, "别院", village, account);
                                            if (upgradMessage.Contains("已有建筑任务"))
                                                continue;
                                        }
                                    }
                                    else
                                    {
                                        createMessage = BuildingCreate(village, spaceBuiding, createbtid.ToString(), account, 0);
                                    }
                                }
                                
                            }
                             
                        }
                    }
                    //升级建筑
                    if(buildings.Count()>0)
                    foreach (var item in buildings)
                    {
                        string objBuildingName = item;                        
                        upgradMessage = UpdateBuilding(buildings, objBuildingName, village, account);
                        if (upgradMessage.Contains("已有建筑任务"))
                            continue;
                    }
                    #endregion
                    //升级资源
                    if(resources.Count()>0)
                    foreach (var item in resources)
                    {
                        var compatiableResource = village.resources.Where(resource => resource.resourceName == item);
                        if (compatiableResource.Count() > 0)
                        {
                            resourceInfo objResource = compatiableResource.FirstOrDefault();
                            if (Convert.ToInt32(objResource.resourceNum) < 20)
                            {
                                string objrid = objResource.resourceId;
                                string upgradeResourceMessage = mainHelper.UpgradeResources(village, objrid, account, 0);
                            }
                        }
                    }
                }
            }
              
        }
        /// <summary>
        /// 创建建筑
        /// </summary>
        /// <param name="village"></param>
        /// <param name="spaceBuiding"></param>
        /// <param name="btid"></param>
        /// <param name="account"></param>
        /// <param name="logIndex"></param>
        /// <returns></returns>
        public string BuildingCreate(village village, IEnumerable<Building> spaceBuiding, string btid, AccountModel account, int logIndex)
        {
            if (spaceBuiding.Count() == 0 && village.buildings.Count() > 0)
            {
              return  DestroyBuilding(Convert.ToInt32(btid), village, account);
            }
            else
            {
              return  mainHelper.BuildingCreate(village, spaceBuiding.FirstOrDefault().buildingId.ToString(), btid, account, Constant.resourceIndex);
            }
        }
        /// <summary>
        /// 拆除建筑
        /// </summary>
        /// <param name="bid"></param>
        /// <param name="createbtid"></param>
        /// <param name="village"></param>
        /// <param name="account"></param>
        private string DestroyBuilding(int createbtid, village village, AccountModel account)
        {
            var cangkus = village.buildings.Where(build => build.buildingName == "仓库").OrderBy(build=>build.buidinglevel);
            var liangcangs = village.buildings.Where(build => build.buildingName == "粮仓").OrderBy(build => build.buidinglevel);
            Building destroyBuilding = new Building();
            if (cangkus.Count() > 10 && createbtid != 1)
            {
                destroyBuilding = cangkus.FirstOrDefault();
                goto destroy;
            }
            if (liangcangs.Count() > 4 && createbtid != 0)
            {
                destroyBuilding = liangcangs.FirstOrDefault();
                goto destroy;
            }
            return "没有可以拆的建筑";
        destroy:
           return mainHelper.DestroyBuilding(destroyBuilding.buildingId, village.VillageID, account).ToString();
        }
        private string UpdateBuilding(List<string> buildings, string objBuildingName, village village,AccountModel account)
        {
            string upgradebtid = Constant.GetBldTypeByName(objBuildingName).ToString();
            var compatiableBuilding = village.buildings.Where(building => building.buildingName == objBuildingName && building.buidinglevel < 20);
            string upgradMessage=string.Empty;
            if (upgradebtid != "-1" && compatiableBuilding.Count() > 0)
            {
                Building building = compatiableBuilding.FirstOrDefault();
                upgradMessage = mainHelper.BuildingUpgrade(village, building.buildingId, account, 0);              
            }
            return upgradMessage;
            
        }
      
        private List<ListBox> logs;
        public void refreshLog(string log, int logindex=1)
        {
            //t = new Thread(delegate() { StartRefreshLog(log, logindex); });
            CommonDelegate.recordLog HandleRef = new CommonDelegate.recordLog(StartRefreshLog);
            HandleRef.Invoke(log, logindex);
            
            
        }
        public void refreshgrid(string chief,string city,string x,string y,string corp)
        {
            report.refreshgrid( chief, city, x, y, corp);            
            report.refreshGrid();
        }

        public void StartRefreshLog(string log, int logindex = 1)
        {

            if (logindex != 0)
            {
                if (logs[logindex-1].InvokeRequired||this.InvokeRequired)
                {
                    logs[logindex - 1].BeginInvoke(new MethodInvoker(() =>
                    {
                        //Mutex pro = new Mutex(true, "log" + logindex);
                        //try
                        //{
                            //pro.WaitOne();
                            logs[logindex - 1].Items.Add(log);
                            if (logs[logindex - 1].Items.Count >= 1000)
                            {
                                Monitor.Enter(MonitorObj1);
                                logs[logindex - 1].Items.Clear();
                                logs[logindex - 1].Refresh();
                                Monitor.Exit(MonitorObj1);
                            }
                            logs[logindex - 1].Refresh();
                                
                        //}
                        //finally
                        //{
                        //    try
                        //    {
                        //        pro.ReleaseMutex();
                        //    }
                        //    catch { }
                        //}
                    }));
                }
                else
                {
                    //Mutex pro = new Mutex(true, "log" + logindex);
                    //try
                    //{
                    //    pro.WaitOne();
                    logs[logindex - 1].Invoke(new MethodInvoker(() =>
                    {
                        int count = logs[logindex - 1].Items.Add(log);
                        if (count >= 1000)
                        {
                            Monitor.Enter(MonitorObj1);
                            logs[logindex - 1].Items.Clear();
                            logs[logindex - 1].Refresh();
                            Monitor.Exit(MonitorObj1);
                        }
                        logs[logindex - 1].Refresh();
                    }));
                        
                    //}
                    //finally
                    //{
                    //    try
                    //    {
                    //        pro.ReleaseMutex();
                    //    }
                    //    catch (Exception)
                    //    {
                    //    }
                    //}
                }   
            }
            else
            {
                report.addItem(log);
            }
                           
        }

        public void InitReport()
        {
            
        }

        private void migrate_start_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(migrate_x.Text.Trim()) && !(string.IsNullOrEmpty(migrate_y.Text.Trim())))
            {
                foreach (AccountModel account in accountList)
                {
                    MyItem item = mainHelper.SearchItem("", account);
                }                
            }
        }
       
        /// <summary>
        /// 运输
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TranStart_Click(object sender, EventArgs e)
        {
            cargo cargo = new cargo();
            if (string.IsNullOrEmpty(Trader_Target_X.Text.Trim()) || !IsWholeNumber(Trader_Target_X.Text.Trim()))
            {
                show("请输入正确的x坐标");
                return;
            }
            else
            {
                cargo.Trader_Target_X = Trader_Target_X.Text.Trim();
            }
            if (string.IsNullOrEmpty(Trader_Target_Y.Text.Trim()) || !IsWholeNumber(Trader_Target_Y.Text.Trim()))
            {
                show("请输入正确的y坐标");
                return;
            }
              else
            {
                cargo.Trader_Target_Y = Trader_Target_Y.Text.Trim();
            }
            List<string> thistr = new List<string>();
            thistr.Add(Trader_Resource_Clay.Text.Trim());
            thistr.Add(Trader_Resource_Crop.Text.Trim());
            thistr.Add(Trader_Resource_Iron.Text.Trim());
            thistr.Add(Trader_Resource_Lumber.Text.Trim());
            List<bool> result = new List<bool>();
            result = thistr.Select(item => { if (IsWholeNumber(item)) { if (Convert.ToInt32(item) > 0)return true; } return false; }).ToList();
            if (result.Where(item => item == true).Count() == 0)
            { show("请检查是否正确输入的比例"); return; }
            if (string.IsNullOrEmpty(Trader_Resource_Clay.Text.Trim()) || !IsWholeNumber(Trader_Resource_Clay.Text.Trim()))
            {
                cargo.Trader_Resource_Clay = 0;
            }
            else cargo.Trader_Resource_Clay = Convert.ToInt32(Trader_Resource_Clay.Text.Trim());
            if (string.IsNullOrEmpty(Trader_Resource_Crop.Text.Trim()) || !IsWholeNumber(Trader_Resource_Crop.Text.Trim()))
            {
                cargo.Trader_Resource_Crop = 0;
            }
            else cargo.Trader_Resource_Crop = Convert.ToInt32(Trader_Resource_Crop.Text.Trim());
            if (string.IsNullOrEmpty(Trader_Resource_Iron.Text.Trim()) || !IsWholeNumber(Trader_Resource_Iron.Text.Trim()))
            {
                cargo.Trader_Resource_Iron = 0;
            }
            else cargo.Trader_Resource_Iron = Convert.ToInt32(Trader_Resource_Iron.Text.Trim());
            if (string.IsNullOrEmpty(Trader_Resource_Lumber.Text.Trim()) || !IsWholeNumber(Trader_Resource_Lumber.Text.Trim()))
            {
                cargo.Trader_Resource_Lumber = 0;
            }
            else cargo.Trader_Resource_Lumber = Convert.ToInt32(Trader_Resource_Lumber.Text.Trim());
            if (!string.IsNullOrEmpty(restrictTime.Text.Trim()) || !IsWholeNumber(restrictTime.Text.Trim()))
            {
                cargo.restrictTime = restrictTime.Text.Trim();
            }
            else
            {
                cargo.restrictTime = "0";
            }

            TraderThr = new Thread(delegate() { StartTradeTask(cargo); });
            TraderThr.Start();
        }

        public bool IsWholeNumber(string strNumber)
        {
            Regex notWholePattern = new Regex(@"^[-]?\d+[.]?\d*$");
            return notWholePattern.IsMatch(strNumber, 0);
        }


        private void StartTradeTask(cargo cargo)
        {
            foreach (var account in accountList)
            {
                foreach (var village in account.village)
                {
                    try
                    {
                        var markets = village.buildings.Where(build => build.buildingName == "集市");
                        if (markets.Count() > 0)
                        {
                            Building market = markets.FirstOrDefault();
                            string url = "http://" + Constant.Server_Url + "/index.php?act=build.worksite&bid=" + market.buildingId + "&villageid=" + village.VillageID + "&rand=";
                            string regexStr = "(?<=(目前可运输最多资源.*?load_sum\">)).*?(?=(</span>))";
                            string maxCount = mainHelper.commonFunction(account, url, regexStr);

                            string PostStr = "Trader_Resource_Lumber=" + mainHelper.doubleous(maxCount, cargo.Trader_Resource_Lumber.ToString()) + "&Trader_Resource_Clay=" + mainHelper.doubleous(maxCount, cargo.Trader_Resource_Clay.ToString()) + "&Trader_Resource_Iron=" + mainHelper.doubleous(maxCount, cargo.Trader_Resource_Iron.ToString()) + "&Trader_Resource_Crop=" + mainHelper.doubleous(maxCount, cargo.Trader_Resource_Crop.ToString()) + "&trade_num=1&Trader_Target_X=" + cargo.Trader_Target_X + "&Trader_Target_Y=" + cargo.Trader_Target_Y + "&con_num=";
                            url = "http://" + Constant.Server_Url + "/index.php?act=build.act&btid=18&    do=startTrade&villageid=" + village.VillageID + "&rand=";
                            string readyTrade = mainHelper.PostForm(url, Constant.Server_Url, "http://" + Constant.Server_Url, account, PostStr);
                            if (readyTrade.Contains("外挂使用者"))
                            {
                                string logger = string.Format("被检测出使用外挂！！！，暂停运输", village.VillageName);
                                refreshLog(logger, Constant.cargoIndex);
                                break;
                            }
                            string time = mainHelper.getParaValue("(?<=(<td>需要时间.*?>)).+?(?=<)", readyTrade);
                            if (readyTrade.Contains("需要时间"))
                            {                                                        
                                if ((cargo.restrictTime != "0") && time.Contains(":"))
                                {
                                    double num1=0;                                    
                                    string[] splitTime = time.Split(new char[] { ':' });
                                    if (splitTime.Length == 3) num1 = 60 * Convert.ToInt32(splitTime[0])+Convert.ToInt32(splitTime[1]);
                                    if (splitTime.Length == 2) num1 = Convert.ToInt32(splitTime[0]);
                                    if(num1>Convert.ToInt32(cargo.restrictTime))
                                    {
                                        refreshLog(string.Format("城镇 {0} 运输到 {1} {2}超出时间限制 ", village.VillageName, cargo.Trader_Target_X, cargo.Trader_Target_Y), Constant.cargoIndex);
                                        continue;
                                    }                                    
                                }
                               
                            }
                            url = "http://" + Constant.Server_Url + "/index.php?act=build.act&btid=18&do=submitTrade&villageid=" + village.VillageID + "&rand=";
                            string TranResult = mainHelper.PostForm(url, Constant.Server_Url, "http://" + Constant.Server_Url, account, PostStr);
                            TranResult = mainHelper.getParaValue("(?<=(<locat act.*?>)).+?(?=</locat)", TranResult);
                            if (TranResult == "资源开始运输")
                            {
                                refreshLog(string.Format("城镇 {0} 将于{1} 运输到 {2} {3} ", village.VillageName, time, cargo.Trader_Target_X, cargo.Trader_Target_Y), Constant.cargoIndex);
                            }
                            else
                            {
                                refreshLog(string.Format("城镇 {0} 将于{1} 运输到 {2} {3} ", village.VillageName, time, cargo.Trader_Target_X, cargo.Trader_Target_Y), Constant.cargoIndex);
                            }

                        }
                        else
                        {
                            string logger = string.Format("城镇：{0}  没有集市哦！",village.VillageName);
                            refreshLog(logger, Constant.cargoIndex);
                        }
                    }
                    catch
                    { 
                        
                    }
                }
            }
        }
        /// <summary>
        /// 祭天
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Heaven_Starts_Click(object sender, EventArgs e)
        {            
            HeavenThr = new Thread(new ThreadStart(startHeavenTask));
            HeavenThr.Start();
        }


        private void startHeavenTask()
        {
            foreach (AccountModel account in accountList)
            {
                refreshLog(account.chief + "   开始祭天", Constant.otherIndex);
                foreach (village village in account.village)
                {
                    string url = "http://" + Constant.Server_Url + "/index.php?act=worship.heaven&villageid=" + village.VillageID + "&rand=";
                    string regexStr = "(?<=(今日还有.*?=\\\"#FF0000\\\">)).*(?=</font></strong>次机会免费使用祭天功能)";
                    int heaven = 0;
                getCount:
                    string count = mainHelper.commonFunction(account, url, regexStr);
                    if (!string.IsNullOrEmpty(count))
                    {
                        string url1 = "http://" + Constant.Server_Url + "/index.php?act=worship.heavendo&villageid=" + village.VillageID;
                        string returnResult = urlCommand.Html_get(url1, "http://" + Constant.Server_Url + "/", account);
                        if (returnResult.Contains("祭天成功！"))
                        {
                            heaven++;
                            refreshLog(village.VillageName + "祭天成功" + "  第 " + heaven + "次", Constant.otherIndex);
                        }
                        goto getCount;
                    }
                }
                refreshLog(account.chief + "   祭天完成", Constant.otherIndex);
            }
        }
        /// <summary>
        /// 搜索道具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchItem_Click(object sender, EventArgs e)
        {
            string itemName=ItemName.Text.Trim();
            if (!string.IsNullOrEmpty(itemName))
            {
                SearchItemThr = new Thread(delegate(){ ShowItem(itemName); });
                SearchItemThr.Start();
            }
        }


        private void ShowItem(string itemName)
        {
            string url = "http://" + Constant.Server_Url + "/index.php?act=store.itemdepot&page=1&iname=" + HttpUtility.UrlEncode(itemName) + "&rand=";
            string regexStr = "现有量：</strong>(?<count>\\d+)</li></ul>";
            foreach (AccountModel account in accountList)
            {
                MatchCollection matchs = mainHelper.commonFunction1(account, url, regexStr,"<html id=\"floatblockleft\"><!\\[CDATA\\[.*?(>\\]\\]></html>)");
                if (matchs.Count>0)
                foreach (Match match in matchs)
                {
                    refreshLog(account.chief + "  " + itemName + " 数量：" + match.Groups["count"].ToString(), Constant.otherIndex);
                }
                else
                    refreshLog(account.chief + "   没有 " + itemName, Constant.otherIndex);
            }
        }
        //从仓库查询并取出物品
        private void getItem(string ItemName)
        {
            string url = "http://" + Constant.Server_Url + "/index.php?act=storage.main&page=1&iname=" + HttpUtility.UrlEncode(ItemName)+"&rand=";
            //string regexStr = "(?<=(<strong>现有量.*?>)).+?(?=</li></ul>)";
            foreach (AccountModel account in accountList)
            {
            //    string itemCount = mainHelper.commonFunction(account, url, regexStr);
            //    if (!string.IsNullOrEmpty(itemCount) && itemCount != "0")
            //    {
            //        refreshLog(account.chief+": "+ItemName+" 数量："+itemCount, 5);
            //        regexStr = "storage.goback.*?(&num=)";
            //    }
                string regexStr = @"<strong>现有量.*?>(?<num>\w+)</li>.*?(?<url>storage.goback.*?')";
                MatchCollection matchs = mainHelper.commonFunction1(account, url, regexStr);
                if (matchs.Count > 0)
                {
                    foreach (Match match in matchs)
                    {
                        string url1 = match.Groups["url"].ToString().Substring(0, match.Groups["url"].ToString().Length-1);
                        string num1 = match.Groups["num"].ToString();
                        if (url1.Contains("num"))
                        {
                            url = "http://" + Constant.Server_Url + "/index.php?act=" + url1 + num1 + "&rand=";
                        }
                        else
                        {
                            url = "http://" + Constant.Server_Url + "/index.php?act=" + url1 + "&num=" + num1 + "&rand=";
                        }
                        string resultHtml = mainHelper.commonFunction(account,url,".*");
                        if (resultHtml.Contains("满"))
                        {
                            refreshLog(account.chief + "   仓库" + ": " + ItemName + " 数量：" + num1 + "无法取出" + "仓库已满", Constant.otherIndex);
                        }
                        else
                            refreshLog(account.chief + "   仓库" + ": " + ItemName + " 数量：" + num1, Constant.otherIndex);
                    }
                }
                else
                {
                    refreshLog(account.chief + "   仓库" + ": " + ItemName + "没有找到", Constant.otherIndex);
                }
            }            
        }
     
        /// <summary>
        /// 仓库取珠子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getPearl_Click(object sender, EventArgs e)
        {
            PearlThr = new Thread(delegate() { getItem("蓝田玉珠"); });
            PearlThr.Start();
        }
        //取道具
        private void depotItem_Click(object sender, EventArgs e)
        {
            string itemName = depotItemName.Text.Trim();
            if (string.IsNullOrEmpty(itemName))
            {
                show("请先输入道具名称");
                return;
            }
            //if (depotItemThr == null)
            //{
                depotItemThr = new Thread(delegate() { getItem(itemName); });
                depotItemThr.Start();
                return;  
            //}
            //else if (depotItemThr.ThreadState==ThreadState.Running)
            //{
            //    show("正在取"+itemName);
            //    return;
            //}                             
        }
        /// <summary>
        /// 刷珠子
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pearlBegin_Click(object sender, EventArgs e)
        {
            foreach (AccountModel account in accountList)
            {
                Thread thr = new Thread(delegate() { PearlStart(account); });
                thr.Start();
            }
        }

        private void PearlStart(AccountModel account)
        {
            List<village> villages = account.village.Where(item => item.Ismain == "0" && item.IsAssistant == "0" && item.IsStateVillage == "0").ToList();
            if (villages.Count() > 2)
            {
               var v1 = Convert.ToInt32(villages[0].VillageID);
               var v2 = Convert.ToInt32(villages[1].VillageID);
               var v3 = Convert.ToInt32(villages[2].VillageID);
               for (var i = 0; i < 800; i++)
               {
                   
                   mainHelper.changeOrder(account, v1, 1);
                   Thread.Sleep(100);
                   mainHelper.repeatClick(account, v1.ToString());                   
                   Thread.Sleep(100);
                   mainHelper.changeOrder(account, v2, 1);
                   Thread.Sleep(100);
                   mainHelper.changeOrder(account, v1, 4);
                   Thread.Sleep(100);
                   mainHelper.changeOrder(account, v3, 4);
                   Thread.Sleep(100);
               }
            }            
        }
        //private void repeatClick(AccountModel account,string villageId)
        //{
        //    string url = string.Empty;
        //    for (var i = 0; i <= 17; i++)
        //    {
        //        for (var j = 0; j < 10; j++)
        //        {
        //            url = "http://h92.sg.kunlun.com/index.php?act=resources.uppertop&_uintrid=" + i + "&userid=" + account .user_id+ "&villageid=" + villageId + "&w61bu=1bb0a5a&rand=753215";
        //            urlCommand.Html_get1(url, Constant.Server_Url, account);
        //            Thread.Sleep(5);
        //        }
        //    }
        //    Thread.Sleep(500);
        //}
        //private void changeOrder(AccountModel account,int orderid, int position)
        //{
        //    var url = "http://h92.sg.kunlun.com/index.php?act=vmanage.setAssistant&orderid=" + orderid + "&cid=" + orderid + "&keep=all&pos=" + position;
        //    urlCommand.Html_get1(url, Constant.Server_Url, account);
        //}

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //tickAccountList = new Dictionary<string, AccountTask>();
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
                        cookieHelper.setCookies(ref account);
                        CurrenList.Add(account);
                    }
                    refreshLog("账号循环开始", 0);

                    foreach (var account in CurrenList)
                    {
                        Interlocked.Increment(ref threadCount);
                        ThreadPool.QueueUserWorkItem(new WaitCallback(loginKunLunServer), account);                        
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
            this.account_manager.BeginInvoke(new Action(() => { this.account_manager.Enabled = true; }));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            report.refreshGrid();
            getCoin.BeginInvoke(new Action(() => { getCoin.Visible = true; }));
            account_manager.BeginInvoke(new Action(() => { account_manager.Visible = true; }));
            refreshLog("所有账号登陆已结束，可以做其他操作啦", 0);
        }
        /// <summary>
        /// 发动批量攻击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void attack_start_Click(object sender, EventArgs e)
        {
           
            Battle battle = new Battle();
            if (!string.IsNullOrEmpty(pattern.Text.Trim()))//多少
            {
                battle.Pattern = pattern.SelectedIndex;
            }
            else
            {
                pattern.SelectedIndex = 0;
                battle.Pattern = 0;
            }
            if (!string.IsNullOrEmpty(chooseCity.Text))//过滤
            {
                battle.CitySelected = chooseCity.SelectedIndex;
            }
            else
            {
                chooseCity.SelectedIndex = 0;
                battle.CitySelected = 0;
            }            

            if(yamiao.Checked)//是否压秒
            {
                DateTime ArriveTime = mainHelper.concact(date.Value, time.Value);
                if (ArriveTime < DateTime.Now)
                {
                    show("请输入正确的时间");
                    return;
                }
                else
                {
                    battle.ArriveTime = ArriveTime;
                }
            }
            
            attackSoldierType = new string[3];
            foreach (var control in bing.Controls)
            {
                if (control is CheckBox)
                {
                    CheckBox chk = control as CheckBox;
                    if (chk.Checked)
                    {
                        string checkboxName = chk.Name;
                        string soldierName = chk.Text;
                        for (int i = 0; i < 3; i++)
                        {
                            if (checkboxName.Contains(Constant.m_strCountryNames[i].ToLower()))
                            {
                                attackSoldierType[i] += Constant.GetSldTypeByName(i, soldierName) + ",";
                            }
                        }                            
                    }
                }
            }
            if (chongche.Checked && !string.IsNullOrEmpty(chong_num.Text.Trim()))
            {
                battle.ArrayChe = "soldier[5]=" + Convert.ToInt32(chong_num.Text.Trim());
            }
            if (piliche.Checked && !string.IsNullOrEmpty(pili_num.Text.Trim()))
            {
                battle.ArrayChe += "soldier[6]=" + Convert.ToInt32(pili_num.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(zhidingbingli.Text.Trim()))
            {
                battle.BattleNum = zhidingbingli.Text.Trim();
            }
            if (string.IsNullOrWhiteSpace(attackTime.Text.Trim()))
            {
                attackTime.SelectedIndex = 4;                
            }
            battle.attackRepeatTime = attackTime.Text.Trim();
            if (string.IsNullOrWhiteSpace(attackBuild.Text.Trim()))//建筑目标
            {
                attackBuild.SelectedIndex = 0;
            }
            battle.attackBuilding = attackBuild.Text.Trim();
           
            battle.AttackPattern = pattern.Text;
            this.attack_start88.BeginInvoke(new MethodInvoker(() => { this.attack_start88.Enabled = false; }));
            this.attack_suspend.BeginInvoke(new MethodInvoker(() => { this.attack_suspend.Enabled = true; }));
            this.attack_end.BeginInvoke(new MethodInvoker(() => { this.attack_end.Enabled = true; }));
            if (AttackThr == null || AttackThr.ThreadState == ThreadState.Aborted || AttackThr.ThreadState == ThreadState.Stopped)
            {
                AttackThr = new Thread(delegate() { Attack_Start(battle); });
                AttackThr.Start();
            }
            else
            {
                AttackThr.Resume();
            }            
        }

        private void attack_suspend_Click(object sender, EventArgs e)
        {
            if (AttackThr!=null&&AttackThr.ThreadState == ThreadState.Running)
            {
                AttackThr.Suspend();
                this.attack_start88.BeginInvoke(new MethodInvoker (() => { this.attack_start88.Enabled = true; }));
                this.attack_suspend.BeginInvoke(new MethodInvoker(() => { this.attack_suspend.Enabled = false; }));
                this.attack_end.BeginInvoke(new MethodInvoker(() => { this.attack_end.Enabled = true; }));
            }
        }

        private void attack_end_Click(object sender, EventArgs e)
        {
            if (AttackThr != null && AttackThr.ThreadState == ThreadState.Running || AttackThr.ThreadState == ThreadState.Suspended)
            {
                this.attack_start88.BeginInvoke(new MethodInvoker(() => { this.attack_start88.Enabled = true; }));
                this.attack_suspend.BeginInvoke(new MethodInvoker(() => { this.attack_suspend.Enabled = false; }));
                this.attack_end.BeginInvoke(new MethodInvoker(() => { this.attack_end.Enabled = false; }));
            }            
        }
        /// <summary>
        /// 导入攻击目标//包括笑看格式或excel格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void import_target_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                DataSet ds = new DataSet();
                ds = new ExcelHelper().getDataSource(filePath);
                this.Target.DataSource = ds.Tables[0].DefaultView.ToTable(false, "x", "y","city","chief","hand");
                this.Target.Columns[0].Name = "x";
                this.Target.Columns[0].HeaderText = "x";                
                this.Target.Columns[1].Name = "y";
                this.Target.Columns[1].HeaderText = "y";
                this.Target.Columns[2].Name = "city";
                this.Target.Columns[2].HeaderText = "城镇名";
                this.Target.Columns[3].Name = "chief";
                this.Target.Columns[3].HeaderText = "君主名";
                this.Target.Columns[4].Name = "hand";
                this.Target.Columns[4].HeaderText = "联盟";
                this.Target.Columns[0].Visible=true;
                this.Target.Columns[1].Visible=true;
                this.Target.Columns[2].Visible=true;
                this.Target.Columns[3].Visible=true;
                this.Target.Columns[4].Visible=true;
                this.Target.Columns[0].Width = 50;
                this.Target.Columns[1].Width = 50;
            }           
        }


        /// <summary>
        /// 开始攻击线程
        /// </summary>
        /// <param name="battle"></param>
        private void Attack_Start(Battle battle)
        {
            targets = new Queue<target>();
            foreach (target target in battle.targets)
            {
                targets.Enqueue(target);
            }
            int accountSuccessTime = 0;//压秒设置次数  
            foreach (var account in accountList)
            {
                int totaltime=0;   
                target target = new target();              
                 int countryId = Constant.getNCountry(account.typeOfCountry);
                 if (!string.IsNullOrEmpty(attackSoldierType[countryId]))
                 {
                     string[] soldiers=attackSoldierType[countryId].Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                     if (soldiers.Count() > 0)
                     {
                         foreach (village village in account.village)
                         {

                             if (battle.CitySelected == 1)
                             {
                                 if (village.Ismain != "1" && village.IsAssistant != "1")//仅主陪
                                     continue;
                             }
                             if (battle.CitySelected == 2)
                             {
                                 if (village.Ismain == "1" || village.IsAssistant == "1")//不含主陪
                                     continue;
                             }
                             string soldierlist = string.Empty;
                             for (int i = 0; i < soldiers.Count(); i++)
                             {
                                 if (battle.Pattern != 4)
                                 {
                                     string soldiernum = mainHelper.doubleous(village.soldierss[i], battle.AttackPattern);
                                     soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(soldiernum) + "&";
                                 }
                                 else
                                 {
                                     if (Convert.ToInt32(village.soldierss[i]) > Convert.ToInt32(battle.BattleNum))
                                     {
                                         soldierlist += "soldier[" + i + "]=" + Convert.ToInt32(battle.BattleNum) + "&";
                                     }
                                 }                                 
                                 //string result=mainHelper.PostForm()
                             }
                                 
                         }
                         if (getTarget(accountSuccessTime, Convert.ToInt32(battle.attackRepeatTime), ref target))
                         {
                             accountSuccessTime = 1;
                         }
                         else
                         {
                             accountSuccessTime++;
                         }
                     }
                 }
            }
        }

        public bool getTarget(int time,int sheding,ref target target)
        {
            if (time < sheding&&target!=null)
            {
                return false;
            }
            else
            {
                target = targets.Dequeue();
                return true;
            }
        }
        //陪都更名
        private void changname_Click(object sender, EventArgs e)
        {
            foreach (var account in accountList)
            {
                Thread thr = new Thread(delegate() { RenameCity(account); });
                thr.Start();
            }
        }
        private void RenameCity(AccountModel account)
        {
            var villages = account.village.Where(v => v.IsAssistant == "1");
            if (villages.Count() > 0)
            {
                List<string> villageName = new List<string>();
                foreach (village village in villages)
                {
                    string name = "tongwan" + 0;
                    int count = 0;
                    while (account.village.Where(v => v.VillageName == name).Count() > 0 ||villageName.Contains(name))
                    {
                        count++;
                        name = "tongwan" + count;
                    }
                    string str = mainHelper.renameVillage(account, village, name);
                    villageName.Add(name);
                    if (!string.IsNullOrEmpty(str))
                        refreshLog(str + " " + account.chief + "的城镇 " + village.VillageName + " 更名为  " + name, Constant.otherIndex);
                    else
                        refreshLog(account.chief + "的城镇 " + village.VillageName + " 更名失败", Constant.otherIndex);
                }               
            }
        }
        //升级资源田等级限制
        private void upRLimitLevel_Click(object sender, EventArgs e)
        {
            foreach (AccountModel account in accountList)
            {
                Thread upLimitedTh = new Thread(delegate() { UpR_limitLevl(account); });
                upLimitedTh.IsBackground = true;
                upLimitedTh.Start();
            }
        }

        private void UpR_limitLevl(AccountModel account)
        {
            List<village> peidus = account.village.Where(item => item.IsAssistant == "1").ToList();
            if (peidus.Count > 0)
            {
                foreach (village village in peidus)
                {
                    mainHelper.repeatClick(account, village.VillageID);
                    refreshLog(account.chief + "  城镇" + village.VillageName + "  资源田等级升级完毕", Constant.otherIndex);
                }
                refreshLog(account.chief + "  资源田等级升级完毕", Constant.otherIndex);
                
            }
        }

       /// <summary>
       /// 搬迁工作
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void migrate_start_Click_1(object sender, EventArgs e)
        {
            int x = 0; int y = 0;
            if (string.IsNullOrWhiteSpace(migrate_x.Text.Trim())||string.IsNullOrWhiteSpace(migrate_y.Text.Trim()))
            {
                show("请检查您的设置是否正确");
                return;
            }
            bool xTran=int.TryParse(migrate_x.Text.Trim(), out x);
            bool yTran=int.TryParse(migrate_y.Text.Trim(), out y);
            if (!(xTran || yTran))
            {
                show("请检查您的设置是否正确");
                return;
            }
            Thread migratethread = new Thread(delegate() { startMigration(x, y); });
            migratethread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void startMigration(int x,int y)
        {
            refreshLog("搬迁开始", Constant.otherIndex);
            Migration migration = new Migration();         
            int distance = 1;
            int nextDistance = 0;
            string init_x = string.Empty;
            string init_y = string.Empty;
            Queue<string> coordinates = migration.get_XY_List(x, y, distance, out nextDistance);
            if (accountList.Count() == 0)
                return;
            else
            {
                foreach (var item in accountList)
                {
                    Queue<string> cities = new Queue<string>();
                    cities = migration.getMigrationList(item);
                    while (cities.Count() > 0)
                    {
                        string currentCityId = cities.Dequeue();
                        bool success = false;
                    deal_city:
                        while (coordinates.Count() > 0 && !success)
                        {
                            string coordinate = coordinates.Dequeue();
                            string[] array = coordinate.Split(new char[] { ',' });
                            init_x = array[0];
                            init_y = array[1];
                            if (migration.CanMigration(item, init_x, init_y))
                            {
                                success = migration.Migrate(item, init_x, init_y, currentCityId);                                
                            }
                        }
                        if (coordinates.Count() == 0)
                        {
                            distance = nextDistance;
                            coordinates = migration.get_XY_List(x, y, distance, out nextDistance);
                            goto deal_city;
                        }
                    }
                }
            }

            refreshLog("搬迁结束", Constant.otherIndex);            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                string foldername = folderBrowserDialog1.SelectedPath;
                DirectoryInfo dir=new DirectoryInfo(foldername);
                string filepath = string.Empty;
                
                FileInfo[] files = dir.GetFiles();
                if (files.Length > 0)
                {
                    foreach (var item in files)
                    {
                        filepath = item.FullName;
                        DBUti dbhelper = new DBUti(filepath);
                        DataSet ds = dbhelper.getAllAccount1();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (new SqlHelper().checkAccount3(row["user_id"].ToString(), dir.Name)) 
                                {
                                    AccountModel account = new AccountModel();
                                    account.username = row["username"].ToString();
                                    account.password = row["password"].ToString();
                                    account.Server_url = row["Server_url"].ToString();
                                    account.cookieStr = row["cookieStr"].ToString();
                                    account.city_num = row["city_num"].ToString();
                                    account.chief = row["chief"].ToString();
                                    account.user_id = row["user_id"].ToString();
                                    account.typeOfCountry = row["typeOfCountry"].ToString();
                                    account.rankOfNobility = row["rankOfNobility"].ToString();
                                    new SqlHelper().ExecuteInsert(account, dir.Name);
                                }
                            }
                        }
                    }
                }
            }
            refreshLog("数据录入完毕", 0);
        }

        private void Direct_city_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                RadioButton check = sender as RadioButton;
                if (check.Name != "Direct_city")
                    groupBox3.Visible = false;
                else
                    groupBox3.Visible = true;
            }
        }

        private void Direct_graincentre_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked)
            {
                groupBox3.Visible = false;
            }
        }

        private void getCoin_Click(object sender, EventArgs e)
        {
            Thread coinThread = new Thread(new ThreadStart(GetCoin));
            coinThread.Start();
        }
        /// <summary>
        /// 刷新获取铸币
        /// </summary>
        private void GetCoin()
        {
            if (accountList.Count() > 0)
            {
                refreshLog("获取铸币开始",Constant.resourceIndex);
                foreach (var account in accountList)
                {
                    string result = mainHelper.reserveRuneToRune(account);
                    refreshLog(result, Constant.resourceIndex);
                }
                refreshLog("获取铸币结束", Constant.resourceIndex);
            }
        }

        public string recognise(string path, int count = 0)
        {
            string result = string.Empty;
            int Softid = 0;
            int.TryParse("96001", out Softid);
            int CodeType = 0;
            int.TryParse("1104", out CodeType);

            long OutInt = 0;
            StringBuilder Dll_Out_Str = new StringBuilder(50);
            int ret = NetRecognizePic.C1_UpFile(path, Imgusername, this.MD5String(Imgpwd), Softid, CodeType, 0, 0, "", ref OutInt, Dll_Out_Str);

            if (ret != 0)
            {
                //result += "[" + DateTime.Now.ToString("HH:mm:ss") + "]错误代码:" + Convert.ToString(ret) + "\r\n";
                ////MessageBox.Show(strerr);
                if (count <= 3)
                {
                    count++;
                    recognise(path, count);
                }
                return "";
            }
            else
                result = Dll_Out_Str.ToString();
            return result;
        }
        public bool ImgValid()
        {
            int OutInt = 0;
            int ret = NetRecognizePic.C1_GetScore(Imgusername, this.MD5String(Imgpwd), ref OutInt);
            if (ret == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public string MD5String(string str)
        {
            if (str == "") return str;
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            return MD5String(b);
        }
        public static string MD5String(byte[] b)
        {
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

     

        private void enabledAuto_CheckedChanged_1(object sender, EventArgs e)
        {
            CheckBox enableAuto = sender as CheckBox;
            if (enabledAuto.Checked)
            {
                xmlHelper.saveConfig("enableAuto", "true");
                if (!string.IsNullOrEmpty(this.txtqUser.Text.Trim()) && !string.IsNullOrEmpty(this.txtqPwd.Text.Trim()))
                {
                    xmlHelper.saveConfig("txtqUser", this.txtqUser.Text.Trim());
                    xmlHelper.saveConfig("txtqPwd", this.txtqPwd.Text.Trim());
                }                
            }
            else
            {
                xmlHelper.saveConfig("enableAuto", "false");               
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "excel文档|*.xlsx;*.csv;*.xls;*.txt";
            openFileDialog1.InitialDirectory = @"C:\Users\liuhao\Desktop";
            DataSet excelds = new DataSet();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string excel_path = openFileDialog1.FileName;
                string File_type = Path.GetExtension(excel_path);
                if (!string.IsNullOrEmpty(excel_path) && (File_type == ".xlsx" || File_type == ".xls" || File_type == ".csv" || File_type == ".txt"))
                {
                    ExcelHelper excelHelper = new ExcelHelper();
                    excelds = excelHelper.getExeclData(excel_path);
                    if (excelds == null || excelds.Tables.Count == 0)
                    {
                        show("没有数据");
                        return;
                    }
                }
                else
                {
                    show("请选择正确的文件");
                    return;
                }
            }
            DataView dv = excelds.Tables[0].DefaultView;
            DataTable dt = dv.ToTable(true, "name", "password");

            DataSet DbDs = new DataSet();
            DbDs = DbHelper.getAllAccount();
            DataTable DbDt = DbDs.Tables[0];
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow item in dt.Rows)
            {
                if (DbDt.AsEnumerable().Where(a => a.Field<string>("username").ToString() == item["name"].ToString()).Count() == 0)
                {
                    rows.Add(item);
                }
            }
            if (rows.Count() > 0)
            {
                AccountModel account = new AccountModel();
                foreach (var item in rows)
                {
                    account.username = item["name"].ToString();
                    account.password = item["password"].ToString();
                    account.Server_url = Constant.Server_Url;
                    DbHelper.insertAccount(account);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string foldername = folderBrowserDialog1.SelectedPath;
                DirectoryInfo dir = new DirectoryInfo(foldername);
                string filepath = string.Empty;

                FileInfo[] files = dir.GetFiles();
                if (files.Length > 0)
                {
                    foreach (var item in files)
                    {
                        filepath = item.FullName;
                        DBUti dbhelper = new DBUti(filepath);
                        DataSet ds = dbhelper.getAllAccount1();
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                if (new SqlHelper().checkAccount4(row["username"].ToString(), dir.Name))
                                {
                                    AccountModel account = new AccountModel();
                                    account.username = row["username"].ToString();
                                    account.password = row["password"].ToString();
                                    account.Server_url = row["Server_url"].ToString();
                                    //account.cookieStr = row["cookieStr"].ToString();
                                    account.AccountName = row["AccountName"].ToString();
                                    new SqlHelper().ExecuteInsert1(account, dir.Name);
                                }
                            }
                        }
                    }
                }
            }
            refreshLog("数据录入完毕", 0);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string foldername = folderBrowserDialog1.SelectedPath;
                DirectoryInfo[] dirs=new DirectoryInfo(foldername).GetDirectories();
                if (dirs.Count() == 0)
                {
                    show("选中的文件夹中未包含任何子文件夹");
                    return;
                }
                else
                {
                    foreach (var item in dirs)
                    {
                        try 
                        {
                            if (!File.Exists(Path.Combine(item.FullName, "Users.db")))
                            {
                                if (!new InitialApp().writeDb(Path.Combine(item.FullName, "Users.db")))
                                {
                                    show("创建数据库文件失败");
                                    return;
                                }                                
                            }
                            DataTable dt = new SqlHelper().getTableData(item.Name);
                            if (dt == null)
                            {
                                show("读取数据失败");
                                return;
                            }
                            foreach (DataRow dr in dt.Rows)
                            {
                                DBUti dbhelper = new DBUti(Path.Combine(item.FullName, "Users.db"));
                                AccountModel currentAccount = new ComGeneric<AccountModel>().ConvertToModel(dr);
                                if (dbhelper.checkAccount3(currentAccount))
                                    dbhelper.insertAccount(currentAccount, "insert");
                            }
                        }
                        catch (Exception ex)
                        {
                            show("出现异常");
                            return;
                        }                     
                        
                    }
                }
                show("账号文件已生成成功");
                return;
            }
            else
            {
                show("请选择一个文件夹");
                return;  
            }
                
        }
    }
}
