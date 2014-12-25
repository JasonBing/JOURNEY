using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using System.Collections;
namespace SAPCOM
{
    public class TableRead
    {
        private SAPDestination Destination;
        public TableRead(SAPDestination destination)
        {
            this.Destination = destination;
        }
       
        public DataTable Read(string tableName, ICollection fields, int rowCout, string where)
        {
            DataTable dt = null;
            SAPConnector conn = new SAPConnector(Destination);
            RfcDestination rfcDestination = conn.SapDestination;
            RfcRepository rfcRep = rfcDestination.Repository;
             /* 读取数据RFC_READ_TABLE 参数说明
            * Improt：
            * QUERY_TABLE： 读取的表
            * DELIMITER：字段之间的分隔符（当选择多个字段时）
            * NO_DATA：输入'X'时，不向传出表DATA输出数据
            * ROWSKIPS：输出数据的第一条数据的行号（从0开始计）
            *  ROWCOUNT：从ROWSKIPS开始，一共输出的数据行数（0代表所有数据）
            *  Table:
            *  OPTIONS：表查询条件，比如对MARA表来说，可以写MATNR = 'ABCD'。留空代表选择所有数据。
            *  FIELDS：输出的表字段。留空代表输出所有字段。
            *  DATA：输入的数据记录
            *  其中第一个参数QUERY_TABLE应该是必输项，否则这次调用就无意义（不知道要选那个表啊！），函数返回TABLE_NOT_AVAILABLE错误。
            */
            IRfcFunction rfcFun = rfcRep.CreateFunction("RFC_READ_TABLE");
            rfcFun.SetValue("QUERY_TABLE", tableName);
            rfcFun.SetValue("DELIMITER", SapSetting.READTABLEDELIMITER);
            rfcFun.SetValue("ROWCOUNT", rowCout);
            if (where != null && where.Length>0)
            {
                //获取查询条件
                IRfcTable rfcOPTIONS = rfcFun.GetTable("OPTIONS");
                string[] options = where.Split(Environment.NewLine.ToCharArray());
                for (int i = 0; i <= options.Length - 1; i++)
                {
                    if (options[i].Length > 75)
                    {
                        throw new Exception("The length of each row of data is not greater than  75 characters!");
                    }
                    //添加新行
                    rfcOPTIONS.Append();
                    rfcOPTIONS.SetValue("TEXT", options[i]);
                }
            }
            //获取表结构
            DataTable tableHeader = null; //表结构头表
            List<string> tableStructFileds = null;//表的字段集合
            DataTable tableStruct = GetTableStruct(tableName, out tableHeader, out tableStructFileds);
            if (fields != null && fields.Count > 0)
            {
                //获取字段IRfcTable
                IRfcTable rfcFIELDS = rfcFun.GetTable("FIELDS");
                foreach (string field in fields)
                {
                    if (tableStructFileds.Contains(field)==false)
                    {
                        throw new Exception(String.Format("字段{0}在表{1}(SAP)中不存在！", field, tableName));
                    }
                    //添加新行
                    rfcFIELDS.Append();
                    rfcFIELDS.SetValue("FIELDNAME", field);
                }
            }
            rfcFun.Invoke(rfcDestination);
            IRfcTable rfcTable = rfcFun.GetTable("DATA");
            if (fields != null &&fields.Count > 0)
            {
                dt = ConvertDataFromRFC_READ_TABLE(rfcTable, tableStruct,fields);
            }
            else
            {
                dt = ConvertDataFromRFC_READ_TABLE(rfcTable, tableStruct, tableStructFileds);
            }
            return dt;
        }
        /// <summary>
        /// 获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="headerTable"></param>
        /// <returns></returns>
        public DataTable GetTableStruct(string tableName, out DataTable headerTable, out List<string> fields)
        {
            SAPConnector Conn = new SAPConnector(Destination);
            RfcDestination rfcDest = Conn.SapDestination;
            RfcRepository rfcRep = rfcDest.Repository;
            /*获取表结构RFC_GET_NAMETAB参数说明
             * Improt:
             * TABNAME:读取表结构的表名
             * Table:
             * NAMETAB:表结构（字段信息）
             * HEADER：表信息说明
             */
            IRfcFunction rfcFun = rfcRep.CreateFunction("RFC_GET_NAMETAB");
            rfcFun.SetValue("TABNAME", tableName);
            rfcFun.Invoke(rfcDest);
            IRfcTable rfcTable = rfcFun.GetTable("NAMETAB");
            IRfcStructure irfStructe = rfcFun.GetStructure("HEADER");
            headerTable = SAPFunction.GetDataTableFromRfcStruct(irfStructe);
            return SAPFunction.GetDataTableFromRfcTable(rfcTable, out fields);
        }

        public string BuilderEntityCS(string tableName)
        {
            DataTable tableHeader=null;
            List<string> fields=null;
            DataTable tableStruct = GetTableStruct(tableName, out tableHeader, out fields);
            
            return "";
        }
        #region 数据转换 RFC_READ_TABLE:NAMETAB->DataTable
        /// <summary>
        /// 将RFC_READ_TABLE读取的数据转换为DataTable
        /// </summary>
        /// <param name="rfcTable">RFC_READ_TABLE读取的NAMETAB</param>
        /// <param name="tableStruct">对应表的字段结构数据</param>
        /// <param name="fields">查询的字段</param>
        /// <returns></returns>
        private DataTable ConvertDataFromRFC_READ_TABLE(IRfcTable rfcTable, DataTable tableStruct,ICollection fields)
        {
            DataTable result = null;
            if (rfcTable == null)
            {
                result = null;
            }
            else
            {
                result = new DataTable();
                //创建table列
                foreach (DataRow structRow in tableStruct.Rows)
                {
                    IEnumerator enumerator = fields.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (enumerator.Current.ToString()==structRow["FIELDNAME"].ToString())
                        {
                            switch (structRow["EXID"].ToString().ToUpper())
                            {
                                case  "D":
                                    result.Columns.Add(structRow["FIELDNAME"].ToString(), typeof(int));
                                    break;
                                case "I":
                                    result.Columns.Add(structRow["FIELDNAME"].ToString(), typeof(decimal));
                                    break;
                                case "F":
                                    result.Columns.Add(structRow["FIELDNAME"].ToString(), typeof(DateTime));
                                    break;
                                default:
                                    result.Columns.Add(structRow["FIELDNAME"].ToString(),typeof(string));
                                    break;
                            }
                        }
                    }
                }
               //组合table数据
                foreach (IRfcStructure item in rfcTable)
                {
                    DataRow newRow = result.NewRow();
                    string [] datas= (item.GetString("WA")).Split(SapSetting.READTABLEDELIMITER);
                    for (int i = 0; i <= datas.Length-1; i++)
                    {
                       
                        Type type=result.Columns[i].DataType;
                        if (type==typeof(int))
                        {
                            newRow[i] = Convert.ToInt32(datas[i]);
                        }
                        else if (type == typeof(decimal))
                        {
                            newRow[i] = Convert.ToDecimal(datas[i]);
                        }
                        else if (type == typeof(DateTime))
                        {
                            string dateStr = datas[i].Substring(0, 4) + "/" + datas[i].Substring(5, 2) + "/" + datas[i].Substring(6, 2);
                            newRow[i] = Convert.ToDateTime(dateStr);
                        }
                        else
                        {
                            newRow[i] =datas[i];
                        }
                    }
                    result.Rows.Add(newRow);
                }
            }
            return result;
        }
        #endregion    
    }
}
