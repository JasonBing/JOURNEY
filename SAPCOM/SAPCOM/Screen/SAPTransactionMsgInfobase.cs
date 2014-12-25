using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    /// <summary>
    /// BDCDATA
    /// </summary>
    public class SAPTransactionMsgInfobase
    {
        private string tCode;
        /// <summary>
        /// 事务码
        /// </summary>
        public string TCode
        {
            get { return tCode; }
            set { tCode = value; }
        }
        private string dyName;
        /// <summary>
        /// 批输入模块名称
        /// </summary>
        public string DyName
        {
            get { return dyName; }
            set { dyName = value; }
        }
        private string dyNum;
        /// <summary>
        /// 批输入屏幕数字
        /// </summary>
        public string DyNum
        {
            get { return dyNum; }
            set { dyNum = value; }
        }
        private string msgTyp;
        /// <summary>
        /// 批输入信息类型
        /// </summary>
        public string MsgTyp
        {
            get { return msgTyp; }
            set { msgTyp = value; }
        }
        private string msgSpra;
        /// <summary>
        /// 报文语言 ID
        /// </summary>
        public string MsgSpra
        {
            get { return msgSpra; }
            set { msgSpra = value; }
        }
        private string msgId;
        /// <summary>
        /// 批输入信息ID
        /// </summary>
        public string MsgId
        {
            get { return msgId; }
            set { msgId = value; }
        }
        private string msgNr;
        /// <summary>
        /// 批输入信息数量
        /// </summary>
        public string MsgNr
        {
            get { return msgNr; }
            set { msgNr = value; }
        }
        private string msgV1;
        /// <summary>
        /// 信息1
        /// </summary>
        public string MsgV1
        {
            get { return msgV1; }
            set { msgV1 = value; }
        }
        private string msgV2;
        /// <summary>
        /// 信息2
        /// </summary>
        public string MsgV2
        {
            get { return msgV2; }
            set { msgV2 = value; }
        }
        private string msgV3;
        /// <summary>
        /// 信息3
        /// </summary>
        public string MsgV3
        {
            get { return msgV3; }
            set { msgV3 = value; }
        }
        private string msgV4;
        /// <summary>
        /// 信息4
        /// </summary>
        public string MsgV4
        {
            get { return msgV4; }
            set { msgV4 = value; }
        }
        private string env;
        /// <summary>
        /// 活动的批输入监控
        /// </summary>
        public string Env
        {
            get { return env; }
            set { env = value; }
        }
        private string fldName;
        /// <summary>
        /// 字段名
        /// </summary>
        public string FldName
        {
            get { return fldName; }
            set { fldName = value; }
        }
    }
}
