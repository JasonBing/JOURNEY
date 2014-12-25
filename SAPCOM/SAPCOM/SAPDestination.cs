using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    /// <summary>
    /// SAP连接信息
    /// </summary>
    public class SAPDestination
    {
        private string sapServerHost;
        /// <summary>
        /// 获取和设置SAP服务器地址
        /// </summary>
        public string SapServerHost
        {
            get { return sapServerHost; }
            set { sapServerHost = value; }
        }
        private string systemNumber;
        /// <summary>
        /// 获取和设置实例编号
        /// </summary>
        public string SystemNumber
        {
            get { return systemNumber; }
            set { systemNumber = value; }
        }
        private string client;
        /// <summary>
        /// 获取和设置客户端编号
        /// </summary>
        public string Client
        {
            get { return client; }
            set { client = value; }
        }
        private string userName;
        /// <summary>
        /// 获取和设置用户名
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string password;
        /// <summary>
        /// 获取和设置密码
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        private string language="ZH";
        /// <summary>
        /// 获取和设置语言
        /// </summary>
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        private bool isUnicodeSystem=false;
        /// <summary>
        /// 获取设置是否为UNICODE编码
        /// </summary>
        public bool IsUnicodeSystem
        {
            get { return isUnicodeSystem; }
            set { isUnicodeSystem = value; }
        }
        /// <summary>
        /// 获取SAP连接字符串
        /// </summary>
        public string ConnectonString
        {
            //ASHOST=10.2.8.41 CLIENT=301 LANG=ZH PASSWD=sq123456 USE_SAPGUI=0 SNC_QOP=8 SYSNR=0 USER=SQJK
            get { return string.Format("ASHOST={0} CLIENT={1} LANG={2} PASSWD={3} USE_SAPGUI={4} SNC_QOP={5} SYSNR={6} USER={7}"); }
        }
    }
}
