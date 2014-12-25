using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class ScreenItemCollection : CollectionBase
    {
        public ScreenItem this[int index]
        {
            get
            {
                return (ScreenItem)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
        public void Add(ScreenItem item)
        {
            base.List.Add(item);
        }
        public ScreenItem FromKey(string key)
        {
            foreach (ScreenItem screenItem in this)
            {
                if (screenItem.Name == key)
                {
                    return screenItem;
                }
            }
            return null;
        }
        public bool Contains(string key)
        {
            return base.List.Contains(key);
        }
    }
}
