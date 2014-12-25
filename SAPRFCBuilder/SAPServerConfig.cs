using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace SAPRFCBuilder
{
    public class SAPServerConfig
    {
        public const string xmlConfigName = "SAPINFO.XML";
        private string Name = "";
        private string ASHOST = "";
        private string SYSNR = "";
        private string CLIENT = "";
        private string LANGUAGE = "EN";
        public SAPServerConfig()
        {
            
        }
        //加载配置文件
        public string LoadConfigXML(string filePathName)
         {
             string restult = null;
             XmlDocument xmlDoc = null;
             try
             {
                 xmlDoc = new XmlDocument();
                 //保留空格
                 xmlDoc.PreserveWhitespace = false;
                 xmlDoc.Load(filePathName);
                 restult = xmlDoc.InnerXml;
             }
             catch (System.IO.FileNotFoundException)
             {
                 restult = null;
             }
             catch (Exception exp)
             {
                 throw exp;
             }

             return restult;
         }
        //获取连接名字
        public string[] GetSAPServerNames(string strXMLConfig)
        {
            string[] arrNames = null;
            XmlNodeList oNodeList = null;
            XmlDocument oXMLDOC = null;

            if (strXMLConfig == null)
                return arrNames;

            try
            {
                oXMLDOC = new XmlDocument();
                oXMLDOC.PreserveWhitespace = false;
                oXMLDOC.LoadXml(strXMLConfig);

                oNodeList = oXMLDOC.SelectNodes("//@NAME");

                arrNames = new string[oNodeList.Count];
                for (int i = 0; i < oNodeList.Count; ++i)
                {
                    arrNames[i] = oNodeList[i].Value;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return arrNames;
        }
        //获取连接信息
        public XmlNodeList SearchConfiguration(string strXMLDOC, string strLocalSAPName)
        {
            XmlNodeList oNodeList = null;
            XmlDocument oXMLDOC = null;
            string strXPath = null;
            try
            {
                oXMLDOC = new XmlDocument();
                oXMLDOC.PreserveWhitespace = false;
                oXMLDOC.LoadXml(strXMLDOC);
                strXPath = String.Format("//*[@NAME = '{0}']", strLocalSAPName);
                oNodeList = oXMLDOC.SelectNodes(strXPath);
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return oNodeList;
        }
        //保存配置文件
        public bool SaveConfigFile(string strXMLFileName, string strXML)
        {
            XmlDocument oXMLDoc = null;
            bool bResult = false;

            try
            {
                if (String.IsNullOrEmpty(strXML))
                    return bResult;

                oXMLDoc = new XmlDocument();
                oXMLDoc.PreserveWhitespace = false;

                oXMLDoc.LoadXml(strXML);

                oXMLDoc.Save(strXMLFileName);

                bResult = true;
            }
            catch (System.IO.FileNotFoundException)
            {
                bResult = false;
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return bResult;
        }
        //添加配置信息
        public bool AddConfiguration(ref string strXML)
        {
            XmlDocument oXMLDoc = null;
            XmlElement oRootNode = null;
            XmlElement oSubNode = null;
            XmlAttribute oAttr = null;
            XmlNodeList oNodeList = null;
            bool bRetVal = false;

            try
            {
                oXMLDoc = new XmlDocument();
                oXMLDoc.PreserveWhitespace = false;

                if (String.IsNullOrEmpty(strXML))
                {
                    oRootNode = (XmlElement)oXMLDoc.CreateNode(XmlNodeType.Element, "", "SAPServer", "");
                }
                else
                {
                    oXMLDoc.LoadXml(strXML);
                    oNodeList = SearchConfiguration(strXML, this.Name);
                    if (oNodeList.Count >= 1)
                        return bRetVal;

                    oRootNode = (XmlElement)oXMLDoc.FirstChild;
                }

                oSubNode = (XmlElement)oXMLDoc.CreateNode(XmlNodeType.Element, "", "ServerInfo", "");

                oAttr = oXMLDoc.CreateAttribute("NAME");
                oAttr.Value = Name;
                oSubNode.SetAttributeNode(oAttr);

                oAttr = oXMLDoc.CreateAttribute("ASHOST");
                oAttr.Value = ASHOST;
                oSubNode.SetAttributeNode(oAttr);

                oAttr = oXMLDoc.CreateAttribute("SYSNR");
                oAttr.Value =SYSNR;
                oSubNode.SetAttributeNode(oAttr);

                oAttr = oXMLDoc.CreateAttribute("CLIENT");
                oAttr.Value = CLIENT;
                oSubNode.SetAttributeNode(oAttr);

                oAttr = oXMLDoc.CreateAttribute("LANGUAGE");
                oAttr.Value = LANGUAGE;
                oSubNode.SetAttributeNode(oAttr);

                oRootNode.AppendChild(oSubNode);
                oXMLDoc.AppendChild(oRootNode);

                strXML = oXMLDoc.InnerXml;

                bRetVal = true;
            }
            catch (System.IO.FileNotFoundException)
            {
                bRetVal = false;
            }
            catch (Exception exp)
            {
                throw exp;
            }

            return bRetVal;
        }
        //删除配置信息
        public bool RemoveConfiguration(ref string strXML)
        {
            XmlDocument oXMLDoc = null;
            XmlNode oNode = null;
            string strXPath = null;
            bool bRetVal = false;

            try
            {
                oXMLDoc = new XmlDocument();
                oXMLDoc.PreserveWhitespace = false;

                oXMLDoc.LoadXml(strXML);
                strXPath = String.Format("//*[@NAME='{0}']", this.Name);
                oNode = oXMLDoc.SelectSingleNode(strXPath);
                if (oNode != null)
                {
                    oXMLDoc.DocumentElement.RemoveChild(oNode);
                    strXML = oXMLDoc.InnerXml;
                    bRetVal = true;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return bRetVal;
        }
        #region Internal Class
        /// <summary>
        /// PropertyGrid 
        /// </summary>
        internal class SAPSeverInfo
        {
            public SAPServerConfig serverConfig;

            public SAPSeverInfo(SAPServerConfig serverConfig)
            {
                this.serverConfig = serverConfig;
            }

            [Category("SAP Sever 连接(本地)")]
            [Description("SAP Sever 本地连接配置别名")]
            public string NAME
            {
                get
                {
                    return serverConfig.Name;
                }
                set
                {
                    serverConfig.Name = value;
                }
            }

            [Category("SAP Server 连接信息")]
            [Description("Host name or IP address of a specific application server (R/3, No Load Balancing)")]
            public string ASHOST
            {
                get
                {
                    return serverConfig.ASHOST;
                }
                set
                {
                    serverConfig.ASHOST = value;
                }
            }

            [Category("SAP Server 连接信息")]
            [Description("R/3 system number (R/3, No Load Balancing)")]
            public string SYSNR
            {
                get
                {
                    return serverConfig.SYSNR;
                }
                set
                {
                    serverConfig.SYSNR = value;
                }
            }

            [Category("SAP Server 连接信息")]
            [Description("SAP logon client number")]
            public string CLIENT
            {
                get
                {
                    return serverConfig.CLIENT;
                }
                set
                {
                    serverConfig.CLIENT = value;
                }
            }

            [Category("SAP Server 连接信息")]
            [Description("SAP logon language (EN-英文，ZH-中文)")]
            public string LANGUAGE
            {
                get
                {
                    return serverConfig.LANGUAGE;
                }
                set
                {
                    serverConfig.LANGUAGE = value;
                }
            }

        } // class

        #endregion
    }
}
