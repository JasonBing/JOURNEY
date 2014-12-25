namespace TCS
{
    partial class FormCommunicate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCommunicate));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbName = new System.Windows.Forms.Label();
            this.lbSign = new System.Windows.Forms.Label();
            this.rtbRecv = new System.Windows.Forms.RichTextBox();
            this.rtbSend = new System.Windows.Forms.RichTextBox();
            this.btSend = new System.Windows.Forms.Button();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btRecvFile = new System.Windows.Forms.Button();
            this.btRejectFile = new System.Windows.Forms.Button();
            this.pbRecvFile = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lvSendList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbRecvTips = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbRecvTips = new System.Windows.Forms.Label();
            this.btCancelSend = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(4, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(58, 9);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(89, 12);
            this.lbName.TabIndex = 1;
            this.lbName.Text = "TCS<127.0.0.1>";
            // 
            // lbSign
            // 
            this.lbSign.AutoSize = true;
            this.lbSign.Location = new System.Drawing.Point(58, 30);
            this.lbSign.Name = "lbSign";
            this.lbSign.Size = new System.Drawing.Size(29, 12);
            this.lbSign.TabIndex = 2;
            this.lbSign.Text = "X200";
            // 
            // rtbRecv
            // 
            this.rtbRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRecv.AutoWordSelection = true;
            this.rtbRecv.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtbRecv.Location = new System.Drawing.Point(12, 53);
            this.rtbRecv.Name = "rtbRecv";
            this.rtbRecv.ReadOnly = true;
            this.rtbRecv.Size = new System.Drawing.Size(346, 242);
            this.rtbRecv.TabIndex = 3;
            this.rtbRecv.Text = "";
            this.rtbRecv.TextChanged += new System.EventHandler(this.rtbRecv_TextChanged);
            // 
            // rtbSend
            // 
            this.rtbSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSend.AutoWordSelection = true;
            this.rtbSend.Location = new System.Drawing.Point(12, 327);
            this.rtbSend.Name = "rtbSend";
            this.rtbSend.Size = new System.Drawing.Size(346, 101);
            this.rtbSend.TabIndex = 0;
            this.rtbSend.Text = "";
            this.rtbSend.TextChanged += new System.EventHandler(this.rtbSend_TextChanged);
            this.rtbSend.KeyDown += new System.Windows.Forms.KeyEventHandler(this.rtbSend_KeyDown);
            // 
            // btSend
            // 
            this.btSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSend.Location = new System.Drawing.Point(282, 434);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(76, 24);
            this.btSend.TabIndex = 6;
            this.btSend.Text = "发送";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.AllowDrop = true;
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.CheckBoxes = true;
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnSize});
            this.lvFiles.Location = new System.Drawing.Point(364, 53);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(145, 210);
            this.lvFiles.TabIndex = 7;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragDrop);
            this.lvFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragEnter);
            // 
            // columnName
            // 
            this.columnName.Text = "名字";
            this.columnName.Width = 80;
            // 
            // columnSize
            // 
            this.columnSize.Text = "大小";
            // 
            // btRecvFile
            // 
            this.btRecvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRecvFile.Location = new System.Drawing.Point(364, 269);
            this.btRecvFile.Name = "btRecvFile";
            this.btRecvFile.Size = new System.Drawing.Size(69, 26);
            this.btRecvFile.TabIndex = 8;
            this.btRecvFile.Text = "接收文件";
            this.btRecvFile.UseVisualStyleBackColor = true;
            this.btRecvFile.Click += new System.EventHandler(this.btRecvFile_Click);
            // 
            // btRejectFile
            // 
            this.btRejectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btRejectFile.Location = new System.Drawing.Point(440, 269);
            this.btRejectFile.Name = "btRejectFile";
            this.btRejectFile.Size = new System.Drawing.Size(69, 26);
            this.btRejectFile.TabIndex = 9;
            this.btRejectFile.Text = "拒绝文件";
            this.btRejectFile.UseVisualStyleBackColor = true;
            this.btRejectFile.Click += new System.EventHandler(this.btRejectFile_Click);
            // 
            // pbRecvFile
            // 
            this.pbRecvFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRecvFile.Location = new System.Drawing.Point(362, 4);
            this.pbRecvFile.Maximum = 1000;
            this.pbRecvFile.Name = "pbRecvFile";
            this.pbRecvFile.Size = new System.Drawing.Size(153, 23);
            this.pbRecvFile.TabIndex = 10;
            this.pbRecvFile.Visible = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(200, 434);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(76, 24);
            this.button2.TabIndex = 11;
            this.button2.Text = "关闭";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 440);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "TSC V1.0";
            // 
            // lvSendList
            // 
            this.lvSendList.AllowDrop = true;
            this.lvSendList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSendList.CheckBoxes = true;
            this.lvSendList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvSendList.Location = new System.Drawing.Point(364, 316);
            this.lvSendList.Name = "lvSendList";
            this.lvSendList.Size = new System.Drawing.Size(145, 142);
            this.lvSendList.TabIndex = 13;
            this.lvSendList.UseCompatibleStateImageBehavior = false;
            this.lvSendList.View = System.Windows.Forms.View.Details;
            this.lvSendList.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragDrop);
            this.lvSendList.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "名字";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "大小";
            // 
            // tbRecvTips
            // 
            this.tbRecvTips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRecvTips.BackColor = System.Drawing.SystemColors.Control;
            this.tbRecvTips.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRecvTips.Location = new System.Drawing.Point(364, 33);
            this.tbRecvTips.Name = "tbRecvTips";
            this.tbRecvTips.Size = new System.Drawing.Size(117, 14);
            this.tbRecvTips.TabIndex = 14;
            this.tbRecvTips.Text = "文件接收列表";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "文件发送列表";
            // 
            // lbRecvTips
            // 
            this.lbRecvTips.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbRecvTips.AutoSize = true;
            this.lbRecvTips.Location = new System.Drawing.Point(479, 33);
            this.lbRecvTips.Name = "lbRecvTips";
            this.lbRecvTips.Size = new System.Drawing.Size(41, 12);
            this.lbRecvTips.TabIndex = 16;
            this.lbRecvTips.Text = "(100%)";
            this.lbRecvTips.Visible = false;
            // 
            // btCancelSend
            // 
            this.btCancelSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btCancelSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btCancelSend.Font = new System.Drawing.Font("宋体", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btCancelSend.Location = new System.Drawing.Point(443, 295);
            this.btCancelSend.Name = "btCancelSend";
            this.btCancelSend.Size = new System.Drawing.Size(66, 18);
            this.btCancelSend.TabIndex = 17;
            this.btCancelSend.Text = "取消发送";
            this.btCancelSend.UseVisualStyleBackColor = true;
            this.btCancelSend.Click += new System.EventHandler(this.btCancelSend_Click);
            // 
            // FormCommunicate
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 465);
            this.Controls.Add(this.btCancelSend);
            this.Controls.Add(this.lbRecvTips);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbRecvTips);
            this.Controls.Add(this.lvSendList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.pbRecvFile);
            this.Controls.Add(this.btRejectFile);
            this.Controls.Add(this.btRecvFile);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.btSend);
            this.Controls.Add(this.rtbSend);
            this.Controls.Add(this.rtbRecv);
            this.Controls.Add(this.lbSign);
            this.Controls.Add(this.lbName);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(534, 450);
            this.Name = "FormCommunicate";
            this.Text = "FormCommunicate";
            this.Load += new System.EventHandler(this.FormCommunicate_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormCommunicate_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbSign;
        private System.Windows.Forms.RichTextBox rtbRecv;
        private System.Windows.Forms.RichTextBox rtbSend;
        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.Button btRecvFile;
        private System.Windows.Forms.Button btRejectFile;
        private System.Windows.Forms.ProgressBar pbRecvFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvSendList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TextBox tbRecvTips;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbRecvTips;
        private System.Windows.Forms.Button btCancelSend;
    }
}