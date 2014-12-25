using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    [DataContract, KnownType(typeof(Dictionary<string, DBNull>)), KnownType(typeof(DBNull))]
    public class EntityData : INotifyPropertyChanged
    {

        private string entitySource;
        [DataMember]
        public string EntitySource
        {
            get { return entitySource; }
            set { entitySource = value; }
        }
        private Dictionary<string, object> values;
        [DataMember]
        public Dictionary<string, object> Values
        {
            get { return values; }
            set { values = value; }
        }
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
        /// 使用索引器
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
                return this.Values[key];
            }
            set
            {
                if (!this.Values.ContainsKey(key))
                {
                    this.Values.Add(key, value);
                    this.NotifyPropertyChanged(key);
                }
                else
                {
                    this.Values[key] = value;
                    this.NotifyPropertyChanged(key);
                }
            }
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fields"></param>
        public void AddFields(params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (!this.Values.ContainsKey(fields[i]))
                {
                    this.Values.Add(fields[i], null);
                }
            }
        }
        /// <summary>
        /// 判断是否相同
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Equals(EntityData data)
        {
            if (data.EntitySource!=this.entitySource)
            {
                return false;
            }
            else
            {
                if (data.Values.Count != this.values.Count)
                {
                    return false;
                }
                else 
                {
                    Dictionary<string, object>.KeyCollection.Enumerator enumerator = this.values.Keys.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (!this.values.ContainsKey(enumerator.Current))
                        {
                            return false;
                        }
                        else
                        {
                            if (this.values[enumerator.Current] != data.Values[enumerator.Current])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        public EntityData()
        {
            this.entitySource = "";
            this.values = new Dictionary<string, object>();
        }
        public EntityData(string entitySource)
        {
            this.entitySource = entitySource;
            this.values = new Dictionary<string, object>();
        }
        #region  获取字段值
        public string GetStringField(string field)
        {
            return DictionaryUtil.GetStringField(this.Values, field);
        }
        public int GetIntField(string field)
        {
            return DictionaryUtil.GetIntField(this.Values, field);
        }
        public int GetBoolField(string field)
        {
            return DictionaryUtil.GetBoolField(this.Values, field) == true ? 1 : 0;
        }
        public decimal GetDecimalField(string field)
        {
            return DictionaryUtil.GetDecimalField(this.Values, field);
        }
        #endregion
        public override string ToString()
        {
            return "未处理：->INFO";
        }
    }
}
