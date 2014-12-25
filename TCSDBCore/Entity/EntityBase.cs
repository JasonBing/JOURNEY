using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TCSDBCore
{
    public class EntityBase : ICloneable, INotifyPropertyChanged, IWhereSQLObject
    {
        #region 字段和属性
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                if (!this.Values.ContainsKey(key))
                {
                    return null;
                }
                else
                {
                    //如果字段是bool类型，并且值为1的时候，返回true
                    if (this.Fields.Contains(key) && this.Fields[key].DataType == FieldDataType.BoolType && this.Values[key] != null && !DBNull.Value.Equals(this.Values[key]) && this.Values[key].ToString().Equals("1"))
                    {
                        return true;
                    }
                    return this.Values[key];
                }
            }
            set
            {

                if (this.Fields.Contains(key))
                {
                    if (!this.Values.ContainsKey(key))
                    {
                        this.Values.Add(key, value);
                        this.NotifyPropertyChanged(key);
                    }
                    else
                    {

                        if (this.Values[key] == value)
                        {
                            this.Values[key] = value;
                            this.NotifyPropertyChanged(key);
                        }

                    }
                    return;
                }
                throw new Exception(key + " is not exist in " + this.entitySource + "!");
            }
        }
        public List<string> PrimaryKeys
        {
            get
            {
                return this.Fields.PrimaryKeys;
            }
        }
        public List<string> AutoIncrementKeys
        {
            get
            {
                return this.Fields.AutoIncrementKeys;
            }
        }
        public List<string> FieldKeys
        {
            get
            {
                return this.Fields.Keys;
            }
        }
        private string entitySource;
        public string EntitySource
        {
            get { return entitySource; }
        }
        /// <summary>
        /// 获取字段集合
        /// </summary>
        public FieldPropertyCollection Fields
        {
            get { return this.schema.FieldProperties; }
        }
        private EntitySchema schema;
        public EntitySchema Schema
        {
            get { return schema; }
        }
        private EntityData data;
        /// <summary>
        /// 获取对应的EntityData
        /// </summary>
        public EntityData Data
        {
            get { return data; }
        }

        public Dictionary<string, object> Values
        {
            get { return this.data.Values; }
        }
        #endregion
        #region 构造函数
        public EntityBase(string entiySource)
        {
            this.entitySource = entiySource;
            this.schema = new EntitySchema(entiySource);
            this.data = new EntityData(entiySource);
        }
        #endregion
        /// <summary>
        /// 属性更改事件
        /// </summary>
        private PropertyChangedEventHandler PropertyChangedEvent;
        public event PropertyChangedEventHandler PropertyChanged
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            add
            {
                this.PropertyChangedEvent = (PropertyChangedEventHandler)Delegate.Combine(this.PropertyChangedEvent, value);
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            remove
            {
                this.PropertyChangedEvent = (PropertyChangedEventHandler)Delegate.Remove(this.PropertyChangedEvent, value);
            }
        }
        private void NotifyPropertyChanged(string info)
        {
            PropertyChangedEventHandler propertyChangedEvent = this.PropertyChangedEvent;
            if (propertyChangedEvent != null)
            {
                propertyChangedEvent(this, new PropertyChangedEventArgs(info));
            }
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this.Fields.Contains(key);
        }
        public string GetStringField(string field)
        {
            if (this.Fields.Contains(field))
            {
                return this.Data.GetStringField(field);
            }
            throw new Exception(field + " is not exist in entity(" + this.entitySource + ")!");
        }
        public long GetIntField(string field)
        {
            if (this.Fields.Contains(field))
            {
                return (long)this.Data.GetIntField(field);
            }
            throw new Exception(field + " is not exist in entity(" + this.entitySource + ")!");
        }
        public decimal GetDecimalField(string field)
        {
            if (this.Fields.Contains(field))
            {
                return this.Data.GetDecimalField(field);
            }
            throw new Exception(field + " is not exist in entity(" + this.entitySource + ")!");
        }
        public bool GetBoolField(string field)
        {
            if (this.Fields.Contains(field))
            {
                return this.Data.GetBoolField(field) > 0;
            }
            throw new Exception(field + " is not exist in entity(" + this.entitySource + ")!");
        }
        public void SetNull()
        {
            SetNull(true);
        }
        public void SetNull(bool isClearPKeys)
        {
            if (isClearPKeys == false)
            {
                Dictionary<string, object>.KeyCollection.Enumerator enumerator = this.Values.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (this.PrimaryKeys.Contains(enumerator.Current) == false)
                    {
                        this.Values.Remove(enumerator.Current);
                    }
                }
            }
            else
            {
                this.Values.Clear();
            }
        }
        /// <summary>
        /// 创建浅副本（实现接口ICloneable）
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public string ToWhereString(ListParameter Params, DatabaseType dbtype)
        {
            StringBuilder sb = new StringBuilder();
            if (Params == null)
            {
                Params = new ListParameter();
            }
            List<string>.Enumerator enumerator = this.FieldKeys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                string newKey = Params.GetNewKey(current);
                object objectValue = this[current];
                if (objectValue != null)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" And ");
                    }
                    WhereObject whereObject;
                    if (objectValue.Equals(DBNull.Value))
                    {
                        whereObject = new WhereObject(current, EnumWhereControlType.空值, true);
                    }
                    else
                    {
                        whereObject = new WhereObject(current, EnumWhereControlType.等于, objectValue);
                    }
                    sb.Append(whereObject.ToWhereString(Params, dbtype));
                }
            }
            return sb.ToString();
        }
        internal string ToWhereString(ListParameter Params, DatabaseType dbtype, bool onlyPrimarykey)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (onlyPrimarykey)
            {
                if (Params == null)
                {
                    Params = new ListParameter();
                }
                if (this.PrimaryKeys.Count <= 0)
                {
                    throw new Exception("Primary key is not exist in " + this.EntitySource.ToString());
                }

                List<string>.Enumerator enumerator = this.PrimaryKeys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string current = enumerator.Current;
                    object objectValue = this[current];
                    if (objectValue == null)
                    {
                        throw new Exception("Primary field (" + current + ") has not yet been assigned!");
                    }
                    if (stringBuilder.ToString().Trim().Length > 0)
                    {
                        stringBuilder.Append(" And ");
                    }
                    WhereObject whereObject = new WhereObject(current, EnumWhereControlType.等于, RuntimeHelpers.GetObjectValue(objectValue));
                    stringBuilder.Append(whereObject.ToWhereString(Params, dbtype));
                }
            }
            else
            {
                stringBuilder.Append(this.ToWhereString(Params, dbtype));
            }
            return stringBuilder.ToString();
        }
    }
}
