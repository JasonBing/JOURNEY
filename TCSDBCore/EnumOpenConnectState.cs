using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 数据库连接枚举
    /// </summary>
    public enum EnumOpenConnectState
    {
        NeedClose = 0,
        NoNeedClose = 1,
    }
}
