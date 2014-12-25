using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// SQLSERVER 数据库操作对象
    /// </summary>
    public class SQLClientDataHandler : DataHandler
    {
        #region 属性
        /// <summary>
        /// SQLSERVER数据库连接对象
        /// </summary>
        protected SqlConnection _Connection;
        /// <summary>
        /// SQLSERVER 事务对象
        /// </summary>
        protected SqlTransaction _Transaction;
        /// <summary>
        /// 获取参数标识
        /// </summary>
        public override string ParameterIndicator
        {
            get
            {
                return Indicator.Get(this.DatabaseType);
            }
        }
        /// <summary>
        /// 获取和设置连接字符串
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                if (this._Connection == null)
                {
                    return "";
                }
                return this._Connection.ConnectionString;
            }
            set
            {
                if (this._Connection != null)
                {
                    this._Connection.ConnectionString = value;
                }
            }
        }
        /// <summary>
        ///获取数据库类型
        /// </summary>
        public override DatabaseType DatabaseType
        {
            get
            {
                return DatabaseType.SQLSERVER;
            }
        }
        /// <summary>
        /// 获取连接状态
        /// </summary>
        public override ConnectionState State
        {
            get
            {
                return this._Connection.State;
            }
        }
        public string _SharedLastSql = "";
        public string _SharedLastParams = "";
        #endregion
        #region 构造函数
        public SQLClientDataHandler()
            : this(SystemSettings.ConnectionString)
        {
        }
        public SQLClientDataHandler(string connectionString)
        {
            this._Connection = new SqlConnection();
            this.ConnectionString = connectionString;
        }
        #endregion
        #region 连接操作 事务操作
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="openConnectionState"></param>
        public override void Open(ref EnumOpenConnectState openConnectionState)
        {
            this.Open(ref openConnectionState, "");
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="openConnectionState"></param>
        /// <param name="connectionString"></param>
        public override void Open(ref EnumOpenConnectState openConnectionState, string connectionString)
        {
            string text = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            openConnectionState = EnumOpenConnectState.NeedClose;

            if (this._Connection.State != ConnectionState.Closed)
            {
                openConnectionState = EnumOpenConnectState.NoNeedClose;
            }
            else
            {
                if (connectionString.Trim().Length > 0)
                {
                    this._Connection.ConnectionString = connectionString;
                }
                this._Connection.Open();
            }



        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="openConnectionState"></param>
        public override void Close(EnumOpenConnectState openConnectionState)
        {
            string text = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;

            if (openConnectionState != EnumOpenConnectState.NoNeedClose)
            {
                if (this.State != ConnectionState.Closed)
                {
                    this.RollBackTransaction(EnumBeginTransactionState.NeedCommit);
                    this._Transaction = null;
                    this._Connection.Close();
                }
            }

        }
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public override void BeginTransaction(ref EnumBeginTransactionState beginTransactionState)
        {
            string text = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            beginTransactionState = EnumBeginTransactionState.NeedCommit;

            if (this.InTransaction & this._Transaction != null)
            {
                beginTransactionState = EnumBeginTransactionState.NoNeedCommit;
            }
            else
            {
                if (SystemSettings.TransactionTimeOut > 0)
                {
                    //设置等待事务锁的时间
                    this.ExecuteNonQuery("SET LOCK_TIMEOUT " + (SystemSettings.TransactionTimeOut * 1000).ToString());
                }
                this._Transaction = this._Connection.BeginTransaction();
                this.InTransaction = true;
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public override void CommitTransaction(EnumBeginTransactionState beginTransactionState)
        {
            string text = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            try
            {
                if (this._Transaction != null & beginTransactionState == EnumBeginTransactionState.NeedCommit)
                {
                    if (this._Transaction.Connection != null && this._Transaction.Connection.State != ConnectionState.Closed)
                    {
                        this._Transaction.Commit();
                    }
                    this._Transaction = null;
                    this.InTransaction = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="beginTransactionState"></param>
        public override void RollBackTransaction(EnumBeginTransactionState beginTransactionState)
        {
            string text = this.GetType().ToString() + "." + MethodBase.GetCurrentMethod().Name;
            try
            {
                if (this._Transaction != null & beginTransactionState == EnumBeginTransactionState.NeedCommit)
                {
                    if (this._Transaction.Connection != null && this._Transaction.Connection.State != ConnectionState.Closed)
                    {
                        this._Transaction.Rollback();
                    }
                    this._Transaction = null;
                    this.InTransaction = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 操作数据库
        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commType"></param>
        /// <returns></returns>
        private SqlCommand CreateCommand(string sql, ListParameter param, CommandType commType)
        {
            SqlCommand sqlCommand = new SqlCommand();
            if (DataHandler.DebugStatus)
            {
                DataHandler.SharedLastSql = "";
                DataHandler.SharedLastParams = null;
            }
            sqlCommand.Connection = this._Connection;
            sqlCommand.CommandTimeout = SystemSettings.CommandTimeout;
            if (this.InTransaction & this._Transaction != null)
            {
                sqlCommand.Transaction = this._Transaction;
            }
            sqlCommand.CommandText = sql;
            sqlCommand.CommandType = commType;
            sqlCommand.Parameters.Clear();
            if (param != null)
            {
                IEnumerator enumerator = param.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    ObjectParameter objectParameter = (ObjectParameter)enumerator.Current;
                    SqlParameter sqlParameter = sqlCommand.Parameters.AddWithValue(this.ParameterIndicator + objectParameter.paramKey, objectParameter.paramValue);
                    sqlParameter.Direction = objectParameter.paramDirection;
                    if (objectParameter.paramLength > 0)
                    {
                        sqlParameter.Size = objectParameter.paramLength;
                    }
                    if (objectParameter.paramValue != null && objectParameter.paramValue.ToString().Length > 5000)
                    {
                        sqlParameter.SqlDbType = SqlDbType.NText;
                    }
                }
            }
            return sqlCommand;
        }
        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private SqlCommand CreateCommand(string sql, ListParameter param)
        {
            return this.CreateCommand(sql, param, CommandType.Text);
        }
        /// <summary>
        /// 创建SqlCommand对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private SqlCommand CreateCommand(string sql)
        {
            return this.CreateCommand(sql, null);
        }
        public override object ExecuteStoredProcedure(string spname, ListParameter param)
        {
            EnumOpenConnectState openConnectState = EnumOpenConnectState.NoNeedClose;
            try
            {
                this.Open(ref openConnectState);
                SqlCommand sqlCommand = this.CreateCommand(spname, param);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter sqlParameter = new SqlParameter("@Return", SqlDbType.BigInt);
                sqlParameter.Direction = ParameterDirection.ReturnValue;
                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.ExecuteNonQuery();
                IEnumerator enumerator = sqlCommand.Parameters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    SqlParameter sqlParameter2 = (SqlParameter)enumerator.Current;
                    if (sqlParameter2.Direction == ParameterDirection.InputOutput | sqlParameter2.Direction == ParameterDirection.Output)
                    {
                        string key = sqlParameter2.ParameterName.Substring(1, sqlParameter2.ParameterName.Length - 1);
                        if (param.Contains(key))
                        {
                            param[key].paramValue = RuntimeHelpers.GetObjectValue(sqlParameter2.Value);
                        }
                    }
                }
                return sqlParameter.Value;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.Close(openConnectState);
            }
        }

        public override DataSet GetStoredProcedureDataSet(string spname, ListParameter param)
        {
            DataSet dataSet = new DataSet();
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            this.Open(ref openConnectionState);
            SqlCommand sqlCommand = this.CreateCommand(spname, param, CommandType.StoredProcedure);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter = new SqlParameter("@Return", SqlDbType.BigInt);
            sqlParameter.Direction = ParameterDirection.ReturnValue;
            sqlCommand.Parameters.Add(sqlParameter);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataSet);
            try
            {
                IEnumerator enumerator = sqlCommand.Parameters.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    SqlParameter sqlParameter2 = (SqlParameter)enumerator.Current;
                    if (sqlParameter2.Direction == ParameterDirection.InputOutput | sqlParameter2.Direction == ParameterDirection.Output)
                    {
                        string key = sqlParameter2.ParameterName.Substring(1, sqlParameter2.ParameterName.Length - 1);
                        if (param.Contains(key))
                        {
                            param[key].paramValue = RuntimeHelpers.GetObjectValue(sqlParameter2.Value);
                        }
                    }
                }
                return dataSet;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.Close(openConnectionState);
            }
        }

        public override int ExecuteNonQuery(string sql, ListParameter param)
        {
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            try
            {

                this.Open(ref openConnectionState);
                SqlCommand sqlCommand = this.CreateCommand(sql, param);
                return sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                this.Close(openConnectionState);
            }

        }

        public override object GetDataReader(string sql, ListParameter param)
        {
            SqlDataReader result = null;
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            try
            {
                this.Open(ref openConnectionState);
                SqlCommand sqlCommand = this.CreateCommand(sql, param);
                if (openConnectionState == EnumOpenConnectState.NoNeedClose)
                {
                    result = sqlCommand.ExecuteReader(CommandBehavior.Default);
                }
                else
                {
                    result = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public override object ExecuteScalar(string sql, ListParameter param)
        {
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            try
            {

                this.Open(ref openConnectionState);
                SqlCommand sqlCommand = this.CreateCommand(sql, param);
                return sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close(openConnectionState);
            }
        }

        public override DataSet GetDataSet(string sql, ListParameter param)
        {
            DataSet dataSet = new DataSet();
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            try
            {
                this.Open(ref openConnectionState);
                SqlCommand selectCommand = this.CreateCommand(sql, param);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close(openConnectionState);
            }
        }
        public override DataTable GetDataTable(string sql, ListParameter param)
        {
            DataSet dataSet = this.GetDataSet(sql, param);
            DataTable result = null;
            if (dataSet == null)
            {
                return result;
            }
            if (dataSet.Tables.Count > 0)
            {
                result = dataSet.Tables[0];
                return result;
            }
            return result;
        }

        public override DataView GetDataView(string sql, ListParameter param)
        {
            DataTable dataTable = this.GetDataTable(sql, param);
            if (dataTable != null)
            {
                return dataTable.DefaultView;
            }
            return null;
        }
        public override int ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(sql, null);
        }

        public override object GetDataReader(string sql)
        {
            return this.GetDataReader(sql, null);
        }

        public override object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, null);
        }

        public override DataSet GetDataSet(string sql)
        {
            return this.GetDataSet(sql, null);
        }

        public override DataTable GetDataTable(string sql)
        {
            return this.GetDataTable(sql, null);
        }

        public override DataView GetDataView(string sql)
        {
            return this.GetDataView(sql, null);
        }

        #endregion
        #region 销毁数据库操作对象
        public override void Dispose()
        {
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NeedClose;
            if (this.State != ConnectionState.Closed)
            {
                this.Close(openConnectionState);
            }
            this._Transaction = null;
            if (this._Connection != null)
            {
                this._Connection.Dispose();
            }
            this._Connection = null;
        }
        #endregion
    }
}
