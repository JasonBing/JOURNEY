
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    [DataContract]
    public class WhereObject:IWhereSQLObject
    {
        private string field;
        /// <summary>
        /// 获取和设置字段
        /// </summary>
        public string Field
        {
            get { return field; }
            set { field = value; }
        }
        private EnumWhereControlType control;
        /// <summary>
        /// 获取和设置WHERE条件
        /// </summary>
        public EnumWhereControlType Control
        {
            get { return control; }
            set { control = value; }
        }
        private object value;
        /// <summary>
        /// 获取和设置值
        /// </summary>
        public object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        /// <summary>
        /// 获取条件的字符
        /// </summary>
        internal string ControlString
        {
            get
            {
                return WhereObject.GetControlString(this.Control);
            }
        }
        public WhereObject()
        {
            this.field = "";
            this.control = EnumWhereControlType.等于;
        }
        public WhereObject(string field, EnumWhereControlType ctrl, object value)
        {
            this.field = field;
            this.control = ctrl;
            this.value = value;
        }
        public static string GetControlString(EnumWhereControlType whereControl)
        {
            if (whereControl == EnumWhereControlType.不等于)
            {
                return "<>";
            }
            if (whereControl == EnumWhereControlType.大于)
            {
                return ">";
            }
            if (whereControl == EnumWhereControlType.大于等于)
            {
                return ">=";
            }
            if (whereControl == EnumWhereControlType.小于)
            {
                return "<";
            }
            if (whereControl == EnumWhereControlType.小于等于)
            {
                return "<=";
            }
            if (whereControl == EnumWhereControlType.相似)
            {
                return " like ";
            }
            if (whereControl == EnumWhereControlType.等于)
            {
                return "=";
            }
            if (whereControl == EnumWhereControlType.不等于)
            {
                return "<>";
            }
            if (whereControl == EnumWhereControlType.包括)
            {
                return "in";
            }
            if (whereControl == EnumWhereControlType.空值)
            {
                return "is null";
            }
            if (whereControl == EnumWhereControlType.不是空值)
            {
                return "is not null";
            }
            if (whereControl == EnumWhereControlType.不相似)
            {
                return " not like ";
            }
            if (whereControl == EnumWhereControlType.表达式)
            {
                return "";
            }
            return "";
        }

        private string ToWhereStringOfDatabase(ListParameter Params, DatabaseType dbType)
        {
            StringBuilder sb = new StringBuilder();
            string identify = DataHandler.GetParamIdentify(dbType);
            switch (this.control)
            {
                case EnumWhereControlType.等于:
                case EnumWhereControlType.大于:
                case EnumWhereControlType.大于等于:
                case EnumWhereControlType.小于:
                case EnumWhereControlType.小于等于:
                case EnumWhereControlType.不等于:
                    string newKey6=Params.GetNewKey(this.field);
                    sb.Append(string.Concat(new string[]{
                        this.field,
                        GetControlString(this.control),
                        DataHandler.GetParamIdentify(newKey6,dbType)
                    }));
                    Params.Add(newKey6, this.value);
                    break;
                case EnumWhereControlType.相似:
                    sb.Append(this.field);
                    sb.Append(" like (");
                    string strValue7 = this.value.ToString().TrimStart();
                    if (strValue7.IndexOf("&@&") == 0)
                    {
                        sb.Append(strValue7.Substring("&@&".Length, strValue7.Length));
                    }
                    else
                    {
                        string newKey = Params.GetNewKey(this.field);
                        sb.Append(DataHandler.GetParamIdentify(newKey,dbType));
                        Params.Add(newKey, strValue7);
                    }
                    sb.Append(")");
                    break;
                case EnumWhereControlType.不相似:
                       sb.Append(this.field);
                    sb.Append(" not like (");
                    string strValue8 = this.value.ToString().TrimStart();
                    if (strValue8.IndexOf("&@&") == 0)
                    {
                        sb.Append(strValue8.Substring("&@&".Length, strValue8.Length));
                    }
                    else
                    {
                        string newKey = Params.GetNewKey(this.field);
                        sb.Append(DataHandler.GetParamIdentify(newKey,dbType));
                        Params.Add(newKey, strValue8);
                    }
                    sb.Append(")");
                    break;
                case EnumWhereControlType.包括:
                    sb.Append(this.field);
                    sb.Append(" in(");
                    break;
                case EnumWhereControlType.不包括:
                    sb.Append(this.field);
                    sb.Append(" not in(");
                    break;
                case EnumWhereControlType.空值:
                    sb.Append(this.field);
                    if (this.value.GetType() != typeof(bool))
                    {
                        this.value = bool.Parse(this.Value.ToString().Trim());
                    }
                    sb.Append(" IS NULL");
                    break;
                case EnumWhereControlType.不是空值:
                    sb.Append(this.field);
                    if (this.value.GetType() != typeof(bool))
                    {
                        this.value = bool.Parse(this.value.ToString().Trim());
                    }
                    sb.Append(" IS NOT NULL");
                    break;
                case EnumWhereControlType.表达式:
                    if (this.field.Trim().Length > 0 || this.value.ToString().Length > 0)
                    {
                        sb.Append(this.field);
                        sb.Append(this.value.ToString());
                    }
                    break;
                default:
                    throw new Exception("This is not support!");
            }

            return sb.ToString();
        }

        public string ToWhereString(ListParameter Params, DatabaseType dbtype)
        {
            if (Params == null)
            {
                Params = new ListParameter();
            }
            return this.ToWhereStringOfDatabase(Params, dbtype);
        }
    }
}
