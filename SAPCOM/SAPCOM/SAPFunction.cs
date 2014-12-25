using SAP.Middleware.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Xml;

namespace SAPCOM
{
    public class SAPFunction
    {

        private RfcDestination destination;
        private RfcDestination Destination
        {
            get
            {
                if (destination == null)
                {
                    return SapSetting.Connector.SapDestination;
                }
                else
                {
                    return destination;
                };
            }
        }
        private RfcTransaction rfcTran;
        private SAPDestination sapDestination;
        private SAPDestination SapDestination
        {
            get
            {
                if (sapDestination == null)
                {
                    return SapSetting.Destination;
                }
                else
                {
                    return this.sapDestination;
                };
            }
        }
        public SAPFunction(SAPDestination destination)
        {
            sapDestination = destination;
            SAPConnector Conn = new SAPConnector(destination);
            this.destination = Conn.SapDestination;
        }
        public SAPFunction(SAPConnector conn)
        {
            this.destination = conn.SapDestination;
        }
        public SAPFunction()
        {
            if (SapSetting.Connector == null)
            {
                throw new Exception("请在Main()或Global.asax添加SapSetting.Connector");
            }
            this.destination = SapSetting.Connector.SapDestination;
        }
        /// <summary>
        /// 开启事务（创建是否对象）
        /// </summary>
        /// <returns></returns>
        public RfcTransaction BeginSAPTransaction()
        {
            rfcTran = new RfcTransaction();
            return rfcTran;
        }
        public void BeginSAPTransaction(RfcTransaction rfcTran)
        {
            if (rfcTran == null)
            {
                this.rfcTran = new RfcTransaction();
            }
            else
            {
                this.rfcTran = rfcTran;
            }
        }
        /// <summary>
        /// 将RFC函数附加到事务中
        /// </summary>
        /// <param name="irfcFuntion"></param>
        public void AddTransaction(IRfcFunction irfcFuntion)
        {
            if (rfcTran == null)
            {
                rfcTran = new RfcTransaction();
            }
            rfcTran.AddFunction(irfcFuntion);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitSAPTransaction()
        {
            rfcTran.Commit(this.destination);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="desctination"></param>
        public void CommitSAPTransaction(RfcDestination desctination)
        {
            if (desctination != null)
            {
                rfcTran.Commit(desctination);
            }
        }
        /// <summary>
        /// 获取连接
        /// </summary>
        public void KeptConnection()
        {
            if (this.destination == null)
            {
                RfcSessionManager.BeginContext(this.Destination);
            }
        }
        /// <summary>
        /// 释放连接
        /// </summary>
        public void RelaseConnection()
        {
            if (this.destination == null)
            {
                RfcSessionManager.EndContext(this.Destination);
            }
        }
        public IRfcFunction Function(string rfcName, Dictionary<string, object> parms)
        {
            try
            {
                RfcRepository rfcRep = this.Destination.Repository;
                IRfcFunction rfcFun = rfcRep.CreateFunction(rfcName.ToUpper()); //创建RFC_FUNC
                //添加参数
                foreach (KeyValuePair<string, object> kvp in parms)
                {
                    rfcFun.SetValue(kvp.Key, kvp.Value);
                }
                rfcFun.Invoke(this.Destination); //执行RFC
                return rfcFun;
            }
            catch (RfcCommunicationException e)
            {
                e.Data.Add("RFC-NET", "RFC通信异常！");
               throw e;
            }
            catch (RfcLogonException e)
            {
                e.Data.Add("RFC-NET", "RFC-SAP账户登录失败！");
                throw e;
            }
            catch (RfcAbapRuntimeException e)
            {
                e.Data.Add("RFC-NET", "RFC-ABAP系统异常！");
                throw e;
            }
            catch (RfcAbapBaseException e)
            {
                e.Data.Add("RFC-NET", "RFC-ABAP执行失败！");
                throw e;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable GetReturnTable(IRfcFunction irfcFunction, string tableName)
        {
            IRfcTable rfcTable = irfcFunction.GetTable(tableName.ToUpper());
            return SAPFunction.GetDataTableFromRfcTable(rfcTable);
        }
        public DataTable GetReturnStruct(IRfcFunction irfcFunction, string tableName)
        {
            IRfcStructure rfcStruct = irfcFunction.GetStructure(tableName.ToUpper());
            return SAPFunction.GetDataTableFromRfcStruct(rfcStruct);
        }
        /// <summary>
        /// 查询RFC函数支持* 模糊查询
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public List<string> SearchFuntion(string groupName, string functionName)
        {
            List<string> lt = new List<string>();
            if (destination != null)
            {
                RfcDestination rfcDest = destination;
                RfcRepository rfcRep = rfcDest.Repository;
                IRfcFunction rfcFun = rfcRep.CreateFunction("RFC_FUNCTION_SEARCH");
                rfcFun.SetValue("GROUPNAME", groupName.ToUpper());
                rfcFun.SetValue("FUNCNAME", functionName.ToUpper());
                rfcFun.SetValue("LANGUAGE", destination.Language);
                rfcFun.Invoke(rfcDest);
                IRfcTable rfcTable = rfcFun.GetTable("FUNCTIONS");
                DataTable table = SAPFunction.GetDataTableFromRfcTable(rfcTable);
                foreach (DataRow item in table.Rows)
                {
                    lt.Add(item["FUNCNAME"].ToString());
                }
            }
            return lt;
        }
        /// <summary>
        /// 获取RFC函数的参数
        /// </summary>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public DataTable GetParams(string functionName)
        {
            RfcDestination rfcDest = destination;
            RfcRepository rfcRep = rfcDest.Repository;
            IRfcFunction rfcFun = rfcRep.CreateFunction("RFC_GET_FUNCTION_INTERFACE");
            rfcFun.SetValue("FUNCNAME", functionName);
            rfcFun.SetValue("LANGUAGE", destination.Language);
            rfcFun.SetValue("NONE_UNICODE_LENGTH", (SapDestination.IsUnicodeSystem == true ? "" : "X")); //是不是UNICODE编码
            rfcFun.Invoke(rfcDest);
            IRfcTable rfcTable = rfcFun.GetTable("PARAMS");
            return GetDataTableFromRfcTable(rfcTable);
        }
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="headerTable"></param>
        /// <returns></returns>
        public DataTable GetTableStruct(string tableName, out DataTable headerTable)
        {
            RfcDestination rfcDest = destination;
            RfcRepository rfcRep = rfcDest.Repository;
            IRfcFunction rfcFun = rfcRep.CreateFunction("RFC_GET_NAMETAB");
            rfcFun.SetValue("TABNAME", tableName);
            rfcFun.Invoke(rfcDest);
            IRfcTable rfcTable = rfcFun.GetTable("NAMETAB");
            IRfcStructure irfStructe = rfcFun.GetStructure("HEADER");
            headerTable = GetDataTableFromRfcStruct(irfStructe);
            return GetDataTableFromRfcTable(rfcTable);
        }

        #region 数据转换IRfcTable->DataTable、IRfcStructure->DataTable
        public static DataTable GetDataTableFromRfcTable(IRfcTable rfcTable)
        {
            DataTable dt = new DataTable();
            if (rfcTable == null)
            {
                dt = null;
            }
            else
            {
                for (int i = 0; i <= rfcTable.ElementCount - 1; i++)
                {
                    RfcElementMetadata rfcEMD = rfcTable.GetElementMetadata(i);
                    dt.Columns.Add(rfcEMD.Name);
                }
                foreach (IRfcStructure item in rfcTable)
                {
                    DataRow newRow = dt.NewRow();
                    for (int i = 0; i <= rfcTable.ElementCount - 1; i++)
                    {
                        RfcElementMetadata rfcEMD = rfcTable.GetElementMetadata(i);
                        newRow[rfcEMD.Name] = item.GetString(rfcEMD.Name);
                    }
                    dt.Rows.Add(newRow);
                }
            }
            return dt;
        }
        /// <summary>
        /// 将IRfcTable转换为DataTtable
        /// </summary>
        /// <param name="rfcTable"></param>
        /// <param name="fields">返回rfcTable的字段key集合 List<string></param>
        /// <returns></returns>
        public static DataTable GetDataTableFromRfcTable(IRfcTable rfcTable, out List<string> fields)
        {
            fields = new List<string>();
            DataTable dt = new DataTable();
            if (rfcTable == null)
            {
                dt = null;
            }
            else
            {
                for (int i = 0; i <= rfcTable.ElementCount - 1; i++)
                {
                    RfcElementMetadata rfcEMD = rfcTable.GetElementMetadata(i);
                    dt.Columns.Add(rfcEMD.Name);
                }
                foreach (IRfcStructure item in rfcTable)
                {
                    DataRow newRow = dt.NewRow();
                    for (int i = 0; i <= rfcTable.ElementCount - 1; i++)
                    {
                        RfcElementMetadata rfcEMD = rfcTable.GetElementMetadata(i);
                        newRow[rfcEMD.Name] = item.GetString(rfcEMD.Name);
                        if (rfcEMD.Name == "FIELDNAME")
                        {
                            fields.Add(item.GetString(rfcEMD.Name));
                        }
                    }
                    dt.Rows.Add(newRow);
                }
            }
            return dt;
        }
        public static DataTable GetDataTableFromRfcStruct(IRfcStructure rfcStruct)
        {
            DataTable dt = new DataTable();
            if (rfcStruct == null)
            {
                dt = null;
            }
            else
            {
                for (int i = 0; i <= rfcStruct.ElementCount - 1; i++)
                {
                    RfcElementMetadata rfcMetData = rfcStruct.GetElementMetadata(i);
                    dt.Columns.Add(rfcMetData.Name);
                }
                DataRow newRow = dt.NewRow();
                for (int i = 0; i <= rfcStruct.ElementCount - 1; i++)
                {
                    RfcElementMetadata rfcMetData = rfcStruct.GetElementMetadata(i);
                    newRow[rfcMetData.Name] = rfcStruct.GetValue(i);
                }
                dt.Rows.Add(newRow);
            }
            return dt;
        }

        #endregion
    }

}