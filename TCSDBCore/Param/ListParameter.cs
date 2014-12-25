using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 参数对象集合
    /// </summary>
    [DataContract]
    public class ListParameter
    {
        /// <summary>
        /// 存储参数对象
        /// </summary>
        [DataMember]
        public Dictionary<string, ObjectParameter> ParamsList;
        /// <summary>
        /// 获取参数对象
        /// </summary>
        /// <param name="key">参数键值</param>
        /// <returns>参数对象</returns>
        public ObjectParameter this[string key]
        {
            get
            {
                if (ParamsList == null)
                {
                    return null;
                }
                else
                {
                    if (ParamsList.ContainsKey(key) == true)
                    {
                        return ParamsList[key];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        /// <summary>
        /// 构造函数 初始化ParamsList
        /// </summary>
        public ListParameter()
        {
            this.ParamsList = new Dictionary<string, ObjectParameter>();
        }
        /// <summary>
        /// 判断是否包含
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return this.ParamsList.ContainsKey(key);
        }
        /// <summary>
        /// 获取Keys
        /// </summary>
        public ICollection Keys
        {
            get
            {
                return this.ParamsList.Keys;
            }
        }
        /// <summary>
        /// 获取值
        /// </summary>
        public ICollection Values
        {
            get
            {
                return this.ParamsList.Values;
            }
        }
        /// <summary>
        /// 获取参数个数
        /// </summary>
        public int Count
        {
            get
            {
                return this.Values.Count;
            }
        }
        /// <summary>
        /// 增加参数
        /// </summary>
        /// <param name="item">参数对象ObjectParameter</param>
        public void Add(ObjectParameter item)
        {
            this.ParamsList.Add(item.paramKey, item);
        }
        /// <summary>
        /// 增减参数并返回参数对象
        /// </summary>
        /// <param name="key">参数键</param>
        /// <param name="value">参数值</param>
        /// <returns>ObjectParameter</returns>
        public ObjectParameter Add(string key, object value)
        {
            ObjectParameter objectParameter = new ObjectParameter(key, value);
            this.Add(objectParameter);
            return objectParameter;
        }
        /// <summary>
        /// 添加参数对象
        /// </summary>
        /// <param name="params">参数对象集合ListParameter</param>
        public void AddRange(ListParameter @params)
        {
            if (@params != null)
            {

                IEnumerator enumerator = @params.Keys.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    string key = Convert.ToString(enumerator.Current);
                    this.Add(@params[key]);
                }

            }
        }
        /// <summary>
        /// 获取处理Key值（过滤key中的特殊字符）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal string GetNewKey(string key)
        {
            string text = "";
            string text2 = key;
            int i = 0;
            int length = text2.Length;
            while (i < length)
            {
                char c = text2[i];
                //IsSymbol字符符号 IsSeparator分割符号 IsPunctuation标点符号
                if (!(char.IsSymbol(c) | char.IsSeparator(c) | char.IsPunctuation(c)))
                {
                    text += Convert.ToString(c);
                }
                i++;
            }
            int num = 1;
            key = text;
            while (this.Contains(text))
            {
                text = key + num.ToString();
                num++;
            }
            return text;
        }
    }
}
