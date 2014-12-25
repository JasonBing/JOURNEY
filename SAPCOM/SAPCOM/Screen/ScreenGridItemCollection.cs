using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SAPCOM
{
    public class ScreenGridItemCollection : CollectionBase
    {
        public ScreenGridItem this[int index]
        {
            get
            {
                return (ScreenGridItem)base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
        public int Add(ScreenGridItem item)
        {
            return base.List.Add(item);
        }
        public void AddRange(ScreenGridItem[] items)
        {
            if (items != null && items.Length > 0)
            {
                ScreenGridItem[] array = items;
                for (int i = 0; i < array.Length; i++)
                {
                    ScreenGridItem item = array[i];
                    this.Add(item);
                }
            }
        }
    }
}
