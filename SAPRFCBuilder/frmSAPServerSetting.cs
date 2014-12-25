using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using SAPCOM;
namespace SAPRFCBuilder
{
    public partial class frmSAPServerSetting : Form
    {
        public frmSAPServerSetting()
        {
            InitializeComponent();
        }
        private string _strXMLConfig = null;
        private void SAPServerSetting_Load(object sender, EventArgs e)
        {
            SAPServerConfig ServerConfig = new SAPServerConfig();
            SAPServerConfig.SAPSeverInfo Info = new SAPServerConfig.SAPSeverInfo(ServerConfig);
            this.pgSAPServerConfig.SelectedObject = Info;
            string strFilePath = Application.ExecutablePath;
            int nPos = strFilePath.LastIndexOf("\\");
            strFilePath = strFilePath.Substring(0, ++nPos);
            strFilePath += SAPServerConfig.xmlConfigName;
            _strXMLConfig = ServerConfig.LoadConfigXML(strFilePath);
            string[] SAPServerNames = ServerConfig.GetSAPServerNames(_strXMLConfig);
            foreach (string ServerName in SAPServerNames)
            {
                this.ltSAPServer.Items.Add(ServerName);
            }
        }

        private void ltSAPServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strLocalSAPName = (string)ltSAPServer.SelectedItem;
            if (strLocalSAPName == null || _strXMLConfig == null)
                return;
            SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
            SAPServerConfig ServerConfig = Info.serverConfig;
            XmlNode oNode = ServerConfig.SearchConfiguration(_strXMLConfig, strLocalSAPName)[0];
            Info.NAME = oNode.Attributes["NAME"].Value;
            Info.LANGUAGE = oNode.Attributes["LANGUAGE"].Value;
            Info.ASHOST = oNode.Attributes["ASHOST"].Value;
            Info.CLIENT = oNode.Attributes["CLIENT"].Value;
            Info.SYSNR = oNode.Attributes["SYSNR"].Value;
            pgSAPServerConfig.Refresh();
        }

        private void btnSaveServer_Click(object sender, EventArgs e)
        {
            string strMsg = "";
            SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
            SAPServerConfig ServerConfig = Info.serverConfig;
            if (Info.NAME.Trim().Length == 0)
                return;
            if (!ServerConfig.AddConfiguration(ref _strXMLConfig))
            {
                strMsg = "名称已经使用，请指定其他名称。";
                MessageBox.Show(this, strMsg, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ltSAPServer.Items.Add(Info.NAME);
        }

        private void btnDeleteServer_Click(object sender, EventArgs e)
        {
            string strMsg = "";

            SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
            SAPServerConfig ServerConfig = Info.serverConfig;

            if ((Info.NAME.Trim().Length == 0)
                || !String.Equals((string)ltSAPServer.SelectedItem, Info.NAME)
                || !ServerConfig.RemoveConfiguration(ref _strXMLConfig))
            {
                strMsg = "表中记录的服务器被取消选择";
                MessageBox.Show(this, strMsg, this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ltSAPServer.Items.Remove(ltSAPServer.SelectedItem);
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            string strFilePath = null;
            int nPos = 0;

            SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
            SAPServerConfig ServerConfig = Info.serverConfig;

            strFilePath = Application.ExecutablePath;
            nPos = strFilePath.LastIndexOf("\\");
            strFilePath = strFilePath.Substring(0, ++nPos);
            strFilePath += SAPServerConfig.xmlConfigName;

            ServerConfig.SaveConfigFile(strFilePath, _strXMLConfig);
        }

        private void btnConnectionTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtUserID.Text.Trim().Length<=0)
                {
                    MessageBox.Show("输入用户名!");
                    return;
                }
                if (this.txtPassword.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("输入密码!");
                    return;
                }
                SAPDestination Dest = new SAPDestination();
                SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
                if (Info.ASHOST ==null || Info.ASHOST.Length<=0)
                {
                    MessageBox.Show("请选中或输入连接信息!");
                    return;
                }
                Dest.SapServerHost = Info.ASHOST;
                Dest.Client = Info.CLIENT;
                Dest.Language = Info.LANGUAGE;
                Dest.SystemNumber = Info.SYSNR;
                Dest.UserName=this.txtUserID.Text.Trim();
                Dest.Password = this.txtPassword.Text.Trim();
                SAPConnector Coon = new SAPConnector(Dest);
                Coon.ConnectionTest();
                MessageBox.Show("连接测试成功!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtUserID.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("输入用户名!");
                    return;
                }
                if (this.txtPassword.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("输入密码!");
                    return;
                }
                this.Visible = false;
                SAPDestination Dest = new SAPDestination();
                SAPServerConfig.SAPSeverInfo Info = (SAPServerConfig.SAPSeverInfo)pgSAPServerConfig.SelectedObject;
                Dest.SapServerHost = Info.ASHOST;
                Dest.Client = Info.CLIENT;
                Dest.Language = Info.LANGUAGE;
                Dest.SystemNumber = Info.SYSNR;
                Dest.UserName=this.txtUserID.Text.Trim();
                Dest.Password = this.txtPassword.Text.Trim();
                frmFunctionSelect frmNext = new frmFunctionSelect(Dest);
                frmNext.ShowDialog();
                this.Visible = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
    }
}
