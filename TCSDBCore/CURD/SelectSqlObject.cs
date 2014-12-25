using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 查询SQL
    /// </summary>
    public class SelectSqlObject : SqlObject
    {
        private string selectSql;

        public string SelectSql
        {
            get { return selectSql; }
            set
            {
                if (value == null)
                {
                    selectSql = "";
                }
                else
                {
                    selectSql = value.Trim();
                }
            }
        }
        private string whereSql;

        public string WhereSql
        {
            get { return whereSql.Trim(); }
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
        private string sortSql;

        public string SortSql
        {
            get { return sortSql; }
            set
            {
                if (value == null)
                {
                    sortSql = "";
                }
                else
                {
                    sortSql = value.Trim();
                }
            }
        }
        private string groupSql;

        public string GroupSql
        {
            get { return groupSql; }
            set
            {
                if (value == null)
                {
                    groupSql = "";
                }
                else
                {
                    groupSql = value.Trim();
                }
            }
        }
        private string havingSql;

        public string HavingSql
        {
            get { return havingSql; }
            set
            {
                if (value == null)
                {
                    havingSql = "";
                }
                else
                {
                    havingSql = value.Trim();
                }
            }
        }
        private string additionSql;

        public string AdditionSql
        {
            get { return additionSql; }
            set
            {
                if (value == null)
                {
                    additionSql = "";
                }
                else
                {
                    additionSql = value.Trim();
                }
            }
        }
        public SelectSqlObject()
        {
            this.selectSql = "";
            this.whereSql = "";
            this.sortSql = "";
            this.groupSql = "";
            this.havingSql = "";
            this.additionSql = "";
        }
        private static string sql(EntityBase entity, DatabaseType dbType, ref bool flag)
        {
            StringBuilder sb = new StringBuilder();
            if (entity.EntitySource.ToUpper().IndexOf("SELECT ") >= 0)
            {
                sb.Append(string.Format("({0}) as viewEntity", entity.EntitySource));
                flag = true;
            }
            else
            {
                if (dbType == DatabaseType.ORACLE)
                {
                    if (entity.EntitySource.IndexOf(".") > 0)
                    {
                        sb.Append(entity.EntitySource);
                    }
                    else
                    {
                        sb.Append(string.Format("\"{0}\"", entity.EntitySource));
                    }
                }
                else
                {
                    sb.Append(entity.EntitySource);
                }
                flag = false;
            }
            return sb.ToString();
        }
        private static string sql(EntityBase entity, DatabaseType dbType)
        {
            bool flag = false;
            return SelectSqlObject.sql(entity, dbType, ref flag);
        }
        private static string sql(ICollection collection)
        {
            StringBuilder sb = new StringBuilder();
            bool flag = true;
            IEnumerator enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string value = enumerator.Current.ToString();
                if (!flag)
                {
                    sb.Append(",");
                }
                flag = false;
                sb.Append(value);
            }
            return sb.ToString();
        }
        internal static string sql(EntityBase entity, ICollection collection)
        {
            string result = String.Empty;
            if (collection != null && collection.Count > 0)
            {
                result = SelectSqlObject.sql(collection);
            }
            else
            {
                result = SelectSqlObject.sql(entity.FieldKeys);
            }
            return result;
        }
        private static string sql(EntityBase entity, string addition, ICollection collection, bool isDistinct, int maxNum, DatabaseType dbType, ref string retSql)
        {
            retSql = "";
            StringBuilder sb = new StringBuilder();
            switch (dbType)
            {
                case DatabaseType.SQLSERVER:
                    sb.Append("select ");
                    if (isDistinct == true)
                    {
                        sb.Append(" distinct");
                    }
                    if (maxNum > 0)
                    {
                        sb.Append(string.Format(" top {0}", maxNum));
                    }
                    if (addition.Trim().Length > 0)
                    {
                        sb.Append(" " + addition + ",");
                    }
                    sb.Append(SelectSqlObject.sql(entity, collection));
                    sb.Append(" from " + SelectSqlObject.sql(entity, dbType));
                    break;
                case DatabaseType.ORACLE:
                    sb.Append("select * from (select");
                    if (isDistinct == true)
                    {
                        sb.Append(" distinct");
                    }
                    retSql = ")";
                    if (maxNum > 0)
                    {
                        retSql += "where RowNum <= " + maxNum.ToString();
                    }
                    if (addition.Trim().Length > 0)
                    {
                        sb.Append(" " + addition + ",");
                    }
                    sb.Append(SelectSqlObject.sql(entity, collection));
                    sb.Append(" from " + SelectSqlObject.sql(entity, dbType));
                    break;
                case DatabaseType.MYSQL:
                    break;
                case DatabaseType.SQLITE:
                    break;
                default:
                    sb.Append("Select * From " + sql(entity, dbType));
                    break;
            }
            return sb.ToString();
        }
        private static string sql(EntityBase entity, string addition, TotalObjectList totalList, int maxNum, ref string retStr, DatabaseType dbType)
        {
            retStr = "";
            string text = string.Empty;
            switch (dbType)
            {
                case DatabaseType.SQLSERVER:
                    text = "Select";
                    if (maxNum > 0)
                    {
                        text = text + " Top " + maxNum.ToString();
                    }
                    if (addition.Trim().Length > 0)
                    {
                        text = text + " " + addition.Trim() + ",";
                    }
                    text = text + " " + totalList.totalSql(dbType, "");
                    text = text + " From " + SelectSqlObject.sql(entity, dbType);
                    break;
                case DatabaseType.ORACLE:
                    text = "Select * From(Select";
                    retStr = ")";
                    if (maxNum > 0)
                    {
                        retStr += " Where RowNum <= " + maxNum.ToString();
                    }
                    if (addition.Trim().Length > 0)
                    {
                        text = text + " " + addition.Trim() + ",";
                    }

                    text = text + " " + totalList.totalSql(dbType, "");
                    text = text + " From " + sql(entity, dbType);
                    break;
                case DatabaseType.MYSQL:
                    break;
                case DatabaseType.SQLITE:
                    break;
                default:
                    text = "Select * From " + sql(entity, dbType);
                    break;
            }
            return text;
        }
        private static string sql(SelectSqlObject selSql, TotalObjectList totalList, int rowNum, ref string retStr, DatabaseType type)
        {
            retStr = "";
            string text = "v1";
            string text2 = string.Empty;
            switch (type)
            {
                case DatabaseType.SQLSERVER:
                    text2 = "Select";
                    if (rowNum > 0)
                    {
                        text2 = text2 + " Top" + rowNum.ToString();
                    }
                    text2 = text2 + " " + totalList.totalSql(type, text);
                    text2 = string.Concat(new string[] { text2, "From (", selSql.ToSql(), ") AS ", text });
                    break;
                case DatabaseType.ORACLE:
                    text2 = "Select * From(Select";
                    retStr = ")";
                    if (rowNum > 0)
                    {
                        text = text + "Where RowNum <=" + rowNum.ToString();
                    }
                    text2 = text2 + " " + totalList.totalSql(type, text);
                    text2 = text2 + "From (" + selSql.ToSql() + ")";
                    break;
                case DatabaseType.MYSQL:
                    break;
                case DatabaseType.SQLITE:
                    break;
                default:
                    text2 = "Select * From (" + selSql.ToSql() + ") As " + text;
                    break;
            }
            return text2;
        }
        public override string ToSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.selectSql);
            if (this.WhereSql.Length > 0)
            {
                sb.Append(string.Format(" Where {0}", this.WhereSql));
            }
            if (this.GroupSql.Length > 0)
            {
                sb.Append(string.Format(" Group By {0}", this.GroupSql));
            }
            if (this.HavingSql.Length > 0)
            {
                sb.Append(string.Format(" Having {0}", this.HavingSql));
            }
            if (this.SortSql.Length > 0)
            {
                sb.Append(string.Format(" Order By {0}", this.SortSql));
            }
            if (this.AdditionSql.Length > 0)
            {
                sb.Append(string.Format(" {0}", this.AdditionSql));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 查询操作助手
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static SelectSqlObject SqlObjectFromGetEntity(EntityBase entity, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(entity, "", null, false, 1, dbType, ref selectSqlObject.additionSql);
            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType, true);
            return selectSqlObject;
        }
        /// <summary>
        /// 查询操作助手
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="OrderSort">排序设置</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static SelectSqlObject SqlObjectFromGetEntityEx(EntityBase entity, string OrderSort, DatabaseType dbType)
        {

            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(entity, "", null, false, 1, dbType, ref selectSqlObject.additionSql);
            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType);
            if (OrderSort != null)
            {
                selectSqlObject.SortSql = OrderSort.Trim();
            }
            return selectSqlObject;

        }
        /// <summary>
        /// 查询操作助手 
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="whereSqlObj">Where条件</param>
        /// <param name="OrderSort">排序设置</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static SelectSqlObject SqlObjectFromGetEntityEx(EntityBase entity, IWhereSQLObject whereSqlObj, string OrderSort, DatabaseType dbType)
        {

            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(entity, "", null, false, 1, dbType, ref selectSqlObject.additionSql);
            if (whereSqlObj != null)
            {
                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, dbType);
            }
            if (OrderSort != null)
            {
                selectSqlObject.SortSql = OrderSort.Trim();
            }
            return selectSqlObject;

        }
        /// <summary>
        /// 查询操作助手
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="Fields">需要返回的字段集合</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <returns></returns>
        public static SqlObject SqlObjectFromDataTable(EntityBase entity, ICollection Fields, DatabaseType dbType, bool isDistinct)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(entity, "", Fields, isDistinct, 0, dbType, ref selectSqlObject.additionSql);
            return selectSqlObject;

        }
        /// <summary>
        /// 查询操作助手
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="Fields">需要返回的字段集合</param>
        /// <param name="MaxRecord">返回的最大记录数</param>
        /// <param name="OrderSort">排序设置</param>
        /// <param name="isDistinct">是否Distinct</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static SqlObject SqlObjectFromDataTableEx(EntityBase entity, ICollection Fields, int MaxRecord, string OrderSort, bool isDistinct, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();

            selectSqlObject.SelectSql = sql(entity, "", Fields, isDistinct, MaxRecord, dbType, ref selectSqlObject.additionSql);
            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType);
            if (OrderSort != null)
            {
                selectSqlObject.SortSql = OrderSort.Trim();
            }
            return selectSqlObject;

        }
        public static SqlObject SqlObjectFromDataTableEx(EntityBase entity, ICollection Fields, int BeginRecord, int EndRecord, int MaxRecord, string OrderSort, bool isDistinct, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            try
            {
                switch (dbType)
                {
                    case DatabaseType.SQLSERVER:
                        {
                            if (OrderSort == null || OrderSort.Trim().Length <= 0)
                            {
                                OrderSort = SelectSqlObject.sql(entity.PrimaryKeys);
                            }
                            if (OrderSort == null || OrderSort.Trim().Length <= 0)
                            {
                                OrderSort = entity.FieldKeys[0];
                            }
                            selectSqlObject.SelectSql = sql(entity, "Row_Number() Over (Order By " + OrderSort + " )As Row", Fields, isDistinct, MaxRecord, dbType, ref selectSqlObject.additionSql);
                            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType);
                            string str = selectSqlObject.ToSql();
                            selectSqlObject.SelectSql = "Select * From(" + str + ") As temp" + entity.EntitySource;
                            selectSqlObject.WhereSql = "Row Between " + BeginRecord.ToString() + " And " + EndRecord.ToString();
                            selectSqlObject.SortSql = "Row";
                            SqlObject result = selectSqlObject;
                            return result;
                        }
                    case DatabaseType.ORACLE:
                        {
                            selectSqlObject.SelectSql = "Select " + sql(entity, Fields) + " From(Select t.*,RowNum RN From(Select";
                            SelectSqlObject selectSqlObject2;
                            if (isDistinct)
                            {
                                selectSqlObject2 = selectSqlObject;
                                selectSqlObject2.SelectSql += " Distinct";
                            }
                            selectSqlObject2 = selectSqlObject;
                            selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " " + sql(entity, Fields);
                            selectSqlObject2 = selectSqlObject;
                            selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " From " + sql(entity, dbType);
                            selectSqlObject.SortSql = OrderSort;
                            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType);
                            string text = ")t ";
                            if (MaxRecord > 0)
                            {
                                text = string.Concat(new string[]
                        {
                            text,
                            "Where RowNum <= ",
                            Math.Min(checked(BeginRecord + MaxRecord), EndRecord).ToString(),
                            ")Where RN >= ",
                            BeginRecord.ToString()
                        });
                            }
                            else
                            {
                                text = string.Concat(new string[]
                        {
                            text,
                            "Where RowNum <= ",
                            EndRecord.ToString(),
                            ")Where RN >= ",
                            BeginRecord.ToString()
                        });
                            }
                            selectSqlObject.AdditionSql = text;
                            SqlObject result = selectSqlObject;
                            return result;
                        }
                }
                throw new Exception("Now no support!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SqlObject SqlObjectFromDataTableEx(EntityBase entity, ICollection Fields, IWhereSQLObject whereSqlObj, int MaxRecord, string OrderSort, bool isDistinct, DatabaseType Dbtype)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            try
            {
                selectSqlObject.SelectSql = sql(entity, "", Fields, isDistinct, MaxRecord, Dbtype, ref selectSqlObject.additionSql);
                if (whereSqlObj != null)
                {
                    selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                }
                if (OrderSort != null)
                {
                    selectSqlObject.SortSql = OrderSort.Trim();
                }
                return selectSqlObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SqlObject SqlObjectFromDataTableEx(EntityBase entity, ICollection Fields, IWhereSQLObject whereSqlObj, int BeginRecord, int EndRecord, int MaxRecord, string OrderSort, bool isDistinct, DatabaseType Dbtype)
        {

            SelectSqlObject selectSqlObject = new SelectSqlObject();
            try
            {
                switch (Dbtype)
                {
                    case DatabaseType.SQLSERVER:
                        {
                            if (OrderSort == null || OrderSort.Trim().Length <= 0)
                            {
                                if (entity.PrimaryKeys.Count > 0)
                                {
                                    OrderSort = SelectSqlObject.sql(entity.PrimaryKeys);
                                }
                            }
                            if (OrderSort == null || OrderSort.Trim().Length <= 0)
                            {
                                OrderSort = entity.FieldKeys[0];
                            }
                            selectSqlObject.SelectSql = sql(entity, "Row_Number() Over (Order By " + OrderSort + ") As Row", Fields, isDistinct, MaxRecord, Dbtype, ref selectSqlObject.additionSql);
                            if (whereSqlObj != null)
                            {
                                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                            }
                            string str = selectSqlObject.ToSql();
                            selectSqlObject.SelectSql = "Select * From(" + str + ") As temp" + entity.EntitySource;
                            selectSqlObject.WhereSql = "Row Between " + BeginRecord.ToString() + " And " + EndRecord.ToString();
                            selectSqlObject.SortSql = "Row";
                            SqlObject result = selectSqlObject;
                            return result;
                        }
                    case DatabaseType.ORACLE:
                        {
                            selectSqlObject.SelectSql = "Select " + sql(entity, Fields) + " From(Select t.*,RowNum RN From(Select";
                            SelectSqlObject selectSqlObject2;
                            if (isDistinct)
                            {
                                selectSqlObject2 = selectSqlObject;
                                selectSqlObject2.SelectSql += " Distinct";
                            }
                            selectSqlObject2 = selectSqlObject;
                            selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " " + sql(entity, Fields);
                            selectSqlObject2 = selectSqlObject;
                            selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " From " + sql(entity, Dbtype);
                            selectSqlObject.SortSql = OrderSort;
                            if (whereSqlObj != null)
                            {
                                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                            }
                            string text = ")t ";
                            if (MaxRecord > 0)
                            {
                                text = string.Concat(new string[]{
                                                                text,
                                                                "Where RowNum <= ",
                                                                Math.Min(checked(BeginRecord + MaxRecord), EndRecord).ToString(),
                                                                ")Where RN >= ",
                                                                BeginRecord.ToString()
                                                                });
                            }
                            else
                            {
                                text = string.Concat(new string[]
                        {
                            text,
                            "Where RowNum <= ",
                            EndRecord.ToString(),
                            ")Where RN >= ",
                            BeginRecord.ToString()
                        });
                            }
                            selectSqlObject.AdditionSql = text;
                            SqlObject result = selectSqlObject;
                            return result;
                        }
                }
                throw new Exception("Now no support!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static SelectSqlObject SqlObjectFromGroup(EntityBase entity, TotalObjectList ListTotal, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(entity, "", ListTotal, 0, ref selectSqlObject.additionSql, dbType);
            selectSqlObject.GroupSql = ListTotal.totalSql();
            return selectSqlObject;
        }
        public static SelectSqlObject SqlObjectFromGroupEx(EntityBase entity, TotalObjectList ListTotal, int MaxRecord, string OrderSort, string dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            try
            {
                selectSqlObject.SelectSql = sql(entity, "", ListTotal, MaxRecord, ref selectSqlObject.additionSql, (DatabaseType)Convert.ToInt32(dbType));
                selectSqlObject.HavingSql = ListTotal.whereCollection((DatabaseType)Convert.ToInt32(dbType)).ToWhereString(selectSqlObject.Params, (DatabaseType)Convert.ToInt32(dbType));
                selectSqlObject.GroupSql = ListTotal.totalSql();
                selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, (DatabaseType)Convert.ToInt32(dbType));
                if (OrderSort != null)
                {
                    selectSqlObject.SortSql = OrderSort.Trim();
                }
                return selectSqlObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static SelectSqlObject SqlObjectFromGroupEx(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int MaxRecord, string OrderSort, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();

            selectSqlObject.SelectSql = sql(entity, "", ListTotal, MaxRecord, ref selectSqlObject.additionSql, dbType);
            selectSqlObject.HavingSql = ListTotal.whereCollection(dbType).ToWhereString(selectSqlObject.Params, dbType);
            selectSqlObject.GroupSql = ListTotal.totalSql();
            selectSqlObject.SortSql = OrderSort;
            if (whereSqlObj != null)
            {
                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, dbType);
            }
            if (OrderSort != null)
            {
                selectSqlObject.SortSql = OrderSort.Trim();
            }
            return selectSqlObject;

        }
        public static SqlObject SqlObjectFromGroupEx(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int BeginRecord, int EndRecord, int MaxRecord, string OrderSort, DatabaseType Dbtype)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();

            switch (Dbtype)
            {
                case DatabaseType.SQLSERVER:
                    {
                        selectSqlObject.GroupSql = ListTotal.totalSql();
                        selectSqlObject.SelectSql = sql(entity, "Row_Number() Over (Order By " + OrderSort + ") As Row", ListTotal, MaxRecord, ref selectSqlObject.additionSql, Dbtype);
                        selectSqlObject.HavingSql = ListTotal.whereCollection(Dbtype).ToWhereString(selectSqlObject.Params, Dbtype);
                        if (whereSqlObj != null)
                        {
                            selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                        }
                        string str = selectSqlObject.ToSql();
                        selectSqlObject.SelectSql = "Select * From(" + str + ") As temp" + entity.EntitySource;
                        selectSqlObject.WhereSql = "Row Between " + BeginRecord.ToString() + " And " + EndRecord.ToString();
                        selectSqlObject.SortSql = "Row";
                        selectSqlObject.GroupSql = "";
                        selectSqlObject.HavingSql = "";
                        SqlObject result = selectSqlObject;
                        return result;
                    }
                case DatabaseType.ORACLE:
                    {
                        selectSqlObject.SelectSql = "Select " + ListTotal.totalSql(Dbtype, "") + " From(Select t.*,RowNum RN From(Select";
                        SelectSqlObject selectSqlObject2 = selectSqlObject;
                        selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " " + ListTotal.totalSql(Dbtype, "");
                        selectSqlObject2 = selectSqlObject;
                        selectSqlObject2.SelectSql = selectSqlObject2.SelectSql + " From " + sql(entity, Dbtype);
                        selectSqlObject.SortSql = OrderSort;
                        selectSqlObject.HavingSql = ListTotal.whereCollection((DatabaseType)Convert.ToInt32(Dbtype)).ToWhereString(selectSqlObject.Params, (DatabaseType)Convert.ToInt32(Dbtype));
                        selectSqlObject.GroupSql = ListTotal.totalSql();
                        if (whereSqlObj != null)
                        {
                            selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                        }
                        if (whereSqlObj != null)
                        {
                            selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, Dbtype);
                        }
                        string text = ")t ";
                        if (MaxRecord > 0)
                        {
                            text = string.Concat(new string[]{
                            text,
                            "Where RowNum <= ",
                            Math.Min(checked(BeginRecord + MaxRecord), EndRecord).ToString(),
                            ")Where RN >= ",
                            BeginRecord.ToString()
                        });
                        }
                        else
                        {
                            text = string.Concat(new string[]
                        {
                            text,
                            "Where RowNum <= ",
                            EndRecord.ToString(),
                            ")Where RN >= ",
                            BeginRecord.ToString()
                        });
                        }
                        selectSqlObject.AdditionSql = text;
                        SqlObject result = selectSqlObject;
                        return result;
                    }
            }
            throw new Exception("Now no support!");


        }
        public static SelectSqlObject SqlObjectFromGroup(SelectSqlObject sqlobj, TotalObjectList ListTotal, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = sql(sqlobj, ListTotal, 0, ref selectSqlObject.additionSql, dbType);
            selectSqlObject.Params.AddRange(sqlobj.Params);
            selectSqlObject.GroupSql = ListTotal.totalSql();
            selectSqlObject.HavingSql = ListTotal.whereCollection(dbType).ToWhereString(selectSqlObject.Params, dbType);
            return selectSqlObject;
        }
        public static SelectSqlObject SqlObjectFromGroupEx(SelectSqlObject sqlobj, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, int MaxRecord, string OrderSort, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();

            selectSqlObject.Params.AddRange(sqlobj.Params);
            selectSqlObject.GroupSql = ListTotal.totalSql();
            if (whereSqlObj != null)
            {
                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, dbType);
            }
            if (OrderSort != null)
            {
                selectSqlObject.SortSql = OrderSort.Trim();
            }
            return selectSqlObject;

        }
        public static SelectSqlObject SqlObjectFromCount(EntityBase entity, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();

            selectSqlObject.SelectSql = "Select Count(*) As RowsCount From " + sql(entity, dbType);
            return selectSqlObject;

        }
        public static SelectSqlObject SqlObjectFromCountEx(EntityBase entity, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = "Select Count(*) As RowsCount From " + sql(entity, dbType);
            selectSqlObject.WhereSql = entity.ToWhereString(selectSqlObject.Params, dbType);
            return selectSqlObject;

        }
        public static SelectSqlObject SqlObjectFromCountEx(EntityBase entity, IWhereSQLObject whereSqlObj, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = "Select Count(*) As RowsCount From " + sql(entity, dbType);
            if (whereSqlObj != null)
            {
                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, dbType);
            }
            return selectSqlObject;

        }
        public static SelectSqlObject SqlObjectFromCountEx(EntityBase entity, TotalObjectList ListTotal, IWhereSQLObject whereSqlObj, DatabaseType dbType)
        {
            SelectSqlObject selectSqlObject = new SelectSqlObject();
            selectSqlObject.SelectSql = "Select Count(*) From (Select Count(*) As RowsCount From " + sql(entity, dbType);
            selectSqlObject.GroupSql = ListTotal.totalSql();
            if (whereSqlObj != null)
            {
                selectSqlObject.WhereSql = whereSqlObj.ToWhereString(selectSqlObject.Params, dbType);
            }
            selectSqlObject.AdditionSql = " )As t";
            return selectSqlObject;
        }
    }
}
