﻿namespace javascripttest
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
            this.typeOfCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rankOfNobility = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.user_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Server_url = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.city_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operation = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddAccount
            // 
            this.btnAddAccount.Location = new System.Drawing.Point(376, 22);
            this.btnAddAccount.Name = "btnAddAccount";
            this.btnAddAccount.Size = new System.Drawing.Size(75, 25);
            this.btnAddAccount.TabIndex = 8;
            this.btnAddAccount.Text = "添加";
            this.btnAddAccount.UseVisualStyleBackColor = false;
            this.btnAddAccount.Click += new System.EventHandler(this.btnAddAccount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "密码：";
            // 
            // T_username
            // 
            this.T_username.AllowDrop = true;
            this.T_username.Location = new System.Drawing.Point(75, 24);
            this.T_username.Name = "T_username";
            this.T_username.Size = new System.Drawing.Size(100, 20);
            this.T_username.TabIndex = 3;
            // 
            // T_password
            // 
            this.T_password.Location = new System.Drawing.Point(261, 24);
            this.T_password.Name = "T_password";
            this.T_password.Size = new System.Drawing.Size(100, 20);
            this.T_password.TabIndex = 4;
            // 
            // delete_account
            // 
            this.delete_account.Location = new System.Drawing.Point(1090, 22);
            this.delete_account.Name = "delete_account";
            this.delete_account.Size = new System.Drawing.Size(75, 25);
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
            this.chief,
            this.typeOfCountry,
            this.rankOfNobility,
            this.user_id,
            this.username,
            this.password,
            this.Server_url,
            this.city_num,
            this.operation});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Green;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(15, 70);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1150, 567);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // EmpireName
            // 
            this.EmpireName.Location = new System.Drawing.Point(849, 24);
            this.EmpireName.Name = "EmpireName";
            this.EmpireName.Size = new System.Drawing.Size(100, 20);
            this.EmpireName.TabIndex = 9;
            // 
            // search
            // 
            this.search.Location = new System.Drawing.Point(974, 22);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(75, 25);
            this.search.TabIndex = 10;
            this.search.Text = "查询";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(779, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "君主名：";
            // 
            // import
            // 
            this.import.Location = new System.Drawing.Point(469, 24);
            this.import.Name = "import";
            this.import.Size = new System.Drawing.Size(75, 23);
            this.import.TabIndex = 13;
            this.import.Text = "导入";
            this.import.UseVisualStyleBackColor = true;
            this.import.Click += new System.EventHandler(this.import_Click);
            // 
            // export
            // 
            this.export.Location = new System.Drawing.Point(564, 24);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(75, 23);
            this.export.TabIndex = 14;
            this.export.Text = "导出";
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // chief
            // 
            this.chief.DataPropertyName = "chief";
            this.chief.HeaderText = "君主名";
            this.chief.Name = "chief";
            this.chief.ReadOnly = true;
            this.chief.Width = 68;
            // 
            // typeOfCountry
            // 
            this.typeOfCountry.DataPropertyName = "typeOfCountry";
            this.typeOfCountry.HeaderText = "国别";
            this.typeOfCountry.Name = "typeOfCountry";
            this.typeOfCountry.ReadOnly = true;
            this.typeOfCountry.Width = 56;
            // 
            // rankOfNobility
            // 
            this.rankOfNobility.DataPropertyName = "rankOfNobility";
            this.rankOfNobility.HeaderText = "爵位";
            this.rankOfNobility.Name = "rankOfNobility";
            this.rankOfNobility.ReadOnly = true;
            this.rankOfNobility.Width = 56;
            // 
            // user_id
            // 
            this.user_id.DataPropertyName = "user_id";
            this.user_id.HeaderText = "用户id";
            this.user_id.Name = "user_id";
            this.user_id.ReadOnly = true;
            this.user_id.Width = 64;
            // 
            // username
            // 
            this.username.DataPropertyName = "username";
            this.username.HeaderText = "用户名";
            this.username.Name = "username";
            this.username.ReadOnly = true;
            this.username.Width = 68;
            // 
            // password
            // 
            this.password.DataPropertyName = "password";
            this.password.HeaderText = "密码";
            this.password.Name = "password";
            this.password.ReadOnly = true;
            this.password.Width = 56;
            // 
            // Server_url
            // 
            this.Server_url.DataPropertyName = "Server_url";
            this.Server_url.HeaderText = "所属服务器";
            this.Server_url.Name = "Server_url";
            this.Server_url.ReadOnly = true;
            this.Server_url.Width = 92;
            // 
            // city_num
            // 
            this.city_num.DataPropertyName = "city_num";
            this.city_num.HeaderText = "城镇个数";
            this.city_num.Name = "city_num";
            this.city_num.ReadOnly = true;
            this.city_num.Width = 80;
            // 
            // operation
            // 
            this.operation.HeaderText = "操作";
            this.operation.Name = "operation";
            this.operation.ReadOnly = true;
            this.operation.Text = "战斗任务";
            this.operation.UseColumnTextForButtonValue = true;
            this.operation.Width = 37;
            // 
            // accountants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1028, 650);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn typeOfCountry;
        private System.Windows.Forms.DataGridViewTextBoxColumn rankOfNobility;
        private System.Windows.Forms.DataGridViewTextBoxColumn user_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
        private System.Windows.Forms.DataGridViewTextBoxColumn password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Server_url;
        private System.Windows.Forms.DataGridViewTextBoxColumn city_num;
        private System.Windows.Forms.DataGridViewButtonColumn operation;
    }
}