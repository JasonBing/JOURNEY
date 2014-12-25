using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    [Serializable]
    public class WhereObjectCollection : CollectionBase, IWhereSQLObject
    {
        public IWhereSQLObject this[int Index]
        {
            get
            {
                return (IWhereSQLObject)base.List[Index];
            }
            set
            {
                base.List[Index] = value;
            }
        }
        public int Add(IWhereSQLObject item)
        {
            return base.List.Add(item);
        }
        public int Add(string field, EnumWhereControlType control, object value)
        {
            WhereObject whereObject = new WhereObject();
            whereObject.Field = field;
            whereObject.Control = control;
            whereObject.Value = value;
            return this.List.Add(whereObject);
        }
        public void AddRange(WhereObjectCollection list)
        {
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IWhereSQLObject item = (IWhereSQLObject)enumerator.Current;
                this.Add(item);
            }
        }
        public string ToWhereString(ListParameter Params, DatabaseType dbtype)
        {
            StringBuilder sb = new StringBuilder();
            if (Params == null)
            {
                Params = new ListParameter();
            }

            IEnumerator enumerator = this.GetEnumerator();
            while (enumerator.MoveNext())
            {
                IWhereSQLObject whereSQLObject = (IWhereSQLObject)enumerator.Current;
                string text = whereSQLObject.ToWhereString(Params, dbtype);
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
