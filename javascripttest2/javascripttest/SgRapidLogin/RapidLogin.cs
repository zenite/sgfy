using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using javascripttest.Properties;
using javascripttest.BLL;
using System.Net;
using System.Web;
using System.Threading;
using System.Windows.Forms;


namespace javascripttest.SgRapidLogin
{
    public partial class RapidLogin : Form
    {
        public RapidLogin()
        {
            InitializeComponent();
        }
        private Dictionary<string, AccountModel> accountDics;
        private string dbPath;
        private cookieHelper cookieHelper;
        private AccountModel CurrentAcc;
        private void RapidLogin_Load(object sender, EventArgs e)
        {
            this.BringToFront();
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Resources.CompanyName, "account.xlsx");
            cookieHelper = new cookieHelper();
            accountDics = new Dictionary<string, AccountModel>();
            button2.PerformClick();
            loadServer();
        }

        private void userList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string url = comboBox1.Text;            
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入服务器地址");
                return;
            }   
            AccountModel account = new AccountModel();
            accountDics.TryGetValue(userList.SelectedItem.ToString(), out account);
            CurrentAcc = account;
            CurrentAcc.Server_url = url.Replace("http://", "").Replace("/", "");
            cookieHelper.ClearIECookie();
            cookieHelper.ClearIECookie();
            //this.webBrowser1.BeginInvoke(new Action(()=>this.webBrowser1.Navigate(new Uri("https://www.hao123.com/")) ));
            //this.webBrowser1.Navigate(new Uri("https://www.hao123.com/"));
            Application.DoEvents();
            if (this.webBrowser1.Document!=null)
            

            cookieHelper.setCookies(ref CurrentAcc);
            Uri uri = new Uri(url);
            if (webBrowser1.Url!=null&&webBrowser1.Url.ToString().CompareTo(url) == 0)
            {
                this.webBrowser1.Document.InvokeScript("MM_xmlLoad", new object[] { "login.logout&cookie_stat=1" });                             
                Application.DoEvents();
                method_0();
                method_0();
                method_0();
                method_0();

                if ((this.webBrowser1.Document.Cookie.Count()) > 0)
                    //this.webBrowser1 = new WebBrowser();
                    this.webBrowser1.Document.Cookie.Remove(0, this.webBrowser1.Document.Cookie.Length);
               
            }

            //this.webBrowser1.BeginInvoke(new Action(() => this.webBrowser1.Navigate(uri))); 
            
            this.webBrowser1.Navigate(uri);
        }


        private void method_0()
        {
            Application.DoEvents();
            Thread.Sleep(200);
            Application.DoEvents();
            while (this.webBrowser1.ReadyState != WebBrowserReadyState.Complete)
            {
                Thread.Sleep(200);
                Application.DoEvents();               
            }
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string currentUrl = comboBox1.Text;
            if ((e.Url.ToString()+"/").CompareTo("http://go.kunlun.com/?ref=" + currentUrl)==0)
            {
                HtmlElement elementById = this.webBrowser1.Document.GetElementById("username");
                if (elementById != null)
                {
                    elementById.SetAttribute("value", this.CurrentAcc.username);
                }
                HtmlElement element2 = this.webBrowser1.Document.GetElementById("userpass");
                if (element2 != null)
                {
                    element2.SetAttribute("value", this.CurrentAcc.password);
                }                
            }
            else if (e.Url.ToString().CompareTo(currentUrl) == 0)
            {               
                  CurrentAcc.cookieStr = this.webBrowser1.Document.Cookie;                  
                  accountDics[CurrentAcc.username] = CurrentAcc;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", dbPath);       
        }

        public DataSet InitialAccountList()
        {            
            ExcelHelper excelHelper = new ExcelHelper();
            DataSet ds = new DataSet();
            ds = excelHelper.getExeclData(dbPath);
            return ds;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = InitialAccountList();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    AccountModel account=new AccountModel();
                    account.username = dr["账号"].ToString().Trim();
                    account.password = dr["密码"].ToString().Trim();
                    accountDics.Add(account.username, account);
                    userList.Items.Add(account.username); 
                }
            }
        }

        private void loadServer()
        {
            DataSet urlDs = xmlHelper.getXmlData();
            if (urlDs.Tables.Count > 0 && urlDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in urlDs.Tables[0].Rows)
                {
                    comboBox1.Items.Add("http://"+dr["url"].ToString()+"/");
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Document.InvokeScript("MM_xmlLoad", new object[] { "login.logout&cookie_stat=1" });
            this.webBrowser1.Refresh();
            Application.DoEvents();
            string url = comboBox1.Text;
            Uri uri = new Uri(url);
            this.webBrowser1.Navigate(uri);
        }

    }
}
