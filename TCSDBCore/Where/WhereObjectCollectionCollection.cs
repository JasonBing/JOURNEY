using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public class WhereObjectCollectionCollection : CollectionBase, IWhereSQLObject
    {
        public WhereObjectCollection this[int Index]
        {
            get
            {
                return (WhereObjectCollection)base.List[Index];
            }
            set
            {
                base.List[Index] = value;
            }
        }
        public int Add(WhereObjectCollection item)
        {
            return base.List.Add(item);
        }
        public void AddRange(WhereObjectCollectionCollection list)
        {
            IEnumerator enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
            {
                WhereObjectCollection item = (WhereObjectCollection)enumerator.Current;
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
                    WhereObjectCollection whereObjectCollection = (WhereObjectCollection)enumerator.Current;
                    string text2 = whereObjectCollection.ToWhereString(Params, dbtype);
                    if (text2.Trim().Length > 0)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(" OR " + text2);
                        }
                        else
                        {
                            sb.Append(text2);
                        }
                    }
                }
               
                if (sb.ToString().Length>0)
                {
                    return "(" + sb.ToString().Trim() + ")";
                }
                return "";
        }
    }
}
