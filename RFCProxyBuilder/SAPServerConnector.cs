using System;

namespace RFCProxyBuilder
{
	/// <summary>
	/// SAP 
	/// </summary>
	public class SAPServerConnector : SAPProxy
	{
		/// <summary>
		/// SAP 
		/// </summary>
		private SAP.Connector.Destination m_oDest = null;

		/// <summary>
		/// 
		/// </summary>
		public SAPServerConnector()
		{
			// SAP
			m_oDest = new SAP.Connector.Destination();
		}

		/// <summary>
		/// 
		/// </summary>
		~SAPServerConnector()
		{
			// 
		    this.DisconnectSAPServer();

			// 
			m_oDest.Dispose();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strTYPE">RFC server type, 2/3/E: R/2 or R/3 or External System</param>
		/// <param name="strASHOST">Host name of a specific application server (R/3, No Load Balancing) </param>
		/// <param name="nSYSNR">R/3 system number (R/3, No Load Balancing) </param>
		/// <param name="nCLIENT">SAP logon client</param>
		/// <param name="strLANG">SAP logon language (1-byte SAP language or 2-byte ISO language) </param>
		/// <param name="strUSER">SAP logon user </param>
		/// <param name="strPASSWD">SAP logon password </param>
		public SAPServerConnector(string strTYPE, string strASHOST, short nSYSNR, short nCLIENT, string strLANG, string strUSER, string strPASSWD)
		{
			m_oDest = new SAP.Connector.Destination();

			SetConnectionInfo(strTYPE, strASHOST, nSYSNR, nCLIENT, strLANG, strUSER, strPASSWD);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="strTYPE">RFC server type, 2/3/E: R/2 or R/3 or External System</param>
		/// <param name="strASHOST">Host name of a specific application server (R/3, No Load Balancing) </param>
		/// <param name="nSYSNR">R/3 system number (R/3, No Load Balancing) </param>
		/// <param name="nCLIENT">SAP logon client</param>
		/// <param name="strLANG">SAP logon language (1-byte SAP language or 2-byte ISO language) </param>
		/// <param name="strUSER">SAP logon user </param>
		/// <param name="strPASSWD">SAP logon password </param>
		public void SetConnectionInfo(string strTYPE, string strASHOST, short nSYSNR, short nCLIENT, string strLANG, string strUSER, string strPASSWD)
		{
			m_oDest.Type = strTYPE;
			m_oDest.AppServerHost = strASHOST;
			m_oDest.SystemNumber = nSYSNR;	
			m_oDest.Client = nCLIENT;
			m_oDest.Language = strLANG;
			m_oDest.Username = strUSER;
			m_oDest.Password = strPASSWD;
		}

		/// <summary>
		/// RFC server type : 3)
		/// </summary>
		/// <param name="strASHOST">Host name of a specific application server (R/3, No Load Balancing) </param>
		/// <param name="nSYSNR">R/3 system number (R/3, No Load Balancing) </param>
		/// <param name="nCLIENT">SAP logon client</param>
		/// <param name="strLANG">SAP logon language (1-byte SAP language or 2-byte ISO language) </param>
		/// <param name="strUSER">SAP logon user </param>
		/// <param name="strPASSWD">SAP logon password </param>
		public void SetConnectionInfo(string strASHOST, short nSYSNR, short nCLIENT, string strLANG, string strUSER, string strPASSWD)
		{
			m_oDest.Type = "3";
			m_oDest.AppServerHost = strASHOST;
			m_oDest.SystemNumber = nSYSNR;	
			m_oDest.Client = nCLIENT;
			m_oDest.Language = strLANG;
			m_oDest.Username = strUSER;
			m_oDest.Password = strPASSWD;
		}

		/// <summary>
		/// SAP 
		/// </summary>
		/// <returns></returns>
		public virtual bool ConnectSAPServer()
		{
			bool bRetVal = false;

			try
			{
				this.ConnectionString = m_oDest.ConnectionString; // 
				//Return 0 if failed, != 0 else.
				if ( 0 != this.Connection.Open())
					bRetVal = true;
			} 
			catch(Exception exp)
			{
				//Console.WriteLine(exp.Message);
				this.Connection.Close(); // 
				throw exp;
			}

			return bRetVal;
		}

		public virtual void DisconnectSAPServer()
		{
            if (this.Connection != null)
            {
                if (this.Connection.IsOpen == true)
                    this.Connection.Close(); //

            }
        }

	}
}