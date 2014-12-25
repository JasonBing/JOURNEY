using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 删除SQL
    /// </summary>
    public class DeleteSqlObject : SqlObject
    {

        /// <summary>
        /// 删除SQL
        /// </summary>
        public string DeleteSql;
        /// <summary>
        /// WHERE SQL
        /// </summary>
        public string WhereSql;
        public DeleteSqlObject()
        {
            this.DeleteSql = "";
            this.WhereSql = "";
        }
        /// <summary>
        /// 转换SQL语句
        /// </summary>
        /// <returns></returns>
        public override string ToSql()
        {
            string text = this.DeleteSql;
            if (this.WhereSql.Trim().Length > 0)
            {
                text = text + " Where " + this.WhereSql;
            }
            return text;
        }
        public static DeleteSqlObject SqlObjectFromDeleteEntity(EntityBase entity, DatabaseType dbType)
        {
            DeleteSqlObject deleteSqlObject = new DeleteSqlObject();
            deleteSqlObject.DeleteSql = "Delete From " + sql(entity, dbType);
            deleteSqlObject.WhereSql = entity.ToWhereString(deleteSqlObject.Params, dbType, true);
            return deleteSqlObject;
        }
        public static DeleteSqlObject SqlObjectFromDeleteEntityEx(EntityBase entity, IWhereSQLObject whereSqlObj, DatabaseType dbType)
        {
            DeleteSqlObject deleteSqlObject = new DeleteSqlObject();
            deleteSqlObject.DeleteSql = "Delete From " + sql(entity, dbType);
            if (whereSqlObj != null)
            {
                deleteSqlObject.WhereSql = whereSqlObj.ToWhereString(deleteSqlObject.Params, dbType);
            }
            return deleteSqlObject;
        }
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
    }
}
