using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Text;
using javascripttest.DbHelper;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }        
        private UrlCommand urlCommand;
        private cookieHelper cookieHelper;
        private DBUti DbHelper;
        private List<AccountModel> CurrenList = new List<AccountModel>();
        private void test_Load(object sender, EventArgs e)
        {
          urlCommand=new UrlCommand();
          cookieHelper = new cookieHelper();
          DbHelper = new DBUti();
          Constant.Server_Url = "http://h90.sg.kunlun.com";
          Initial_urlBox();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string user = username.Text.Trim();
            string pass = password.Text.Trim();
            string destination_url = string.Empty;
            string login_serverurl = "h90.sg.kunlun.com";
            string server_url = "http://" + login_serverurl;
            //获取验证码图片
            AccountModel account = new AccountModel(); 
            Login_Start:
            string image_path=(string)urlCommand.getImage(ref account, "login.kunlun.com", "http://sg.kunlun.com/");
            dialogbox dialog = new dialogbox();
            Image tmp_img = Image.FromFile(image_path);
            Image img_copy =new Bitmap(tmp_img);
            dialog.CodePicture.Image = img_copy;
            tmp_img.Dispose();
            string code = string.Empty;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                code = dialog.CodeTextBox.Text;
            }
            else//record error
            { 
                
            }

            string form_str = "username=" + user + "&userpass=" + pass + "&returl=" + HttpUtility.UrlEncode("http://static.kunlun.com/v3/login_sso_js_v1/callback.html") + "&usercode=" + code;
            string hostUrl="login.kunlun.com";
            string referenceUrl = "http://sg.kunlun.com/";
            string OriginUrl = "http://sg.kunlun.com";
            string login_url = "http://login.kunlun.com/index.php?act=user.webLogin";
            urlCommand.PostForm(login_url, hostUrl, referenceUrl, OriginUrl,ref account,form_str);
            //登陆官网，获取相应cookie
            string serverurl=server_url+"/index.php?act=login.form";
            urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
            //登陆相应服务器，判断路程
            urlCommand.urlPost(server_url, ref account, login_serverurl, "http://game.sg.kunlun.com/?sid=h90");
            urlCommand.urlPost(serverurl, ref account, login_serverurl, server_url);
            cookieHelper.GetSsid(ref account, server_url);
            //登陆相应账号，获取账号相关的cookie            
            if (account.hasMulti=="0")
            {
                IEnumerable<string> user_ids = null;
                destination_url = server_url + "/index.php?act=login.rolelist";
                urlCommand.urlPost(destination_url, ref account, login_serverurl, serverurl, true);
                //RegexHtml.getList(SGEnum.RegexType.User_id, account.extreHtml, out user_ids);
                List<string> LoginInList = DbHelper.checkAccount(account);
                foreach (var user_id in user_ids)
                {
                    if ((from item in LoginInList where item == user_id select item).Count() == 0)
                    {
                        account.user_id = user_id;
                        goto Login_Server;
                    }                    
                }
            Login_Server:
                string role_loginUrl = server_url + "/index.php?act=login.role&user_id=" + account.user_id;
                string role_loginrefernce = destination_url;
                urlCommand.urlPost(role_loginUrl, ref account, login_serverurl, role_loginrefernce, true);
                cookieHelper.GetSsid(ref account, server_url);
                StoreAccount(account);
                if (user_ids.Count()!=LoginInList.Count+1)
                goto Login_Start;
            }
            else
            {
                StoreAccount(account);
            }
            //直接登录
            //存储该账号的cookie
        }

        /// <summary>
        /// 存储账号
        /// </summary>
        /// <param name="account"></param>
        private void StoreAccount(AccountModel account)
        {
            try
            {
                if (DbHelper.InsertOrUpdate(account))
                    DbHelper.insertAccount(account);
                else
                    DbHelper.updateAccount(account);
                
            }
            catch (Exception ex)//记录错误
            {

            }            
        }        
        /// <summary>
        /// 上传账号文件并与数据库同步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //DbHelper.test();            
            openFileDialog1.Filter = "excel文档|*.xlsx";
            openFileDialog1.InitialDirectory = @"C:\Users\liuhao\Desktop";
            DataSet ds = new DataSet();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string excel_path = openFileDialog1.FileName;
                string File_type = Path.GetExtension(excel_path);
                if (!string.IsNullOrEmpty(excel_path) && (File_type == ".xlsx" || File_type == ".xls"))
                {
                    ExcelHelper excelHelper = new ExcelHelper();
                    ds = excelHelper.getExeclData(excel_path);
                    if (ds == null || ds.Tables.Count == 0)
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
            DataView dv = ds.Tables[0].DefaultView;
            DataTable dt = dv.ToTable(true, "账号", "密码");

            DataSet DbDs = new DataSet();
            DbDs = DbHelper.getAllAccount();
            DataTable DbDt = DbDs.Tables[0];
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow item in dt.Rows)
            {
                if (DbDt.AsEnumerable().Where(a => a.Field<string>("username").ToString() == item["账号"].ToString()).Count() == 0)
                {
                    rows.Add(item);
                }
            }

            if (rows.Count() > 0)
            {
                AccountModel account = new AccountModel();
                foreach (var item in rows)
                {
                    account.username = item["账号"].ToString();
                    account.password = item["密码"].ToString();
                    account.Server_url = Constant.Server_Url;
                    DbHelper.insertAccount(account);
                }
            }
        }


        private void show(string message)
        {
            MessageBox.Show(message);
        }
        private void getUser_id(ref AccountModel account)
        {
            
        }

        private void username_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            this.button1.PerformClick();
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            this.button1.PerformClick();
        }
        private void Initial_urlBox()
        {
            DataSet urlDs = xmlHelper.getXmlData();
        }
    }
}
