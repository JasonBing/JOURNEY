namespace SAPRFCBuilder
{
    partial class frmSAPServerSetting
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pgSAPServerConfig = new System.Windows.Forms.PropertyGrid();
            this.ltSAPServer = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveServer = new System.Windows.Forms.Button();
            this.btnDeleteServer = new System.Windows.Forms.Button();
            this.btnSaveSetting = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnectionTest = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgSAPServerConfig
            // 
            this.pgSAPServerConfig.Location = new System.Drawing.Point(243, 23);
            this.pgSAPServerConfig.Name = "pgSAPServerConfig";
            this.pgSAPServerConfig.Size = new System.Drawing.Size(416, 195);
            this.pgSAPServerConfig.TabIndex = 0;
            this.pgSAPServerConfig.ToolbarVisible = false;
            // 
            // ltSAPServer
            // 
            this.ltSAPServer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ltSAPServer.FormattingEnabled = true;
            this.ltSAPServer.ItemHeight = 12;
            this.ltSAPServer.Location = new System.Drawing.Point(12, 23);
            this.ltSAPServer.Name = "ltSAPServer";
            this.ltSAPServer.Size = new System.Drawing.Size(225, 328);
            this.ltSAPServer.TabIndex = 1;
            this.ltSAPServer.SelectedIndexChanged += new System.EventHandler(this.ltSAPServer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "SAP服务器列表";
            // 
            // btnSaveServer
            // 
            this.btnSaveServer.Location = new System.Drawing.Point(243, 224);
            this.btnSaveServer.Name = "btnSaveServer";
            this.btnSaveServer.Size = new System.Drawing.Size(116, 33);
            this.btnSaveServer.TabIndex = 3;
            this.btnSaveServer.Text = "添加配置信息";
            this.btnSaveServer.UseVisualStyleBackColor = true;
            this.btnSaveServer.Click += new System.EventHandler(this.btnSaveServer_Click);
            // 
            // btnDeleteServer
            // 
            this.btnDeleteServer.Location = new System.Drawing.Point(380, 224);
            this.btnDeleteServer.Name = "btnDeleteServer";
            this.btnDeleteServer.Size = new System.Drawing.Size(124, 33);
            this.btnDeleteServer.TabIndex = 4;
            this.btnDeleteServer.Text = "删除配置信息";
            this.btnDeleteServer.UseVisualStyleBackColor = true;
            this.btnDeleteServer.Click += new System.EventHandler(this.btnDeleteServer_Click);
            // 
            // btnSaveSetting
            // 
            this.btnSaveSetting.Location = new System.Drawing.Point(535, 224);
            this.btnSaveSetting.Name = "btnSaveSetting";
            this.btnSaveSetting.Size = new System.Drawing.Size(126, 33);
            this.btnSaveSetting.TabIndex = 5;
            this.btnSaveSetting.Text = "保存设置";
            this.btnSaveSetting.UseVisualStyleBackColor = true;
            this.btnSaveSetting.Click += new System.EventHandler(this.btnSaveSetting_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnectionTest);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUserID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(243, 270);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 80);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "用户信息";
            // 
            // btnConnectionTest
            // 
            this.btnConnectionTest.Location = new System.Drawing.Point(201, 15);
            this.btnConnectionTest.Name = "btnConnectionTest";
            this.btnConnectionTest.Size = new System.Drawing.Size(75, 55);
            this.btnConnectionTest.TabIndex = 4;
            this.btnConnectionTest.Text = "连接测试";
            this.btnConnectionTest.UseVisualStyleBackColor = true;
            this.btnConnectionTest.Click += new System.EventHandler(this.btnConnectionTest_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(61, 49);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(133, 21);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(61, 22);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(133, 21);
            this.txtUserID.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "UserID:";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(535, 283);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(124, 55);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // SAPServerSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 356);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSaveSetting);
            this.Controls.Add(this.btnDeleteServer);
            this.Controls.Add(this.btnSaveServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ltSAPServer);
            this.Controls.Add(this.pgSAPServerConfig);
            this.Name = "SAPServerSetting";
            this.Text = "SAPRFCBuilder";
            this.Load += new System.EventHandler(this.SAPServerSetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgSAPServerConfig;
        private System.Windows.Forms.ListBox ltSAPServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSaveServer;
        private System.Windows.Forms.Button btnDeleteServer;
        private System.Windows.Forms.Button btnSaveSetting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConnectionTest;
        private System.Windows.Forms.Button btnNext;

    }
}

