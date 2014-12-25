using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 参数标识
    /// </summary>
    public static class Indicator
    {
        public static string Get(DatabaseType dtType)
        {
            switch (dtType)
            {
                case DatabaseType.SQLSERVER:
                    return "@";
                case DatabaseType.ORACLE:
                    return null;
                case DatabaseType.MYSQL:
                    return null;
                case DatabaseType.SQLITE:
                    return null;
                default:
                    throw new Exception("Does not support this type of database!");
            }
        }
    }
}
