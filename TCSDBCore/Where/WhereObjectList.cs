using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace TCSDBCore
{
    [DataContract, KnownType(typeof(EntityBase)), KnownType(typeof(WhereObject)), KnownType(typeof(WhereObjectLists))]
    public class WhereObjectList:IWhereSQLObject
    {
        [DataMember]
        public readonly List<IWhereSQLObject> Values;
        public int Count()
        {
            return this.Values.Count;
        }
        public WhereObjectList()
        {
            this.Values = new List<IWhereSQLObject>();
        }
        public void Add(IWhereSQLObject item)
        {
            this.Values.Add(item);
        }
        public void Add(string field, EnumWhereControlType control, object value)
        {
            WhereObject whereObject = new WhereObject();
            whereObject.Field = field;
            whereObject.Control = control;
            whereObject.Value = value;
            this.Add(whereObject);
        }
        public void Clear()
        {
            this.Values.Clear();
        }
        public string ToWhereString(ListParameter Params, DatabaseType dbtype)
        {
            StringBuilder sb = new StringBuilder();
            if (Params == null)
            {
                Params = new ListParameter();
            }
            List<IWhereSQLObject>.Enumerator enumerator = this.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IWhereSQLObject current = enumerator.Current;
                string text = current.ToWhereString(Params, dbtype);
                if (text.Trim().Length > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" And ");
                    }
                    sb.Append(text);
                }
            }

            if (sb.ToString().Length > 0)
            {
                return "(" + sb.ToString() + ")";
            }
            return "";
        }
    }
}
