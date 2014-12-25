using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class SAPTransactionMsgInfo:SAPTransactionMsgInfobase
    {
        private string msgtx;
		public string Msgtx
		{
			get
			{
				return this.msgtx;
			}
			set
			{
				this.msgtx=value;
			}
		}
		public SAPTransactionMsgInfo(SAPTransactionMsgInfobase msgInfoBase)
		{
		    this.LoadMsg(msgInfoBase);
		}
		internal void LoadMsg(SAPTransactionMsgInfobase msgInfoBase)
        {
            base.TCode=msgInfoBase.TCode;
            base.DyName=msgInfoBase.DyName;
            base.DyNum=msgInfoBase.DyNum;
            base.Env=msgInfoBase.Env;
            base.FldName=msgInfoBase.FldName;
            base.MsgId=msgInfoBase.MsgId; 
            base.MsgNr=msgInfoBase.MsgNr;
            base.MsgSpra=msgInfoBase.MsgSpra;
            base.MsgTyp=msgInfoBase.MsgTyp;
            base.MsgV1=msgInfoBase.MsgV1;
            base.MsgV2=msgInfoBase.MsgV2;
            base.MsgV3=msgInfoBase.MsgV3;
            base.MsgV4=msgInfoBase.MsgV4;
        }
    }
}
