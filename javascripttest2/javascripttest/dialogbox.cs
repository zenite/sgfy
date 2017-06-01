using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace javascripttest
{
    public partial class dialogbox : Form
    {
        public PictureBox CodePicture;
        private Label CodeLabel;
        
        public TextBox CodeTextBox;
        private TableLayoutPanel Tablelayout;
        private IContainer icontainer_0;
        private Button button_cancel;
        private Button button_ok;
        private Timer timer_0;
        public dialogbox()
        {           
            this.icontainer_0 = null;
            InitializeComponent();            
        }

        private void dialogbox_Load(object sender, EventArgs e)
        {

        }
        private void button_ok_Click(object sender, EventArgs e) {
            base.DialogResult = DialogResult.OK;
            base.Close();
        }
        private void button_cancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }
        private void CodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.CodeTextBox.Text.Length >=4) {
                button_ok.PerformClick(); 
            }
        }
        private void CodeTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            timer_0.Tag = 10;
        }
        private void timer_0_Tick(object sender, EventArgs e)
        {
            CodeLabel.Text = timer_0.Tag.ToString();
            timer_0.Tag = Convert.ToInt32(timer_0.Tag)-1;
            if ((int)timer_0.Tag <= 0) {
                button_ok.PerformClick();
            }
        }
        private void dialogbox_Shown(object sender, EventArgs e)
        {
            timer_0.Tag = 20;
            timer_0.Enabled = true;
            base.BringToFront();
        }


    }
}
