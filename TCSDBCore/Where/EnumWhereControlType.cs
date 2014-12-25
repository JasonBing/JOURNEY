using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public enum EnumWhereControlType
    {
        等于=0,
        大于=1,
        大于等于=2,
        小于=3,
        小于等于=4,
        不等于=5,
        相似=6,
        包括=7,
        不包括=8,
        空值=9,
        不是空值=10,
        不相似=11,
        表达式 = 99
    }
}
