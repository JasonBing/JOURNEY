using SAPCOM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SAPRFCBuilder
{
    public partial class frmFunctionSelect : Form
    {
        public frmFunctionSelect(SAPDestination Destination)
        {
            InitializeComponent();
            this.Destination = Destination;
        }
        private string FunctionName = string.Empty;
        private SAPDestination Destination = null;
        private void frmFunctionSelect_Load(object sender, EventArgs e)
        {
            this.radioButton1.Checked = true;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1.Checked == true)
            {
                FunctionName = (string)this.radioButton1.Tag;
            }
            else if (this.radioButton2.Checked == true)
            {
                FunctionName = (string)this.radioButton2.Tag;
            }
            else if (this.radioButton3.Checked == true)
            {
                FunctionName = (string)this.radioButton3.Tag;
            }
            else if (this.radioButton4.Checked == true)
            {
                FunctionName = (string)this.radioButton4.Tag;
            }
            else
            {
                FunctionName = "";
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                this.Visible = false;
                Form frmNext = null;
                if (FunctionName == "RFC")
                {
                    frmNext = new frmRFC(this.Destination);
                    
                }
                else if (FunctionName == "ENTITY")
                {
                    
                }
                else if (FunctionName == "TRANSACTION")
                {
                   
                }
                else if (FunctionName == "SCREEN")
                {
                   
                }
                else
                {
                    this.Visible = true;
                    throw new Exception("暂不支持!");
                }
                frmNext.ShowDialog();
                this.Visible = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}
