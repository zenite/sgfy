namespace javascripttest
{
    partial class accountants
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnAddAccount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.T_username = new System.Windows.Forms.TextBox();
            this.T_password = new System.Windows.Forms.TextBox();
            this.delete_account = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.EmpireName = new System.Windows.Forms.TextBox();
            this.search = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.import = new System.Windows.Forms.Button();
            this.export = new System.Windows.Forms.Button();
            this.chief = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Location = new System.Drawing.Point(501, 25);
            this.btnAddAccount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(100, 29);
            this.btnAddAccount.TabIndex = 8;
            this.btnAddAccount.Text = "添加";
            this.btnAddAccount.UseVisualStyleBackColor = false;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(285, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // T_username
            // 
            this.T_username.AllowDrop = true;
            this.T_username.Location = new System.Drawing.Point(100, 28);
            this.T_username.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.T_username.Name = "T_username";
            this.T_username.Size = new System.Drawing.Size(132, 25);
            this.T_username.TabIndex = 3;
            // 
            // T_password
            // 
            this.T_password.Location = new System.Drawing.Point(348, 28);
            this.T_password.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.T_password.Name = "T_password";
            this.T_password.Size = new System.Drawing.Size(132, 25);
            this.T_password.TabIndex = 4;
            // 
            // delete_account
            // 
            this.delete_account.Location = new System.Drawing.Point(1453, 25);
            this.delete_account.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.delete_account.Name = "delete_account";
            this.delete_account.Size = new System.Drawing.Size(100, 29);
            this.delete_account.TabIndex = 7;
            this.delete_account.Text = "删除";
            this.delete_account.UseVisualStyleBackColor = false;
            this.delete_account.Click += new System.EventHandler(this.delete_account_Click);
            this.delete_account.MouseDown += new System.Windows.Forms.MouseEventHandler(this.delete_account_MouseDown);
            this.delete_account.MouseUp += new System.Windows.Forms.MouseEventHandler(this.delete_account_MouseUp);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chief});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Green;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(20, 81);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1533, 654);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // EmpireName
            // 
            this.EmpireName.Location = new System.Drawing.Point(1132, 28);
            this.EmpireName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.EmpireName.Name = "EmpireName";
            this.EmpireName.Size = new System.Drawing.Size(132, 25);
            this.EmpireName.TabIndex = 9;
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(1299, 25);
            this.search.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(100, 29);
            this.search.TabIndex = 10;
            this.search.Text = "查询";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1039, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "君主名：";
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(625, 28);
            this.import.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(100, 27);
            this.import.TabIndex = 13;
            this.import.Text = "导入";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(752, 28);
            this.export.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(100, 27);
            this.export.TabIndex = 14;
            this.export.Text = "导出";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // chief
            // 
            this.chief.HeaderText = "君主名";
            this.chief.Name = "chief";
            this.chief.ReadOnly = true;
            this.chief.Width = 81;
            // 
            // accountants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1600, 750);
            this.Controls.Add(this.export);
            this.Controls.Add(this.import);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.search);
            this.Controls.Add(this.EmpireName);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.delete_account);
            this.Controls.Add(this.T_password);
            this.Controls.Add(this.T_username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAddAccount);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "accountants";
            this.Text = "accountants";
            this.Load += new System.EventHandler(this.accountants_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddAccount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox T_username;
        private System.Windows.Forms.TextBox T_password;
        private System.Windows.Forms.Button delete_account;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox EmpireName;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button import;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.DataGridViewTextBoxColumn chief;
    }
}