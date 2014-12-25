using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class ScreenItem
    {
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description = "";

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private object itemValue;

        public object ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }
        public ScreenItem()
        {
        }
        public ScreenItem(string functionname, string functiondesc)
        {
            this.Name = functionname;
            this.Description = functiondesc;
        }
        //public ScreenItem(string functionname, string functiondesc, ScreenGridItemCollection items)
        //{
        //    this.Name = functionname;
        //    this.Description = functiondesc;
        //    this.ItemValue = items;
        //}
    }
}
