namespace TCS
{
    partial class FormMain
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("我的好友");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("陌生人");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("黑名单");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.cbState = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tvUsersList = new System.Windows.Forms.TreeView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lvFindResult = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnIPAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Notifytimer = new System.Windows.Forms.Timer(this.components);
            this.btBroadCast = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.controlFind1 = new TCS.ControlFind();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cbState
            // 
            this.cbState.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbState.FormattingEnabled = true;
            this.cbState.Items.AddRange(new object[] {
            "在线",
            "隐身"});
            this.cbState.Location = new System.Drawing.Point(56, 1);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(52, 20);
            this.cbState.TabIndex = 0;
            this.cbState.SelectedIndexChanged += new System.EventHandler(this.cbState_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 55);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(274, 415);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.tabControl1.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tvUsersList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(266, 389);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "聊天";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tvUsersList
            // 
            this.tvUsersList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvUsersList.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tvUsersList.FullRowSelect = true;
            this.tvUsersList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tvUsersList.Indent = 19;
            this.tvUsersList.ItemHeight = 20;
            this.tvUsersList.Location = new System.Drawing.Point(0, 0);
            this.tvUsersList.Name = "tvUsersList";
            treeNode1.Name = "MyFriends";
            treeNode1.Text = "我的好友";
            treeNode2.Name = "Strangers";
            treeNode2.Text = "陌生人";
            treeNode3.Name = "BlackList";
            treeNode3.Text = "黑名单";
            this.tvUsersList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.tvUsersList.ShowLines = false;
            this.tvUsersList.ShowPlusMinus = false;
            this.tvUsersList.ShowRootLines = false;
            this.tvUsersList.Size = new System.Drawing.Size(265, 389);
            this.tvUsersList.TabIndex = 0;
            this.tvUsersList.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvUsersList_AfterCollapse);
            this.tvUsersList.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvUsersList_AfterExpand);
            this.tvUsersList.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvUsersList_NodeMouseDoubleClick);
            this.tvUsersList.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.tvUsersList.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(266, 389);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "待加";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "TSC";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // lvFindResult
            // 
            this.lvFindResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFindResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnIPAddr});
            this.lvFindResult.FullRowSelect = true;
            this.lvFindResult.GridLines = true;
            this.lvFindResult.Location = new System.Drawing.Point(75, 43);
            this.lvFindResult.Name = "lvFindResult";
            this.lvFindResult.Size = new System.Drawing.Size(194, 279);
            this.lvFindResult.TabIndex = 5;
            this.lvFindResult.UseCompatibleStateImageBehavior = false;
            this.lvFindResult.View = System.Windows.Forms.View.Details;
            this.lvFindResult.Visible = false;
            this.lvFindResult.DoubleClick += new System.EventHandler(this.lvFindResult_DoubleClick);
            this.lvFindResult.Leave += new System.EventHandler(this.lvFindResult_Leave);
            this.lvFindResult.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.lvFindResult.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            // 
            // columnName
            // 
            this.columnName.Text = "名字";
            // 
            // columnIPAddr
            // 
            this.columnIPAddr.Text = "IP地址";
            this.columnIPAddr.Width = 100;
            // 
            // Notifytimer
            // 
            this.Notifytimer.Interval = 500;
            this.Notifytimer.Tick += new System.EventHandler(this.Notifytimer_Tick);
            // 
            // btBroadCast
            // 
            this.btBroadCast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btBroadCast.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btBroadCast.Location = new System.Drawing.Point(195, 54);
            this.btBroadCast.Name = "btBroadCast";
            this.btBroadCast.Size = new System.Drawing.Size(69, 21);
            this.btBroadCast.TabIndex = 7;
            this.btBroadCast.Text = "群发";
            this.btBroadCast.UseVisualStyleBackColor = true;
            this.btBroadCast.Click += new System.EventHandler(this.btBroadCast_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(53, 53);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(115, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "TCS局域网通讯工具";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 11;
            // 
            // controlFind1
            // 
            this.controlFind1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlFind1.BackColor = System.Drawing.Color.Transparent;
            this.controlFind1.Location = new System.Drawing.Point(56, 32);
            this.controlFind1.Name = "controlFind1";
            this.controlFind1.Size = new System.Drawing.Size(219, 21);
            this.controlFind1.TabIndex = 6;
            this.controlFind1.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.controlFind1.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(271, 470);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lvFindResult);
            this.Controls.Add(this.btBroadCast);
            this.Controls.Add(this.controlFind1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cbState);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(277, 496);
            this.Name = "FormMain";
            this.Text = "TCS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseEnter += new System.EventHandler(this.FormMain_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.FormMain_MouseLeave);
            this.Move += new System.EventHandler(this.FormMain_Move);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TreeView tvUsersList;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ListView lvFindResult;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnIPAddr;
        private ControlFind controlFind1;
        private System.Windows.Forms.Timer Notifytimer;
        private System.Windows.Forms.Button btBroadCast;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}

