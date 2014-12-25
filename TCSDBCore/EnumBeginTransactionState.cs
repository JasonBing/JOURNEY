using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    /// <summary>
    /// 事务枚举
    /// </summary>
    public enum EnumBeginTransactionState
    {
        NeedCommit = 0,
        NoNeedCommit = 1,
    }
}
