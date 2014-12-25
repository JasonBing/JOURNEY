using SAPCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAPRFCBuilder
{
    public partial class frmRFC : Form
    {
        private SAPDestination Destination;
        public frmRFC(SAPDestination Destination)
        {
            InitializeComponent();
            this.Destination = Destination;
        }
        /// <summary>
        /// 查询SAP函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtSAPGroup.Text.Trim().Length <= 0 && this.txtSAPName.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("筛选条件 Group 和 Name 至少输入一个！");
                    return;
                }
                SAPFunction Func = new SAPFunction(Destination);
                List<string> ltSearchData = Func.SearchFuntion(this.txtSAPGroup.Text.Trim(), this.txtSAPName.Text.Trim());
                foreach (string value in ltSearchData)
                {
                    this.ltSearchResult.Items.Add(value);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ltSearchResult.SelectedIndex < 0)
                {
                    MessageBox.Show("请选中要添加的数据!"); return;
                }
                else
                {
                    this.ltSelectResult.Items.Add(this.ltSearchResult.SelectedItem);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ltSelectResult.SelectedIndex < 0)
                {
                    MessageBox.Show("请选中要移除的数据!"); return;
                }
                else
                {
                    this.ltSelectResult.Items.Remove(this.ltSelectResult.SelectedItem);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        ///  选择保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 生成CS文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtPath.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("请选择保存路径！"); return;
                }
                if (this.ltSelectResult.Items.Count <= 0)
                {
                    MessageBox.Show("请选择 RFC ！"); return;
                }
                if (this.txtNameSpace.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("请输入NameSpace！"); return;
                }
                if (this.txtClassName.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("请输入ClassName！"); return;
                }
                //SAPFunction Func = new SAPFunction(this.Destination);
                //bool result = Func.SAPFunctionBulider((string)this.ltSelectResult.SelectedItem, this.txtNameSpace.Text.Trim(), this.txtClassName.Text.Trim(), this.txtPath.Text.Trim(), true);
               //if (result == true)
               // {
               //     MessageBox.Show("编译成功!");
               // }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
