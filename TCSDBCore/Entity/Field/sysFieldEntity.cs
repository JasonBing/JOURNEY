using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCSDBCore
{
    public class sysFieldEntity
    {
        private Dictionary<string, object> fieldsDict = null;
        public sysFieldEntity()
        {
            this.fieldsDict=new Dictionary<string,object>();
            //this["FLD_DATASQLTYPE"]=SysGlobals.DataType();
        }
        /// <summary>
        /// 使用索引器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get {
                return fieldsDict[key];
            }
            set {
                fieldsDict[key] = value;
            }
        }
        /// <summary>
        /// 获取和设置表或试图名
        /// </summary>
        public string TABLEVIEW_NAME
        {
            get
            {
                if (this["TABLEVIEW_NAME"] == null)
                {
                    return "";
                }
                return Convert.ToString(this["TABLEVIEW_NAME"]);
            }
            set
            {
                this["TABLEVIEW_NAME"] = value;
            }
        }
        /// <summary>
        ///获取和设置字段名
        /// </summary>
        public string FLD_NAME
        {
            get
            {
                if (this["FLD_NAME"] == null)
                {
                    return "";
                }
                return Convert.ToString(this["FLD_NAME"]);
            }
            set
            {
                this["FLD_NAME"] = value;
            }
        }
        /// <summary>
        /// 获取和设置字段位置
        /// </summary>
        public int FLD_POS
        {
            get
            {
                if (this["FLD_POS"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(this["FLD_POS"]);
            }
            set
            {
                this["FLD_POS"] = value;
            }
        }
        /// <summary>
        /// 获取和设置是否为关键字段
        /// </summary>
        public bool FLD_PRIMARYKEY
        {
            get
            {
                return this["FLD_PRIMARYKEY"] != null && Convert.ToBoolean(this["FLD_PRIMARYKEY"]);
            }
            set
            {
                this["FLD_PRIMARYKEY"] = value;
            }
        }
        /// <summary>
        /// 获取和设置是否为自增字段
        /// </summary>
        public bool FLD_AUTOINCREMENT
        {
            get
            {
                return this["FLD_AUTOINCREMENT"] != null && Convert.ToBoolean(this["FLD_AUTOINCREMENT"]);
            }
            set
            {
                this["FLD_AUTOINCREMENT"] = value;
            }
        }
        /// <summary>
        /// 获取和设置字段类型
        /// </summary>
        public int FLD_DATATYPE
        {
            get
            {
                if (this["FLD_DATATYPE"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(this["FLD_DATATYPE"]);
            }
            set
            {
                this["FLD_DATATYPE"] = value;
            }
        }
        /// <summary>
        /// 获取和设置字段长度
        /// </summary>
        public int FLD_LENGTH
        {
            get
            {
                if (this["FLD_LENGTH"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(this["FLD_LENGTH"]);
            }
            set
            {
                this["FLD_LENGTH"] = value;
            }
        }
        /// <summary>
        /// 获取设置字段小数位
        /// </summary>
        public int FLD_DEC
        {
            get
            {
                if (this["FLD_DEC"] == null)
                {
                    return 0;
                }
                return Convert.ToInt32(this["FLD_DEC"]);
            }
            set
            {
                this["FLD_DEC"] = value;
            }
        }
        /// <summary>
        /// 获取和设置字段描述
        /// </summary>
        public string FLD_DESCRIPTION
        {
            get
            {
                if (this["FLD_DESCRIPTION"] == null)
                {
                    return "";
                }
                return Convert.ToString(this["FLD_DESCRIPTION"]);
            }
            set
            {
                this["FLD_DESCRIPTION"] = value;
            }
        }
        /// <summary>
        /// 获取和设置数据库类型
        /// </summary>
        public string FLD_DATASQLTYPE
        {
            get
            {
                if (this["FLD_DATASQLTYPE"] == null)
                {
                    return "";
                }
                return Convert.ToString(this["FLD_DATASQLTYPE"]);
            }
            set
            {
                this["FLD_DATASQLTYPE"] = value;
            }
        }
        /// <summary>
        /// 获取和设置是否允许为空值
        /// </summary>
        public bool FLD_NULLABLE
        {
            get
            {
                return this["FLD_NULLABLE"] != null && Convert.ToBoolean(this["FLD_NULLABLE"]);
            }
            set
            {
                this["FLD_NULLABLE"] = value;
            }
        }
    }
}
