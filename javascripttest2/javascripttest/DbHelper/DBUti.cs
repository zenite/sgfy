using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using javascripttest.Properties;
using javascripttest.entity;

namespace javascripttest.DbHelper
{
    public class DBUti
    {
        private string connectionStr;        
        public DBUti()
        {
            //string appPath = System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appPath = System.Windows.Forms.Application.StartupPath;
            string appDataPath = Path.Combine(appPath, Resources.CompanyName, Resources.UserDbName+ Resources.DbExtension);
            connectionStr= "Data Source=" + appDataPath;            
        }
        public DBUti(string dbpath)
        {
            connectionStr = "Data Source=" + dbpath;    
        }
        /// <summary>
        /// 添加账号信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool insertAccount(AccountModel account,string direct="")
        {
            StringBuilder sb = new StringBuilder();
            int result=0;
            if (string.IsNullOrEmpty(direct))
                sb.AppendFormat("insert into account(username,password,cookieStr,hasMulti,Server_Url,user_id,villageid,chief,typeOfCountry,rankOfNobility) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", account.username, account.password, account.cookieStr, account.hasMulti, account.Server_url, account.user_id,account.villageid,account.chief,account.typeOfCountry,account.rankOfNobility);
            else
                sb.AppendFormat("insert into account(username,password,cookieStr,hasMulti,Server_Url,user_id,villageid,chief,typeOfCountry,rankOfNobility,Initial_Status) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','hasInitial')", account.username, account.password, account.cookieStr, account.hasMulti, account.Server_url, account.user_id, account.villageid, account.chief, account.typeOfCountry, account.rankOfNobility);
            try
            {
               result= SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 清空全部生成的账号
        /// </summary>
        /// <returns></returns>
        public bool deleteAccounts(string paraName,string paraValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(string.Format("delete from account where {0}='{1}' and Server_Url='{2}'",paraName,paraValue,Constant.Server_Url));
            int result = 0;
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result > -1)
                return true;
            return false;

        }

        /// <summary>
        /// 更新账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool updateAccount(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            int result = 0;
            if(string.IsNullOrEmpty(account.Initial_Status))
                sb.AppendFormat("update account set password='{1}',cookieStr='{2}',hasMulti='{3}',user_id='{5}',villageid='{6}',chief='{7}',typeOfCountry='{8}',rankOfNobility='{9}',Initial_Status='hasInitial' where username='{0}' and Server_Url='{4}'", account.username, account.password, account.cookieStr, account.hasMulti, account.Server_url, account.user_id, account.villageid, account.chief, account.typeOfCountry, account.rankOfNobility);
            else
                sb.AppendFormat("update account set password='{1}',cookieStr='{2}',hasMulti='{3}',villageid='{6}',chief='{7}',typeOfCountry='{8}',rankOfNobility='{9}' ,Initial_Status='hasInitial' where username='{0}' and user_id='{5}' and Server_Url='{4}'", account.username, account.password, account.cookieStr, account.hasMulti, account.Server_url, account.user_id, account.villageid, account.chief, account.typeOfCountry, account.rankOfNobility);
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        public bool UpdateError(AccountModel account, string errorLevel)
        {
            StringBuilder sb = new StringBuilder();
            int result = 0;
            sb.AppendFormat("update account set AccountName='{0}' where username='{1}' and Server_Url='{2}'",errorLevel, account.username, account.Server_url);
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        public List<string> checkAccount(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            sb.AppendFormat("select * from account where username='{0}' and Server_Url='{1}' ", account.username, Constant.Server_Url);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr,sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {                
            }
            if (ds.Tables.Count > 0)
            {
                return (from item in ds.Tables[0].AsEnumerable() select item.Field<string>("user_id").ToString()).ToList();
            }
            return new List<string>();
        }

        public List<string> checkAccount1(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            sb.AppendFormat("select * from account where username='{0}' and Server_Url='{1}' and user_id is not null and user_id!='' and Initial_Status='hasInitial'", account.username, Constant.Server_Url);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
            }
            if (ds.Tables.Count > 0)
            {
                return (from item in ds.Tables[0].AsEnumerable() select item.Field<string>("user_id").ToString()).ToList();
            }
            return new List<string>();
        }

        public bool checkAccount2(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            sb.AppendFormat("select * from account where username='{0}' and Server_Url='{1}' and password='{2}'", account.username, Constant.Server_Url,account.password);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
            }
            if (ds!=null&&ds.Tables.Count > 0&&ds.Tables[0].Rows.Count>0)
            {
                return false;
            }
            return true;
        }

        public bool checkAccount3(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            sb.AppendFormat("select * from account where user_id='{0}' and Server_Url='{1}'", account.user_id, Constant.Server_Url);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

       
        public bool UpdateMultiAccount(string paraName,string paraValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update account set Initial_Status='repeat' where {0}='{1}' and Server_Url='{2}'", paraName, paraValue,Constant.Server_Url);
            int result = 0;
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        public bool updateAccount(string paraName, string paraValue,string user_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update account set {0}='{1}' where Server_Url='{2}' and user_id='{3}'", paraName, paraValue, Constant.Server_Url,user_id);
            int result = 0;
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        public DataSet getAllAccount()
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from account where Server_Url='{0}'", Constant.Server_Url);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                
            }            
            return ds;            
        }

        public DataSet getAllAccount1()
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from account ");
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {

            }
            return ds;               
        }
        public DataSet getAccounts(string paraName, string paraValue)
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from account where Server_Url='{0}' and ({1}='{2}' or ('{2}' is null or '{2}'=''))", Constant.Server_Url,paraName,paraValue);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {

            }
            return ds;            
        }
        public bool InsertOrUpdate(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = new DataSet();
            sb.AppendFormat("select * from account where (user_id='{0}' or user_id='') and username='{1}'  and Server_Url='{2}'", account.user_id, account.username, account.Server_url);
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0) return false;
            }
            return true;
        }

        public AccountModel getAccount(string paraName, string paraValue)
        {
            AccountModel account = new AccountModel();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from account where {0}='{1}' and Server_Url='{2}'", paraName, paraValue, Constant.Server_Url);
            DataSet ds = new DataSet();
            try {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch(Exception ex){}
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow datas = ds.Tables[0].Rows[0];
                    account = assignAccount(datas);
                }
            }
            return account;
        }


        public AccountModel assignAccount(DataRow dr)
        {
            AccountModel account=new AccountModel();
            IEnumerable<string> columnNames = from item in dr.Table.Columns.Cast<DataColumn>() select item.ColumnName;
            Type type = account.GetType();
            System.Reflection.PropertyInfo[] properties = type.GetProperties();
            foreach (var item in properties)
            {
                if (columnNames.Where(column => column == item.Name).Count() > 0)
                {
                    item.SetValue(account,dr[item.Name].ToString(),null);
                }
            }
            return account;
        }
        /// <summary>
        /// create temporarytable ->insert into temporarytable select ...->
        /// drop table account ->create table account  ->
        /// insert into account select ..from temporary  ->drop temporary
        /// </summary>
        /// <returns></returns>
        public bool autoUpdateDatabase()
        {
            Dictionary<string, string> ModelType = getModelType();
            Dictionary<string, string> databaseType = getDatabaseType();
            string temTName = "t1_backup";
            string commomSql = "'id',";
            string createTem = "create TEMPORARY  table " + temTName + "(";
            string insertTem = "";
            string dropAccount = "";
            string createAccount = "Create table account(";
            string insertAccount = "";
            string dropTem = "";
            createTem += commomSql + CreateTable(ModelType) + ",PRIMARY KEY(id)" + ")";
            insertTem = "insert into " + temTName + "(" + InsertTable(databaseType) + ") select " + InsertTable(databaseType) + " from account";
            dropAccount = "drop table account";
            createAccount += commomSql + CreateTable(ModelType) + ",PRIMARY KEY(id)" + ")";
            insertAccount = "insert into account select * from " + temTName;
            dropTem = "drop table " + temTName;
            try
            {
                List<string> sqlStr = new List<string>() ;
                sqlStr.Add(createTem);
                sqlStr.Add(insertTem);
                sqlStr.Add(dropAccount);
                sqlStr.Add(createAccount);
                sqlStr.Add(insertAccount);
                sqlStr.Add(dropTem);
                SqliteHelper.ExecuteNonQuery(connectionStr, sqlStr, CommandType.Text);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;       
        }
        /// <summary>
        /// 反射获取本地类字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getModelType()
        {
            Dictionary<string, string> ModelType = new Dictionary<string, string>();
            Type type = new accountBase().GetType();
            PropertyInfo[] propertyInfo = type.GetProperties();
            foreach (var item in propertyInfo)
            {
                if (item.PropertyType.FullName == "System.String")
                ModelType.Add(item.Name,"text");
            }
            return ModelType;
        }
        /// <summary>
        /// 获取指定数据表字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getDatabaseType()
        {
            Dictionary<string, string> databaseType = new Dictionary<string, string>();
            StringBuilder sb = new StringBuilder();
            sb.Append("PRAGMA table_info([account]) ");
            DataSet ds = new DataSet();
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex) {
                return null;
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        databaseType.Add(dr["name"].ToString(), dr["type"].ToString());
                    }
                }
            }
            
            return databaseType;
        }

        public string CreateTable(Dictionary<string,string> modelType)
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in modelType)
            {
                result.Append("'"+item.Key + "' " + item.Value + ",");
            }
            return result.ToString().Substring(0, result.ToString().Length - 1);
        }
        public string InsertTable(Dictionary<string,string> databaseType)
        {
            StringBuilder result = new StringBuilder() ;
            foreach (var item in databaseType.Keys)
            {
                result.Append(item + ",");
            }
            return result.ToString().Substring(0,result.ToString().Length-1);
        }



        /// <summary>
        /// 添加账号信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool insertAttack(Attack attack)
        {
            StringBuilder sb = new StringBuilder();
            int result = 0;
            //(index,T_general1,T_general1_ID,T_general2,T_general2_ID,T_general3,T_general3_ID,T_general4	,T_general4_ID,
            //                 T_general5,T_general5_ID,T_soldier_0,T_soldier_1,T_soldier_2,T_soldier_4,T_soldier_5,T_soldier_6,T_soldier_11,recentGo,user_id,villageId,x,y,city,chief,hand)

            sb.AppendFormat(@"insert into Attack values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}')",
                                     attack.Aindex, attack.T_general1, attack.T_general1_ID, attack.T_general2, attack.T_general2_ID, attack.T_general3, attack.T_general3_ID,
attack.T_general4, attack.T_general4_ID, attack.T_general5, attack.T_general5_ID, attack.T_soldier_0, attack.T_soldier_1,
attack.T_soldier_2, attack.T_soldier_4, attack.T_soldier_5, attack.T_soldier_6, attack.T_soldier_11, attack.recentGo,
attack.user_id, attack.villageId, attack.x, attack.y, attack.city, attack.chief, attack.hand,attack.Atype,attack.target1,attack.target2);
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 清空全部生成的账号
        /// </summary>
        /// <returns></returns>
        public bool deleteAttack(string user_id, string villageId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("delete from Attack where user_id='{0}' and villageId='{1}'", user_id, villageId));
            int result = 0;
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result > -1)
                return true;
            return false;

        }
        public DataSet getAttack(string user_id,string villageId)
        {
            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from Attack where user_id='{0}' and villageId='{1}'",user_id,villageId );
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {

            }
            return ds;
        }
        public bool updateAttack(string user_id, string villageId, string Aindex,string newAindex)
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("update Attack set Aindex='{0}',recentGo='{1}' where user_id='{2}' and villageId='{3}' and Aindex='{4}'", newAindex, System.DateTime.Now, user_id, villageId, Aindex);
            try
            {
                result = SqliteHelper.ExecuteNonQuery(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == 0)
                return false;
            return true;
        }
        public int getMaxIndex(string user_id, string villageId)
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select max(cast(Aindex as int)) as AIndex from Attack where user_id='{0}' and villageId='{1}'", user_id, villageId);
            DataSet ds = new DataSet();
            try
            {
                ds = SqliteHelper.ExecuteDataSet(connectionStr, sb.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception ex) { }
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow data = ds.Tables[0].Rows[0];
                    result = data[0]!=System.DBNull.Value? Convert.ToInt32(data[0]):0;
                }
            }
            return result;
        }

    }
}




