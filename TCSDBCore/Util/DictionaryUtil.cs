using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public class DictionaryUtil
    {
        public static string GetStringField(Dictionary<string, object> dict, string field)
        {
            if (dict.ContainsKey(field))
            {
                if (dict[field] != null)
                {
                    if (!dict[field].Equals(DBNull.Value))
                    {
                        return dict[field].ToString();
                    }
                }
            }
            return "";
        }
        public static int GetIntField(Dictionary<string, object> dict, string field)
        {
            if (dict.ContainsKey(field))
            {
                if (dict[field] != null)
                {
                    if (!dict[field].Equals(DBNull.Value))
                    {
                        object objectValue = dict[field];
                        if (objectValue.GetType() == typeof(int) || objectValue.GetType() == typeof(decimal))
                        {
                            return Convert.ToInt32(objectValue);
                        }
                        string text = dict[field].ToString().Trim();
                        if (text.Trim().Length > 0)
                        {
                            return int.Parse(text);
                        }
                        return 0;
                    }
                }
            }
            return 0;
        }
        public static decimal GetDecimalField(Dictionary<string, object> dict, string field)
        {
            if (dict.ContainsKey(field))
            {
                if (dict[field] != null)
                {
                    if (!dict[field].Equals(DBNull.Value))
                    {
                        object objectValue = dict[field];
                        if (objectValue.GetType() == typeof(int) || objectValue.GetType() == typeof(decimal))
                        {
                            return Convert.ToDecimal(objectValue);
                        }
                        string text = dict[field].ToString().Trim();
                        if (text.Trim().Length > 0)
                        {
                            return decimal.Parse(text);
                        }
                        return decimal.Zero;
                    }
                }
            }
            return decimal.Zero;
        }
        public static bool GetBoolField(Dictionary<string, object> dict, string field)
        {
            if (dict.ContainsKey(field))
            {
                if (dict[field] != null)
                {
                    if (!dict[field].Equals(DBNull.Value))
                    {
                        object objectValue = dict[field];
                        if (objectValue.GetType() == typeof(bool))
                        {
                            return Convert.ToBoolean(objectValue);
                        }
                        if (objectValue.Equals(1))
                        {
                            return true;
                        }
                        if (objectValue.Equals(0))
                        {
                            return false;
                        }
                        string text = dict[field].ToString().Trim();
                        return text.Length > 0 && bool.Parse(text);
                    }
                }
            }
            return false;
        }
        public static object GetField(Dictionary<string, object> dict, string field)
        {
            if (dict.ContainsKey(field))
            {
                return dict[field];
            }
            return null;
        }
        public static void SetField(Dictionary<string, object> dict, string field, object obj)
        {
            if (!dict.ContainsKey(field))
            {
                dict.Add(field,obj);
            }
            else
            {
                dict[field] = obj;
            }
        }
    }
}
