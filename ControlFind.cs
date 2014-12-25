using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TCS
{
    public partial class ControlFind : UserControl
    {
        public ControlFind()
        {
            InitializeComponent();
        }



        private void tbName_Enter(object sender, EventArgs e)
        {
            tbName.BorderStyle = BorderStyle.Fixed3D;
            btCancel.Visible = true;

            FormMain MainForm = (FormMain)this.ParentForm;
            if (MainForm.lvFindResultContrl.Items.Count!=0)
            {
                MainForm.lvFindResultContrl.Visible = true;
            }          
        }
        private void tbName_Leave(object sender, EventArgs e)
        {
            tbName.BorderStyle = BorderStyle.None;
            btCancel.Visible = false;

            FormMain MainForm = (FormMain)this.ParentForm;
            if (MainForm.lvFindResultContrl.Focused==false)
            {
                MainForm.lvFindResultContrl.Visible = false;
            }
            
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            FormMain MainForm = (FormMain)this.ParentForm;
            if (tbName.Text.Length!=0)
            {
                MainForm.lvFindResultContrl.Items.Clear();

                QXinUserUI FindResult = MainForm.TreeViewFindEx(tbName.Text);
                if (FindResult != null)
                {
                    MainForm.lvFindResultContrl.Visible = true;
                    ListViewItem NewItem = MainForm.lvFindResultContrl.Items.Add(FindResult.Name);
                    NewItem.SubItems.Add(FindResult.IPAddress);
                    NewItem.Tag = FindResult;
                }
                else MainForm.lvFindResultContrl.Visible = false;
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            FormMain MainForm = (FormMain)this.ParentForm;
            MainForm.lvFindResultContrl.Items.Clear();
            MainForm.lvFindResultContrl.Visible = false;      
        }



    }
}
