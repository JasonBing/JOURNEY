using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCS
{
    public class QXinUser
    {
        private string fName;

        public string Name
        {
            get { return fName; }
            set { fName = value; }
        }
        private string fPCName;

        public string PCName
        {
            get { return fPCName; }
            set { fPCName = value; }
        }
        private string fIPAddress;

        public string IPAddress
        {
            get { return fIPAddress; }
            set { fIPAddress = value; }
        }

        public QXinUser(string pName, string pPCName, string pIPAddress)
        {
            fName = pName;
            fPCName = pPCName;
            fIPAddress = pIPAddress;
        }
    }
}
