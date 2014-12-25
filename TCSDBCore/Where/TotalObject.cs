using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
     [Serializable]
    public class TotalObject
    {
        private string aliasName;

        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value.Trim(); }
        }
        private string fieldName;

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value.Trim(); }
        }
        private EnumTotalFunction totalFunction;

        public EnumTotalFunction TotalFunction
        {
            get { return totalFunction; }
            set { totalFunction = value; }
        }
        private string caption;

        public string Caption
        {
            get { return caption; }
            set { caption = value.Trim(); }
        }
        private EnumWhereControlType havingControl;

        public EnumWhereControlType HavingControl
        {
            get { return havingControl; }
            set { havingControl = value; }
        }
        private object havingValue;

        public object HavingValue
        {
            get { return havingValue; }
            set { havingValue = value; }
        }
        private string tableAlias;
        internal string TableAlias
        {
            get { return tableAlias; }
            set { tableAlias = value.Trim(); }
        }
        internal string FuncntionString
        {
            get { return GetFunctionString(this.totalFunction); }
        }

        public TotalObject()
        {
            this.aliasName = "";
            this.totalFunction = EnumTotalFunction.Group;
            this.fieldName = "";
            this.caption = "";
            this.havingControl = EnumWhereControlType.等于;
            this.havingValue = null;
            this.tableAlias = "";
        }
        private string GetFunctionString(EnumTotalFunction totalFunction)
        {
            switch (totalFunction)
            {
                case EnumTotalFunction.Group:
                   return "group by";
                case EnumTotalFunction.Sum:
                   return "sum";
                case EnumTotalFunction.Count:
                   return "count";
                case EnumTotalFunction.Max:
                   return "max";
                case EnumTotalFunction.Min:
                   return "min";
                case EnumTotalFunction.Avg:
                   return "avg";
                case EnumTotalFunction.Stdev:
                   return "stdev";
                case EnumTotalFunction.Stdevp:
                   return "stdevp";
                case EnumTotalFunction.Var:
                   return "var";
                case EnumTotalFunction.Varp:
                   return "varp";
                case EnumTotalFunction.Express:
                   return "express";
                default:
                    return "";
            }
        }
         /// <summary>
         /// 根据枚举类型，创建枚举对应的table
         /// </summary>
         /// <param name="enumType"></param>
         /// <returns></returns>
        protected  DataTable CreateEnumTable(Type enumType)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Index", typeof(int));
            table.Columns.Add("Name", typeof(string));
            IEnumerator enumerator = Enum.GetValues(enumType).GetEnumerator();
            int index = -1;
            while (enumerator.MoveNext())
            {
                index = Convert.ToInt32(enumerator.Current);
                string name = Enum.GetName(enumType, index);
                DataRow newRow = table.NewRow();
                newRow["Index"] = index;
                newRow["Name"] = name;
                table.Rows.Add(newRow);
            }
            return table;
        }
         /// <summary>
        /// 获取EnumTotalFunctionCatpion的table数据实体
         /// </summary>
         /// <returns></returns>
        public DataTable TotalFunctionCaptionTable()
        {
            return this.CreateEnumTable(typeof(EnumTotalFunctionCatpion));
        }
         /// <summary>
         /// 获取sql语句
         /// </summary>
         /// <param name="withAliasName"></param>
         /// <param name="dbType"></param>
         /// <returns></returns>
        public string ToSelectSql(bool withAliasName, DatabaseType dbType)
        {
            StringBuilder sb = new StringBuilder();
            if (this.TotalFunction == EnumTotalFunction.Express)
            {
                sb.Append(this.FieldName);
            }
            else
            {
                if (this.FuncntionString.Length > 0)
                {
                    sb.Append(this.FuncntionString + "(" + this.FieldName + ")");
                }
                else
                {
                    sb.Append((this.TableAlias.Length>0?this.TableAlias+".":"") + this.FieldName);
                }
            }
            if (withAliasName && this.AliasName.Length > 0)
            {
                sb.Append(" as " + this.AliasName.Trim());
            }
            return sb.ToString();
        }
    }
}
