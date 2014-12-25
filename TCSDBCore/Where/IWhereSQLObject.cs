using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public interface IWhereSQLObject
    {
        string ToWhereString(ListParameter Params, DatabaseType dbtype);
    }
}
