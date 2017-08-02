using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using javascripttest.entity;
using javascripttest.DbHelper;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace javascripttest
{
    public partial class AutoAttack : Form
    {
        private BLL.HandlerAttack offenseConfig;
        private BLL.HandlerAttack destroyConfig;
        public MainLogic mainHelper;
        private DBUti dbHelper;
        public AccountModel account;
        public string user_id;
        public string VillageId;
        public string[] villageSoldier;
        public DataTable dSource=null;
        private Control[] ActiveOffenseControl;
        private Control[] ActiveDestroyControl;
        private string[] nodeList;
        private List<entity.NodeAttack> offense;
        private List<entity.NodeAttack> destroy;
        public AutoAttack(AccountModel account, string user_id, MainLogic mainHelper)
        {
            this.account = account;
            this.user_id = user_id;
            this.nodeList = new string[] { "Controls", "Attacks" };
            this.mainHelper = mainHelper;
            InitializeComponent();
            offenseConfig = new BLL.HandlerAttack("Attack\\" + this.account.chief + "\\offense","AutoAttack", nodeList);
            destroyConfig = new BLL.HandlerAttack("Attack\\" + this.account.chief + "\\destroy", "AutoAttack", nodeList);
            offense = new List<NodeAttack>();
            destroy = new List<NodeAttack>();
            ActiveOffenseControl = new Control[] { Setting, this};
            ActiveDestroyControl = new Control[] { destroySettings, groupBox3 };
        }
        
        private void importCoor_Click(object sender, EventArgs e)
        {
            if (!CheckSetting())
                return;
            if (type.SelectedIndex < 0)
            {
                MessageBox.Show("请选择攻击模式");
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                DataSet ds = new DataSet();
                try
                {
                    ds = new ExcelHelper().getDataSource(filePath, VillageId);
                    if (!checkCurrentSolider(Setting))
                    {
                        MessageBox.Show("当前城镇出兵数量错误");
                        return;
                    }                    
                    //DataTable datasource = makeDataSource(ds.Tables[0]);
                    new Thread(delegate() {
                        offenseConfig.AttackAttribute(ds, "Attacks", "Attack");
                        ReBindOffense();
                    }).Start();
                   
                         
                }
                catch (Exception ex)
                { 
                    
                }
            }
            return;
        }

        #region generalSelectChanged
        private void general1_SelectedIndexChanged(object sender, EventArgs e)
        {
            doAttack(sender, e);
        }

        private void general2_SelectedIndexChanged(object sender, EventArgs e)
        {
            doAttack(sender, e);
        }

        private void general3_SelectedIndexChanged(object sender, EventArgs e)
        {
            doAttack(sender, e);
        }

        private void general4_SelectedIndexChanged(object sender, EventArgs e)
        {
            doAttack(sender, e);
        }

        private void general5_SelectedIndexChanged(object sender, EventArgs e)
        {
            doAttack(sender, e);
        }
        #endregion
        private void doAttack(object sender, EventArgs e)
        {
            string currentText = (sender as ComboBox).Text;
            Control.ControlCollection controls = Setting.Controls;
            foreach (Control control in controls)
            {
                if (control.Name.Contains("general")&&!control.Name.Equals((sender as ComboBox).Name))
                {
                    ComboBox comboBox = control as ComboBox;
                    string text = comboBox.Text;
                    if (currentText == text)
                    {
                        (sender as ComboBox).SelectedIndex = 0;
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 确保武将选择正确
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoAttack_Load(object sender, EventArgs e)
        {
            dbHelper = new DBUti();
            List<village> Villages = mainHelper.getVillages(account);
            villegelist.DataSource = toDataSource_V(Villages);
            villegelist.DisplayMember = "Name";
            villegelist.ValueMember = "Value";
            this.villegelist.SelectedIndexChanged += new System.EventHandler(this.villegelist_SelectedIndexChanged);

            BindTarget();
            Bind(Villages[0].VillageID);
            setOffenseControlState();
            setDestroyControlState();
        }

        private bool CheckSetting()
        {
            bool result = true;
            if (type.SelectedIndex==0&& general1.SelectedIndex == 0)
            {
                MessageBox.Show("请选择主将（攻击模式下）");
                result = false;
            }
            try
            {
                if (!(C(soldier_0) || C(soldier_1) || C(soldier_2) || C(soldier_4) || C(soldier_5) || C(soldier_6) || C(soldier_11)))
                {
                    MessageBox.Show("请填写兵力");
                    result = false;
                }
            }
            catch (Exception ex)
            { 
                
            }
            
            return result;
        }

        private bool CheckSetting1()
        {
            bool result = true;
            if (type.SelectedIndex == 0 && C_general1.SelectedIndex == 0)
            {
                MessageBox.Show("请选择主将（攻击模式下）");
                result = false;
            }
            try
            {
                if (!(C(C_soldier_0) || C(C_soldier_1) || C(C_soldier_2) || C(C_soldier_4) || C(C_soldier_5) || C(C_soldier_6) || C(C_soldier_11)))
                {
                    MessageBox.Show("请填写兵力");
                    result = false;
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        /// <summary>
        /// 检查当前出征的士兵数是否足够
        /// </summary>
        /// <returns></returns>
        private bool checkCurrentSolider(GroupBox sett,int flag=0)
        {
            bool result = true;
            Control.ControlCollection controls = sett.Controls;
            foreach (Control control in controls)
            { 
                if(control.Name.Contains("soldier") && control is TextBox)
                {
                    TextBox textbox = control as TextBox;
                    try 
                    {
                        if (Convert.ToInt32(villageSoldier[Convert.ToInt32(textbox.Name.Split('_')[flag+1])]) < Convert.ToInt32(string.IsNullOrEmpty(textbox.Text.Trim()) ? "0" : textbox.Text.Trim()))
                        {
                            result = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return result;
        }

        private bool C(TextBox textbox)
        {
            string value = string.IsNullOrEmpty(textbox.Text.Trim()) ? "0" : textbox.Text.Trim();       
            return Convert.ToInt32(value)>0?true:false;
        }
        public int M(string str)
        {
            try
            {
                return Convert.ToInt32(string.IsNullOrEmpty(str) ? "0" : str);
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        private void addCor_Click(object sender, EventArgs e)
        {

            if (!CheckSetting())
                return;
            if (type.SelectedIndex < 0)
            {
                MessageBox.Show("请选择攻击模式");
                return;
            }
            //Attack attack = getCurrentAttack();
            //if (dbHelper.insertAttack(attack))
            //{
            //    ReBindGrid();
            //}
            entity.NodeAttack attack = mainHelper.GetCityInfo(this.x_cor.Text, this.y_cor.Text, account);
            attack.VillageId = VillageId;
            if (attack.name != null)
            {
                offenseConfig.setAttribute(attack, "Attacks");
                ReBindOffense();
                
            }
          
            
        }

        private Attack getCurrentAttack() {
            Attack attack = new Attack();
            attack.x = x_cor.Text.Trim();
            attack.y = y_cor.Text.Trim();
            attack.T_general1 = general1.Text;
            attack.T_general2 = general2.Text;
            attack.T_general3 = general3.Text;
            attack.T_general4 = general4.Text;
            attack.T_general5 = general5.Text;
            attack.T_general1_ID = general1.SelectedValue.ToString();
            attack.T_general2_ID = general2.SelectedValue.ToString();
            attack.T_general3_ID = general3.SelectedValue.ToString();
            attack.T_general4_ID = general4.SelectedValue.ToString();
            attack.T_general5_ID = general5.SelectedValue.ToString();
            attack.T_soldier_0 = cText(soldier_0.Text.Trim());
            attack.T_soldier_1 =  cText(soldier_1.Text.Trim());
            attack.T_soldier_2 =  cText(soldier_2.Text.Trim());
            attack.T_soldier_4 =  cText(soldier_4.Text.Trim());
            attack.T_soldier_11 =  cText(soldier_11.Text.Trim());
            attack.T_soldier_5 =  cText(soldier_5.Text.Trim());
            attack.T_soldier_6 =  cText(soldier_6.Text.Trim());
           // attack.Aindex = getMaxIndex(user_id, VillageId).ToString()+1;
            attack.user_id = user_id;
            attack.villageId = VillageId;
            attack.Atype = type.SelectedIndex.ToString();
            attack.target1 = T_target1.SelectedValue.ToString() =="-1"?T_target1.SelectedValue.ToString():"0";
            attack.target2 = T_target2.SelectedValue.ToString() == "-1" ? T_target2.SelectedValue.ToString() : "0";
            return attack;
        }

        private DataTable toDataSource_V(List<village> Villages)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Value");
            dt.Columns.Add("Name");            
            foreach (var item in Villages)
            {
                DataRow dr = dt.NewRow();
                dr[0] = item.VillageID;
                dr[1] = item.VillageName;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// 城镇变化时选择的武将将会发生变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void villegelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox com = (sender as ComboBox);
            VillageId = (com.SelectedItem as DataRowView)[0].ToString();
            Bind(VillageId);//重新绑定武将
            setOffenseAttr(sender, e);
            setOffenseControlState();
            setDestroyControlState();
        }

        private void Bind(string villageId)
        {
            bindGeneral(villageId);
            getSodliers(villageId);
            VillageId = villageId;
            ReBindGrid();
        }
        private void bindGeneral(string villageId)
        {
            DataTable dt = toDataSource_G( villageId);
            bindGe1(dt);
            bindGe2(dt);
        }

        private void bindGe1(DataTable dt)
        {
            general1.DataSource = dt.Copy();
            general1.DisplayMember = "Name";
            general1.ValueMember = "Value";

            general2.DataSource = dt.Copy();
            general2.DisplayMember = "Name";
            general2.ValueMember = "Value";

            general3.DataSource = dt.Copy();
            general3.DisplayMember = "Name";
            general3.ValueMember = "Value";

            general4.DataSource = dt.Copy();
            general4.DisplayMember = "Name";
            general4.ValueMember = "Value";

            general5.DataSource = dt.Copy();
            general5.DisplayMember = "Name";
            general5.ValueMember = "Value";
        }
        private void bindGe2(DataTable dt)
        {
            C_general1.DataSource = dt.Copy();
            C_general1.DisplayMember = "Name";
            C_general1.ValueMember = "Value";

            C_general2.DataSource = dt.Copy();
            C_general2.DisplayMember = "Name";
            C_general2.ValueMember = "Value";

            C_general3.DataSource = dt.Copy();
            C_general3.DisplayMember = "Name";
            C_general3.ValueMember = "Value";

            C_general4.DataSource = dt.Copy();
            C_general4.DisplayMember = "Name";
            C_general4.ValueMember = "Value";

            C_general5.DataSource = dt.Copy();
            C_general5.DisplayMember = "Name";
            C_general5.ValueMember = "Value";
            DataTable gen = dt.Copy();
            gen.Rows.RemoveAt(0);
            GeneralList.DataSource = null;
            GeneralList.DataSource = gen;
            GeneralList.DisplayMember = "Name";
            GeneralList.ValueMember = "Value";
            
        }
        private void getSodliers(string villageId)
        {
            villageSoldier   = mainHelper.getSoldiers(account, villageId);
            setTextbox(soldier0, villageSoldier[0]);
            setTextbox(soldier1, villageSoldier[1]);
            setTextbox(soldier2, villageSoldier[2]);
            setTextbox(soldier4, villageSoldier[4]);
            setTextbox(soldier5, villageSoldier[5]);
            setTextbox(soldier6, villageSoldier[6]);
            setTextbox(soldier11  , villageSoldier[11]);
            setTextbox(C_soldier0 , villageSoldier[0]);
            setTextbox(C_soldier1 , villageSoldier[1]);
            setTextbox(C_soldier2 , villageSoldier[2]);
            setTextbox(C_soldier4 , villageSoldier[4]);
            setTextbox(C_soldier5 , villageSoldier[5]);
            setTextbox(C_soldier6 , villageSoldier[6]);
            setTextbox(C_soldier11 , villageSoldier[11]);
        }
        public void setTextbox(Label lable,string str)
        {
            if (lable.InvokeRequired)
            {
                lable.BeginInvoke(new Action(() =>
                {
                    lable.Text = str;
                }));
            }
            else
                lable.Text = str;
        }
        private DataTable toDataSource_G( string villageId)
        {
            List<General> list = mainHelper.GetVillageGenerals(account, villageId);
            DataTable dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("Value");
            DataRow firstRow = dt.NewRow();
            firstRow[0] = "";
            firstRow[1] = -1;
            dt.Rows.Add(firstRow);
            foreach (General general in list)
            {
                DataRow dr = dt.NewRow();
                dr[0] = general.Gname;
                dr[1] = general.Gid;
                dt.Rows.Add(dr);
            }
            return dt;
        }
        private DataTable makeDataSource(DataTable coor)
        {

            //coor.Columns.Add(new DataColumn("recentGo", typeof(string)));
            //coor.Columns.Add(new DataColumn("Atype", typeof(string)));
            //coor.Columns.Add(new DataColumn("target1", typeof(string)));
            //coor.Columns.Add(new DataColumn("target2", typeof(string)));
            
            //int initialIndex = getMaxIndex(user_id,VillageId);
            for (var i = 0; i < coor.Rows.Count;i++)
            {
                //coor.Rows[i][5] = initialIndex+i + 1;
                //coor.Rows[i][6] = general1.Text;
                //coor.Rows[i][7] = general2.Text;
                //coor.Rows[i][8] = general3.Text;
                //coor.Rows[i][9] = general4.Text;
                //coor.Rows[i][10] = general5.Text;
                //coor.Rows[i][11] = cText(soldier_0.Text);
                //coor.Rows[i][12] = cText(soldier_1.Text);
                //coor.Rows[i][13] = cText(soldier_2.Text);
                //coor.Rows[i][14] = cText(soldier_4.Text);
                //coor.Rows[i][15] = cText(soldier_11.Text);
                //coor.Rows[i][16] = cText(soldier_5.Text);
                //coor.Rows[i][17] = cText(soldier_6.Text);
                //coor.Rows[i][18] = general1.SelectedValue;
                //coor.Rows[i][19] = general2.SelectedValue;
                //coor.Rows[i][20] = general3.SelectedValue;
                //coor.Rows[i][21] = general4.SelectedValue;
                //coor.Rows[i][22] = general5.SelectedValue;
                coor.Rows[i][24] = type.SelectedIndex;
                coor.Rows[i][25] = T_target1.SelectedValue.ToString() == "-1" ? T_target1.SelectedValue.ToString() : "0";
                coor.Rows[i][26] = T_target2.SelectedValue.ToString() == "-1" ? T_target2.SelectedValue.ToString() : "0";
            }
                return coor;

        }
        
        private string cText(string text)
        {
            return string.IsNullOrEmpty(text) ? "0" : text;
        }
        private bool saveAttackTarget(DataTable dt)
        {
            bool result = true;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Attack attack = new ComGeneric<Attack>().ConvertToModel(dr);
                    attack.user_id = user_id;
                    attack.villageId = VillageId;
                    dbHelper.insertAttack(attack);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        
        private DataTable getAttackTargets(string user_id, string villageId)
        {
            DataSet ds = dbHelper.getAttack(user_id, villageId);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
                //.DefaultView.ToTable(false,"Aindex", "T_general1", "T_general2", "T_general3", "T_general4", "T_general5", "T_soldier_0", "T_soldier_1", "T_soldier_2", "T_soldier_4", "T_soldier_5", "T_soldier_6", "T_soldier_11", "recentGo", "x", "y", "city", "chief", "hand");
            }
            return null;
        }
        //private int getMaxIndex(string user_id, string villageId)
        //{
        //    return dbHelper.getMaxIndex(user_id, villageId);
        //}
        private void ReBindGrid()
        {
            ReBindOffense();
            ReBindDestroy();
        }

        private void ReBindOffense()
        {
            offense = offenseConfig.getAttackXml(VillageId) == null ? new List<entity.NodeAttack>() : offenseConfig.getAttackXml(VillageId);
            new Thread(delegate()
            {                
                if (offense != null)
                    this.AttackTarget.BeginInvoke(new Action(() =>
                    {
                        if (offense.Count() > 0)
                            this.AttackTarget.DataSource = new BindingList<entity.NodeAttack>(offense);
                        else
                            this.AttackTarget.DataSource = null;
                        //this.AttackTarget.Sort(AttackTarget.Columns[0], ListSortDirection.Ascending);
                    }));
            }).Start();
        }

        private void ReBindDestroy()
        {
            destroy = destroyConfig.getAttackXml(VillageId);
            new Thread(delegate()
            {
                if (destroy != null)
                    this.DestroyTarget.BeginInvoke(new Action(() =>
                    {
                        this.DestroyTarget.DataSource = new BindingList<entity.NodeAttack>(destroy);
                    }));
            }).Start();
        }
      

        private void BindTarget()
        {
            DataTable dSource = getTargetSource();
            T_target1.DataSource = dSource;
            T_target1.DisplayMember = "Name";
            T_target1.ValueMember = "Value";

            T_target2.DataSource = dSource.Copy();
            T_target2.DisplayMember = "Name";
            T_target2.ValueMember = "Value";

            C_T_target1.DataSource = dSource.Copy();
            C_T_target1.DisplayMember = "Name";
            C_T_target1.ValueMember = "Value";
            
            C_T_target2.DataSource = dSource.Copy();
            C_T_target2.DisplayMember = "Name";
            C_T_target2.ValueMember = "Value";
        }
        private DataTable getTargetSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Value", typeof(string)));
            DataRow defaultRow = dt.NewRow();
            defaultRow[0] = "";
            defaultRow[1] = "-1";
            dt.Rows.Add(defaultRow);
            for (var i = 0; i < Constant.m_BldNames.Length; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = Constant.m_BldNames[i];
                dr[1] = Constant.m_BldValues[i];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void AttackStart_Click(object sender, EventArgs e)
        {
            new Thread(delegate() {
                string x = string.Empty;
                string y = string.Empty;
                string type = string.Empty;
                string general1 = string.Empty;
                bool Repeat =false;
                while (offenseConfig.getAttackXml(VillageId).Count() > 0)
                {
                    village village = account.village.Find(item => item.VillageID == VillageId);
                    string urlstr = getSoldierStr(out x, out y, out type, out general1,out Repeat);
                    List<General> list = mainHelper.GetVillageGenerals(account, VillageId);
                    if (list.Find(item => item.Gid == general1 && item.Status == "待命") != null)
                    {
                        bool success = false;
                        string costTime = string.Empty;
                        string msg = mainHelper.attack(x, y, village, account, urlstr, type, out costTime, out success);
                        if (success)
                        {
                            if (Repeat)
                                offenseConfig.updateTime(x, y, VillageId);
                            else
                                offenseConfig.removeNode(x, y, VillageId);
                            ReBindOffense();
                        }
                        recordMsg(msg, listView1);
                        Thread.Sleep(TimeToMilesecond(costTime)*2);
                    }            
                }              
            }).Start();
           
        }
        public int TimeToMilesecond(string costTime)
        { 
           string[] timer=  costTime.Split(':');
           if (timer.Length > 0)
           {
               return (Convert.ToInt32(timer[0]) * 3600 + Convert.ToInt32(timer[1]) * 60 + Convert.ToInt32(timer[2])+2) * 1000;//延迟四秒攻击
           }
           return 60000;//一分钟
        }
     
        public string getSoldierStr(out string x,out string y,out string type,out string general1,out bool Repeat)
        {
            StringBuilder urlStr = new StringBuilder();
            entity.NodeAttack node=offense.FirstOrDefault();

            try
            {
                getSodliers(VillageId);
                CheckAllSoldiers(checkAttackModel);
                if (Convert.ToInt32(this.general1.SelectedValue) != 0) urlStr.Append("general1=" + this.general1.SelectedValue + "&");
                if (Convert.ToInt32(this.general2.SelectedValue) != 0) urlStr.Append("general2=" + this.general2.SelectedValue + "&");
                if (Convert.ToInt32(this.general3.SelectedValue) != 0) urlStr.Append("general3=" + this.general3.SelectedValue + "&");
                if (Convert.ToInt32(this.general4.SelectedValue) != 0) urlStr.Append("general4=" + this.general4.SelectedValue + "&");
                if (Convert.ToInt32(this.general5.SelectedValue) != 0) urlStr.Append("general5=" + this.general5.SelectedValue + "&");
                if (M(this.soldier_0.Text) != 0) urlStr.Append("soldier[0]=" + M(soldier_0.Text) + "&");
                if (M(this.soldier_1.Text) != 0) urlStr.Append("soldier[1]=" + M(soldier_1.Text) + "&");
                if (M(this.soldier_2.Text) != 0) urlStr.Append("soldier[2]=" + M(soldier_2.Text) + "&");
                if (M(this.soldier_4.Text) != 0) urlStr.Append("soldier[4]=" + M(soldier_4.Text) + "&");
                if (M(this.soldier_11.Text) != 0) urlStr.Append("soldier[11]=" + M(soldier_11.Text) + "&");
                if (M(this.soldier_5.Text) != 0) urlStr.Append("soldier[5]=" + M(soldier_5.Text) + "&");
                if (M(this.soldier_6.Text) != 0) urlStr.Append("soldier[6]=" + M(soldier_6.Text) + "&");
                if (Convert.ToInt32(this.T_target1.SelectedValue) != 0) urlStr.Append("target[0]=" + T_target1.SelectedValue + "&");
                if (Convert.ToInt32(this.T_target2.SelectedValue) != 0) urlStr.Append("target[1]=" + T_target2.SelectedValue + "&");
            }
            catch (Exception ex)
            {


            }
            finally
            {
                Repeat = this.Offense_Repeat.SelectedIndex.ToString() == "0";
                x = node.x;
                y = node.y;
                type = this.type.SelectedIndex.ToString(); ;
                //Aindex = dr["Aindex"].ToString();
                general1 = this.general1.SelectedValue.ToString();
            }          
            return urlStr.ToString();
        }
        public string getSoldierStr1(out string x, out string y, out string outtype, out string general1, out string general5,out bool Repeat)
        {
            StringBuilder urlStr = new StringBuilder();
            entity.NodeAttack node =destroy.FirstOrDefault();
            try
            {
                getSodliers(VillageId);
                if (Convert.ToInt32(croThrVal(C_general1)) != 0 && Convert.ToInt32(croThrVal(C_general1)) != -1) urlStr.Append("general1=" + croThrVal(C_general1) + "&");
                if (Convert.ToInt32(croThrVal(C_general2)) != 0 && Convert.ToInt32(croThrVal(C_general2)) != -1) urlStr.Append("general2=" + croThrVal(C_general2) + "&");
                if (Convert.ToInt32(croThrVal(C_general3)) != 0 && Convert.ToInt32(croThrVal(C_general3)) != -1) urlStr.Append("general3=" + croThrVal(C_general3) + "&");
                if (Convert.ToInt32(croThrVal(C_general4)) != 0&& Convert.ToInt32(croThrVal(C_general4)) != -1) urlStr.Append("general4=" + croThrVal(C_general4) + "&");
                if (Convert.ToInt32(croThrVal(C_general5)) != 0 && Convert.ToInt32(croThrVal(C_general5)) != -1) urlStr.Append("general5=" + croThrVal(C_general5) + "&");
                if (M(croThrVal(C_soldier_0)) != 0) urlStr.Append("soldier[0]=" + croThrVal(C_soldier_0) + "&");
                if (M(croThrVal(C_soldier_1)) != 0) urlStr.Append("soldier[1]=" + croThrVal(C_soldier_1) + "&");
                if (M(croThrVal(C_soldier_2)) != 0) urlStr.Append("soldier[2]=" + croThrVal(C_soldier_2) + "&");
                if (M(croThrVal(C_soldier_4)) != 0) urlStr.Append("soldier[4]=" + croThrVal(C_soldier_4) + "&");
                if (M(croThrVal(C_soldier_11)) != 0) urlStr.Append("soldier[11]=" + croThrVal(C_soldier_11) + "&");
                if (M(croThrVal(C_soldier_5)) != 0) urlStr.Append("soldier[5]=" + croThrVal(C_soldier_5) + "&");
                if (M(croThrVal(C_soldier_6)) != 0) urlStr.Append("soldier[6]=" + croThrVal(C_soldier_6) + "&");
                if (Convert.ToInt32(croThrVal(this.C_T_target1)) != 0&& Convert.ToInt32(croThrVal(this.C_T_target1)) != -1) urlStr.Append("target[0]=" + croThrVal(C_T_target1) + "&");
                if (Convert.ToInt32(croThrVal(this.C_T_target2)) != 0&& Convert.ToInt32(croThrVal(this.C_T_target2)) != -1) urlStr.Append("target[1]=" + croThrVal(C_T_target2) + "&");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                Repeat = getRepeat( this.Destory_Repeat)== "0";
            }
            x = node.x;
            y = node.y;
            outtype = "0";
            //Aindex = dr["Aindex"].ToString();
            general1 = croThrVal(this.C_general1);
            general5 = croThrVal(this.C_general5);
            return urlStr.ToString();
        }

        public string getRepeat(ComboBox repeat)
        {
            string re = string.Empty;
            this.Invoke(new Action(() => { re = repeat.SelectedIndex.ToString(); }));
            return re;
        }
        public string croThrVal(Control control)
        {
            string val = string.Empty;
            if (control is TextBox)
            {
                if ((control as TextBox).InvokeRequired)
                    (control as TextBox).Invoke(new Action(() => { val = (control as TextBox).Text; }));
                else
                    val = (control as TextBox).Text;
            }
            else if (control is ComboBox)
            {
                if ((control as ComboBox).InvokeRequired)
                    (control as ComboBox).Invoke(new Action(() => { val = (control as ComboBox).SelectedValue.ToString(); }));
                else
                    val = (control as ComboBox).SelectedValue.ToString();
            }
            return val;
        }
        public string getDestroyStr( string gid, string general5)
        {
            StringBuilder urlStr = new StringBuilder();

            try
            {
                getSodliers(VillageId);
                if (Convert.ToInt32(gid) != 0) urlStr.Append("general1=" + gid + "&");
                if (Convert.ToInt32(general5) != 0) urlStr.Append("general5=" + general5 + "&");
                if (M(croThrVal(this.H_soldier_0)) != 0) urlStr.Append("soldier[0]=" +  croThrVal(H_soldier_0) + "&");
                if (M(croThrVal(this.H_soldier_1)) != 0) urlStr.Append("soldier[1]=" +  croThrVal(H_soldier_1) + "&");
                if (M(croThrVal(this.H_soldier_2)) != 0) urlStr.Append("soldier[2]=" +  croThrVal(H_soldier_2) + "&");
                if (M(croThrVal(this.H_soldier_4)) != 0) urlStr.Append("soldier[4]=" +  croThrVal(H_soldier_4) + "&");
                if (M(croThrVal(this.H_soldier_1)) != 0) urlStr.Append("soldier[11]=" + croThrVal(H_soldier_11) + "&");
                if (M(croThrVal(this.H_soldier_5)) != 0) urlStr.Append("soldier[5]=" +  croThrVal(H_soldier_5) + "&");
                if (M(croThrVal(this.H_soldier_6)) != 0) urlStr.Append("soldier[6]=" +  croThrVal(H_soldier_6) + "&");
                //if (Convert.ToInt32(this.C_T_target1.SelectedValue) != 0) urlStr.Append("target[0]=" + C_T_target1.SelectedValue + "&");
                //if (Convert.ToInt32(this.C_T_target2.SelectedValue) != 0) urlStr.Append("target[1]=" + C_T_target2.SelectedValue + "&");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //Aindex = dr["Aindex"].ToString();
            return urlStr.ToString();
        }

      

        private void recordMsg(string msg,ListBox box)
        {
            box.BeginInvoke(new Action(() =>
            {
                box.Items.Add(msg);
            }));          
        }

        private void save_CSetting_Click(object sender, EventArgs e)
        {

        }

        public void setOffenseAttr(object sender, EventArgs e)
        {
            offenseConfig.ControlAttribute(sender,  nodeList[0], "Control", VillageId);
        }
        public void setDestroyAttr(object sender, EventArgs e)
        {
            destroyConfig.ControlAttribute(sender,  nodeList[0], "Control", VillageId);
        }
        public void setDestroyAttr1(object sender, EventArgs e)
        {
            destroyConfig.ControlAttribute1(sender,  nodeList[0], "Control", VillageId);
        }
        public void setOffenseControlState()
        {
            List<entity.Node> nodes = new List<Node>();
            nodes = offenseConfig.getControlXml(VillageId);
            try
            {
                if (nodes != null && nodes.Count() > 0)
                {
                    foreach (var item in ActiveOffenseControl)
                    {
                        var controls = item.Controls;
                        if (controls.Count > 0)
                        {
                            foreach (Control control in controls)
                            {
                                var relativeitem = (from node in nodes where node.name == control.Name select node).FirstOrDefault();
                                if (relativeitem != null)
                                {
                                    setControl(control, relativeitem);
                                }
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void setDestroyControlState()
        {
            List<entity.Node> nodes = new List<Node>();
            nodes = destroyConfig.getControlXml(VillageId);
            try
            {
                if (nodes != null && nodes.Count() > 0)
                {
                    foreach (var item in ActiveDestroyControl)
                    {
                        var controls = item.Controls;
                        if (controls.Count > 0)
                        {
                            foreach (Control control in controls)
                            {
                                var relativeitem = (from node in nodes where node.name == control.Name select node).FirstOrDefault();
                                if (relativeitem != null)
                                {
                                    setControl(control, relativeitem);
                                }
                            }
                        }
                    }
                    setGeneralList(nodes);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
          //  ActiveDestroyControl
        }
        public void setGeneralList(List<entity.Node> nodes)
        {
            var relativeitem = from node in nodes where node.name == "GeneralList"&&node.state=="true" select node;
            if (relativeitem != null&&relativeitem.Count()>0)
            {
                if (GeneralList.Items.Count > 0)
                {
                    for (int i = 0; i < GeneralList.Items.Count; i++)
                    {
                        string Value = (GeneralList.Items[i] as DataRowView).Row["Value"].ToString();
                        var pointItem = (from t in relativeitem where t.text == Value select t).FirstOrDefault();
                        if (pointItem != null)
                        {
                            GeneralList.SetItemChecked(i, true);
                        }
                        else
                        {
                            GeneralList.SetItemChecked(i,false);
                        }
                    }
                }
            }
        }
        public void setControl(Control control, entity.Node node)
        {
            if (control is CheckBox)
            {
                (control as CheckBox).Checked = node.state == "true" ? true : false;
            }
            else if (control is TextBox)
            {
                (control as TextBox).Text = node.state;
            }
            else if (control is RadioButton)
            {
                (control as RadioButton).Checked = node.state == "true" ? true : false;
            }
            else if (control is ComboBox)
            {
                (control as ComboBox).SelectedIndex = Convert.ToInt32(node.state);
            }
        }

        private void checkAttackModel_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox control = sender as CheckBox;
            Control.ControlCollection controls = Setting.Controls;
            if (control.Checked)
            {
                foreach (Control item in controls)
                {
                    item.Enabled = false;  
                }
            }
            else
            {
                foreach (Control item in controls)
                {
                    item.Enabled = true;
                }
            }
        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (GeneralList.Items.Count > 0)
            {
                for (int i = 0; i < GeneralList.Items.Count; i++)
                {
                    GeneralList.SetItemChecked(i, checkbox.Checked);
                }
            }
        }

        private void GeneralList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckBox check = new CheckBox();
            check.Name = "GeneralList";
            check.Text= (GeneralList.Items[e.Index] as DataRowView).Row["Value"].ToString();
            check.Checked =(int)e.NewValue == 1 ? true : false;
            setDestroyAttr1(check, e);
            
        }

        private void checkAttackModel_CheckedChanged_1(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            CheckAllSoldiers(check);           
            
            setOffenseAttr(sender,e);
        }
        public void CheckAllSoldiers(CheckBox check)
        {
            if (check.Checked)
            {
                soldier_0.Text = soldier0.Text;
                soldier_1.Text = soldier1.Text;
                soldier_2.Text = soldier2.Text;
                soldier_4.Text = soldier4.Text;
                soldier_11.Text = soldier11.Text;
            }
            else
            {
                soldier_0.Text = "";
                soldier_1.Text = "";
                soldier_2.Text = "";
                soldier_4.Text = "";
                soldier_11.Text = "";
            }
        }

        private void C_AddCoor_Click(object sender, EventArgs e)
        {

            if (!CheckSetting())
                return;
            if (type.SelectedIndex < 0)
            {
                MessageBox.Show("请选择攻击模式");
                return;
            }
            entity.NodeAttack attack = mainHelper.GetCityInfo(this.x_cor.Text, this.y_cor.Text, account);
            attack.VillageId = VillageId;
            if (attack.name != null)
            {
                offenseConfig.setAttribute(attack, "Attacks");
                ReBindOffense();
            }
          
        }

        private void C_AttackStart_Click(object sender, EventArgs e)
        {
            string x = string.Empty;
            string y = string.Empty;
            string type = string.Empty;
            string general1 = string.Empty;
            string general5 = string.Empty;
            bool Repeat = false;
            village village = account.village.Find(item => item.VillageID == VillageId);
            new Thread(delegate() {
                while (destroyConfig.getAttackXml(VillageId) != null && destroyConfig.getAttackXml(VillageId).Count() > 0)
                {
                    string urlstr = getSoldierStr1(out x, out y, out type, out general1, out general5,out Repeat);
                    List<General> list = mainHelper.GetVillageGenerals(account, VillageId);
                    Queue<string> FollowGenerals = new Queue<string>();//跟车武将
                    if (GeneralList.Items.Count > 0)
                    {
                        for (var i = 0; i < GeneralList.Items.Count; i++)
                        {
                            if (GeneralList.GetItemChecked(i))
                            {
                                FollowGenerals.Enqueue((GeneralList.Items[i] as DataRowView).Row["Value"].ToString());
                            }
                        }
                    }
                    foreach (var f in FollowGenerals)
                    {
                        if (list.Find(item => item.Gid == f && (item.Status == "待命" || item.Status == "流亡")) == null || list.Find(item => item.Gid == general1 && item.Status == "待命") == null)
                            goto toNextQueue;
                    }
                    bool success = false;
                    BeginDestroy(x, y, village, account, urlstr, type, out success, FollowGenerals, general5);
                    if (success)
                    {
                        if (Repeat)
                            destroyConfig.updateTime(x, y, VillageId);
                        else
                            destroyConfig.removeNode(x, y, VillageId);
                        ReBindDestroy();
                    }
                toNextQueue:
                    continue;
                }           
            }).Start();
        }

    

        public void BeginDestroy(string x,string  y, village village, AccountModel account,string urlstr,string  type, out bool success,Queue<string> FollowGenerals,string general5)
        {
            bool success1 = false;
            success = false;
            string costTime = string.Empty;
            string msg = mainHelper.attack(x, y, village, account, urlstr, type,out costTime, out success1);
            if (success1)
            {
                recordMsg(msg, listView2);
                Thread.Sleep(3500);
                while (FollowGenerals.Count > 0)
                {
                    string f = FollowGenerals.Dequeue();
                    int count = 0;
                desBegin:
                    string destroyStr = getDestroyStr(f, general5);
                    bool desSuccess = false;
                    string costTime1 = string.Empty;
                    string desMsg = mainHelper.attack(x, y, village, account, destroyStr, type, out costTime1, out desSuccess);
                    recordMsg(desMsg, listView2);
                    if (!desSuccess && count < 4)
                    {
                        count++;
                        Thread.Sleep(3500);
                        goto desBegin;
                    }
                    else
                        Thread.Sleep(3500);
                }
                Thread.Sleep((TimeToMilesecond(costTime)-2) * 2);//暂停出征时间
                success = true;
            }
            else if (msg.Contains("一片空地"))
            {
                success = true;
            }
            else
                return;
        }

        private void C_importCoor_Click(object sender, EventArgs e)
        {
            if (!CheckSetting1())
                return;
            if (type.SelectedIndex < 0)
            {
                MessageBox.Show("请选择攻击模式");
                return;
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                DataSet ds = new DataSet();
                try
                {
                    ds = new ExcelHelper().getDataSource(filePath, VillageId);
                    if (!checkCurrentSolider(destroySettings,1))
                    {
                        MessageBox.Show("当前城镇出兵数量错误");
                        return;
                    }
                    new Thread(delegate()
                    {
                        destroyConfig.AttackAttribute(ds, "Attacks", "Attack");
                        ReBindDestroy();
                    }).Start();
                }
                catch (Exception ex)
                {

                }
            }
            return;
        }   
    }
}












