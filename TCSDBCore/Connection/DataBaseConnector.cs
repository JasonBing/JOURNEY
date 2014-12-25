using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 数据库连接对象基类
    /// </summary>
    public abstract class DataBaseConnector
    {
        private string databaseSource;
        /// <summary>
        /// 获取和设置数据库地址
        /// </summary>
        public string DatabaseSource
        {
            get { return databaseSource; }
            set { databaseSource = value; }
        }
        private string currentDataBase;
        /// <summary>
        /// 获取和设置数据库名字
        /// </summary>
        public string CurrentDataBase
        {
            get { return currentDataBase; }
            set { currentDataBase = value; }
        }
        private string user;
        /// <summary>
        /// 获取和设置用户名
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
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
        private string connectionTimeOut;
        /// <summary>
        /// 获取和设置连接超时时间
        /// </summary>
        public string ConnectionTimeOut
        {
            get { return connectionTimeOut; }
            set { connectionTimeOut = value; }
        }
        /// <summary>
        ///获取连接字符串
        /// </summary>
        public abstract string ConnectionString
        {
            get;
        }
        /// <summary>
        /// 创建数据库操作助手
        /// </summary>
        /// <returns></returns>
        public abstract DataHandler CreateDataHandler();
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据库连接</returns>
        public static DataBaseConnector CreateConnector(DatabaseType dbType)
        {
            switch (dbType)
            {
                case DatabaseType.SQLSERVER:
                    return new SQLServerConnector();
                case DatabaseType.ORACLE:
                    return null;
                case DatabaseType.MYSQL:
                    return null;
                case DatabaseType.SQLITE:
                    return null;
                default:
                    throw new Exception("Does not support this type of database!");
            }
        }
        /// <summary>
        /// 获取表的字段集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract List<sysFieldEntity> GetFields(string name);
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="name"></param>
        /// <param name="column"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public abstract string GetFiledDescription(string name, string column, DataHandler handler);
        /// <summary>
        /// FieldDataType转数据库字段类型字符
        /// </summary>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public abstract string DataTypeToSQLType(FieldDataType datatype);
        /// <summary>
        /// 数据库字段类型字符转FieldDataType
        /// </summary>
        /// <param name="sqltype"></param>
        /// <returns></returns>
        public abstract FieldDataType DataTypeFromSQLType(string sqltype);
    }
}
