using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 更新SQL
    /// </summary>
    public class UpdateSqlObject : SqlObject
    {
        private string updateSql;

        public string UpdateSql
        {
            get { return updateSql; }
            set
            {
                if (value == null)
                {
                    updateSql = "";
                }
                else
                {
                    updateSql = value.Trim();
                }
            }
        }
        private string setSql;

        public string SetSql
        {
            get { return setSql; }
            set
            {
                if (value == null)
                {
                    setSql = "";
                }
                else
                {
                    setSql = value.Trim();
                }
            }
        }
        private string whereSql;

        public string WhereSql
        {
            get { return whereSql; }
            set
            {
                if (value == null)
                {
                    whereSql = "";
                }
                else
                {
                    whereSql = value.Trim();
                }
            }
        }
        public static string sql(EntityBase entity, DatabaseType dbType)
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
            StringBuilder sb = new StringBuilder();
            sb.Append(this.UpdateSql + " SET " + this.SetSql);
            if (this.WhereSql.Trim().Length > 0)
            {
                sb.Append(" Where " + this.WhereSql);
            }
            return sb.ToString();
        }

        public static UpdateSqlObject SqlObjectFromUpdateEntity(EntityBase entity, DatabaseType dbType)
        {
            UpdateSqlObject updateSqlObject = new UpdateSqlObject();
            updateSqlObject.UpdateSql = "Update " + sql(entity, dbType);
            StringBuilder sb = new StringBuilder();
            bool flag = true;
            List<string>.Enumerator enumerator = entity.FieldKeys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                object objectValue = entity[current];
                if (objectValue != null && entity.Fields.AutoIncrementKeys.Contains(current) != true && entity.Fields.PrimaryKeys.Contains(current) == false)
                {
                    if (!flag)
                    {
                        sb.Append(",");
                    }
                    sb.Append(current + " = ");
                    if (objectValue.ToString().IndexOf("&@&") == 0)
                    {
                        sb.Append(objectValue.ToString().Substring("&@&".Length, objectValue.ToString().Length - "&@&".Length).Trim());
                    }
                    else
                    {
                        sb.Append(DataHandler.GetParamIdentify(current, dbType));
                        updateSqlObject.Params.Add(current, objectValue);
                    }
                    flag = false;
                }
            }
            if (flag)
            {
                throw new Exception("No value were updated");
            }
            updateSqlObject.SetSql = sb.ToString();
            updateSqlObject.WhereSql = entity.ToWhereString(updateSqlObject.Params, dbType, true);
            return updateSqlObject;
        }
        public static UpdateSqlObject SqlObjectFromUpdateEntityEx(EntityBase entity, IWhereSQLObject whereSqlObj, DatabaseType dbType)
        {
            UpdateSqlObject updateSqlObject = new UpdateSqlObject();
            updateSqlObject.UpdateSql = "Update " + sql(entity, dbType);
            StringBuilder sb = new StringBuilder();
            bool flag = true;
            List<string>.Enumerator enumerator = entity.FieldKeys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                object objectValue = entity[current];
                if (objectValue != null && entity.Fields.AutoIncrementKeys.Contains(current) != true && entity.Fields.PrimaryKeys.Contains(current) == false)
                {
                    if (!flag)
                    {
                        sb.Append(",");
                    }
                    sb.Append(current + " = ");
                    if (objectValue.ToString().IndexOf("&@&") == 0)
                    {
                        sb.Append(objectValue.ToString().Substring("&@&".Length, checked(objectValue.ToString().Length - "&@&".Length)).Trim());
                    }
                    else
                    {
                        sb.Append(DataHandler.GetParamIdentify(current, dbType));
                        updateSqlObject.Params.Add(current, objectValue);
                    }
                    flag = false;
                }
            }
            if (flag)
            {
                throw new Exception("No value were updated");
            }
            updateSqlObject.SetSql = sb.ToString();
            updateSqlObject.WhereSql = whereSqlObj.ToWhereString(updateSqlObject.Params, dbType);
            return updateSqlObject;
        }
    }
}
