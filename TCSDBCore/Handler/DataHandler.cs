using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace TCSDBCore
{
    public abstract class DataHandler : IDisposable
    {
        public abstract void Dispose();
        #region 属性
        private static bool debugStatus = true;
        public abstract ConnectionState State
        {
            get;
        }
        private bool _inTransaction;
        /// <summary>
        /// 获取是否启用事务
        /// </summary>
        protected bool InTransaction
        {
            get { return _inTransaction; }
            set { _inTransaction = value; }
        }
        /// <summary>
        /// 设置和获取是否为调试模式
        /// </summary>
        public static bool DebugStatus
        {
            get { return DataHandler.debugStatus; }
            set { DataHandler.debugStatus = value; }
        }
        #region "暂时未启用"
        //internal abstract string lastExpress;
        ///// <summary>
        ///// 获取最后执行的SQL语句
        ///// </summary>
        //internal string LastExpress
        //{
        //    get { return lastExpress; }
        //}
        //public override ConnectionState State
        //{
        //    get;
        //}
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public abstract string ConnectionString
        {
            get;
            set;
        }
        public abstract DatabaseType DatabaseType
        {
            get;
        }
        #endregion
        public abstract string ParameterIndicator
        {
            get;
        }
        /// <summary>
        /// 连接状态
        /// </summary>
       

        private static string _sharedLastSql;

        protected static string SharedLastSql
        {
            get { return DataHandler._sharedLastSql; }
            set { DataHandler._sharedLastSql = value; }
        }
        private static ListParameter _sharedLastParams;

        protected static ListParameter SharedLastParams
        {
            get { return DataHandler._sharedLastParams; }
            set { DataHandler._sharedLastParams = value; }
        }
        #endregion
        #region  构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataHandler()
            : this(SystemSettings.ConnectionString)
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public DataHandler(string connectionString)
        {
            this.ConnectionString = connectionString;
            this._inTransaction = false;
        }
        #endregion
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="openConnectionState"></param>
        public abstract void Open(ref EnumOpenConnectState openConnectionState);
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="openConnectionState"></param>
        /// <param name="connectionString"></param>
        public abstract void Open(ref EnumOpenConnectState openConnectionState, string connectionString);
        /// <summary>
        /// 关闭数据库
        /// </summary>
        /// <param name="openConnectionState"></param>
        public abstract void Close(EnumOpenConnectState openConnectionState);
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public abstract void BeginTransaction(ref EnumBeginTransactionState beginTransactionState);
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public abstract void CommitTransaction(EnumBeginTransactionState beginTransactionState);
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public abstract void RollBackTransaction(EnumBeginTransactionState beginTransactionState);
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract object ExecuteStoredProcedure(string spname, ListParameter param);
        /// <summary>
        /// 根据存储过程获取数据
        /// </summary>
        /// <param name="spname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract DataSet GetStoredProcedureDataSet(string spname, ListParameter param);
        /// <summary>
        /// 用NonQuery模式执行SQL(无参数)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract int ExecuteNonQuery(string sql);
        /// <summary>
        /// 用Read模式执行SQL(无参数)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract object GetDataReader(string sql);
        /// <summary>
        /// 用Scalar模式执行SQL(无参数)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public abstract object ExecuteScalar(string sql);
        /// <summary>
        /// 读取返回DataSet(无参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataSet GetDataSet(string sql);
        /// <summary>
        /// 读取返回DataView(无参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataView GetDataView(string sql);
        /// <summary>
        /// 读取返回DataTable(无参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataTable GetDataTable(string sql);
        /// <summary>
        /// 用NonQuery模式执行SQL(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract int ExecuteNonQuery(string sql, ListParameter param);
        /// <summary>
        /// 用Reader模式执行SQL(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract object GetDataReader(string sql, ListParameter param);
        /// <summary>
        /// 用Scalar模式执行SQL(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract object ExecuteScalar(string sql, ListParameter param);
        /// <summary>
        ///读取返回DataSet(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataSet GetDataSet(string sql, ListParameter param);
        /// <summary>
        ///读取返回DataView(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataView GetDataView(string sql, ListParameter param);
        /// <summary>
        ///读取返回DataTable(有参数)
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public abstract DataTable GetDataTable(string sql, ListParameter param);
        /// <summary>
        /// 获取参数标识
        /// </summary>
        /// <param name="DBType"></param>
        /// <returns></returns>
        internal static string GetParamIdentify(DatabaseType DBType)
        {
            string result = string.Empty;
            switch (DBType)
            {
                case DatabaseType.SQLSERVER:
                    result = "@";
                    break;
                case DatabaseType.ORACLE:
                    result = ":p";
                    break;
                case DatabaseType.MYSQL:
                    result = "?";
                    break;
                case DatabaseType.SQLITE:
                    break;
                default:
                    result = "?";
                    break;
            }
            return result;
        }
        internal static string GetParamIdentify(string field, DatabaseType DBType)
        {
            string result = string.Empty;
            switch (DBType)
            {
                case DatabaseType.SQLSERVER:
                    result = "@" + field;
                    break;
                case DatabaseType.ORACLE:
                    result = ":p" + field;
                    break;
                case DatabaseType.MYSQL:
                    result = "?";
                    break;
                case DatabaseType.SQLITE:
                    break;
                default:
                    result = "?";
                    break;
            }
            return result;
        }
    }
}
