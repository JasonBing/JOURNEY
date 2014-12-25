using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DBManager
    {
        #region 属性 构造函数
        private string _sessionId;
        private DataHandler _Handler;
        protected DatabaseType DBType
        {
            get
            {
                return this._Handler.DatabaseType;
            }
        }
        public DBManager(DataHandler handler, string sessionid)
        {
            this._Handler = handler;
            this._sessionId = sessionid;
            if (handler == null)
            {
                this._Handler = DBManager.CreateHandler();
            }

        }
        public DBManager(DataBaseConnector cntor, string sessionid)
        {
            this._Handler = cntor.CreateDataHandler();
            this._sessionId = sessionid;
        }
        public DBManager(string sessionid)
            : this(SystemSettings.DataConnector, sessionid)
        {

        }
        #endregion
        #region 查询
        public virtual bool GetEntity(EntityBase entity)
        {
            object obj = null;
            try
            {
                SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGetEntity(entity, this.DBType);

                obj = this._Handler.GetDataReader(selectSqlObject.ToSql(), selectSqlObject.Params);
                bool result = false;
                if (obj == null)
                {
                    result = false;
                    return result;
                }
                else
                {
                    Type objType = obj.GetType();
                    //执行Read读取
                    bool flag = (bool)objType.InvokeMember("Read", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
                    if (flag == false)
                    {
                        result = false;
                        return result;
                    }
                    else
                    {
                        if (entity.FieldKeys.Count <= 0)
                        {
                            result = false;
                            return result;
                        }
                        else
                        {
                            foreach (string item in entity.FieldKeys)
                            {
                                //获取this[string]索引器--get_Item
                                MethodInfo info = objType.GetMethod("get_Item", new Type[] { typeof(string) });
                                entity[item] = info.Invoke(obj, new object[] { item });
                            }
                        }
                    }
                }
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual bool GetEntityEx(EntityBase entity, string OrderSort)
        {
            object obj = null;
            bool result = false;
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGetEntityEx(entity, OrderSort, this.DBType);
            obj = this._Handler.GetDataReader(selectSqlObject.ToSql(), selectSqlObject.Params);

            if (obj == null)
            {
                result = false;
                return result;
            }
            else
            {
                Type objType = obj.GetType();
                bool flag = (bool)objType.InvokeMember("Read", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
                if (flag == false)
                {
                    result = false;
                    return result;
                }
                else
                {
                    if (entity.FieldKeys.Count <= 0)
                    {
                        result = false;
                        return result;
                    }
                    else
                    {
                        foreach (string item in entity.FieldKeys)
                        {
                            //获取this[string]索引器--get_Item
                            MethodInfo info = objType.GetMethod("get_Item", new Type[] { typeof(string) });
                            entity[item] = info.Invoke(obj, new object[] { item });
                        }
                    }
                }
            }
            return result;
        }
        public virtual bool GetEntityEx(EntityBase entity, IWhereSQLObject whereSqlObj, string OrderSort)
        {
            bool result = false;
            object obj = null;
            try
            {
                SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGetEntityEx(entity, whereSqlObj, OrderSort, this.DBType);
                obj = this._Handler.GetDataReader(selectSqlObject.ToSql(), selectSqlObject.Params);
                if (obj == null)
                {
                    result = false;
                    return result;
                }
                else
                {
                    Type objType = obj.GetType();
                    objType.InvokeMember("Read", BindingFlags.Default | BindingFlags.InvokeMethod, null, obj, new object[] { });
                    if (objType == null)
                    {
                        result = false;
                        return result;
                    }
                    else
                    {
                        if (entity.FieldKeys.Count <= 0)
                        {
                            result = false;
                            return result;
                        }
                        else
                        {
                            foreach (string item in entity.FieldKeys)
                            {
                                //获取this[string]索引器--get_Item
                                MethodInfo info = objType.GetMethod("get_Item", new Type[] { typeof(string) });
                                entity[item] = info.Invoke(obj, new object[] { item });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        public virtual int Count(EntityBase entity)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromCount(entity, this.DBType);
            object objectValue = this._Handler.ExecuteScalar(selectSqlObject.ToSql(), selectSqlObject.Params);
            return Convert.ToInt32(objectValue);
        }
        public virtual int CountEx(EntityBase entity, IWhereSQLObject whereSqlObj)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromCountEx(entity, whereSqlObj, this.DBType);
            object objectValue = this._Handler.ExecuteScalar(selectSqlObject.ToSql(), selectSqlObject.Params);
            return Convert.ToInt32(objectValue);
        }
        public virtual DataTable DataTable(EntityBase entity, ICollection Fields, bool distinct = false)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromDataTable(entity, Fields, this.DBType, distinct);
            DataSet dataSet = this._Handler.GetDataSet(selectSqlObject.ToSql(), selectSqlObject.Params);
            dataSet.DataSetName = "ds_" + entity.EntitySource;
            DataTable dataTable = dataSet.Tables[0];
            dataTable.TableName = entity.EntitySource;
            return dataTable;
        }
        public virtual DataTable DataTableEx(EntityBase entity, ICollection Fields, int MaxRecord, string SortOrder, bool distinct = false)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromDataTableEx(entity, Fields, MaxRecord, SortOrder, distinct, this.DBType);
            DataSet dataSet = this._Handler.GetDataSet(selectSqlObject.ToSql(), selectSqlObject.Params);
            dataSet.DataSetName = "ds_" + entity.EntitySource;
            DataTable dataTable = dataSet.Tables[0];
            dataTable.TableName = entity.EntitySource;
            return dataTable;
        }
        public virtual DataTable DataTableEx(EntityBase entity, ICollection Fields, IWhereSQLObject whereSqlObj, int MaxRecord, string SortOrder, bool distinct = false)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromDataTableEx(entity, Fields, whereSqlObj, MaxRecord, SortOrder, distinct, this.DBType);
            DataSet dataSet = this._Handler.GetDataSet(selectSqlObject.ToSql(), selectSqlObject.Params);
            dataSet.DataSetName = "ds_" + entity.EntitySource;
            DataTable dataTable = dataSet.Tables[0];
            dataTable.TableName = entity.EntitySource;
            return dataTable;
        }
        public virtual DataTable DataTableEx1(EntityBase entity, ICollection Fields, int BeginRecord, int EndRecord, int MaxRecord, string SortOrder, bool distinct = false)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromDataTableEx(entity, Fields, BeginRecord, EndRecord, MaxRecord, SortOrder, distinct, this.DBType);
            DataSet dataSet = this._Handler.GetDataSet(selectSqlObject.ToSql(), selectSqlObject.Params);
            dataSet.DataSetName = "ds_" + entity.EntitySource;
            DataTable dataTable = dataSet.Tables[0];
            dataTable.TableName = entity.EntitySource;
            return dataTable;
        }
        public virtual DataTable DataTableEx1(EntityBase entity, ICollection Fields, IWhereSQLObject whereSqlObj, int BeginRecord, int EndRecord, int MaxRecord, string SortOrder, bool distinct = false)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromDataTableEx(entity, Fields, whereSqlObj, BeginRecord, EndRecord, MaxRecord, SortOrder, distinct, this.DBType);
            DataSet dataSet = this._Handler.GetDataSet(selectSqlObject.ToSql(), selectSqlObject.Params);
            dataSet.DataSetName = "ds_" + entity.EntitySource;
            DataTable dataTable = dataSet.Tables[0];
            dataTable.TableName = entity.EntitySource;
            return dataTable;
        }
        public DataTable DataTableEx(EntityBase entity, ICollection fields, IWhereSQLObject whereObject, int pageIndex, int pageSize, int MaxRecord, string SortOrder, bool distinct = false)
        {
            checked
            {
                DataTable result;

                DataTable dataTable;
                if (pageSize <= 0)
                {
                    if (whereObject == null)
                    {
                        dataTable = this.DataTableEx(entity, fields, MaxRecord, SortOrder, distinct);
                    }
                    else
                    {
                        dataTable = this.DataTableEx(entity, fields, whereObject, MaxRecord, SortOrder, distinct);
                    }
                }
                else
                {
                    if (pageIndex == 0)
                    {
                        if (whereObject == null)
                        {
                            dataTable = this.DataTableEx(entity, fields, pageSize, SortOrder, distinct);
                        }
                        else
                        {
                            dataTable = this.DataTableEx(entity, fields, whereObject, pageSize, SortOrder, distinct);
                        }
                    }
                    else
                    {
                        int num = (pageIndex + 1) * pageSize;
                        if (MaxRecord > 0)
                        {
                            num = Math.Min(num, MaxRecord);
                        }
                        if (whereObject == null)
                        {
                            dataTable = this.DataTableEx1(entity, fields, pageIndex * pageSize + 1, num, MaxRecord, SortOrder, distinct);
                        }
                        else
                        {
                            dataTable = this.DataTableEx1(entity, fields, whereObject, pageIndex * pageSize + 1, num, MaxRecord, SortOrder, distinct);
                        }
                    }
                }
                result = dataTable;

                return result;
            }
        }
        public virtual DataTable Group(EntityBase entity, TotalObjectList ListTotal)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGroup(entity, ListTotal, this.DBType);
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public virtual DataTable GroupEx(EntityBase entity, TotalObjectList ListTotal, int MaxRecord, string OrderSort)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGroupEx(entity, ListTotal, MaxRecord, OrderSort, Convert.ToString((int)this.DBType));
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public virtual DataTable GroupEx(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int MaxRecord, string OrderSort)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGroupEx(entity, ListTotal, whereSqlObj, MaxRecord, OrderSort, this.DBType);
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public virtual DataTable Group(SelectSqlObject sqlObj, TotalObjectList ListTotal)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGroup(sqlObj, ListTotal, this.DBType);
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public virtual DataTable GroupEx(SelectSqlObject sqlObj, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int MaxRecord, string OrderSort)
        {
            SelectSqlObject selectSqlObject = SelectSqlObject.SqlObjectFromGroupEx(sqlObj, ListTotal, whereSqlObj, MaxRecord, OrderSort, this.DBType);
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public virtual DataTable GroupEx1(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int BeginRecord, int EndRecord, int MaxRecord, string SortOrder)
        {
            SelectSqlObject selectSqlObject = (SelectSqlObject)SelectSqlObject.SqlObjectFromGroupEx(entity, ListTotal, whereSqlObj, BeginRecord, EndRecord, MaxRecord, SortOrder, this.DBType);
            DataTable dataTable = this._Handler.GetDataTable(selectSqlObject.ToSql(), selectSqlObject.Params);
            return dataTable;
        }
        public DataTable GroupEx(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereObject, int pageIndex, int pageSize, int MaxRecord, string SortOrder)
        {
            checked
            {
                DataTable dataTable;
                if (pageSize <= 0)
                {
                    if (whereObject == null)
                    {
                        dataTable = this.GroupEx(entity, ListTotal, MaxRecord, SortOrder);
                    }
                    else
                    {
                        dataTable = this.GroupEx(entity, ListTotal, whereObject, MaxRecord, SortOrder);
                    }
                }
                else
                {
                    if (pageIndex == 0)
                    {
                        if (whereObject == null)
                        {
                            dataTable = this.GroupEx(entity, ListTotal, pageSize, SortOrder);
                        }
                        else
                        {
                            dataTable = this.GroupEx(entity, ListTotal, whereObject, pageSize, SortOrder);
                        }
                    }
                    else
                    {
                        int num = (pageIndex + 1) * pageSize;
                        if (MaxRecord > 0)
                        {
                            num = Math.Min(num, MaxRecord);
                        }
                        if (whereObject == null)
                        {
                            dataTable = this.GroupEx1(entity, ListTotal, null, pageIndex * pageSize + 1, num, MaxRecord, SortOrder);
                        }
                        else
                        {
                            dataTable = this.GroupEx1(entity, ListTotal, whereObject, pageIndex * pageSize + 1, num, MaxRecord, SortOrder);
                        }
                    }
                }
                return dataTable;
            }
        }
        #endregion
        #region 新增
        public virtual void AddNewEntity(EntityBase entity)
        {
            List<string>.Enumerator enumerator = entity.AutoIncrementKeys.GetEnumerator();
            //处理自增列
            while (enumerator.MoveNext())
            {
                entity[enumerator.Current] = null;
            }
            //获取插入SQL
            InsertSqlObject insertSqlObject = InsertSqlObject.SqlObjectFromAddNewEntity(entity, this.DBType);
            if (entity.AutoIncrementKeys.Count > 0 & this.DBType == DatabaseType.SQLSERVER)
            {
                EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
                EnumBeginTransactionState beginTransactionState = EnumBeginTransactionState.NoNeedCommit;
                DataHandler hander = this._Handler;
                try
                {
                    hander.Open(ref openConnectionState);
                    hander.BeginTransaction(ref beginTransactionState);
                    int num = this._Handler.ExecuteNonQuery(insertSqlObject.ToSql(), insertSqlObject.Params);
                    if (num <= 0)
                    {
                        throw new Exception("Insert the data failure！");
                    }
                    //SQLSERVER中一个表只能最多有一个自增列
                    object indentityValue = this._Handler.ExecuteScalar("SELECT @@IDENTITY AS 'Identity'");
                    if (indentityValue == DBNull.Value)
                    {
                        throw new Exception("Failed to get the Indetity!");
                    }
                    this._Handler.CommitTransaction(beginTransactionState);
                    int num2 = entity.AutoIncrementKeys.Count - 1;
                    for (int i = 0; i <= num2; i++)
                    {
                        entity[entity.AutoIncrementKeys[i]] = Convert.ToInt32(indentityValue);
                    }
                }
                catch (Exception ex)
                {
                    hander.RollBackTransaction(beginTransactionState);
                    throw ex;
                }
                finally
                {
                    hander.Close(openConnectionState);
                }
            }
            else if (entity.AutoIncrementKeys.Count > 0 & this.DBType == DatabaseType.SQLSERVER)
            {
                EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
                EnumBeginTransactionState beginTransactionState = EnumBeginTransactionState.NoNeedCommit;
                try
                {
                    DataHandler handler = this._Handler;
                    handler.Open(ref openConnectionState);
                    handler.BeginTransaction(ref beginTransactionState);
                    int num3 = this._Handler.ExecuteNonQuery(insertSqlObject.ToSql(), insertSqlObject.Params);
                    if (num3 <= 0)
                    {
                        throw new Exception("Insert the data failure！");
                    }
                    int num4 = entity.AutoIncrementKeys.Count - 1;
                    for (int j = 0; j <= num4; j++)
                    {
                        string text = entity.AutoIncrementKeys[j];
                        object objectValue = handler.ExecuteScalar(string.Concat(new string[]
								{
									"select ",
									(entity.EntitySource + "_" + text).ToUpper(),
									".currval from \"",
									entity.EntitySource,
									"\""
								}));
                        if (objectValue == DBNull.Value)
                        {
                            throw new Exception("Failed to get the Indetity!");
                        }
                        entity[text] = Convert.ToInt32(objectValue);
                    }
                    this._Handler.CommitTransaction(beginTransactionState);
                    return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    this._Handler.Close(openConnectionState);
                }
            }
            else
            {
                int num5 = this._Handler.ExecuteNonQuery(insertSqlObject.ToSql(), insertSqlObject.Params);
                if (num5 <= 0)
                {
                    throw new Exception("Insert the data failure！");
                }
            }
        }
        /// <summary>
        /// 将一个表的指定列复制到另外一个表中(目的表和源表复制列的列名必须一致)
        /// </summary>
        /// <param name="entity">表实体</param>
        /// <param name="collection">字段集合</param>
        /// <param name="SqlObject">SelectSqlObject  查询SQL</param>
        /// <returns></returns>
        public virtual int AddNewEntityEx(EntityBase entity, ICollection collection, SelectSqlObject SqlObject)
        {
            int result = 0;
            InsertSqlObject insertSqlObject = InsertSqlObject.SqlObjectFromAddNewEntity(entity, collection, SqlObject, this.DBType);
            result = this._Handler.ExecuteNonQuery(insertSqlObject.ToSql(), insertSqlObject.Params);
            return result;
        }
        #endregion
        #region 更新
        public virtual int UpdateEntity(EntityBase entity)
        {
            UpdateSqlObject updateSqlObject =UpdateSqlObject.SqlObjectFromUpdateEntity(entity,this.DBType);
            return this._Handler.ExecuteNonQuery(updateSqlObject.ToSql(), updateSqlObject.Params);
        }
        public virtual int UpdateEntityEx(EntityBase entity,IWhereSQLObject where)
        {
            UpdateSqlObject updateSqlObject = UpdateSqlObject.SqlObjectFromUpdateEntityEx(entity, where, this.DBType);
            return this._Handler.ExecuteNonQuery(updateSqlObject.ToSql(), updateSqlObject.Params);
        }

        #endregion
        #region 删除
        public virtual int DeleteEntity(EntityBase entity)
        {
            DeleteSqlObject deleteSqlObject = DeleteSqlObject.SqlObjectFromDeleteEntity(entity, this.DBType);
            return this._Handler.ExecuteNonQuery(deleteSqlObject.ToSql(), deleteSqlObject.Params);
        }
        public virtual int DeleteEntityEx(EntityBase entity, IWhereSQLObject where)
        {
            DeleteSqlObject deleteSqlObject = DeleteSqlObject.SqlObjectFromDeleteEntityEx(entity, where, this.DBType);
            return this._Handler.ExecuteNonQuery(deleteSqlObject.ToSql(), deleteSqlObject.Params);
        }
        #endregion
        /// <summary>
        /// 创建Handler
        /// </summary>
        /// <returns></returns>
        public static DataHandler CreateHandler()
        {
            return SystemSettings.DataConnector.CreateDataHandler();
        }


    }
}
