using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public class SQLServerConnector : DataBaseConnector
    {
        private bool integratedSecurity = false;
        /// <summary>
        /// 是否启用集成验证
        /// </summary>
        public bool IntegratedSecurity
        {
            get { return integratedSecurity; }
            set { integratedSecurity = value; }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public override string ConnectionString
        {
            get
            {
                if (!(this.CurrentDataBase != null && this.DatabaseSource != null && this.DatabaseSource.Length > 0 && this.CurrentDataBase.Length > 0))
                {
                    return null;
                }
                string result = "";
                //判断是否启用集成验证
                if (this.integratedSecurity == true)
                {
                    result = string.Concat(new string[]
					{
						"Persist Security Info=False;Integrated Security=SSPI;initial catalog=",
						this.CurrentDataBase,
						";multipleactiveresultsets=true;data source=",
						this.DatabaseSource,
						";Connect Timeout=",
						(this.ConnectionTimeOut==null?"30":this.ConnectionTimeOut.ToString()),
						""
					});
                }
                else
                {
                    result = string.Concat(new string[]
					{
						"user id=",
						this.User,
						";password=",
						this.Password,
						";initial catalog=",
						this.CurrentDataBase,
						";multipleactiveresultsets=true;data source=",
						this.DatabaseSource,
						";Connect Timeout=",
						(this.ConnectionTimeOut==null?"30":this.ConnectionTimeOut.ToString()),
						""
					});
                }
                return result;
            }
        }
        /// <summary>
        /// 创建数据库操作对象
        /// </summary>
        /// <returns></returns>
        public override DataHandler CreateDataHandler()
        {
            if (this.ConnectionString != null)
            {
                return new SQLClientDataHandler(this.ConnectionString);
            }
            else
            {
                return new SQLClientDataHandler();
            }
        }
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="name">表名</param>
        /// <param name="column">字段名</param>
        /// <param name="handler">数据操作助手</param>
        /// <returns></returns>
        public override string GetFiledDescription(string name, string column, DataHandler handler)
        {
            SqlDataReader sqlDataReader = null;
            EnumOpenConnectState openConnectionState = EnumOpenConnectState.NoNeedClose;
            string text = string.Empty;
            if (handler == null)
            {
                handler = new SQLClientDataHandler(this.ConnectionString);
                openConnectionState = EnumOpenConnectState.NeedClose;
            }
            string text2 = string.Concat(new string[]{
                "SELECT * FROM ::fn_listextendedproperty (NULL, 'user', 'dbo', 'TABLE', '",
                name,
                 "', 'column', '",
               column,
                "')"
            });
            try
            {
                //1.先从对应表中获取描述
                sqlDataReader = (SqlDataReader)handler.GetDataReader(text2);
                if (sqlDataReader.Read())
                {
                    text = sqlDataReader["value"].ToString().Trim();
                }
                sqlDataReader.Close();
                if (text.Trim().Length <= 0)
                {
                    text2 = "select sys.tables.name as tablename,sys.columns.name as fieldname ,sys.extended_properties.value ";
                    text2 += "from sys.extended_properties inner join sys.columns ";
                    text2 += "on sys.extended_properties.major_id = sys.columns.object_id and sys.extended_properties.minor_id = sys.columns.column_id ";
                    text2 += "left join sys.tables on sys.tables.object_id = sys.columns.object_id ";
                    text2 = text2 + "where sys.columns.name = '" + column + "'";
                    //2.当表没有描述时，获取其他表设置的相同字段名的描述
                    sqlDataReader = (SqlDataReader)handler.GetDataReader(text2);
                    if (sqlDataReader.Read())
                    {
                        text = sqlDataReader["value"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }
                if (handler.State != ConnectionState.Closed)
                {
                    handler.Close(openConnectionState);
                }
            }
            return text;
        }
        /// <summary>
        /// 获取表的字段集合
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override List<sysFieldEntity> GetFields(string name)
        {
            List<sysFieldEntity> list = new List<sysFieldEntity>();
            SqlDataReader sqlDataReader = null;
            SQLClientDataHandler sqlClientDataHandler = new SQLClientDataHandler(this.ConnectionString);
            try
            {
                string sql = "EXEC sp_tables @table_name = '" + name + "'";
                //1.校验表是否存在
                sqlDataReader = (SqlDataReader)sqlClientDataHandler.GetDataReader(sql);
                if (!sqlDataReader.Read())
                {
                    throw new Exception(name + " is not exist!");
                }
                sqlDataReader.Close();
                ArrayList arrayList = new ArrayList();
                //2.判断表是否存在主键
                sql = "EXEC sp_pkeys @table_name = '" + name + "'";
                sqlDataReader = (SqlDataReader)sqlClientDataHandler.GetDataReader(sql);
                while (sqlDataReader.Read())
                {
                    string value = sqlDataReader["COLUMN_NAME"].ToString().Trim();
                    arrayList.Add(value);
                }
                sqlDataReader.Close();
                sql = "EXEC sp_columns @table_name = '" + name + "'";
                //3.获取表对应的字段
                sqlDataReader = (SqlDataReader)sqlClientDataHandler.GetDataReader(sql);
                while (sqlDataReader.Read())
                {
                    sysFieldEntity fieldEntity = new sysFieldEntity();

                    fieldEntity.FLD_NAME = sqlDataReader["COLUMN_NAME"].ToString().Trim();
                    fieldEntity.FLD_AUTOINCREMENT = false;
                    //判断是否为主键
                    if (arrayList.Contains(fieldEntity.FLD_NAME))
                    {
                        fieldEntity.FLD_PRIMARYKEY = true;
                    }
                    else
                    {
                        fieldEntity.FLD_PRIMARYKEY = false;
                    }
                    string text = sqlDataReader["TYPE_NAME"].ToString().Trim();
                    if (text.IndexOf(" ") > 0)
                    {
                        //判断是否为自增列
                        if (text.Split(new char[] { ' ' })[1].Trim().ToLower() == "identity")
                        {
                            fieldEntity.FLD_AUTOINCREMENT = true;
                        }
                        text = text.Split(new char[] { ' ' })[0].Trim();
                    }
                    fieldEntity.FLD_DATASQLTYPE = text;
                    fieldEntity.FLD_DATATYPE = (int)this.DataTypeFromSQLType(fieldEntity.FLD_DATASQLTYPE);
                    fieldEntity.FLD_LENGTH = Convert.ToInt32(sqlDataReader["PRECISION"]);
                    if (sqlDataReader["SCALE"].Equals(DBNull.Value))
                    {
                        fieldEntity.FLD_DEC = 0;
                    }
                    else
                    {
                        fieldEntity.FLD_DEC = Convert.ToInt32(sqlDataReader["SCALE"]);
                    }
                    fieldEntity.FLD_NULLABLE = Convert.ToBoolean(sqlDataReader["NULLABLE"]);
                    fieldEntity.TABLEVIEW_NAME = name;
                    fieldEntity.FLD_POS = Convert.ToInt32(sqlDataReader["ORDINAL_POSITION"]);
                    fieldEntity.FLD_DESCRIPTION = this.GetFiledDescription(name, fieldEntity.FLD_NAME, sqlClientDataHandler);
                    //获取字段描述
                    list.Add(fieldEntity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }
                if (sqlClientDataHandler != null)
                {
                    sqlClientDataHandler.Close(EnumOpenConnectState.NeedClose);
                }
            }
            return list;
        }
        /// <summary>
        /// FieldDataType转数据库字段类型字符
        /// </summary>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public override string DataTypeToSQLType(FieldDataType datatype)
        {

            switch (datatype)
            {
                case FieldDataType.IntType:
                    return "int";
                case FieldDataType.BoolType:
                    return "bit";
                case FieldDataType.DateTimeType:
                    return "datetime";
                case FieldDataType.DecimalType:
                    return "decimal";
                case FieldDataType.BinaryType:
                    return "image";
                case FieldDataType.TextType:
                    return "ntext";
                default:
                    return "nvarchar";
            }
        }
        /// <summary>
        /// 数据库字段类型字符转FieldDataType
        /// </summary>
        /// <param name="sqltype"></param>
        /// <returns></returns>
        public override FieldDataType DataTypeFromSQLType(string sqltype)
        {
            string type = sqltype.Trim().ToLower();
            if (type != "nvarchar")
            {
                if (type != "varchar")
                {
                    if (type != "char")
                    {
                        if (type != "nchar")
                        {
                            if (type != "timestamp")
                            {
                                if (type == "ntext" || type == "text")
                                {
                                    return FieldDataType.TextType;
                                }
                                if (type != "biginit")
                                {
                                    if (type != "int")
                                    {
                                        if (type != "smallint")
                                        {
                                            if (type != "tinyint")
                                            {
                                                if (type != "binary")
                                                {
                                                    if (type != "image")
                                                    {
                                                        if (type != "varbinary")
                                                        {
                                                            if (type == "bit")
                                                            {
                                                                return FieldDataType.BoolType;
                                                            }
                                                            if (type != "datetime")
                                                            {
                                                                if (type != "smalldatetime")
                                                                {
                                                                    if (type != "date")
                                                                    {
                                                                        if (type != "decimal")
                                                                        {
                                                                            if (type != "float")
                                                                            {
                                                                                if (type != "money")
                                                                                {
                                                                                    if (type != "numeric")
                                                                                    {
                                                                                        if (type != "real")
                                                                                        {
                                                                                            if (type != "smallmoney")
                                                                                            {
                                                                                                return FieldDataType.StringType;
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        return FieldDataType.DecimalType;
                                                                    }
                                                                }
                                                            }
                                                            return FieldDataType.DateTimeType;
                                                        }
                                                    }
                                                }
                                                return FieldDataType.BinaryType;
                                            }
                                        }
                                    }
                                }
                                return FieldDataType.IntType;
                            }
                        }
                    }
                }
            }
            return FieldDataType.StringType;
        }
    }
}
