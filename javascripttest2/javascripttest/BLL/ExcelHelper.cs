using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace javascripttest
{
    public class ExcelHelper
    {
        public ExcelHelper() { }

        public DataSet getExeclData(string path)
        {
            DataSet ds = new DataSet();
            string extension = Path.GetExtension(path);
            string strCon = string.Empty;
            if (string.IsNullOrEmpty(extension)) return null;
            else
            {
                switch(extension)
                {
                    case ".xlsx":
                        strCon = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source= '{0} ';Extended Properties= 'Excel 12.0;HDR=Yes;IMEX=1 '; ", path);
                        break;
                    case ".xls":
                        strCon = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= '{0} ';Extended Properties= 'Excel 4.0;HDR=Yes;IMEX=1 '; ", path);
                        break;
                    case ".csv":
                        strCon = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source= '{0} ';Extended Properties= 'Text;HDR=Yes;FMT=Delimited(,);IMEX=1'", new FileInfo(path).DirectoryName);
                        break;                  
                }                    

            }
            try
            {
                OleDbConnection myConn = new OleDbConnection(strCon);
                string strCom=string.Empty;
                if (extension != ".csv"&&extension !=".txt")
                    strCom = " SELECT * FROM [Sheet1$] ";
                else
                    strCom = "select * from " + new FileInfo(path).Name;
                myConn.Open();
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                myCommand.Fill(ds, "Sheet1");
                myConn.Close();
            }
            catch (Exception ex)
            {
                //log                
            }           
            return ds;
        }

        public DataSet getDataSource(string filePath, string VillageId)
        {
            DataSet ds = new DataSet();
            string extension = Path.GetExtension(filePath);
            if (extension.Contains("txt"))
            {
                ds = getDataTxt(filePath, VillageId);
            }
            else
            {
                ds=getExeclData(filePath);
            }
            return ds;
        }

        public DataSet getDataTxt(string filepath, string VillageId)
        {
           
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("x",typeof(string)));
            dt.Columns.Add(new DataColumn("y", typeof(string)));
            dt.Columns.Add(new DataColumn("city", typeof(string)));
            dt.Columns.Add(new DataColumn("chief", typeof(string)));
            dt.Columns.Add(new DataColumn("hand", typeof(string)));
            dt.Columns.Add(new DataColumn("name", typeof(string)));
            dt.Columns.Add(new DataColumn("NodeName", typeof(string)));
            dt.Columns.Add(new DataColumn("VillageId", typeof(string)));
            DataRow dr;
            string[] list = File.ReadAllLines(filepath, Encoding.GetEncoding("gb2312"));
            bool mark = false;
            string[] str=new string[8];
            for (int i = 0; i < list.Length; i++)
            {                
                if (mark)
                {
                    dr = dt.NewRow();
                    str=list[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    dr["x"] = str[0];
                    dr["y"] = str[1];
                    dr["city"] = str[2];
                    dr["chief"] = str[3];
                    dr["hand"] = str[4];
                    dr["name"] = "Attack";
                    dr["NodeName"] = "Attack";
                    dr["VillageId"] = VillageId;
                    dt.Rows.Add(dr);
                }
                if (list[i] == "") mark = true;
            }
            ds.Tables.Add(dt);
                return ds;
        }
    }
}
