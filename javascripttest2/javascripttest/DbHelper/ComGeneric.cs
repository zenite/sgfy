using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace javascripttest.DbHelper
{
   public  class ComGeneric<T>
    {
       public T ConvertToModel(DataRow dr)
       {
           T obj = default(T);
           if(dr!=null)
           {
               obj = Activator.CreateInstance<T>();
               foreach (DataColumn column in dr.Table.Columns)
               {
                   PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                   try
                   {
                       object value = dr[column.ColumnName];
                       prop.SetValue(obj, value, null);
                   }
                   catch
                   {  //You can log something here     
                       //throw;    
                   }
               }
           }
           return obj;
       }
    }
}
