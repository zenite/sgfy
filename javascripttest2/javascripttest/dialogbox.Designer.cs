namespace javascripttest
{
    using System.Windows.Forms;
    partial class dialogbox
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
            this.components = new System.ComponentModel.Container();
            this.Tablelayout = new System.Windows.Forms.TableLayoutPanel();
            this.CodePicture = new System.Windows.Forms.PictureBox();
            this.button_cancel = new System.Windows.Forms.Button();
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.CodeLabel = new System.Windows.Forms.Label();
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.Tablelayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CodePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Tablelayout
            // 
            this.Tablelayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.Tablelayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.Tablelayout.Controls.Add(this.CodePicture, 1, 0);
            this.Tablelayout.Controls.Add(this.button_cancel, 1, 2);
            this.Tablelayout.Controls.Add(this.button_ok, 0, 2);
            this.Tablelayout.Location = new System.Drawing.Point(0, 0);
            this.Tablelayout.Name = "Tablelayout";
            this.Tablelayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.Tablelayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.Tablelayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.Tablelayout.Size = new System.Drawing.Size(200, 176);
            this.Tablelayout.TabIndex = 3;
            // 
            // CodePicture
            // 
            this.CodePicture.Location = new System.Drawing.Point(67, 3);
            this.CodePicture.Name = "CodePicture";
            this.CodePicture.Size = new System.Drawing.Size(130, 70);
            this.CodePicture.TabIndex = 2;
            this.CodePicture.TabStop = false;
            // 
            // button_cancel
            // 
            this.button_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_cancel.Location = new System.Drawing.Point(67, 153);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(45, 20);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "取消";
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CodeTextBox.Location = new System.Drawing.Point(3, 103);
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.Size = new System.Drawing.Size(100, 21);
            this.CodeTextBox.TabIndex = 1;
            this.CodeTextBox.TextChanged += new System.EventHandler(this.CodeTextBox_TextChanged);
            this.CodeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CodeTextBox_KeyPress);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(3, 153);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(45, 20);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确定";
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // CodeLabel
            // 
            this.CodeLabel.Location = new System.Drawing.Point(0, 0);
            this.CodeLabel.Name = "CodeLabel";
            this.CodeLabel.Size = new System.Drawing.Size(50, 25);
            this.CodeLabel.TabIndex = 0;
            // 
            // timer_0
            // 
            this.timer_0.Interval = 1000;
            this.timer_0.Tag = "20";
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // dialogbox
            // 
            this.AcceptButton = this.button_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_cancel;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.CodeLabel);
            this.Controls.Add(this.CodeTextBox);
            this.Controls.Add(this.Tablelayout);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dialogbox";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.RightToLeftLayout = true;
            this.Text = "dialogbox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.dialogbox_Load);
            this.Shown += new System.EventHandler(this.dialogbox_Shown);
            this.Tablelayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CodePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        #endregion
    }
}