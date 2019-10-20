using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class report : UserControl
    {
        public report()
        {
            InitializeComponent();
            resource.Width=200;
            initGrid();
        }
        private DataTable table;
        
        private void initGrid()
        {
            table = new DataTable("Datas");
            table.Columns.Add("chief", Type.GetType("System.String"));
            table.Columns.Add("city", Type.GetType("System.String"));
            table.Columns.Add("x", Type.GetType("System.String"));
            table.Columns.Add("y", Type.GetType("System.String"));
            table.Columns.Add("output", Type.GetType("System.String"));

            resource.DataSource = table.DefaultView;
            resource.Columns[0].Name = "chief";
            resource.Columns[0].HeaderText = "君主名";
            resource.Columns[0].Visible = true;
            resource.Columns[1].Name = "city";
            resource.Columns[1].HeaderText = "城镇";
            resource.Columns[1].Visible = true;
            resource.Columns[2].Name = "x";
            resource.Columns[2].HeaderText = "x";
            resource.Columns[2].Visible = true;
            resource.Columns[3].Name = "y";
            resource.Columns[3].HeaderText = "y";
            resource.Columns[3].Visible = true;
            resource.Columns[4].Name = "output";
            resource.Columns[4].HeaderText = "产量";
            resource.Columns[4].Visible = true;            
        }

        public void addItem(string log)
        {
            if (log0.InvokeRequired)
            {
                log0.BeginInvoke(new Action(() => { log0.Items.Add(log); }));
            }
            else
            {
                log0.Items.Add(log);
            }
        }

        public void refreshgrid(string chief, string city, string x, string y, string corp)
        {
            lock (table)
            {
                DataRow dr = table.NewRow();
                dr["chief"] = chief;
                dr["city"] = city;
                dr["x"] = x;
                dr["y"] = y;
                dr["output"] = corp;
                table.Rows.Add(dr);
            }
                     
        }

        public void refreshGrid()
        {
            resource.DataSource = table.DefaultView;
            resource.BeginInvoke(new Action(() => { resource.Refresh(); }));
        }

        private void log0_DrawItem(object sender, DrawItemEventArgs e)
        {
            Color vColor = e.ForeColor;
            ListBox listbox=(ListBox)sender;
            if(e.Index>-1)
            {
                var text=listbox.Items[e.Index].ToString();
                if (text.Contains("误")) vColor = Color.Red;
                else  if (text.Contains("没有")) vColor = Color.OrangeRed;
                else vColor = Color.Black;
            }
          
            e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font,
                new SolidBrush(vColor), e.Bounds);
            e.DrawFocusRectangle();
        }      
    }
}
    