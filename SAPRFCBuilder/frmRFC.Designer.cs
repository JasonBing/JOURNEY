namespace SAPRFCBuilder
{
    partial class frmRFC
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
            this.ltSearchResult = new System.Windows.Forms.ListBox();
            this.ltSelectResult = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSAPGroup = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSAPName = new System.Windows.Forms.TextBox();
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gpLocal = new System.Windows.Forms.GroupBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.gbFilter.SuspendLayout();
            this.gpLocal.SuspendLayout();
            this.SuspendLayout();
            // 
            // ltSearchResult
            // 
            this.ltSearchResult.FormattingEnabled = true;
            this.ltSearchResult.ItemHeight = 12;
            this.ltSearchResult.Location = new System.Drawing.Point(12, 56);
            this.ltSearchResult.Name = "ltSearchResult";
            this.ltSearchResult.Size = new System.Drawing.Size(303, 196);
            this.ltSearchResult.TabIndex = 0;
            // 
            // ltSelectResult
            // 
            this.ltSelectResult.FormattingEnabled = true;
            this.ltSelectResult.ItemHeight = 12;
            this.ltSelectResult.Location = new System.Drawing.Point(357, 56);
            this.ltSelectResult.Name = "ltSelectResult";
            this.ltSelectResult.Size = new System.Drawing.Size(302, 196);
            this.ltSelectResult.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(322, 91);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(29, 52);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "添加";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(321, 170);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(30, 52);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "移除";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Group:";
            // 
            // txtSAPGroup
            // 
            this.txtSAPGroup.Location = new System.Drawing.Point(53, 23);
            this.txtSAPGroup.Name = "txtSAPGroup";
            this.txtSAPGroup.Size = new System.Drawing.Size(188, 21);
            this.txtSAPGroup.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Name:";
            // 
            // txtSAPName
            // 
            this.txtSAPName.Location = new System.Drawing.Point(297, 23);
            this.txtSAPName.Name = "txtSAPName";
            this.txtSAPName.Size = new System.Drawing.Size(203, 21);
            this.txtSAPName.TabIndex = 7;
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnSearch);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Controls.Add(this.txtSAPName);
            this.gbFilter.Controls.Add(this.txtSAPGroup);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Location = new System.Drawing.Point(12, 0);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(647, 50);
            this.gbFilter.TabIndex = 8;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(507, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(134, 38);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gpLocal
            // 
            this.gpLocal.Controls.Add(this.txtClassName);
            this.gpLocal.Controls.Add(this.label5);
            this.gpLocal.Controls.Add(this.btnOpenFile);
            this.gpLocal.Controls.Add(this.txtPath);
            this.gpLocal.Controls.Add(this.label4);
            this.gpLocal.Controls.Add(this.btnCreate);
            this.gpLocal.Controls.Add(this.btnPrevious);
            this.gpLocal.Controls.Add(this.txtNameSpace);
            this.gpLocal.Controls.Add(this.label3);
            this.gpLocal.Location = new System.Drawing.Point(12, 258);
            this.gpLocal.Name = "gpLocal";
            this.gpLocal.Size = new System.Drawing.Size(647, 96);
            this.gpLocal.TabIndex = 9;
            this.gpLocal.TabStop = false;
            this.gpLocal.Text = "Local";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Font = new System.Drawing.Font("华文中宋", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(134)));
            this.btnOpenFile.Location = new System.Drawing.Point(294, 58);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(32, 23);
            this.btnOpenFile.TabIndex = 6;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(79, 60);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(212, 21);
            this.txtPath.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Path:";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(500, 56);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(141, 27);
            this.btnCreate.TabIndex = 3;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(345, 54);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(141, 32);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "上一步";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(79, 21);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(189, 21);
            this.txtNameSpace.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "NameSpace:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(274, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "ClassName:";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(345, 21);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(296, 21);
            this.txtClassName.TabIndex = 8;
            // 
            // frmRFC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 356);
            this.Controls.Add(this.gpLocal);
            this.Controls.Add(this.gbFilter);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.ltSelectResult);
            this.Controls.Add(this.ltSearchResult);
            this.Name = "frmRFC";
            this.Text = "frmRFC";
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.gpLocal.ResumeLayout(false);
            this.gpLocal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ltSearchResult;
        private System.Windows.Forms.ListBox ltSelectResult;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSAPGroup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSAPName;
        private System.Windows.Forms.GroupBox gbFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox gpLocal;
        private System.Windows.Forms.TextBox txtNameSpace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtClassName;
    }
}