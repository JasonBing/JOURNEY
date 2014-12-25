using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TCS
{

    public delegate void RecvCallBack(string IPFrom,IPMSG MsgPack);
    public delegate void UpdateListCallBack(List<QXinUser> Users);

    public class TQRunner:IDisposable
    {
        private bool TQExit = false;

        public event RecvCallBack RecvEvent;
        public event UpdateListCallBack UpdateListEvent;

        private static int sNewPackNo = (int)(DateTime.Now.Ticks & 0xFFFFFFF);

        public static string NewPackNo
        {
            get { return TQRunner.sNewPackNo++.ToString(); }
        }

        private static readonly int MyPoint = 2425;

        private UdpClient TQClient;

        private readonly IPEndPoint IPEndBroadcast = new IPEndPoint(IPAddress.Broadcast, 2425);
        private QXinUser pQXinUserMe;

        private Thread ThreRece;
        private List<QXinUser> fListUsers=new List<QXinUser>();
        public readonly string IPBroadCast = IPAddress.Broadcast.ToString();
    
        public QXinUser QXinUserMe
        {
            get { return pQXinUserMe; }
            set { pQXinUserMe = value; }
        }
        public List<QXinUser> ListQXinUsers
        {
            get { return fListUsers; }
        }


        #region TQGC
        NetworkStream ClientSream;
        FileStream fileStream;
        TcpListener tcpListener;
        Socket TcpConnect;
        TcpClient recevie;
        public void Collect()
        {
            if (tcpListener != null)
            {
                tcpListener.Stop();
            }
            if (fileStream != null)
            {
                fileStream.Dispose();
            }
            if (TcpConnect != null)
            {
                TcpConnect.Dispose();
            }
            if (ClientSream != null)
            {
                ClientSream.Dispose();
            }
            if (recevie != null)
            {
                recevie.Close();
            }
        } 

        #endregion


        //Add User to ListUsers and CallBack UpdateListEvent for FormMain
        public void ListUsersAdd(string Name, string PCName, string IP)
        {
            if (IP==string.Empty||IP==null)
            {
                return;
            }
            foreach (QXinUser item in fListUsers)
            {
                if (item.IPAddress==IP)
                {
                    return;
                }
            }
            QXinUser AddTQ = new QXinUser(Name, PCName, IP);
            fListUsers.Add(AddTQ);
            if (UpdateListEvent != null)
            {
                UpdateListEvent(fListUsers);
            }
        }

        public void ListUsersDelete(string Name, string PCName, string IP)
        {
            if (IP == string.Empty || IP == null)
            {
                return;
            }

            for (int i = 0; i < fListUsers.Count; i++)
            {
                if (fListUsers[i].IPAddress == IP)
                {
                    fListUsers.Remove(fListUsers[i]);
                    break;
                }
            }

            if (UpdateListEvent != null)
            {
                UpdateListEvent(fListUsers);
            }
            
        }


        #region Constructed Function

        public TQRunner()
        {
            try
            {
                TQClient = new UdpClient(MyPoint);
            }
            catch (Exception)
            {
                TQClient.Close();
                throw new Exception("端口被占用");
            }
            IPAddress IPHost = Dns.GetHostAddresses(Dns.GetHostName())[0];

            pQXinUserMe = new QXinUser("Journey-1", "T-K-1", IPHost.ToString());
            ListUsersAdd("Journey-Test", "XP", "192.168.48.129");

            SendOnlineMsg();

            ThreRece = new Thread(new ThreadStart (ThreadReceiveMsg));
            ThreRece.IsBackground = true;
            ThreRece.Start();
        }

        ~TQRunner()
        {
            if (ThreRece!=null)
            {
                ThreRece.Abort();
            }           
        } 
        #endregion

        #region ReceiveFiles、SendFiles

        public delegate void ProgressBarShow(int Max, int Value , string CurFileName);

        private void RecvAFile(string FullFileName, NetworkStream ClientSream, Int64 FileSize, ProgressBarShow pbShow)
        {
            const int bLength = 500 * 1024;
            fileStream = new FileStream(FullFileName, FileMode.Create, FileAccess.Write);
            int iCRead = 0;
            Int64 iComplete = 0;

            byte[] bFile = new byte[bLength];
            do
            {
                try
                {
                    if (iComplete + bLength > FileSize)
                    {
                        iCRead = ClientSream.Read(bFile, 0, (int)(FileSize - iComplete));
                    }
                    else iCRead = ClientSream.Read(bFile, 0, bFile.Length);
                    iComplete += iCRead;
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.Message);
                    break;
                }

                fileStream.Write(bFile, 0, iCRead);
                if (pbShow != null)
                {
                    if (FileSize!=0)
                    {
                        pbShow(1000, (int)(iComplete * 1000 / FileSize), fileStream.Name);
                    }
                   
                }
            } while (ClientSream.CanRead && iComplete < FileSize);
            fileStream.Dispose();
        }

        public void RecvFileFromIPMsg(string IPSentTo, string fbdSelectedPath, IPMSG RecFile, ProgressBarShow pbShow)
        {
            recevie = new TcpClient();
            recevie.Connect(IPSentTo, 2425);
            ClientSream = recevie.GetStream();

            Int64 FileSize = Convert.ToInt64(RecFile.FileSize[0], 16);

            switch (RecFile.FileType[0])
            {
                case IPMsgFileType.IPMSG_FILE_REGULAR:
                    {
                        byte[] bRequest = IPMSG.MsgToByte(QXinUserMe, IPMsgType.IPMSG_GETFILEDATA, int.Parse(RecFile.PackNo).ToString("X") + ":" + RecFile.FileNo[0] + ":0");
                        if (ClientSream.CanWrite)
                        {
                            ClientSream.Write(bRequest, 0, bRequest.Length);
                        }
                        RecvAFile(fbdSelectedPath + @"\" + RecFile.FileName[0], ClientSream, FileSize, pbShow);
                    }
                    break;

                case IPMsgFileType.IPMSG_FILE_DIR:
                    {
                        #region RecvFolderMethod

                        byte[] bRequest = IPMSG.MsgToByte(QXinUserMe, IPMsgType.IPMSG_GETDIRFILES, int.Parse(RecFile.PackNo).ToString("X") + ":" + RecFile.FileNo[0] + ":0");
                        if (ClientSream.CanWrite)
                        {
                            ClientSream.Write(bRequest, 0, bRequest.Length);
                        }

                        Stack<string> StackFilePaths = new Stack<string>();
                        string FilePath = fbdSelectedPath + @"\";

                        for (int i = StackFilePaths.Count - 1; i >= 0; i--)
                        {
                            FilePath += StackFilePaths.ToArray()[i] + @"\";
                        }

                        do
                        {
                            byte[] bFilehead8 = new byte[8];
                            try
                            {
                                if (ClientSream.Read(bFilehead8, 0, bFilehead8.Length) != 8)
                                {
                                    Console.WriteLine("报错");
                                }
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                                break;
                            }

                            int cFileHeadLength = Convert.ToInt32(Encoding.Default.GetString(bFilehead8).Split(':')[0], 16);  //获取文件信息头长度
                            byte[] bFilehead = new byte[cFileHeadLength];

                            bFilehead8.CopyTo(bFilehead, 0);
                            try
                            {
                                ClientSream.Read(bFilehead, bFilehead8.Length, cFileHeadLength - bFilehead8.Length);
                            }
                            catch (Exception Ex)
                            {
                                Console.WriteLine(Ex.Message);
                                break;
                            }
                            string[] sFilehead = Encoding.Default.GetString(bFilehead).Split(':');
                            string SubFileName = sFilehead[1];
                            string SubFileSize = sFilehead[2];
                            IPMsgFileType SubFileType = (IPMsgFileType)int.Parse(sFilehead[3]);

                            switch (SubFileType)
                            {
                                case IPMsgFileType.IPMSG_FILE_REGULAR:
                                    {
                                        RecvAFile(FilePath + SubFileName, ClientSream, Convert.ToInt64(SubFileSize, 16), pbShow);
                                    }
                                    break;
                                case IPMsgFileType.IPMSG_FILE_DIR:
                                    {
                                        StackFilePaths.Push(SubFileName);
                                        FilePath = fbdSelectedPath + @"\";
                                        for (int i = StackFilePaths.Count - 1; i >= 0; i--)
                                        {
                                            FilePath += StackFilePaths.ToArray()[i] + @"\";
                                        }
                                        System.IO.Directory.CreateDirectory(FilePath);
                                    }
                                    break;
                                case IPMsgFileType.IPMSG_FILE_RETPARENT:
                                    {
                                        StackFilePaths.Pop();
                                        FilePath = fbdSelectedPath + @"\";
                                        for (int i = StackFilePaths.Count - 1; i >= 0; i--)
                                        {
                                            FilePath += StackFilePaths.ToArray()[i] + @"\";
                                        }
                                    }
                                    break;
                                case IPMsgFileType.IPMSG_FILE_SYSHIDE:
                                    {
                                        RecvAFile(FilePath + SubFileName, ClientSream, Convert.ToInt64(SubFileSize, 16), pbShow);
                                        File.SetAttributes(FilePath + SubFileName, FileAttributes.System | FileAttributes.Hidden);
                                    }
                                    break;
                                case IPMsgFileType.IPMSG_FILE_HIDDENOPT:
                                    {
                                        RecvAFile(FilePath + SubFileName, ClientSream, Convert.ToInt64(SubFileSize, 16), pbShow);
                                        File.SetAttributes(FilePath + SubFileName, FileAttributes.Hidden);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        } while (StackFilePaths.Count > 0);
                        #endregion
                    }
                    break;

                default:
                    break;
            }

            ClientSream.Dispose();
            recevie.Close();
        }

        public bool SendAFile(Socket TcpConnect, FileInfo FileInfo, ProgressBarShow pbShow)
        {
            const int AskByteCount = 64 * 1024;
            Byte[] bSendFile=new byte[AskByteCount];
          
            fileStream = new FileStream(FileInfo.FullName,FileMode.Open,FileAccess.Read);

            Int64 iCount = 0;
            do
            {
                try
                {
                    if (iCount + AskByteCount < FileInfo.Length)
                    {
                        iCount += fileStream.Read(bSendFile, 0, AskByteCount);
                        TcpConnect.Send(bSendFile, AskByteCount, SocketFlags.None);
                    }
                    else
                    {
                        int iLast = fileStream.Read(bSendFile, 0, (int)(FileInfo.Length - iCount));
                        iCount += TcpConnect.Send(bSendFile, iLast, SocketFlags.None);
                        if (iCount==0)
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                }


                if (pbShow!=null)
                {
                    if (FileInfo.Length!=0)
                    {
                        pbShow(1000, (int)(iCount * 1000 / FileInfo.Length), FileInfo.Name);
                    } 
                }
      
            } while (iCount != FileInfo.Length);
            fileStream.Dispose();

            return true;
        }


        public void SendFilesItem(string IPSentTo, SendFileInfo[] sFiles, ProgressBarShow pbShow)
        {
            string Commond=string.Empty;
            foreach (SendFileInfo sFile in sFiles)
            {

                FileInfo fileInfo = new FileInfo(sFile.Path);
                if (fileInfo.Attributes != FileAttributes.Directory)
                {
                    IPMsgFileType FileType = GetTypeFromInfo(fileInfo);
                    Commond = sFile.FileNo + ":" + fileInfo.Name + ":" + fileInfo.Length.ToString("X") + ":" + fileInfo.LastWriteTime.TimeOfDay.Ticks.ToString("X") + ":" + ((int)FileType).ToString() + ":";
                    SendCommonMsg(IPMsgType.IPMSG_FILEATTACHOPT | IPMsgType.IPMSG_SENDMSG, IPSentTo, "\0" + Commond, sFile.PackNo);
                }
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(sFile.Path);
                    Commond = sFile.FileNo + ":" + dirInfo.Name + ":00:" + dirInfo.LastWriteTime.TimeOfDay.Ticks.ToString("X") + ":2:";
                    SendCommonMsg(IPMsgType.IPMSG_FILEATTACHOPT | IPMsgType.IPMSG_SENDMSG, IPSentTo, "\0" + Commond, sFile.PackNo);
                }

            }

            List<SendFileInfo> lFiles = new List<SendFileInfo>();
            do
            {
                tcpListener = new TcpListener(IPAddress.Any, MyPoint);
                try
                {
                    tcpListener.Start();
                    
                }
                catch (Exception)
                {
                    return;
                }
                

                byte[] bAsk = new byte[1024];
                try
                {
                    TcpConnect = tcpListener.AcceptSocket();
                    TcpConnect.Receive(bAsk);
                }
                catch (Exception)
                {
                    if (TcpConnect!=null)
                    {
                        TcpConnect.Dispose();
                    }                    
                    tcpListener.Stop();
                    return;
                }
                

                IPMSG AskMsg = new IPMSG(bAsk);
                if (AskMsg.IPMsgCommand == IPMsgType.IPMSG_GETFILEDATA)
                {
                    string[] sTips = AskMsg.sMsg.Split(':');
                    foreach (SendFileInfo sFileI in sFiles)
                    {
                        if (Int64.Parse(sFileI.PackNo).ToString("x").ToLower() == sTips[0].ToLower() && sFileI.FileNo == sTips[1])
                        {
                            FileInfo fileInfo = new FileInfo(sFileI.Path);
                            SendAFile(TcpConnect, fileInfo, pbShow);
                        }
                        else lFiles.Add(sFileI);
                    }
                }
                else if (AskMsg.IPMsgCommand == IPMsgType.IPMSG_GETDIRFILES)
                {
                    string[] sTips = AskMsg.sMsg.Split(':');
                    foreach (SendFileInfo sFileI in sFiles)
                    {
                        if (Int64.Parse(sFileI.PackNo).ToString("x").ToLower() == sTips[0].ToLower() && sFileI.FileNo == sTips[1])
                        {
                            SendADirectory(pbShow, TcpConnect, sFileI.Path);
                        }
                        else lFiles.Add(sFileI);
                    }
                }
                sFiles = lFiles.ToArray();
                lFiles.Clear();
                TcpConnect.Dispose();
                tcpListener.Stop();           
            } while (sFiles.Length != 0);
        }

        public void BroadCastFilesItem(string IPSentTo, SendFileInfo[] sFiles, ProgressBarShow pbShow)
        {
            bool bTimeOut = false;
            Thread TimeControl = new Thread(new ThreadStart(delegate() 
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(60*1000);
                    }
                    bTimeOut = true;
                    this.Collect();
                }));
            TimeControl.IsBackground = true;
            TimeControl.Start();


            string Commond = string.Empty;
            foreach (SendFileInfo sFile in sFiles)
            {
                FileInfo fileInfo = new FileInfo(sFile.Path);
                if (fileInfo.Attributes != FileAttributes.Directory)
                {
                    IPMsgFileType FileType = GetTypeFromInfo(fileInfo);
                    Commond = sFile.FileNo + ":" + fileInfo.Name + ":" + fileInfo.Length.ToString("X") + ":" + fileInfo.LastWriteTime.TimeOfDay.Ticks.ToString("X") + ":" + ((int)FileType).ToString() + ":";
                    SendCommonMsg(IPMsgType.IPMSG_FILEATTACHOPT | IPMsgType.IPMSG_SENDMSG, IPSentTo, "\0" + Commond, sFile.PackNo);
                }
                else
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(sFile.Path);
                    Commond = sFile.FileNo + ":" + dirInfo.Name + ":00:" + dirInfo.LastWriteTime.TimeOfDay.Ticks.ToString("X") + ":2:";
                    SendCommonMsg(IPMsgType.IPMSG_FILEATTACHOPT | IPMsgType.IPMSG_SENDMSG, IPSentTo, "\0" + Commond, sFile.PackNo);
                }

            }
            do
            {
                tcpListener = new TcpListener(IPAddress.Any, MyPoint);
                try
                {
                    tcpListener.Start();
                }
                catch (Exception)
                {
                    continue;
                }
                

                byte[] bAsk = new byte[1024];

                try
                {
                    TcpConnect = tcpListener.AcceptSocket();
                    TcpConnect.Receive(bAsk);
                }
                catch (Exception)
                {
                    tcpListener.Stop();
                    TcpConnect.Close();
                    continue;
                }
                

                IPMSG AskMsg = new IPMSG(bAsk);
                if (AskMsg.IPMsgCommand == IPMsgType.IPMSG_GETFILEDATA)
                {
                    string[] sTips = AskMsg.sMsg.Split(':');
                    foreach (SendFileInfo sFileI in sFiles)
                    {
                        if (Int64.Parse(sFileI.PackNo).ToString("x").ToLower() == sTips[0].ToLower() && sFileI.FileNo == sTips[1])
                        {
                            FileInfo fileInfo = new FileInfo(sFileI.Path);
                            SendAFile(TcpConnect, fileInfo, pbShow);
                        }
                    }
                }
                else if (AskMsg.IPMsgCommand == IPMsgType.IPMSG_GETDIRFILES)
                {
                    string[] sTips = AskMsg.sMsg.Split(':');
                    foreach (SendFileInfo sFileI in sFiles)
                    {
                        if (Int64.Parse(sFileI.PackNo).ToString("x").ToLower() == sTips[0].ToLower() && sFileI.FileNo == sTips[1])
                        {
                            SendADirectory(pbShow, TcpConnect, sFileI.Path);
                        }
                    }
                }
                tcpListener.Stop();
            } while (!bTimeOut);
        }

        private static IPMsgFileType GetTypeFromInfo(FileInfo fileInfo)
        {
            IPMsgFileType FileType;
            switch (fileInfo.Attributes)
            {
                case FileAttributes.Directory:
                    FileType = IPMsgFileType.IPMSG_FILE_DIR;
                    break;
                case FileAttributes.Hidden:
                    FileType = IPMsgFileType.IPMSG_FILE_HIDDENOPT;
                    break;
                case FileAttributes.System:
                    FileType = IPMsgFileType.IPMSG_FILE_SYSHIDE;
                    break;
                case FileAttributes.Archive:
                case FileAttributes.Compressed:
                case FileAttributes.Encrypted:
                case FileAttributes.Normal:
                case FileAttributes.NotContentIndexed:
                case FileAttributes.Offline:
                case FileAttributes.ReadOnly:
                case FileAttributes.ReparsePoint:
                case FileAttributes.SparseFile:
                case FileAttributes.Temporary:
                default:
                    FileType = IPMsgFileType.IPMSG_FILE_REGULAR;
                    break;
            }
            return FileType;
        }

        private void SendADirectory(ProgressBarShow pbShow, Socket TcpConnect, string DirPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
            SendDirHeadMsg(TcpConnect,directoryInfo.Name);
            FileInfo[] fileInfos = directoryInfo.GetFiles();
            foreach (FileInfo fileInfoItem in fileInfos)
            {
                SendFileHeadMsg(TcpConnect,fileInfoItem);
                if (!SendAFile(TcpConnect, fileInfoItem, pbShow))
                {
                    return;
                } 
            }
            DirectoryInfo[] SubDirs = directoryInfo.GetDirectories();
            foreach (DirectoryInfo SubDir in SubDirs)
            {
                SendADirectory( pbShow,  TcpConnect,  SubDir.FullName);
            }
            SendDirBackMsg(TcpConnect);
        } 

        #endregion

        #region ReceiveMsg、SendMsg

        private void ThreadReceiveMsg()
        {                                          
            IPEndPoint IPEndFrom = new IPEndPoint(IPAddress.Any,0);
            IPAddress IPFrom;
            IPMSG ipMsgRec;

            while (!TQExit)
            {
                try
                {
                    byte[] bRecMsg = TQClient.Receive(ref IPEndFrom);

                    ipMsgRec = new IPMSG(bRecMsg);

                }
                catch (Exception)
                {
                    continue;
                }

                IPFrom = IPEndFrom.Address;
                if (TQExit)
                {
                    return;
                }else
                switch (ipMsgRec.IPMsgCommand)
                {
                    case IPMsgType.IPMSG_NOOPERATION:
                        break;       
                    case IPMsgType.IPMSG_BR_ENTRY:
                        ListUsersAdd(ipMsgRec.Name, ipMsgRec.PCName, IPFrom.ToString());
                        SendCommonMsg(IPMsgType.IPMSG_ANSENTRY, IPFrom.ToString(), ipMsgRec.PackNo);
                        break;
                    case IPMsgType.IPMSG_BR_EXIT:
                        ListUsersDelete(ipMsgRec.Name, ipMsgRec.PCName, IPFrom.ToString());
                        break;
                    case IPMsgType.IPMSG_ANSLIST:
                    case IPMsgType.IPMSG_ANSENTRY:
                        ListUsersAdd(ipMsgRec.Name, ipMsgRec.PCName, IPFrom.ToString());
                        break;
                    case IPMsgType.IPMSG_BR_ABSENCE:
                        break;
                    case IPMsgType.IPMSG_BR_ISGETLIST:
                        break;
                    case IPMsgType.IPMSG_OKGETLIST:
                        break;
                    case IPMsgType.IPMSG_GETLIST:
                        SendCommonMsg(IPMsgType.IPMSG_ANSLIST, IPFrom.ToString(), "Xing\0自个一组");
                        break;

                    case IPMsgType.IPMSG_SENDMSG:
                        ListUsersAdd(ipMsgRec.Name, ipMsgRec.PCName, IPFrom.ToString());
                        SendCommonMsg(IPMsgType.IPMSG_RECVMSG, IPFrom.ToString(), ipMsgRec.PackNo);
                        break;
                    case IPMsgType.IPMSG_RECVMSG:
                        break;
                    case IPMsgType.IPMSG_READMSG:
                        break;
                    case IPMsgType.IPMSG_DELMSG:
                        break;
                    case IPMsgType.IPMSG_ANSREADMSG:
                        break;
                    case IPMsgType.IPMSG_GETINFO:
                        break;
                    case IPMsgType.IPMSG_SENDINFO:
                        break;
                    case IPMsgType.IPMSG_GETABSENCEINFO:
                        break;
                    case IPMsgType.IPMSG_SENDABSENCEINFO:
                        break;
                    case IPMsgType.IPMSG_GETFILEDATA:
                        break;
                    case IPMsgType.IPMSG_RELEASEFILES:
                        break;
                    case IPMsgType.IPMSG_GETDIRFILES:
                        break;
                    case IPMsgType.IPMSG_GETPUBKEY:
                        break;
                    case IPMsgType.IPMSG_ANSPUBKEY:
                        break;
                    case IPMsgType.IPMSG_ABSENCEOPT:
                        break;
                    case IPMsgType.IPMSG_WRITING:

                        break;
                    case IPMsgType.IPMSG_UNWRITING:

                        break;
                    case IPMsgType.IPMSG_CANCELFILE:

                        break;
                    case IPMsgType.IPMSG_FILEATTACHOPT:
                        SendCommonMsg(IPMsgType.IPMSG_RECVMSG, IPFrom.ToString(), ipMsgRec.PackNo); 
                        break;
                    default:
                        break;
                }
                if (RecvEvent != null)
                {
                    RecvEvent(IPFrom.ToString(), ipMsgRec);
                }
            }
        }

        public void SendOnlineMsg()
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, IPMsgType.IPMSG_BR_ENTRY, "Bing\0自个一组");
            TQClient.Send(bMsg, bMsg.Length, IPEndBroadcast);
        }

        public void SendOffLineMsg()
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, IPMsgType.IPMSG_BR_EXIT, null);
            TQClient.Send(bMsg, bMsg.Length, IPEndBroadcast);
        }

        public void SendCommonMsg(IPMsgType Msg, string sIPTo)
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, Msg, null);
            IPEndPoint IPTo = new IPEndPoint(IPAddress.Parse(sIPTo), 2425);
            TQClient.Send(bMsg, bMsg.Length, IPTo);
        }
        public void SendCommonMsg(IPMsgType Msg, string sIPTo, string sMsg)
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, Msg, sMsg);
            IPEndPoint IPTo = new IPEndPoint(IPAddress.Parse(sIPTo), 2425);
            TQClient.Send(bMsg, bMsg.Length, IPTo);
        }
        public void SendCommonMsg(IPMsgType Msg, string sIPTo, string sMsg ,string PackNo)
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, Msg, sMsg, PackNo);
            Console.WriteLine(Encoding.Default.GetString(bMsg));
            IPEndPoint IPTo = new IPEndPoint(IPAddress.Parse(sIPTo), 2425);
            TQClient.Send(bMsg, bMsg.Length, IPTo);
        }
        public void SendMsg(string Msg, string sIPTo)
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, IPMsgType.IPMSG_SENDMSG, Msg);
            IPEndPoint IPTo = new IPEndPoint(IPAddress.Parse(sIPTo), 2425);
            TQClient.Send(bMsg, bMsg.Length, IPTo);
        }
        public void SendRevcFileMsg(string sIPTo, string PackNo)
        {
            PackNo = int.Parse(PackNo).ToString("X");
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, IPMsgType.IPMSG_GETDIRFILES, PackNo + ":0:");
            IPEndPoint IPTo = new IPEndPoint(IPAddress.Parse(sIPTo), 2425);
            TQClient.Send(bMsg, bMsg.Length, IPTo);
        }

        public void BroadCastMsg(string Msg)
        {
            byte[] bMsg = IPMSG.MsgToByte(pQXinUserMe, IPMsgType.IPMSG_SENDMSG, Msg);
            TQClient.Send(bMsg, bMsg.Length, IPEndBroadcast);
        }

        public void SendFileHeadMsg(Socket TcpConnect,FileInfo fileInfo)
        {
            string sFileInfoMsg;
            byte[] bFileHeadMsg;
            string sFileInfo = fileInfo.Name + ":"+ fileInfo.Length.ToString("X") + ":" + ((int)GetTypeFromInfo(fileInfo)).ToString()+":";
            int iFileLength = Encoding.Default.GetBytes(sFileInfo).Length;
            if (iFileLength + 5 <= 65535)
            {
                bFileHeadMsg = new byte[iFileLength + 5];
                sFileInfoMsg = (iFileLength + 5).ToString("X4") + ":" + sFileInfo;
            }
            else
            {
                bFileHeadMsg = new byte[iFileLength + 9];
                sFileInfoMsg = (iFileLength + 9).ToString("X8") + ":" + sFileInfo;
            }
            try
            {
                TcpConnect.Send(Encoding.Default.GetBytes(sFileInfoMsg));
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SendDirHeadMsg(Socket TcpConnect,string DirShortName)
        {
            string sDirInfoMsg;                                                           
            byte[] bDirHeadMsg ;
            string sDirInfo = DirShortName + ":000000000:"+(int)IPMsgFileType.IPMSG_FILE_DIR;
            int iDirLength = Encoding.Default.GetBytes(sDirInfo).Length;
            if (iDirLength+ 5 <= 65535)
            {
                bDirHeadMsg = new byte[iDirLength+ 5];
                sDirInfoMsg = (iDirLength+ 5).ToString("X4") + ":" + sDirInfo;
            }
            else
            {
                bDirHeadMsg = new byte[iDirLength+ 9];
                sDirInfoMsg = (iDirLength+ 9).ToString("X8") + ":" + sDirInfo;
            }
            try
            {
                TcpConnect.Send(Encoding.Default.GetBytes(sDirInfoMsg));
            }
            catch (Exception)
            {
                return;
            }
        }

        public void SendDirBackMsg(Socket TcpConnect)
        {
            string sDirInfo = "000A:.:0:" + (int)IPMsgFileType.IPMSG_FILE_RETPARENT;
            try
            {
                TcpConnect.Send(Encoding.Default.GetBytes(sDirInfo));
            }
            catch (Exception)
            {
                return;
            }
        }
	    #endregion

        #region Test
        public void TestSendMsg(string msg)
        {
            byte[] bMsg = Encoding.Default.GetBytes(msg);
            TQClient.Send(bMsg, bMsg.Length, IPEndBroadcast);
        } 
        #endregion



        #region IDisposable Members

        public void Dispose()
        {
            Collect();
            ThreRece.Abort();
            TQExit = true;
            TQClient.Close();
        }

        #endregion
    }

    public enum IPMsgType
    {
        IPMSG_NOOPERATION=0x0,     //不进行任何操作
        IPMSG_BR_ENTRY,        // 用户上线（以广播方式登陆） 
        IPMSG_BR_EXIT,         //用户下线（以广播方式退出）
        IPMSG_ANSENTRY,        //应答用户上线   
        IPMSG_BR_ABSENCE,      //改为缺席模式   
        IPMSG_BR_ISGETLIST=0x10,    //寻找有效的可以发送用户列表的成员
        IPMSG_OKGETLIST,    //   通知用户列表已经获得
        IPMSG_GETLIST,        // 用户列表发送请求
        IPMSG_ANSLIST ,       // 应答用户列表发送请求   
        IPMSG_SENDMSG=0x20,       //  发送消息
        IPMSG_RECVMSG ,        //消息接受验证
        IPMSG_READMSG=0x30 ,        //消息打开通知
        IPMSG_DELMSG ,         //消息丢弃通知
        IPMSG_ANSREADMSG ,     //消息打开确认通知（version-8中添加）
        IPMSG_GETINFO=0x40,         //获得IPMSG版本信息
        IPMSG_SENDINFO,        //发送IPMSG版本信息
        IPMSG_GETABSENCEINFO=0x50,     //获得缺席信息
        IPMSG_SENDABSENCEINFO,    //发送缺席信息
        IPMSG_GETFILEDATA =0x60,    //文件传输请求
        IPMSG_RELEASEFILES ,   //丢弃附加文件
        IPMSG_GETDIRFILES ,    //附着同级文件请求
        IPMSG_GETPUBKEY=0x72,          //获得RSA公钥
        IPMSG_ANSPUBKEY,          //应答RSA公钥　2)   选项位(32位命令字的高24位)
        IPMSG_ABSENCEOPT ,    　//缺席模式(Member recognition command)
        IPMSG_WRITING=0x79,     //用户正在输入
        IPMSG_UNWRITING=0x7A,   //用户结束输入
        IPMSG_FILEATTACHOPT = 0x00200100,    //附加文件
        IPMSG_CANCELFILE = 0x00000006,    //取消发送文件
     }

    public enum IPMsgFileType 
    {
        IPMSG_FILE_REGULAR = 0x00000001,
        IPMSG_FILE_DIR = 0x00000002,
        IPMSG_FILE_RETPARENT = 0x00000003,	// return parent directory
        IPMSG_FILE_SYMLINK = 0x00000004,
        IPMSG_FILE_SYSHIDE = 9001,
        IPMSG_FILE_HIDDENOPT = 1001
    }

    public class IPMSG
    {
        private string fVersion;
        private string fPackNo;
        private string fName;
        private string fPCName;
        private IPMsgType fIPMsgCommand;

        private string fsMsg;

        private List<string> fFileNo = new List<string>();
        private List<string> fFileName = new List<string>();
        private List<string> fFileSize=new List<string>();
        private List<IPMsgFileType> fFileType = new List<IPMsgFileType>();

        #region Property
        public string PackNo
        {
            get { return fPackNo; }
            set { fPackNo = value; }
        }

        public string Name
        {
            get { return fName; }
            set { fName = value; }
        }

        public string PCName
        {
            get { return fPCName; }
            set { fPCName = value; }
        }

        public IPMsgType IPMsgCommand
        {
            get { return fIPMsgCommand; }
            set { fIPMsgCommand = value; }
        }

        public string sMsg
        {
            get { return fsMsg; }
            set { fsMsg = value; }
        }

        public List<string> FileNo
        {
            get { return fFileNo; }
            set { fFileNo = value; }
        }

        public List<string> FileName
        {
            get { return fFileName; }
            set { fFileName = value; }
        }

        public List<string> FileSize
        {
            get { return fFileSize; }
            set { fFileSize = value; }
        }

        public List<IPMsgFileType> FileType
        {
            get { return fFileType; }
            set { fFileType = value; }
        }


        #endregion

        #region Constructed Function
        public IPMSG(byte[] bMsgPack)
        {
            string sMsgPack = Encoding.Default.GetString(bMsgPack);
            

            string[] sMsgs = sMsgPack.Split(':');
            if (sMsgs.Length < 5)
            {
                throw new Exception("数据包不正确");
            }
            fVersion = sMsgs[0];
            fPackNo = sMsgs[1];
            fName = sMsgs[2];
            fPCName = sMsgs[3];

            //begin to translate the type of IPMsg
            int iMsg = int.Parse(sMsgs[4]);
            int iTemp = iMsg & (int)0x00FFF000;
            iTemp |= 0x100;
            if (iTemp != (int)IPMsgType.IPMSG_FILEATTACHOPT)
            {
                //Common Msg Without File 
                iTemp = iMsg & 0x000000FF;
                fIPMsgCommand = (IPMsgType)iTemp;
                if (sMsgs.Length >= 6)
                {
                    fsMsg = sMsgs[5];
                    for (int i = 6; i < sMsgs.Length; i++)
                    {
                        fsMsg += ":"+sMsgs[i];
                    }    
                }
                return;
            }
            else        //Packer File Message
            {
                fIPMsgCommand = (IPMsgType)iTemp;

                sMsgs = sMsgPack.Split('\0');
                char[] splitChars = { ':', '\a' };
                sMsgs = sMsgs[1].Split(splitChars,StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < sMsgs.Length; i+=5)
                {
                    fFileNo.Add(sMsgs[i]);
                    fFileName.Add(sMsgs[i + 1]);
                    fFileSize.Add(sMsgs[i + 2]);


                    switch (sMsgs[i + 4])
                    {
                        case "1":
                            fFileType.Add(IPMsgFileType.IPMSG_FILE_REGULAR);
                            break;
                        case "2":
                            fFileType.Add(IPMsgFileType.IPMSG_FILE_DIR);
                            break;
                        case "3":
                            fFileType.Add(IPMsgFileType.IPMSG_FILE_RETPARENT);
                            break;
                        case "4":
                            fFileType.Add(IPMsgFileType.IPMSG_FILE_SYMLINK);
                            break;
                        default:
                            break;
                    }
                    
                }
               

            }
        }


        public IPMSG(IPMsgType IPmsg, string PackNo,string FileNo, string FileName, string FileSize, IPMsgFileType FileType)
        {
            fPackNo = PackNo;
            fIPMsgCommand = IPmsg;
            fFileNo.Add(FileNo);
            fFileName.Add(FileName);
            fFileSize.Add(FileSize);
            fFileType.Add(FileType);

        } 
        #endregion

        public string GetMsgString()
        {
            return fsMsg;
        }

        #region External Methods

        public static byte[] MsgToByte(QXinUser QXinUserMe, IPMsgType IPmsg, string Msg)
        {
            string sMsgHead = "1_lbt4_0#128#001F161FD5AC#1195#0#0#2.5a:" + TQRunner.NewPackNo + ":" + QXinUserMe.Name + ":" + QXinUserMe.PCName + ":";

            if (Msg == string.Empty || Msg == null)
            {
                return Encoding.Default.GetBytes(sMsgHead + ((int)IPmsg).ToString());
            }
            else
            {
                return Encoding.Default.GetBytes(sMsgHead + ((int)IPmsg).ToString() + ":" + Msg);
            }
        }

        public static byte[] MsgToByte(QXinUser QXinUserMe, IPMsgType IPmsg, string Msg ,string PackNo)
        {
            string sMsgHead = "1_lbt4_0#128#001F161FD5AC#1195#0#0#2.5a:" + PackNo + ":" + QXinUserMe.Name + ":" + QXinUserMe.PCName + ":";

            if (Msg == string.Empty || Msg == null)
            {
                return Encoding.Default.GetBytes(sMsgHead + ((int)IPmsg).ToString());
            }
            else
            {
                return Encoding.Default.GetBytes(sMsgHead + ((int)IPmsg).ToString() + ":" + Msg);
            }
        }
        #endregion


    }

    public class SendFileInfo
    {
        private string fPath;
        private string fPackNo;
        private string fFileNo;

        public string Path
        {
            get { return fPath; }
            set { fPath = value; }
        }

        public string PackNo
        {
            get { return fPackNo; }
            set { fPackNo = value; }
        }  

        public string FileNo
        {
            get { return fFileNo; }
            set { fFileNo = value; }
        }

        public SendFileInfo(string Path,string PackNo,string FileNo)
        {
            fPath = Path;
            fPackNo = PackNo;
            fFileNo = FileNo;
        }
    }


   
}
