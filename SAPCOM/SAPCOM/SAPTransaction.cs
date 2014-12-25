using SAP.Middleware.Connector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SAPCOM
{
    public class SAPTransaction
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
        public SAPTransaction(SAPDestination destination)
        {
            sapDestination = destination;
            SAPConnector Conn = new SAPConnector(destination);
            this.destination = Conn.SapDestination;
        }
        public SAPTransaction(SAPConnector conn)
        {
            this.destination = conn.SapDestination;
        }
        public SAPTransaction()
        {
            if (SapSetting.Connector == null)
            {
                throw new Exception("请在Main()或Global.asax添加SapSetting.Connector");
            }
            this.destination = SapSetting.Connector.SapDestination;
        }
        public List <SAPTransactionMsgInfo> EXEC_TRANSACTION(string T_CODE, TransactionParam options, ScreenCollection screens)
        {
            List< SAPTransactionMsgInfo > ltReturnMsg= new List<SAPTransactionMsgInfo>();
            RfcDestination rfcDest = destination;
            RfcRepository rfcRep = rfcDest.Repository;
            IRfcFunction rfcFun = rfcRep.CreateFunction("/ISDFPS/EXEC_TRANSACTION");
            rfcFun.SetValue("IF_TCODE", T_CODE);
            rfcFun.SetValue("IF_SKIP_FIRST_SCREEN", ""); //是否跳过第一个屏幕
            rfcFun.SetValue("IF_LEAVE", "");
            rfcFun.SetValue("IF_RFC_DEST", "");
            //获取CTU_PARAMS数据结构
            IRfcStructure CTU_PARAMS_Structure = rfcFun.GetStructure("IS_OPTIONS");
            //参数CTU_PARAMS赋值
            CTU_PARAMS_Structure.SetValue("DISMODE", options.DisMode);
            CTU_PARAMS_Structure.SetValue("UPDMODE", options.UpDmode);
            CTU_PARAMS_Structure.SetValue("CATTMODE", options.CattMode);
            CTU_PARAMS_Structure.SetValue("DEFSIZE", options.DefSize);
            CTU_PARAMS_Structure.SetValue("RACOMMIT", options.RadCommit);
            CTU_PARAMS_Structure.SetValue("NOBINPT", options.NoBinpt);
            CTU_PARAMS_Structure.SetValue("NOBIEND", options.NoBiend);
            //获取屏幕数据结构
            IRfcTable BDC_Table = rfcFun.GetTable("IT_BDCDATA");
            //构件屏幕数据
            foreach (ScreenBase screenBase in screens)
            {
                //添加新行
                BDC_Table.Append();
                //Begin  添加屏幕开始
                BDC_Table.SetValue("PROGRAM", screenBase.ScreenName);
                BDC_Table.SetValue("DYNPRO", screenBase.ScreenNumber.ToString().PadLeft(4, '0'));
                BDC_Table.SetValue("DYNBEGIN", "X"); //屏幕开始标识
                //特殊标识处理
                if (screenBase.Cursor.Trim().Length > 0)
                {
                    //添加新行
                    BDC_Table.Append();
                    BDC_Table.SetValue("FNAM", "BDC_CURSOR"); //光标
                    BDC_Table.SetValue("FVAL", screenBase.Cursor.Trim());
                }
                if (screenBase.Subscrs.Count > 0)
                {
                    foreach (string fval in screenBase.Subscrs)
                    {
                        //添加新行
                        BDC_Table.Append();
                        BDC_Table.SetValue("FNAM", "BDC_SUBSCR"); //
                        BDC_Table.SetValue("FVAL", fval);
                    }
                }
                if (screenBase.Subscr.Trim().Length > 0)
                {
                    //添加新行
                    BDC_Table.Append();
                    BDC_Table.SetValue("FNAM", "BDC_SUBSCR"); //
                    BDC_Table.SetValue("FVAL", screenBase.Subscr.Trim());
                }
                if (screenBase.OkCode.Trim().Length > 0)
                {
                    //添加新行
                    BDC_Table.Append();
                    BDC_Table.SetValue("FNAM", "BDC_OKCODE"); //OK 标识
                    BDC_Table.SetValue("FVAL", screenBase.OkCode.Trim());
                }
                //数据添加
                foreach (ScreenItem item in screenBase.Fields)
                {
                    if (item.ItemValue != null)
                    {
                        if (item.ItemValue is ScreenGridItemCollection)
                        {
                            ScreenGridItemCollection screenGridItemCollection = (ScreenGridItemCollection)item.ItemValue;
                            IEnumerator enumerator = screenGridItemCollection.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                ScreenGridItem screenGridItem = (ScreenGridItem)enumerator.Current;
                                if (screenGridItem.RowValue != null)
                                {
                                    //添加新行
                                    BDC_Table.Append();
                                    BDC_Table.SetValue("FNAM", string.Format("{0}({1})", item.Name, screenGridItem.RowNum)); //OK 标识
                                    BDC_Table.SetValue("FVAL", screenGridItem.RowValue);
                                }
                            }
                        }
                        else
                        {
                            //添加新行
                            BDC_Table.Append();
                            BDC_Table.SetValue("FNAM", item.Name); //OK 标识
                            BDC_Table.SetValue("FVAL", item.ItemValue.ToString());
                        }
                    }
                }//foreach
            }//foreach
            //执行RFC
            rfcFun.Invoke(rfcDest);
            //获取返回信息
           IRfcTable ETMSG_Table= rfcFun.GetTable("ET_MSG");
           TableRead tableRead = new TableRead(this.sapDestination);
           foreach (IRfcStructure item in ETMSG_Table)
           {
               SAPTransactionMsgInfobase msgInfoBase = new SAPTransactionMsgInfobase();
               msgInfoBase.MsgId=item.GetString("MSGID");
               msgInfoBase.MsgNr = item.GetString("MSGNR");
               msgInfoBase.MsgSpra = item.GetString("MSGSPRA");
               msgInfoBase.MsgTyp = item.GetString("MSGTYP");
               msgInfoBase.MsgV1 = item.GetString("MSGV1");
               msgInfoBase.MsgV2 = item.GetString("MSGV2");
               msgInfoBase.MsgV3 = item.GetString("MSGV3");
               msgInfoBase.MsgV4 = item.GetString("MSGV4");
               msgInfoBase.TCode = item.GetString("TCODE");
               string whereStr = string.Concat(new string []{
                   "SPRSL = '",
                    msgInfoBase.MsgSpra ,
					"' AND ARBGB = '",
					 msgInfoBase.MsgId,
					"' AND MSGNR = '",
					 msgInfoBase.MsgNr,
					"'"
               });
               DataTable T100Table = tableRead.Read("T100", null, 0, whereStr);
               string text = "";
               if (T100Table.Rows.Count>0)
               {
                    text = T100Table.Rows[0]["TEXT"].ToString().Trim();
                    text = text.Replace("&1", msgInfoBase.MsgV1);
                    text = text.Replace("&2", msgInfoBase.MsgV2);
                    text = text.Replace("&3", msgInfoBase.MsgV3);
                    text = text.Replace("&4", msgInfoBase.MsgV4);
					int num = text.IndexOf("&");
					if (num >= 0)
					{
                        text = text.Substring(0, num) + msgInfoBase.MsgV1 + text.Substring(num + 1);
						num = text.IndexOf("&");
						if (num >= 0)
						{
                            text = text.Substring(0, num) + msgInfoBase.MsgV2+ text.Substring(num + 1);
							num = text.IndexOf("&");
							if (num >= 0)
							{
                                text = text.Substring(0, num) + msgInfoBase.MsgV3 + text.Substring(num + 1);
								num = text.IndexOf("&");
								if (num >= 0)
								{
                                    text = text.Substring(0, num) + msgInfoBase.MsgV4 + text.Substring(num + 1);
								}
							}
						}
					}

               }//if (T100Table.Rows.Count>0)
               SAPTransactionMsgInfo msgInfo = new SAPTransactionMsgInfo(msgInfoBase);
               msgInfo.Msgtx = text;
               ltReturnMsg.Add(msgInfo);
           }
           return ltReturnMsg;
        }//List <SAPTransactionMsgInfo>
    }//class
    public class TransactionParam
    {
        private string disMode;
        /// <summary>
        ///处理方式 '[A、E、N、P]  A:显示所有屏幕； E：显示错误屏幕 ；N:不显示屏幕 ；P:用于调试ABAP，跳转到ABAP断点
        /// </summary>
        public string DisMode
        {
            get { return disMode; }
            set { disMode = value; }
        }
        private string upDmode;
        /// <summary>
        ///更新模式 '[A,S,L] A:异步；S：同步 ； L:本地
        /// </summary>
        public string UpDmode
        {
            get { return upDmode; }
            set { upDmode = value; }
        }
        private string cattMode;
        /// <summary>
        ///  'CATT [""、N、A] "":CATT模式；N：NOT CATT模式；A：CATT模式并对每个屏幕进行控制
        /// </summary>
        public string CattMode
        {
            get { return cattMode; }
            set { cattMode = value; }
        }
        private string defSize;
        /// <summary>
        /// 缺省屏幕大小'DEFALUT SIZE ["",X] "":不使用屏幕定义大小；X：使用屏幕定义大小
        /// </summary>
        public string DefSize
        {
            get { return defSize; }
            set { defSize = value; }
        }
        private string radCommit;
        /// <summary>
        /// 提交模式
        /// </summary>
        public string RadCommit
        {
            get { return radCommit; }
            set { radCommit = value; }
        }
        private string noBinpt;

        public string NoBinpt
        {
            get { return noBinpt; }
            set { noBinpt = value; }
        }
        private string noBiend;

        public string NoBiend
        {
            get { return noBiend; }
            set { noBiend = value; }
        }
        public TransactionParam()
        {
            this.cattMode = "";
            this.defSize = "X";
            this.disMode = "N";
            this.upDmode = "S";
            this.radCommit = "";
            this.noBiend = "";
            this.noBinpt = "";
        }
    }
}
