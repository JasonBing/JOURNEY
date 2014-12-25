using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public abstract class SqlObject
    {
        public ListParameter Params;
        protected SqlObject()
        {
            this.Params = new ListParameter();
        }
        public abstract string ToSql();
    }
}
