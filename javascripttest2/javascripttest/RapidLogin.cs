using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using javascripttest.DbHelper;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace javascripttest
{
    public partial class RapidLogin : Form
    {
        public RapidLogin(AccountModel currentAccount)
        {
            InitializeComponent();
            this.currentAccount = currentAccount;
        }
        

        private void webBr_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().Contains("act=resources.status&"))
            {
                Match match = mainHelper.commonRegex(e.Url.ToString(), @"villageid=.*?(?<num>\w+)"); 
                village_id=match.Groups["num"].ToString();
            }
        }

        private DBUti dbhelper;
        public MainLogic mainHelper;
        private cookieHelper cookieHelper;
        public  string user_id;
        private string village_id;
        public  AccountModel currentAccount;
        private void login()
        {
            
        }

        private void accountList_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void RapidLogin_Load(object sender, EventArgs e)
        {
            dbhelper = new DBUti();
            cookieHelper = new cookieHelper();
            
            DataSet ds=dbhelper.getAllAccount();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                listBox1.DataSource = ds.Tables[0];
                this.listBox1.DisplayMember = "username"; 
            }          
            Uri uri=new Uri("http://"+Constant.Server_Url+"/");
            this.webBr.Navigate(uri);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                cookieHelper.ClearIECookie();
                AccountModel account=new AccountModel();
                account.cookieStr = ((sender as ListBox).SelectedItem as DataRowView)["cookieStr"].ToString();
                account.Server_url = Constant.Server_Url;
                cookieHelper.setCookies(ref account);
                Uri uri = new Uri("http://" + Constant.Server_Url + "/");
                this.webBr.Navigate(uri);                
            }
            catch (Exception ex)
            {
                
            }
        }

        public void account_login()
        {
            int index = listBox1.Items.IndexOf((from item in listBox1.Items.Cast<DataRowView>() where item.Row["user_id"].ToString() == user_id select item).FirstOrDefault() as object);
            listBox1.SelectedIndex = index;
            EventArgs e = new EventArgs();
            listBox1_DoubleClick(listBox1 as object, e);            
        }

        private void RapidLogin_Resize(object sender, EventArgs e)
        {
            this.panel1.Height = this.Height;
            this.panel1.Width = this.Width;
        }

        private void upperCap_btn_Click(object sender, EventArgs e)
        {
            new Thread(delegate() 
                {
                    this.BeginInvoke(new Action(() =>{
                        updateTips.ForeColor = Color.OrangeRed;
                        updateTips.Text = "资源田正在升级，请稍等";
                        upperCap_btn.Enabled = false;
                    }) );
                    mainHelper.repeatClick(currentAccount, village_id);
                    this.BeginInvoke(new Action(() => {
                        upperCap_btn.Enabled = true;
                        updateTips.ForeColor = Color.Black;
                        updateTips.Text = "本城资源田升级完毕";
                    }));
                    MessageBox.Show("本城资源田升级完毕");
                }
                ).Start();
        }

        private void attackinTime_Click(object sender, EventArgs e)
        {

        }

        private void setCap_btn_Click(object sender, EventArgs e)
        {
            int position = 0;
            if (!string.IsNullOrEmpty(capLevel_txt.Text))
            {
                position = Convert.ToInt32(capLevel_txt.Text);
            }
            else
                position = 1;
            mainHelper.changeOrder(currentAccount,Convert.ToInt32(village_id), position);
        }
    }
}
