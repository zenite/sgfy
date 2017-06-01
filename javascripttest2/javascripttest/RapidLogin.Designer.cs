namespace javascripttest
{
    partial class RapidLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.webBr = new System.Windows.Forms.WebBrowser();
            this.attackinTime = new System.Windows.Forms.Button();
            this.upperCap_btn = new System.Windows.Forms.Button();
            this.setCap_btn = new System.Windows.Forms.Button();
            this.capLevel_txt = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.updateTips = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 75);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1307, 506);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusStrip1);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.webBr);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1301, 500);
            this.panel1.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 507);
            this.listBox1.TabIndex = 2;
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // webBr
            // 
            this.webBr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBr.Location = new System.Drawing.Point(142, 4);
            this.webBr.MinimumSize = new System.Drawing.Size(20, 22);
            this.webBr.Name = "webBr";
            this.webBr.ScriptErrorsSuppressed = true;
            this.webBr.Size = new System.Drawing.Size(1156, 500);
            this.webBr.TabIndex = 1;
            this.webBr.Url = new System.Uri("about:blank", System.UriKind.Absolute);
            this.webBr.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBr_DocumentCompleted);
            // 
            // attackinTime
            // 
            this.attackinTime.Location = new System.Drawing.Point(114, 12);
            this.attackinTime.Name = "attackinTime";
            this.attackinTime.Size = new System.Drawing.Size(75, 23);
            this.attackinTime.TabIndex = 1;
            this.attackinTime.Text = "压秒发攻击";
            this.attackinTime.UseVisualStyleBackColor = true;
            this.attackinTime.Click += new System.EventHandler(this.attackinTime_Click);
            // 
            // upperCap_btn
            // 
            this.upperCap_btn.Location = new System.Drawing.Point(1043, 21);
            this.upperCap_btn.Name = "upperCap_btn";
            this.upperCap_btn.Size = new System.Drawing.Size(75, 23);
            this.upperCap_btn.TabIndex = 1;
            this.upperCap_btn.Text = "陪都升级";
            this.upperCap_btn.UseVisualStyleBackColor = true;
            this.upperCap_btn.Click += new System.EventHandler(this.upperCap_btn_Click);
            // 
            // setCap_btn
            // 
            this.setCap_btn.Location = new System.Drawing.Point(1248, 21);
            this.setCap_btn.Name = "setCap_btn";
            this.setCap_btn.Size = new System.Drawing.Size(75, 23);
            this.setCap_btn.TabIndex = 1;
            this.setCap_btn.Text = "设置陪都";
            this.setCap_btn.UseVisualStyleBackColor = true;
            this.setCap_btn.Click += new System.EventHandler(this.setCap_btn_Click);
            // 
            // capLevel_txt
            // 
            this.capLevel_txt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.capLevel_txt.FormattingEnabled = true;
            this.capLevel_txt.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.capLevel_txt.Location = new System.Drawing.Point(1196, 23);
            this.capLevel_txt.Name = "capLevel_txt";
            this.capLevel_txt.Size = new System.Drawing.Size(46, 21);
            this.capLevel_txt.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1140, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "位置：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateTips});
            this.statusStrip1.Location = new System.Drawing.Point(0, 478);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1301, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // updateTips
            // 
            this.updateTips.Name = "updateTips";
            this.updateTips.Size = new System.Drawing.Size(0, 17);
            // 
            // RapidLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1328, 593);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.capLevel_txt);
            this.Controls.Add(this.setCap_btn);
            this.Controls.Add(this.upperCap_btn);
            this.Controls.Add(this.attackinTime);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "RapidLogin";
            this.Text = "RapidLogin";
            this.Load += new System.EventHandler(this.RapidLogin_Load);
            this.Resize += new System.EventHandler(this.RapidLogin_Resize);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.WebBrowser webBr;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button attackinTime;
        private System.Windows.Forms.Button upperCap_btn;
        private System.Windows.Forms.Button setCap_btn;
        private System.Windows.Forms.ComboBox capLevel_txt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel updateTips;
    }
}