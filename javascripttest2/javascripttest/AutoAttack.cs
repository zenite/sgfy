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
            ActiveDestroyControl = new Control[] { destroySettings };
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
                    ds = new ExcelHelper().getDataSource(filePath);
                    if (!checkCurrentSolider())
                    {
                        MessageBox.Show("当前城镇出兵数量错误");
                        return;
                    }
                    
                    //DataTable datasource = makeDataSource(ds.Tables[0]);
                    new Thread(delegate() {
                        offenseConfig.AttackAttribute(ds, "Attacks", "Attack");
                    }).Start();
                    ReBindOffense();
                         
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

        /// <summary>
        /// 检查当前出征的士兵数是否足够
        /// </summary>
        /// <returns></returns>
        private bool checkCurrentSolider()
        {
            bool result = true;
            Control.ControlCollection controls = Setting.Controls;
            foreach (Control control in controls)
            { 
                if(control.Name.Contains("soldier") && control is TextBox)
                {
                    TextBox textbox = control as TextBox;
                    try 
                    {
                        if (Convert.ToInt32(villageSoldier[Convert.ToInt32(textbox.Name.Split('_')[1])]) < Convert.ToInt32(string.IsNullOrEmpty(textbox.Text.Trim()) ? "0" : textbox.Text.Trim()))
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
            attack.Aindex = getMaxIndex(user_id, VillageId).ToString()+1;
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
            GeneralList.DataSource = gen;
            GeneralList.DisplayMember = "Name";
            GeneralList.ValueMember = "Value";
            
        }
        private void getSodliers(string villageId)
        {
            villageSoldier = mainHelper.getSoldiers(account, villageId);
            soldier0.Text = villageSoldier[0];
            soldier1.Text = villageSoldier[1];
            soldier2.Text = villageSoldier[2];
            soldier4.Text = villageSoldier[4];
            soldier5.Text = villageSoldier[5];
            soldier6.Text = villageSoldier[6];
            soldier11.Text = villageSoldier[11];
            C_soldier0.Text = villageSoldier[0];
            C_soldier1.Text = villageSoldier[1];
            C_soldier2.Text = villageSoldier[2];
            C_soldier4.Text = villageSoldier[4];
            C_soldier5.Text = villageSoldier[5];
            C_soldier6.Text = villageSoldier[6];
            C_soldier11.Text = villageSoldier[11];
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
        private int getMaxIndex(string user_id, string villageId)
        {
            return dbHelper.getMaxIndex(user_id, villageId);
        }
        private void ReBindGrid()
        {
            ReBindOffense();
            ReBindDestroy();
        }

        private void ReBindOffense()
        {
            offense = offenseConfig.getAttackXml(VillageId);
            new Thread(delegate()
            {                
                if (offense != null)
                    this.AttackTarget.BeginInvoke(new Action(() =>
                    {
                        this.AttackTarget.DataSource = new BindingList<entity.NodeAttack>(offense);
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
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.timer1.Start();
        }
     
        public string getSoldierStr(out string x,out string y,out string type,out string general1)
        {
            StringBuilder urlStr = new StringBuilder();
            entity.NodeAttack node=offense.FirstOrDefault();
            
            try {
                if (Convert.ToInt32(this.general1.SelectedValue) != 0) urlStr.Append("general1=" + this.general1.SelectedValue + "&");
                if (Convert.ToInt32(this.general2.SelectedValue) != 0) urlStr.Append("general2=" + this.general2.SelectedValue + "&");
                if (Convert.ToInt32(this.general3.SelectedValue) != 0) urlStr.Append("general3=" + this.general3.SelectedValue + "&");
                if (Convert.ToInt32(this.general4.SelectedValue) != 0) urlStr.Append("general4=" + this.general4.SelectedValue + "&");
                if (Convert.ToInt32(this.general5.SelectedValue) != 0) urlStr.Append("general5=" + this.general5.SelectedValue + "&");
                if (Convert.ToInt32(this.soldier_0.Text) != 0) urlStr.Append("soldier[0]=" + soldier_0.Text + "&");
                if (Convert.ToInt32(this.soldier_1.Text) != 0) urlStr.Append("soldier[1]=" + soldier_1.Text + "&");
                if (Convert.ToInt32(this.soldier_2.Text) != 0) urlStr.Append("soldier[2]=" + soldier_2.Text + "&");
                if (Convert.ToInt32(this.soldier_4.Text) != 0) urlStr.Append("soldier[4]=" + soldier_4.Text + "&");
                if (Convert.ToInt32(this.soldier_11.Text) != 0) urlStr.Append("soldier[11]="+soldier_11.Text + "&");
                if (Convert.ToInt32(this.soldier_5.Text) != 0) urlStr.Append("soldier[5]=" + soldier_5.Text + "&");
                if (Convert.ToInt32(this.soldier_6.Text) != 0) urlStr.Append("soldier[6]=" + soldier_6.Text + "&");
                if (Convert.ToInt32(this.T_target1.SelectedValue) != 0) urlStr.Append("target[0]=" + T_target1.SelectedValue + "&");
                if (Convert.ToInt32(this.T_target2.SelectedValue) != 0) urlStr.Append("target[1]=" + T_target2.SelectedValue + "&");               
            }
            catch (Exception ex)
            { 
                
            }
            x = node.x;
            y = node.y;
            type = this.type.SelectedIndex.ToString(); ;
            //Aindex = dr["Aindex"].ToString();
            general1 = this.general1.SelectedValue.ToString();
            return urlStr.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string x = string.Empty;
            string y = string.Empty;
            string type = string.Empty;
            string general1 = string.Empty;
            village village = account.village.Find(item => item.VillageID == VillageId);
            string urlstr = getSoldierStr(out x, out y, out type,  out general1);            
            List<General> list = mainHelper.GetVillageGenerals(account,VillageId);
            string newAindex=(getMaxIndex(user_id,VillageId)+1).ToString();
            if (list.Find(item => item.Gid == general1 && item.Status == "待命") != null)
            {
                recordMsg(mainHelper.attack(x, y, village, account, urlstr, type));
                //dbHelper.updateAttack(user_id, VillageId, Aindex, newAindex);
                offenseConfig.removeNode(x, y, VillageId);
                ReBindOffense();                
            }            
        }

        private void recordMsg(string msg)
        {
            this.listView1.BeginInvoke(new Action(() =>
            {
                this.listView1.Items.Add(msg);
            }));          
        }

        private void save_CSetting_Click(object sender, EventArgs e)
        {

        }

        public void setOffenseAttr(object sender, EventArgs e)
        {
            offenseConfig.ControlAttribute(sender, e, nodeList[0], "Control", VillageId);
        }
        public void setDestroyAttr(object sender, EventArgs e)
        {
            destroyConfig.ControlAttribute(sender, e, nodeList[0], "Control", VillageId);
        }
        public void setDestroyAttr1(object sender, EventArgs e)
        {
            destroyConfig.ControlAttribute1(sender, e, nodeList[0], "Control", VillageId);
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
                        string name=(GeneralList.Items[i] as DataRowView).Row["Name"].ToString();
                        var pointItem = (from t in relativeitem where t.name == name select t).FirstOrDefault();
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
            check.Checked = GeneralList.GetItemChecked(e.Index);
            setDestroyAttr1(check, e);
            
        }

  
    }
}












