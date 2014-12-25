using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    public class EntitySchema
    {
        #region 构造函数
        public EntitySchema(string source)
        {
            this.entiySource = source;
            this.connector = SystemSettings.DataConnector;
            fieldProperties = FieldPropertyCollection.LoadFrom(this.entiySource, this.connector);
        }
        public EntitySchema(string source, DataBaseConnector connector)
        {
            this.entiySource = source;
            this.connector = connector;
        }
        #endregion
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DataBaseConnector connector = null;
        private string entiySource;
        /// <summary>
        /// 获取和设置数据源
        /// </summary>
        [DataMember]
        public string EntiySource
        {
            get { return entiySource; }
        }
        private FieldPropertyCollection fieldProperties;
        /// <summary>
        /// 获取字段集合
        /// </summary>
        [DataMember]
        public FieldPropertyCollection FieldProperties
        {
            get
            {
                if (fieldProperties == null || fieldProperties.Keys.Count <= 0)
                {
                    //获取字段集合
                    if (connector != null)
                    {
                        fieldProperties = FieldPropertyCollection.LoadFrom(this.entiySource, this.connector);
                    }
                    else
                    {
                        fieldProperties = new FieldPropertyCollection();
                    }
                }
                return fieldProperties;
            }
        }


        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("EntitySource=" + this.entiySource);
            stringBuilder.AppendLine(this.fieldProperties.ToString());
            return stringBuilder.ToString();
        }
    }
}
