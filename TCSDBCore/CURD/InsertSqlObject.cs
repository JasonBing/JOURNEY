using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 新增SQL
    /// </summary>
    public class InsertSqlObject : SqlObject
    {
        #region 属性
        private string insertSql;

        public string InsertSql
        {
            get { return insertSql; }
            set
            {
                if (value == null)
                {
                    insertSql = "";
                }
                else
                {
                    insertSql = value.Trim();
                }
            }
        }
        private string valuesSql;

        public string ValuesSql
        {
            get { return valuesSql; }
            set
            {
                if (value == null)
                {
                    valuesSql = "";
                }
                else
                {
                    valuesSql = value.Trim();
                }
            }
        }
        #endregion
        private static string sql(EntityBase entity, DatabaseType dbType)
        {
            string result = string.Empty;
            switch (dbType)
            {
                case DatabaseType.SQLSERVER:
                    result = entity.EntitySource;
                    break;
                case DatabaseType.ORACLE:
                    result = "\"" + entity.EntitySource + "\"";
                    break;
                case DatabaseType.MYSQL:
                    result = entity.EntitySource;
                    break;
                case DatabaseType.SQLITE:
                    result = entity.EntitySource;
                    break;
                default:
                    result = entity.EntitySource;
                    break;
            }
            return result;
        }
        public override string ToSql()
        {
            return this.InsertSql + " " + this.valuesSql;
        }
        internal static InsertSqlObject SqlObjectFromAddNewEntity(EntityBase entity, DatabaseType dbType)
        {

            InsertSqlObject insertSqlObject = new InsertSqlObject();
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            sb.Append("Insert Into " + sql(entity, dbType) + "(");
            sb1.Append("Values(");
            bool flag = false;
            List<string>.Enumerator enumerator = entity.FieldKeys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                if (entity.AutoIncrementKeys.Contains(current) == false && dbType == DatabaseType.ORACLE)
                {
                    if (flag == true)
                    {
                        sb.Append(",");
                        sb1.Append(",");
                    }
                    flag = true;
                    sb.Append(current);
                    sb1.Append(entity.EntitySource.ToUpper() + "_" + current.ToUpper() + ".NEXTVAL");
                }
                else
                {
                    if (entity[current] != null && entity.AutoIncrementKeys.Contains(current) == false)
                    {
                        if (flag == true)
                        {
                            sb.Append(",");
                            sb1.Append(",");
                        }
                        flag = true;
                        sb.Append(current);
                        sb1.Append(DataHandler.GetParamIdentify(current, dbType));
                        insertSqlObject.Params.Add(current, entity[current]);
                    }
                }
            }
            sb.Append(")");
            sb1.Append(")");
            insertSqlObject.InsertSql = sb.ToString();
            insertSqlObject.ValuesSql = sb1.ToString();
            return insertSqlObject;
        }
        internal static InsertSqlObject SqlObjectFromAddNewEntity(EntityBase entity, ICollection Fields, SelectSqlObject selSqlObject, DatabaseType DBType)
        {
            InsertSqlObject insertSqlObject = new InsertSqlObject();
            StringBuilder stringBuilder = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder.Append("Insert Into " + sql(entity, DBType) + "(");
            stringBuilder.Append(SelectSqlObject.sql(entity, Fields));
            stringBuilder.Append(") ");
            stringBuilder2.Append(selSqlObject.ToSql());
            insertSqlObject.Params.AddRange(selSqlObject.Params);
            insertSqlObject.InsertSql = stringBuilder.ToString();
            insertSqlObject.ValuesSql = stringBuilder2.ToString();
            return insertSqlObject;
        }
    }
}
