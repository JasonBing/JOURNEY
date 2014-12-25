using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    public class FieldPropertyCollection
    {
        #region 属性
        [DataMember]
        public FieldProperty this[string key]
        {
            get
            {
                return this.items[key];
            }
        }
        private Dictionary<string, FieldProperty> items;

        [DataMember]
        public Dictionary<string, FieldProperty> Items
        {
            get
            {
                return this.items;
            }
            private set
            {
                this.items = value;
            }
        }
        private List<string> primaryKeys;

        [DataMember]
        public List<string> PrimaryKeys
        {
            get
            {
                return this.primaryKeys;
            }
            private set
            {
                this.primaryKeys = value;
            }
        }
        private List<string> autoIncrementKeys;

        [DataMember]
        public List<string> AutoIncrementKeys
        {
            get
            {
                return this.autoIncrementKeys;
            }
            private set
            {
                this.autoIncrementKeys = value;
            }
        }
        private List<string> keys;

        [DataMember]
        public List<string> Keys
        {
            get
            {
                return this.keys;
            }
            private set
            {
                this.keys = value;
            }
        }
        #endregion
        public FieldPropertyCollection()
        {
            this.items = new Dictionary<string, FieldProperty>();
            this.primaryKeys = new List<string>();
            this.autoIncrementKeys = new List<string>();
            this.keys = new List<string>();
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="field">字段FieldProperty</param>
        public void Add(FieldProperty field)
        {
            this.items.Add(field.Name, field);
            this.keys.Add(field.Name);
            if (field.PrimaryKey == true)
            {
                this.primaryKeys.Add(field.Name);
            }
            if (field.AtuoIncrement == true)
            {
                this.autoIncrementKeys.Add(field.Name);
            }
        }
        public void Add(string field, FieldDataType dataType, int length, bool isPrimary, bool isAuto)
        {
            this.Add(new FieldProperty
            {
                Name = field,
                DataType = dataType,
                Length = length,
                PrimaryKey = isPrimary,
                AtuoIncrement = isAuto
            });
        }
        public void Add(string field, FieldDataType dataType, int length)
        {
            this.Add(field, dataType, length, false, false);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">字段名</param>
        public void Remove(string key)
        {
            FieldProperty field = this[key];
            if (field != null)
            {
                if (field.PrimaryKey == true)
                {
                    this.primaryKeys.Remove(field.Name);
                }
                if (field.AtuoIncrement == true)
                {
                    this.autoIncrementKeys.Remove(field.Name);
                }
                this.items.Remove(field.Name);
                this.keys.Remove(field.Name);
            }
        }
        /// <summary>
        /// 判断是否包含
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>bool</returns>
        public bool Contains(string key)
        {
            return this.keys.Contains(key);
        }
        /// <summary>
        /// 加载字段集合
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cntor"></param>
        /// <returns></returns>
        public static FieldPropertyCollection LoadFrom(string name, DataBaseConnector cntor)
        {

            List<sysFieldEntity> fields = cntor.GetFields(name);
            FieldPropertyCollection fieldPropertyCollection = new FieldPropertyCollection();
            List<sysFieldEntity>.Enumerator enumerator = fields.GetEnumerator();
            while (enumerator.MoveNext())
            {
                sysFieldEntity current = enumerator.Current;
                FieldProperty fieldProperty = new FieldProperty();
                fieldProperty.CopyDataFrom(current);
                fieldPropertyCollection.Add(fieldProperty);
            }

            return fieldPropertyCollection;

        }
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            Dictionary<string, FieldProperty>.KeyCollection.Enumerator enumerator = this.Items.Keys.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string current = enumerator.Current;
                stringBuilder.AppendLine(this.Items[current].ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
