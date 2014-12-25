using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TCS
{
    public class QXinUserUI : QXinUser
    {
        //枚举+事件 将 信息传递到FormCommunicate
        public delegate void deleShowRecvMsg(QXinUser Myself, IPMSG MsgPack);
        public event deleShowRecvMsg eventShowRecv;

        private List<string> pInitMsgs = new List<string>();
        public Icon UserIcon;
        public List<string> InitMsgs
        {
            get { return pInitMsgs; }
            set { pInitMsgs = value; }
        }

        private FormCommunicate pFormCommun;

        public FormCommunicate FormCommun
        {
            get { return pFormCommun; }
            set { pFormCommun = value; }
        }

        public void CallShowRecvEvent(IPMSG MsgPack)
        {
            eventShowRecv(this, MsgPack);
        }

        public QXinUserUI(string pName, string pPCName, string pIPAddress)
            : base(pName, pPCName, pIPAddress)
        {
        }
    }
}
