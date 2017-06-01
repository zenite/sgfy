using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class fform : Form
    {
        public fform()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "test";
            string controlStr = this.textBox1.Text.Trim();
            pretreatment(str, controlStr);
        }
        private void pretreatment(string str, string controlStr)
        {
            string result = string.Empty;
            MethodInfo method = this.GetType().GetMethod(controlStr);
            object retrunObj= method.Invoke(this, new object[] { str, controlStr });
            this.show_Lable.Text = "结果是："+retrunObj as string;
        }

        public string test1(string str, string controlStr)
        {
            string result = string.Empty;
            if (controlStr == "test1")
            {
                result = str + "_" + controlStr + "_" + "1";
            }
            return result;
        }
        public string test2(string str, string controlStr)
        {
            string result = string.Empty;
            if (controlStr == "test2")
            {
                result = str + "_" + controlStr + "_" + "2";
            }
            return result;
        }
        public string test3(string str, string controlStr)
        {
            string result = string.Empty;
            if (controlStr == "test3")
            {
                result = str + "_" + controlStr + "_" + "3";
            }
            return result;
        }
        public string test4(string str, string controlStr)
        {
            string result = string.Empty;
            if (controlStr == "test4")
            {
                result = str + "_" + controlStr + "_" + "4";
            }
            return result;
        }
        public string test5(string str, string controlStr)
        {
            string result = string.Empty;
            if (controlStr == "test5")
            {
                result = str + "_" + controlStr + "_" + "5";
            }
            return result;
        }
    }
}
