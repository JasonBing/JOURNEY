using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public class TotalObjectList : CollectionBase
    {
        public TotalObject this[int index]
        {
            get
            {
                return (TotalObject)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
        public int Add(string fieldname, EnumTotalFunction totalfunction, string aliasname, string Caption, EnumWhereControlType HavingControl, object HavingValue)
        {
            return this.Add(new TotalObject
            {
                AliasName = aliasname,
                FieldName = fieldname,
                TotalFunction = totalfunction,
                Caption = Caption,
                HavingControl = HavingControl,
                HavingValue = HavingValue
            });
        }
        public int Add(string fieldname, EnumTotalFunction totalfunction, string aliasname)
        {
            return this.Add(fieldname, totalfunction, aliasname, "", EnumWhereControlType.等于, null);
        }
        public int Add(string fieldname, EnumTotalFunction totalfunction, string aliasname, string Caption)
        {
            return this.Add(fieldname, totalfunction, aliasname, Caption, EnumWhereControlType.等于, null);
        }
        public int Add(TotalObject value)
        {
            return base.List.Add(value);
        }
        internal WhereObjectCollection whereCollection(DatabaseType type)
        {
            WhereObjectCollection whereObjColl = new WhereObjectCollection();
            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                TotalObject totalObject = (TotalObject)enumerator.Current;
                if (totalObject.HavingValue != null)
                {
                    whereObjColl.Add(totalObject.ToSelectSql(false, type), totalObject.HavingControl, totalObject.HavingValue);
                }
            }
            return whereObjColl;
        }
        internal string totalSql(DatabaseType type, string tableAlias)
        {
            StringBuilder strBuilder = new StringBuilder();
            IEnumerator enumerator = this.GetEnumerator();
            bool flag = true;
            while (enumerator.MoveNext())
            {
                TotalObject totalObject = (TotalObject)enumerator.Current;
                if (!flag)
                {
                    strBuilder.Append(",");
                }
                totalObject.TableAlias = tableAlias;
                strBuilder.Append(totalObject.ToSelectSql(true, type));
                flag = false;
            }
            return strBuilder.ToString();
        }
        internal string totalSql()
        {
            StringBuilder sb = new StringBuilder();
            IEnumerator enumerator = this.GetEnumerator();
            bool flag = true;
            while (enumerator.MoveNext())
            {
                TotalObject totalObject = (TotalObject)enumerator.Current;
                if (totalObject.FuncntionString.Length > 0)
                {
                    if (!flag)
                    {
                        sb.Append(",");
                    }
                    string fieldName = totalObject.FieldName;
                    sb.Append(fieldName);
                    flag = false;
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 表达式
        /// </summary>
        /// <param name="express"></param>
        public void AddExpress(string express)
        {
            string[] array = express.Split(',');
            string[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                string text = array2[i];
                if (text.Trim().Length > 0)
                {
                    string text2 = "";
                    int num = text.ToUpper().IndexOf(" AS ");
                    if (num > 0)
                    {
                        text2 = text.Substring(num + 4).Trim();
                        text = text.Substring(0, num);
                    }
                    int num2 = text2.IndexOf("{");
                    int num3 = text2.IndexOf("}");
                    if (num2 > 0 && num3 > 0 && num3 > num2 + 1)
                    {
                        string expressValue = text2.Substring(num2 + 1, num3 - num2 - 1);
                        text2 = text2.Substring(0, num2);
                        this.Add(text, EnumTotalFunction.Express, text2, "", EnumWhereControlType.表达式, expressValue);
                    }
                    else
                    {
                        this.Add(text, EnumTotalFunction.Express, text2);
                    }
                }
            }
        }
        /// <summary>
        /// 通过totalFunciton获取对应的EnumTotalFunction
        /// </summary>
        /// <param name="totalFunction"></param>
        /// <returns></returns>
        private EnumTotalFunction GetEnumTotalFunction(string totalFunction)
        {
            EnumTotalFunction result = EnumTotalFunction.Avg;
            switch (totalFunction.ToUpper().Trim())
            {
                case "GROUP":
                    result = EnumTotalFunction.Group;
                    break;
                case "SUM":
                    result = EnumTotalFunction.Sum;
                    break;
                case "COUNT":
                    result = EnumTotalFunction.Count;
                    break;
                case "MAX":
                    result = EnumTotalFunction.Max;
                    break;
                case "MIN":
                    result = EnumTotalFunction.Min;
                    break;
                case "AVG":
                    result = EnumTotalFunction.Avg;
                    break;
                case "STDEV":
                    result = EnumTotalFunction.Stdev;
                    break;
                case "STDEVP":
                    result = EnumTotalFunction.Stdevp;
                    break;
                case "VAR":
                    result = EnumTotalFunction.Var;
                    break;
                case "VARP":
                    result = EnumTotalFunction.Varp;
                    break;
                case "EXPRESS":
                    result = EnumTotalFunction.Express;
                    break;
                default:
                    result = EnumTotalFunction.Avg;
                    break;
            }
            return result;
        }
    }
}
