using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace javascripttest.entity
{
    public class CommonDelegate
    {
        public delegate void recordLog(string log, int logindex);
        public delegate void refreshForm(string chief,string city,string x,string y,string corp);
        public delegate void reLogin(object o);
    }
}
