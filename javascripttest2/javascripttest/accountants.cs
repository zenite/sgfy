using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using javascripttest.DbHelper;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class accountants : Form
    {
        public accountants(Dictionary<string, AccountModel> accoutDics)
        {
            InitializeComponent();
            this.accoutDics = accoutDics;
            autoAttacks = new List<AutoAttack>();
        }
        private DBUti dbHelper ;
        private RapidLogin rapidForm;
        public MainLogic mainHelper;
        public Regular.SetConfig mainConfig;
        public Dictionary<string, AccountModel> accoutDics;
        private List<AutoAttack> autoAttacks;
        public void refreshAccounts(AccountModel refreshAccount)
        {
            AccountModel outAccount = new AccountModel();
            accoutDics.TryGetValue(refreshAccount.user_id,out outAccount);
            if (outAccount != null)
                accoutDics[refreshAccount.user_id] = refreshAccount;
            else
            {
                accoutDics.Add(refreshAccount.user_id, refreshAccount);
            }
            if (rapidForm.currentAccount.user_id == refreshAccount.user_id)
                rapidForm.currentAccount = refreshAccount;
            if (autoAttacks.Count > 0)
            {
                for (var i = autoAttacks.Count - 1; i >= 0; i--)
                {
                    if (autoAttacks[i] == null || autoAttacks[i].IsDisposed)
                    {
                        autoAttacks.Remove(autoAttacks[i]);
                    }
                }
                AutoAttack AutoAttack = autoAttacks.Find(item => item.account.user_id == refreshAccount.user_id);
                AutoAttack.account = refreshAccount;                
            }
        }
        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            AccountModel account = new AccountModel();            
            account.username = this.T_username.Text.Trim() ;
            account.password = this.T_password.Text.Trim();
            if (string.IsNullOrEmpty(account.username) || string.IsNullOrEmpty(account.password))
                MessageBox.Show("请确认您的账号密码正确");
            account.Server_url = Constant.Server_Url;
            if (dbHelper.checkAccount2(account))
            if (!dbHelper.insertAccount(account))
            {
                MessageBox.Show("添加失败");
            }
            search.PerformClick();
        }

        private void delete_account_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    string username = (item.DataBoundItem as DataRowView).Row["username"].ToString();
                    dbHelper.deleteAccounts("username", username);
                }
                accountants_Load(sender, e); 
            }
            else
            {
                MessageBox.Show("请选择要删除的账号");
            }
            
        }

        private void accountants_Load(object sender, EventArgs e)
        {
            dbHelper = new DBUti();
            DataSet ds = new DataSet();
            ds = dbHelper.getAllAccount();
            //this.dataGridView1.AutoGenerateColumns = false;
            showGridView(ds);
            
        }
        private void showGridView(DataSet ds)
        {
            this.dataGridView1.DataSource = ds.Tables[0].DefaultView.ToTable(false, "chief", "typeOfCountry", "rankOfNobility", "user_id", "username", "password", "Server_url","city_num");
            for(var i=0;i< ds.Tables[0].Rows.Count;i++)
            {
                DataRow row = ds.Tables[0].Rows[i];
                if (row["AccountName"].ToString() == "1") {
                    this.dataGridView1.Rows[i].DefaultCellStyle.BackColor= Color.Red;
                }
            }
            //try
            //{
            //    this.dataGridView1.Columns[0].Name = "chief";
            //    this.dataGridView1.Columns[0].HeaderText = "君主名";
            //    this.dataGridView1.Columns[0].Visible = true;
            //    this.dataGridView1.Columns[1].Name = "typeOfCountry";
            //    this.dataGridView1.Columns[1].HeaderText = "国别";
            //    this.dataGridView1.Columns[1].Visible = true;
            //    this.dataGridView1.Columns[2].Name = "rankOfNobility";
            //    this.dataGridView1.Columns[2].HeaderText = "爵位";
            //    this.dataGridView1.Columns[2].Visible = true;
            //    this.dataGridView1.Columns[3].Name = "user_id";
            //    this.dataGridView1.Columns[3].HeaderText = "用户id";
            //    this.dataGridView1.Columns[3].Visible = true;
            //    this.dataGridView1.Columns[4].Name = "username";
            //    this.dataGridView1.Columns[4].HeaderText = "用户名";
            //    this.dataGridView1.Columns[4].Visible = true;
            //    this.dataGridView1.Columns[5].Name = "password";
            //    this.dataGridView1.Columns[5].HeaderText = "密码";
            //    this.dataGridView1.Columns[5].Visible = true;
            //    this.dataGridView1.Columns[6].Name = "Server_url";
            //    this.dataGridView1.Columns[6].HeaderText = "所属服务器";
            //    this.dataGridView1.Columns[6].Visible = true;
            //    this.dataGridView1.Columns[7].Name = "city_num";
            //    this.dataGridView1.Columns[7].HeaderText = "城镇个数";
            //    this.dataGridView1.Columns[7].Visible = true;
            //    DataGridViewButtonColumn button = new DataGridViewButtonColumn();
            //    button.HeaderText = "操作";
            //    button.UseColumnTextForButtonValue = true;
            //    button.Text = "战斗任务";
            //    this.dataGridView1.Columns[8].Name = "operation";
            //    this.dataGridView1.Columns[8].HeaderText = "操作";
            //    this.dataGridView1.Columns[8].Visible = true;
            //    this.dataGridView1.Columns.Add(button);

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }       

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            string user_id = ((sender as DataGridView).CurrentRow.DataBoundItem as DataRowView).Row["user_id"].ToString();                 
            ToRapidLogin_Click(user_id);
        }

        private void ToRapidLogin_Click(string user_id)
        {
            AccountModel account = new AccountModel();
            accoutDics.TryGetValue(user_id, out account);
            if (rapidForm == null || rapidForm.IsDisposed)
            {
                rapidForm = new RapidLogin(account);
                rapidForm.mainHelper = mainHelper;
                rapidForm.user_id = user_id;
                rapidForm.Show();
            }
            else
            {
                rapidForm.user_id = user_id;
                rapidForm.Show();
            }
            rapidForm.account_login();
        }
        private void ToAttack_Click(string user_id)
        {
            AccountModel account = new AccountModel();
            accoutDics.TryGetValue(user_id, out account);
            AutoAttack attack = new AutoAttack(account, user_id, mainHelper);
            attack.mainConfig = mainConfig;
            autoAttacks.Add(attack);
            attack.Show();      
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string user_id = ((sender as DataGridView).CurrentRow.DataBoundItem as DataRowView).Row["user_id"].ToString();
            ToRapidLogin_Click(user_id);
        }

        private void delete_account_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            }
        }

        private void delete_account_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                btn.BackColor = System.Drawing.SystemColors.Control;
            }           
        }

        private void search_Click(object sender, EventArgs e)
        {
            string chief = EmpireName.Text.Trim();
            DataSet ds = dbHelper.getAccounts("chief", chief);
            showGridView(ds);
        }


        private void import_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                DataSet excelds = new ExcelHelper().getExeclData(filePath);
                if (excelds == null || excelds.Tables.Count == 0)
                {
                    MessageBox.Show("没有数据");
                    return;
                }

                DataView dv = excelds.Tables[0].DefaultView;
                DataTable dt = dv.ToTable(true, "name", "password");

                DataSet DbDs = new DataSet();
                DbDs = new DBUti().getAllAccount();
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
                        new DBUti().insertAccount(account);
                    }
                }
            }

        }

        private void export_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                DataGridViewColumn column = this.dataGridView1.Columns[e.ColumnIndex];
                if (column is DataGridViewButtonColumn)
                {
                    string user_id = ((sender as DataGridView).CurrentRow.DataBoundItem as DataRowView).Row["user_id"].ToString();
                    ToAttack_Click(user_id);
                }
            }
        }

       
      
    }
}
