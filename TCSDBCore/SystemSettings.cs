using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    ///DBCore操作参数设置
    /// </summary>
   public static class SystemSettings
    {
        private static DataBaseConnector dataConnector;
       /// <summary>
       /// 获取和设置数据库连接对象
       /// </summary>
        public static DataBaseConnector DataConnector
        {
            get { return SystemSettings.dataConnector; }
            set { SystemSettings.dataConnector = value; }
        }
        private static string connectionString;
       /// <summary>
       /// 获取数据库连接字符串
       /// </summary>
        public static string ConnectionString
        {
            get { return SystemSettings.connectionString; }
        }
        private static int commandTimeout;
       /// <summary>
       /// 获取和设置执行命令的时间
       /// </summary>
        public static int CommandTimeout
        {
            get { return SystemSettings.commandTimeout; }
            set { SystemSettings.commandTimeout = value; }
        }
        private static bool isManagerLog;
       /// <summary>
       /// 是否进行日志管理
       /// </summary>
        public static bool IsManagerLog
        {
            get { return SystemSettings.IsManagerLog; }
            set { SystemSettings.IsManagerLog = value; }
        }
        private static string managerLogPath;
       /// <summary>
       /// 设置和获取日志文件存储地址
       /// </summary>
        public static string ManagerLogPath
        {
            get { return SystemSettings.managerLogPath; }
            set { SystemSettings.managerLogPath = value; }
        }
        private static int transactionTimeOut;
        /// <summary>
        ///  获取和设置事务的超时时间
        /// </summary>
        public static int TransactionTimeOut
        {
            get { return SystemSettings.transactionTimeOut; }
            set { SystemSettings.transactionTimeOut = value; }
        }
        static SystemSettings()
        {
            SystemSettings.commandTimeout = 30;
            SystemSettings.isManagerLog = false;
            //默认是当前程序的地址
            SystemSettings.managerLogPath = AppDomain.CurrentDomain.BaseDirectory;
            SystemSettings.TransactionTimeOut = 0;
        }
    }
}
