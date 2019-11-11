using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;


namespace javascripttest.DbHelper
{
    public class SqlHelper
    {
        private string constr { set; get; }        
        public SqlHelper()
        {
            //constr ="Data Source=LocalHost;Integrated Security=SSPI;Initial Catalog=test;";
            constr = "Data Source=LocalHost;Integrated Security=SSPI;Initial Catalog=ThreeCountry;";
        }
        public bool ExecuteInsert( AccountModel account,string dbName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {10} (id,username,password,cookieStr,hasMulti,Server_Url,user_id,villageid,chief,typeOfCountry,rankOfNobility,Initial_Status,city_num,originalperson,originalserver) values(N'{5}',N'{0}',N'{1}',N'{2}',N'{3}',N'{4}',N'{5}',N'{6}',N'{7}',N'{8}',N'{9}','hasInitial',N'{11}',N'{12}',N'{13}')", account.username, account.password, account.cookieStr, account.hasMulti, account.Server_url, account.user_id, account.villageid, account.chief, account.typeOfCountry, account.rankOfNobility, dbName,account.city_num, account.originalperson, account.originalserver);
            int rowIndex = ExecuteInsert(sb.ToString());
            if (rowIndex > 0) return true;            
            return false;
        }

        public bool ExecuteInsert1(AccountModel account)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {4} (username,password,AccountName,Server_url,originalperson,originalserver) values(N'{0}',N'{1}',N'{2}',N'{3}',N'{5}',N'{6}')", account.username, account.password, account.AccountName,account.Server_url, "myaccount",account.originalperson,account.originalserver);
            int rowIndex = ExecuteInsert(sb.ToString(), "Data Source=LocalHost;Integrated Security=SSPI;Initial Catalog=ThreeCountry;");
            if (rowIndex > 0) return true;
            return false;
        }
       

        public bool checkAccount3(string user_id, string dbpath)
        {
            StringBuilder sb = new StringBuilder();            
            sb.AppendFormat("select * from {1} where user_id='{0}' ", user_id, dbpath);
            DataSet ds = getData(sb.ToString(),"ddd");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        public bool checkAccount4(string username, string dbpath)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("select * from {1} where username='{0}' ", username, dbpath);
            DataSet ds = getData(sb.ToString(), "ddd");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        public DataSet getData(string sqlstr,string resultName)
        {
            DataSet ds = new DataSet();
            using (SqlConnection sqlcon = new SqlConnection(constr))
            {
                sqlcon.Open();
                using (SqlCommand com = new SqlCommand(sqlstr, sqlcon))
                {
                    com.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(ds, resultName);
                }
            }
            return ds;
        }

        public DataTable getTableData( string dbpath)
        {
            string sqlstr = string.Format("select * from {0}",dbpath);
            DataSet ds = new DataSet();
            using (SqlConnection sqlcon = new SqlConnection(constr))
            {
                sqlcon.Open();
                using (SqlCommand com = new SqlCommand(sqlstr, sqlcon))
                {
                    com.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(ds, dbpath);
                }
            }
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else return null;
        }

        public int ExecuteInsert(string sqlstr)
        {
            int rowIndex = 0;
            using (SqlConnection sqlcon = new SqlConnection(constr))
            {
                sqlcon.Open();
                using (SqlCommand com = new SqlCommand(sqlstr, sqlcon))
                {

                    com.CommandType = CommandType.Text;
                    rowIndex = com.ExecuteNonQuery();
                }
            }
            return rowIndex;
        }
        public int ExecuteInsert(string sqlstr,string connectstr)
        {
            int rowIndex = 0;
            using (SqlConnection sqlcon = new SqlConnection(connectstr))
            {
                sqlcon.Open();
                using (SqlCommand com = new SqlCommand(sqlstr, sqlcon))
                {

                    com.CommandType = CommandType.Text;
                    rowIndex = com.ExecuteNonQuery();
                }
            }
            return rowIndex;
        }
    }
}
