using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 参数对象
    /// </summary>
    [DataContract]
    public class ObjectParameter
    {
        /// <summary>
        /// 参数键
        /// </summary>
        [DataMember]
        public string paramKey;
        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember]
        public object paramValue;
        /// <summary>
        /// 参数类型
        /// </summary>
        [DataMember]
        public ParameterDirection paramDirection;
        /// <summary>
        /// 参数长度
        /// </summary>
        [DataMember]
        public int paramLength;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        public ObjectParameter(string key, object value)
        {
            this.paramDirection = ParameterDirection.Input;
            this.paramLength = 0;
            this.paramKey = key;
            this.paramValue = RuntimeHelpers.GetObjectValue(value);
        }
    }
}
