using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
namespace TCSDBCore
{
    public class FieldProperty
    {
        #region 属性
        private string name;
        /// <summary>
        /// 获取和设置字段名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int length;
        /// <summary>
        /// 获取和设置字段长度
        /// </summary>
        public int Length
        {
            get { return length; }
            set { length = value; }
        }
        private int dec;
        /// <summary>
        /// 获取和设置小数位数
        /// </summary>
        public int Dec
        {
            get { return dec; }
            set { dec = value; }
        }
        private FieldDataType dataType;
        /// <summary>
        /// 获取和设置字段的数据类型
        /// </summary>
        public FieldDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }
        private bool primaryKey;
        /// <summary>
        /// 获取和设置字段是否为主键
        /// </summary>
        public bool PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        private bool atuoIncrement;
        /// <summary>
        /// 获取和设置字段是否为自增
        /// </summary>
        public bool AtuoIncrement
        {
            get { return atuoIncrement; }
            set { atuoIncrement = value; }
        }
        #endregion
        /// <summary>
        /// 构造函数初始化对象的属性
        /// </summary>
        public FieldProperty()
        {
            this.name = "";
            this.dataType = FieldDataType.StringType;
            this.length = 0;
            this.dec = 0;
            this.primaryKey = false;
            this.atuoIncrement = false;
           
        }
        public FieldProperty(SerializationInfo info,StreamingContext context)
        {
            this.name = info.GetString("_field");
            this.dataType = (FieldDataType)info.GetInt16("_type");
            this.length = info.GetInt32("_len");
            this.primaryKey = info.GetBoolean("_pkey");
            this.atuoIncrement = info.GetBoolean("_auto");
        }
        /// <summary>
        /// 获取字段的类型FieldDataType-->Type
        /// </summary>
        /// <returns></returns>
        public Type GetFieldType()
        {
            switch (this.dataType)
            {
                case FieldDataType.StringType:
                    return typeof(string);
                case FieldDataType.IntType:
                    return typeof(int);
                case FieldDataType.BoolType:
                    return typeof(bool);
                case FieldDataType.DateTimeType:
                    return typeof(DateTime);
                case FieldDataType.DecimalType:
                    return typeof(decimal);
                case FieldDataType.BinaryType:
                    return typeof(byte[]);
                case FieldDataType.TextType:
                    return typeof(string);
                default:
                    return typeof(object);
            }
        }
        /// <summary>
        /// 获取字段类型Type-->FieldDataType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FieldDataType FromType(Type type)
        {
            if (type == typeof(string) || type == typeof(char))
            {
                return FieldDataType.StringType;
            }
            if (type != typeof(int))
            {
                if (type != typeof(short))
                {
                    if (type != typeof(int))
                    {
                        if (type != typeof(long))
                        {
                            if (type == typeof(bool))
                            {
                                return FieldDataType.BoolType;
                            }
                            if (type == typeof(decimal) || type == typeof(double))
                            {
                                return FieldDataType.DecimalType;
                            }
                            if (type == typeof(DateTime) || type == typeof(DateTime))
                            {
                                return FieldDataType.DateTimeType;
                            }
                            return FieldDataType.StringType;
                        }
                    }
                }
            }
            return FieldDataType.IntType;
        }
        public void CopyDataFrom(sysFieldEntity field)
        {
            this.name = field.FLD_NAME;
            this.length = field.FLD_LENGTH;
            this.dec = field.FLD_DEC;
            this.dataType = (FieldDataType)field.FLD_DATATYPE;
            this.atuoIncrement = field.FLD_AUTOINCREMENT;
            this.primaryKey = field.FLD_PRIMARYKEY;
        }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Field=" + this.name + ",");
            stringBuilder.Append("DataType=" + this.dataType + ",");
            stringBuilder.Append("Length=" + this.length.ToString() + ",");
            stringBuilder.Append("AutoIncrement=" + this.atuoIncrement.ToString() + ",");
            stringBuilder.Append("PrimaryKey=" + this.primaryKey.ToString());
            return stringBuilder.ToString();
        }
    }
}
