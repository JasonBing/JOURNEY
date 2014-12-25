using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class ScreenGridItem
    {
        private int rowNum;

        public int RowNum
        {
            get { return rowNum; }
            set { rowNum = value; }
        }
        private string rowValue;

        public string RowValue
        {
            get { return rowValue; }
            set { rowValue = value; }
        }
        public ScreenGridItem()
        {
            this.rowValue = "";
        }
        public ScreenGridItem(int num, string val)
        {
            this.rowNum = num;
            this.rowValue = val;
        }
    }
}
