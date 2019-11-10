namespace javascripttest
{
    partial class ChangePwd
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
            this.loginbtn = new System.Windows.Forms.Button();
            this.changebtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pwdTxt = new System.Windows.Forms.TextBox();
            this.log0 = new System.Windows.Forms.ListBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // loginbtn
            // 
            this.loginbtn.Location = new System.Drawing.Point(21, 9);
            this.loginbtn.Margin = new System.Windows.Forms.Padding(2);
            this.loginbtn.Name = "loginbtn";
            this.loginbtn.Size = new System.Drawing.Size(83, 28);
            this.loginbtn.TabIndex = 0;
            this.loginbtn.Text = "登录官网";
            this.loginbtn.UseVisualStyleBackColor = true;
            this.loginbtn.Click += new System.EventHandler(this.loginbtn_Click);
            // 
            // changebtn
            // 
            this.changebtn.Location = new System.Drawing.Point(1116, 5);
            this.changebtn.Margin = new System.Windows.Forms.Padding(2);
            this.changebtn.Name = "changebtn";
            this.changebtn.Size = new System.Drawing.Size(83, 28);
            this.changebtn.TabIndex = 0;
            this.changebtn.Text = "修改密码";
            this.changebtn.UseVisualStyleBackColor = true;
            this.changebtn.Click += new System.EventHandler(this.changebtn_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // pwdTxt
            // 
            this.pwdTxt.Location = new System.Drawing.Point(1037, 9);
            this.pwdTxt.Margin = new System.Windows.Forms.Padding(2);
            this.pwdTxt.Name = "pwdTxt";
            this.pwdTxt.Size = new System.Drawing.Size(75, 20);
            this.pwdTxt.TabIndex = 2;
            // 
            // log0
            // 
            this.log0.FormattingEnabled = true;
            this.log0.Location = new System.Drawing.Point(21, 65);
            this.log0.Name = "log0";
            this.log0.Size = new System.Drawing.Size(1178, 524);
            this.log0.TabIndex = 3;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(479, 5);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(221, 42);
            this.webBrowser1.TabIndex = 4;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // ChangePwd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1207, 612);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.log0);
            this.Controls.Add(this.pwdTxt);
            this.Controls.Add(this.changebtn);
            this.Controls.Add(this.loginbtn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChangePwd";
            this.Text = "ChangePwd";
            this.Load += new System.EventHandler(this.ChangePwd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginbtn;
        private System.Windows.Forms.Button changebtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox pwdTxt;
        private System.Windows.Forms.ListBox log0;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}