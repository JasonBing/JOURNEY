using System;
using System.Collections.Generic;
using System.Text;
using SAP.Middleware.Connector;
using System.Data;
namespace SAPCOM
{
    public class SAPConnector:IDestinationConfiguration
    {
        protected RfcConfigParameters sapConfigParam;
        public RfcConfigParameters SapConfigParam
        {
            get { return sapConfigParam; }
        }
        public RfcDestination SapDestination
        {
            get 
            {
              return  RfcDestinationManager.GetDestination(sapConfigParam);
            }
        }

        public SAPConnector(SAPDestination destination)
        {
            sapConfigParam = new RfcConfigParameters();
            sapConfigParam.Add(RfcConfigParameters.Name, "JOURNEY");
            sapConfigParam.Add(RfcConfigParameters.AppServerHost, destination.SapServerHost);
            sapConfigParam.Add(RfcConfigParameters.Client, destination.Client);
            sapConfigParam.Add(RfcConfigParameters.User, destination.UserName);
            sapConfigParam.Add(RfcConfigParameters.Password, destination.Password);
            sapConfigParam.Add(RfcConfigParameters.SystemNumber, destination.SystemNumber);
            sapConfigParam.Add(RfcConfigParameters.Language, destination.Language);
            //RfcDestinationManager.RegisterDestinationConfiguration(this);
        }

        public void ConnectionTest()
        {
            RfcDestination Dest = this.SapDestination;
            RfcRepository rfcrep = Dest.Repository;
        }




        public bool ChangeEventsSupported()
        {
            RfcDestinationManager.UnregisterDestinationConfiguration(this);
            return true;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;


        /// <summary>
        /// 主要用户根据不用的标识获取不同的SAP连接
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public RfcConfigParameters GetParameters(string destinationName)
        {
            return SapConfigParam;
            //DEMO
            //if ("PRD_000".Equals(destinationName))
            //{
            //    RfcConfigParameters parms = new RfcConfigParameters();
            //    parms.Add(RfcConfigParameters.AppServerHost, "192.168.1.3");   //SAP主机IP
            //    parms.Add(RfcConfigParameters.SystemNumber, "00");  //SAP实例
            //    parms.Add(RfcConfigParameters.User, "MENGXIN");  //用户名
            //    parms.Add(RfcConfigParameters.Password, "5239898");  //密码
            //    parms.Add(RfcConfigParameters.Client, "888");  // Client
            //    parms.Add(RfcConfigParameters.Language, "ZH");  //登陆语言
            //    parms.Add(RfcConfigParameters.PoolSize, "5");
            //    parms.Add(RfcConfigParameters.MaxPoolSize, "10");
            //    parms.Add(RfcConfigParameters.IdleTimeout, "60");
            //    return parms;
            //}
        }
    }
}
