using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Media;

namespace TCS
{
    public partial class FormCommunicate : Form
    {
        private static bool IsBroadCastCreate = false;
        public static FormCommunicate BroatCastForm(TQRunner pTQ)
        {
            if (!IsBroadCastCreate)
            {
                return new FormCommunicate(pTQ);
            }
            else return null;
            
        }
        private TQRunner TQ;
        private QXinUser UserSendTo;
        private string IPSendTo;
        private SoundPlayer SoundDownload = new SoundPlayer(Properties.Resources.download_complete);

        private FormCommunicate(TQRunner pTQ)
        {
            this.TQ = pTQ;
            UserSendTo = new QXinUser("All", "所有人", TQ.IPBroadCast);
            this.IPSendTo = TQ.IPBroadCast;
            InitializeComponent();
        }
        public FormCommunicate(TQRunner pTQ, QXinUser pUserSendTo)
        {
            UserSendTo = pUserSendTo;
            this.IPSendTo = pUserSendTo.IPAddress;
            this.TQ = pTQ;
            ((QXinUserUI)pUserSendTo).eventShowRecv += ShowMsg;
            InitializeComponent();
        }
        public FormCommunicate(TQRunner pTQ, QXinUser pUserSendTo ,string[] InitMsgs)
        {
            UserSendTo = pUserSendTo;
            this.IPSendTo = pUserSendTo.IPAddress;
            this.TQ = pTQ;
            ((QXinUserUI)pUserSendTo).eventShowRecv += ShowMsg;
            InitializeComponent();
            foreach (string Msg in InitMsgs)
            {
                rtbRecv.Text += Msg + "\n";
            }
            
        }
        public void ShowMsg(QXinUser IPFrom, IPMSG pMsg)
        {
            switch (pMsg.IPMsgCommand)
            {
                case IPMsgType.IPMSG_ANSENTRY:
                    break;
                case IPMsgType.IPMSG_BR_ABSENCE:
                    break;
                case IPMsgType.IPMSG_BR_ISGETLIST:
                    break;
                case IPMsgType.IPMSG_OKGETLIST:
                    break;
                case IPMsgType.IPMSG_SENDMSG:
                    Thread.Sleep(100);
                    rtbRecv.Text += IPFrom.Name + "  " + DateTime.Now.ToLongTimeString() + "\n  " + pMsg.sMsg;
                    rtbRecv.Text += "\n";
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
                case IPMsgType.IPMSG_CANCELFILE:

                    lvFiles.Items.Clear();

                    break;
                case IPMsgType.IPMSG_WRITING:
                    this.Text = "与" + UserSendTo.Name + "在交谈中 -- 正在输入";
                    break;
                case IPMsgType.IPMSG_UNWRITING:
                    this.Text = "与" + UserSendTo.Name + "在交谈中";
                    break;
                case IPMsgType.IPMSG_FILEATTACHOPT:   
                    for (int i = 0; i < pMsg.FileName.Count; i++)
                    {
                        IPMSG NewPag = new IPMSG(IPMsgType.IPMSG_FILEATTACHOPT,pMsg.PackNo, pMsg.FileNo[i], pMsg.FileName[i], pMsg.FileSize[i], pMsg.FileType[i]);
                        ListViewItem NewItem = lvFiles.Items.Add(pMsg.FileName[i]);
                        if (pMsg.FileType[i]!=IPMsgFileType.IPMSG_FILE_DIR)
                        {
                            long Length = Convert.ToInt64(pMsg.FileSize[i], 16);
                            if (Length / 1024 / 1024 > 0)
                            {
                                NewItem.SubItems.Add((Length / 1024 / 1024).ToString() + "MB");
                            }
                            else if (Length / 1024 > 0)
                            {
                                NewItem.SubItems.Add((Length / 1024).ToString() + "KB");
                            }
                            else NewItem.SubItems.Add(Length.ToString() + "B"); 
                        }
                        NewItem.Tag = NewPag;
                    }      
                    break;
                default:
                    break;
            } 
        }

        Thread ThreSend;
        private void btSend_Click(object sender, EventArgs e)
        {
            if (lvSendList.CheckedItems.Count == 0 ||lvSendList.Enabled==false)
            {
                if (rtbSend.Text.Length == 0)
                {
                    return;
                }
                TQ.SendMsg(rtbSend.Text, IPSendTo);
                rtbRecv.Text += TQ.QXinUserMe.Name + "  " + DateTime.Now.ToLongTimeString() + "\n  " + rtbSend.Text + "\n";
                rtbSend.Text = "";
                return;
            }
            else
            {
               ThreSend=new Thread(new ThreadStart(
                    delegate()
                    {
                        List<SendFileInfo> FileInfos = new List<SendFileInfo>();
                        foreach (ListViewItem item in lvSendList.CheckedItems)
                        {
                            FileInfos.Add( (SendFileInfo)item.Tag);
                        }
                        if (IPSendTo == TQ.IPBroadCast)
                        {
                            TQ.BroadCastFilesItem(IPSendTo, FileInfos.ToArray(), ProgressShow);
                        }
                        else TQ.SendFilesItem(IPSendTo, FileInfos.ToArray(), ProgressShow);         
                        
                        foreach (ListViewItem item in lvSendList.CheckedItems)
                        {
                            lvSendList.Items.Remove(item);
                        }
                        tbRecvTips.Text = "接收文件列表";
                        lvSendList.Enabled = true;
                        lbRecvTips.Visible = false;
                        pbRecvFile.Visible = false;
                    }  
                    ));
                ThreSend.IsBackground = true;
                ThreSend.Start();
                lvSendList.Enabled = false;
                lbRecvTips.Visible = true;
                pbRecvFile.Visible = true;
                pbRecvFile.Value = 0;
            }

        }

        public void UserExitMsg()
        { 
            lbName.Text+=" - 用户已下线";
        }
        public void UserEnterMsg()
        {
            lbName.Text = UserSendTo.Name + "(" + UserSendTo.IPAddress + ")";
        }
        private void FormCommunicate_Load(object sender, EventArgs e)
        {
            this.Text = "与" + UserSendTo.Name + "在交谈中";
            lbName.Text = UserSendTo.Name + "(" + UserSendTo.IPAddress + ")";
            lbSign.Text = UserSendTo.PCName;
        }


        private void btRejectFile_Click(object sender, EventArgs e)
        {
            while (lvFiles.CheckedItems.Count != 0)
            {
                lvFiles.Items.Remove(lvFiles.CheckedItems[0]);
            }
            if (btRejectFile.Text == "中断接收")
            {
                TQ.Collect();
                tbRecvTips.Text = "接收文件列表";
                btRecvFile.Enabled = true;
                btRejectFile.Text = "拒绝文件";
                lbRecvTips.Visible = false;
                pbRecvFile.Visible = false;
            }
            else
            {
                foreach (ListViewItem item in lvFiles.Items)
                {
                    item.Checked = true;
                }
            }
        }

        void ProgressShow (int max, int value ,string CurFileName)
        {
            pbRecvFile.Maximum = max;
            pbRecvFile.Value = value;
            tbRecvTips.Text = "接收" + Path.GetFileNameWithoutExtension(CurFileName);
            lbRecvTips.Text = "(" + value * 100 / max + "%)";                 
       }

        private void btRecvFile_Click(object sender, EventArgs e)
        {
            if (lvFiles.CheckedItems.Count != 0 && lvSendList.Enabled!=false)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                btRecvFile.Enabled = false;
                btRejectFile.Text = "中断接收";
                lbRecvTips.Visible = true;
                pbRecvFile.Visible = true;
                
                Thread ThreRecv = new Thread(new ThreadStart
                    (delegate()
                    {       
                        foreach (ListViewItem item in lvFiles.CheckedItems)
                        {
                            TQ.RecvFileFromIPMsg(IPSendTo, fbd.SelectedPath, (IPMSG)item.Tag, ProgressShow);
                            lvFiles.Items.Remove(item);
                        }
                        tbRecvTips.Text = "接收文件列表";
                        btRecvFile.Enabled = true;
                        btRejectFile.Text = "拒绝文件";
                        lbRecvTips.Visible = false;
                        pbRecvFile.Visible = false;
                        SoundDownload.Play();
                    }
                    )
                    );

                ThreRecv.Start();
            }
            else
                if (lvSendList.Enabled == false)
                {
                    MessageBox.Show("请先取消本地发送文件，再接收文件。");
                }

        }

        private void rtbSend_TextChanged(object sender, EventArgs e)
        {
            if (rtbSend.Text.Length!=0)
            {
                TQ.SendCommonMsg(IPMsgType.IPMSG_WRITING, IPSendTo);
            }
            else TQ.SendCommonMsg(IPMsgType.IPMSG_UNWRITING, IPSendTo);
            
        }

        private void FormCommunicate_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] DragFilePaths =(string[]) e.Data.GetData(DataFormats.FileDrop);
                

                foreach (string DragFilePath in DragFilePaths)
                {
                    FileInfo NewAddFile = new FileInfo(DragFilePath);
                    ListViewItem NewItem = lvSendList.Items.Add(NewAddFile.Name);
                    if (NewAddFile.Attributes != FileAttributes.Directory)
                    {
                        if (NewAddFile.Length / 1024 / 1024 > 0)
                        {
                            NewItem.SubItems.Add((NewAddFile.Length / 1024 / 1024).ToString() + "MB");
                        }
                        else if (NewAddFile.Length / 1024 > 0)
                        {
                            NewItem.SubItems.Add((NewAddFile.Length / 1024).ToString() + "KB");
                        }
                        else NewItem.SubItems.Add((NewAddFile.Length).ToString() + "B");
                    }
                    NewItem.Tag = new SendFileInfo(NewAddFile.FullName,TQRunner.NewPackNo, "0");

                }

            }
        }

        private void FormCommunicate_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }


        private void btCancelSend_Click(object sender, EventArgs e)
        {
            if (lvSendList.CheckedItems.Count!=0)
            {
                while (lvSendList.CheckedItems.Count!=0)
                {
                    lvSendList.Items.Remove(lvSendList.CheckedItems[0]);
                }
                if (ThreSend != null)
                {
                    ThreSend.Abort();
                    ThreSend = null;
                    GC.Collect();
                    TQ.Collect();   
                }
                tbRecvTips.Text = "接收文件列表";
                lvSendList.Enabled = true;
                lbRecvTips.Visible = false;
                pbRecvFile.Visible = false;
                TQ.SendCommonMsg(IPMsgType.IPMSG_CANCELFILE, IPSendTo);

            }else 
            {
                foreach (ListViewItem item in lvSendList.Items)
                {
                    item.Checked = true;
                } 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rtbSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode.ToString() == "Return")
            {
                btSend_Click(sender, null);
            }  
        }

        private void rtbRecv_TextChanged(object sender, EventArgs e)
        {
            this.rtbRecv.Select(this.rtbRecv.Text.Length, 0);
            rtbRecv.ScrollToCaret();
        }
    }
}
