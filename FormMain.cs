using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.Threading;
using System.Text.RegularExpressions;

namespace TCS
{
    public partial class FormMain : Form
    {
        private SoundPlayer SoundMsg = new SoundPlayer(Properties.Resources.msg);
        public FormMain()
        {
            InitializeComponent();
        }
        private TQRunner TQ;

        internal TQRunner TQOper
        {
            get { return TQ; }
            set { TQ = value; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            cbState.SelectedIndex = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            try
            {
                TQ = new TQRunner();
            }
            catch (Exception)
            {
                MessageBox.Show("端口被占用，TSC已在运行或者用类型的软件例如飞秋飞鸽等在运行。");
                Application.Exit();
                return;
            }
            
            TQ.RecvEvent += RecvMsg;
            TQ.UpdateListEvent += UpdateList;


            ImageList myImageList = new ImageList();
            myImageList.Images.Add(Properties.Resources.Plus);
            myImageList.Images.Add(Properties.Resources.Minus);
            myImageList.Images.Add(notifyIcon.Icon);
            tvUsersList.ImageList = myImageList;

        }
        

        public ListView lvFindResultContrl
        {
            get 
            {
                return this.lvFindResult;
            }
        }

        #region TreeViewOper
        public delegate void TVAddDelegate(QXinUser item);

        public void TreeViewAdd(QXinUser item)
        {
            //本地账户
            QXinUserUI NewItem = new QXinUserUI(item.Name, item.PCName, item.IPAddress);
            NewItem.UserIcon = notifyIcon.Icon;   //可以自定义图标
            TreeNode NodeNewAdd = tvUsersList.Nodes[0].Nodes.Add(NewItem.Name + "(" + NewItem.IPAddress + ")");
            NodeNewAdd.Tag = NewItem;
            NodeNewAdd.ImageIndex = 2;
            NodeNewAdd.SelectedImageIndex = 2;
            NodeNewAdd.StateImageIndex = 2;

           

        } 

        public QXinUserUI TreeViewFind(string sIPAddr)
        {         
            foreach (TreeNode Node in tvUsersList.Nodes[0].Nodes)
            {
                QXinUserUI User = (QXinUserUI)Node.Tag;
                if (User.IPAddress == sIPAddr)
                {
                    return User;
                }
            }
            return null;
       }
        public QXinUserUI TreeViewFindEx(string aPart)
        {
            
            foreach (TreeNode Node in tvUsersList.Nodes[0].Nodes)
            {
                QXinUserUI User = (QXinUserUI)Node.Tag;
                if (Regex.IsMatch(User.IPAddress,@"\d+\.\d+\.\d+\."+aPart))
                {
                    return User;
                }
            }
            return null;
        }

        public void UpdateList(List<QXinUser> Users)
        {
            tvUsersList.Nodes[0].Nodes.Clear();
            foreach (QXinUser item in Users)
            {
                tvUsersList.Invoke(new TVAddDelegate(TreeViewAdd), new object[] { item });
            }
            if (tvUsersList.Nodes[0].Nodes.Count != 0)
            {
                tvUsersList.Nodes[0].Text = "我的好友(" + tvUsersList.Nodes[0].Nodes.Count + ")";
            }
            else tvUsersList.Nodes[0].Text = "我的好友";
           

        }
        #endregion


        public void RecvMsg(string IPFrom, IPMSG MsgPack)
        {
            switch (MsgPack.IPMsgCommand)
            {
                case IPMsgType.IPMSG_NOOPERATION:
                    break;
                case IPMsgType.IPMSG_BR_ENTRY:
                    {
                        QXinUserUI NewFind = TreeViewFind(IPFrom);
                        if (NewFind != null && NewFind.FormCommun != null && NewFind.FormCommun.Visible!=false)
                        {
                            NewFind.FormCommun.UserEnterMsg();
                        }
                        break;
                    }
                case IPMsgType.IPMSG_BR_EXIT:
                    {
                        QXinUserUI NewFind = TreeViewFind(IPFrom);
                        if (NewFind != null && NewFind.FormCommun != null && NewFind.FormCommun.Visible != false)
                        {
                            NewFind.FormCommun.UserExitMsg();
                        }
                    }
                    break;
                case IPMsgType.IPMSG_ANSENTRY:
                    break;
                case IPMsgType.IPMSG_BR_ABSENCE:
                    break;
                case IPMsgType.IPMSG_BR_ISGETLIST:
                    break;
                case IPMsgType.IPMSG_OKGETLIST:
                    break;
                case IPMsgType.IPMSG_GETLIST:
                    break;
                case IPMsgType.IPMSG_ANSLIST:
                    break;
                case IPMsgType.IPMSG_SENDMSG:
                case IPMsgType.IPMSG_FILEATTACHOPT:
                    {
                        if (MsgPack.IPMsgCommand == IPMsgType.IPMSG_SENDMSG || MsgPack.IPMsgCommand == IPMsgType.IPMSG_FILEATTACHOPT)
                        {
                            SoundMsg.Play();
                        }
                        QXinUserUI User = TreeViewFind(IPFrom);
                        if (User == null)
                        {
                            return;
                        }
                        if (User.FormCommun != null && User.FormCommun.Visible)
                        {
                            User.CallShowRecvEvent(MsgPack);
                        }
                        else
                        {
                            if (MsgPack.IPMsgCommand == IPMsgType.IPMSG_FILEATTACHOPT)
                            {
                                this.Invoke(new ThreadStart(delegate()
                                {
                                    User.FormCommun = new FormCommunicate(TQ, User);
                                    User.FormCommun.Show();
                                    User.FormCommun.Focus();
                                    User.CallShowRecvEvent(MsgPack);
                                })
                                );

                                return;
                            }
                            if (MsgPack.IPMsgCommand == IPMsgType.IPMSG_SENDMSG)
                            {
                                notifyIcon.ShowBalloonTip(1000, "收到" + User.Name + "的信息", MsgPack.sMsg, ToolTipIcon.None);
                                User.InitMsgs.Add(User.Name + "  " + DateTime.Now.ToLongTimeString() + "\n  " + MsgPack.sMsg.Split('\0')[0]);
                                if (!StaWaitUsers.Contains(User))
                                {
                                    StaWaitUsers.Push(User);
                                    notifyIcon.Tag = User;
                                    this.Invoke(new MethodInvoker(Notifytimer.Start));
                                }

                            }
                        }
                    }
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
                case IPMsgType.IPMSG_CANCELFILE:
                    {
                        QXinUserUI NewFind = TreeViewFind(IPFrom);
                        if (NewFind != null && NewFind.FormCommun != null && NewFind.FormCommun.Visible != false)
                        {
                            NewFind.FormCommun.ShowMsg((QXinUser)NewFind, MsgPack);
                        }
                        
                    }
                    break;
                case IPMsgType.IPMSG_ABSENCEOPT:
                    break;
                case IPMsgType.IPMSG_WRITING:
                    {
                        QXinUserUI NewFind = TreeViewFind(IPFrom);
                        if (NewFind != null && NewFind.FormCommun != null && NewFind.FormCommun.Visible != false)
                        {
                            NewFind.FormCommun.ShowMsg((QXinUser)NewFind, MsgPack);
                        }
                        
                    }
                    break;
                case IPMsgType.IPMSG_UNWRITING:
                    {
                        QXinUserUI NewFind = TreeViewFind(IPFrom);
                        if (NewFind != null && NewFind.FormCommun != null && NewFind.FormCommun.Visible != false)
                        {
                            NewFind.FormCommun.ShowMsg((QXinUser)NewFind, MsgPack);
                        }
                    }
                    break;

                default:
                    break;
            }
          
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (TQ!=null)
            {
                TQ.SendOffLineMsg();
                TQ.Dispose();
            }    
        }


        private void tvUsersList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag==null)
            {
                return;
            }
            if (((QXinUserUI)e.Node.Tag).FormCommun != null&&((QXinUserUI)e.Node.Tag).FormCommun.Visible)
            {
                ((QXinUserUI)e.Node.Tag).FormCommun.Focus();
                return;
            }
            else
            {
                ((QXinUserUI)e.Node.Tag).FormCommun = new FormCommunicate(TQ, (QXinUserUI)e.Node.Tag);
                ((QXinUserUI)e.Node.Tag).FormCommun.Show();
            }
        }


        private void cbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TQOper==null)
            {
                return;
            }
            switch (cbState.SelectedIndex)
            {
                case 0:
                    TQOper.SendOnlineMsg();
                    break;
                case 1:
                    TQOper.SendOffLineMsg();
                    break;
                default:     
                    break;
            }
        }

        private void controlFind1_Load(object sender, EventArgs e)
        {

        }

        private void lvFindResult_Leave(object sender, EventArgs e)
        {
            if (this.controlFind1.Focused==false)
            {
                lvFindResultContrl.Visible = false;
            }
        }

        private void lvFindResult_DoubleClick(object sender, EventArgs e)
        {
            if (lvFindResult.SelectedItems.Count==0)
            {
                return;
            }
            QXinUserUI SelectItem = (QXinUserUI)lvFindResult.SelectedItems[0].Tag;
            if (SelectItem.FormCommun != null && (SelectItem.FormCommun.Visible))
            {
                SelectItem.FormCommun.Focus();
                return;
            }
            else
            {
                SelectItem.FormCommun = new FormCommunicate(TQ, SelectItem);
                SelectItem.FormCommun.Show();
            }
        }

        #region NotifyOper
        Stack<QXinUserUI> StaWaitUsers = new Stack<QXinUserUI>();
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (StaWaitUsers.Count != 0)
            {
                QXinUserUI OpenUser = StaWaitUsers.Pop();
                OpenUser.FormCommun = new FormCommunicate(TQ, OpenUser, OpenUser.InitMsgs.ToArray());
                OpenUser.FormCommun.Show();
                notifyIcon.Tag = OpenUser;
                Notifytimer.Enabled = false;
                notifyIcon.Icon = ((QXinUserUI)notifyIcon.Tag).UserIcon;
            }
            else 
            {
                this.WindowState = FormWindowState.Normal;
                this.TopMost = true;
                this.Focus();
                this.TopMost = false;
            }
        }

        static bool shark = false;
        private void Notifytimer_Tick(object sender, EventArgs e)
        {
            if (shark != false)
            {
                notifyIcon.Icon = Properties.Resources.Icon1;
                shark = false;
            }
            else
            {
                notifyIcon.Icon = ((QXinUserUI)notifyIcon.Tag).UserIcon;
                shark = true;
            }

        } 
        #endregion

        private void btBroadCast_Click(object sender, EventArgs e)
        {
            FormCommunicate bcForm = FormCommunicate.BroatCastForm(TQ);
            if (bcForm!=null)
            {
                bcForm.Show();
            }    
        }

        private void tvUsersList_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            e.Node.SelectedImageIndex = 1;
            e.Node.StateImageIndex = 1;
        }

        private void tvUsersList_AfterCollapse(object sender, TreeViewEventArgs e)
        {       
            e.Node.ImageIndex = 0;         
            e.Node.SelectedImageIndex = 0;    
            e.Node.StateImageIndex = 0;
        }

        private Size sLastSize = new Size();
        private bool s1 = false;



        private void FormMain_Move(object sender, EventArgs e)
        {
            if (this.Location.X < 0 && s1 == false && this.Location.X!=-32000)
            {
                s1 = true;
                sLastSize = this.Size;
                this.Size = new Size(277, Screen.PrimaryScreen.WorkingArea.Height);
                this.Location = new Point(-268, 0);
            }else
                if (this.Location.X <= Screen.PrimaryScreen.WorkingArea.Width - 277 && this.Location.X > 0 && s1)
                {
                    s1 = false;
                    this.Size = sLastSize;
                }

            if (this.Location.X > Screen.PrimaryScreen.WorkingArea.Width - 277 && s1 == false && this.Location.X != -32000)
            {
                s1 = true;
                sLastSize = this.Size;
                this.Size = new Size(277, Screen.PrimaryScreen.WorkingArea.Height);
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 9 , 0);
            }
            else
                if (this.Location.X >= 277 && this.Location.X < Screen.PrimaryScreen.WorkingArea.Width - 277 && s1)
                {
                    s1 = false;
                    this.Size = sLastSize;
                }
        }

        private void FormMain_MouseEnter(object sender, EventArgs e)
        {
            if (this.Location.X == -268&& this.Location.Y == 0)
            {
                this.Location = new Point(-1, 0);
            }else
                if (this.Location.X == Screen.PrimaryScreen.WorkingArea.Width - 9 && this.Location.Y == 0)
                {
                    this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 276, 0);
                }
        }

        private void FormMain_MouseLeave(object sender, EventArgs e)
        {
            if (this.Location.X == -1 && this.Location.Y == 0 && MousePosition.X >= 262)
            {
                this.Location = new Point(-268, 0);
            }
            else if (this.Location.X == Screen.PrimaryScreen.WorkingArea.Width - 276 && this.Location.Y == 0 && MousePosition.X <=Screen.PrimaryScreen.WorkingArea.Width - 265)
            {
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 9, 0);
            }
        }

    }
}
