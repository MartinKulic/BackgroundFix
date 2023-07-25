using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundFix
{
    public enum MyDialogResults
    {
        YES,
        NO,
        CANCEL,
        NONE
    }

    public partial class MyCloseDialog : Form
    {
        public static MyDialogResults result  = MyDialogResults.NONE;
        private Form1 parent;
        public MyCloseDialog(Point parentPos)
        {
            InitializeComponent();
            this.CenterToParent();
            result = MyDialogResults.NONE;
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            result = MyDialogResults.YES;
            this.Close();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            result=MyDialogResults.NO;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            result = MyDialogResults.CANCEL;
            this.Close();
        }
    }
}
