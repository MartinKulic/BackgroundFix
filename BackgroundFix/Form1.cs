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
    public partial class Form1 : Form
    {
        private int displayCount;
        public Form1()
        {
            InitializeComponent();
            displayCount = Screen.AllScreens.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.AppendText("Screens:");
            foreach (Screen screen in Screen.AllScreens) {
                this.richTextBox1.AppendText("\nworkArea: "+screen.WorkingArea + "\nbounds: " + screen.Bounds + "\nbitsPerBixell: " + screen.BitsPerPixel + "\ndeviceName: " +screen.DeviceName);
                this.richTextBox1.AppendText("\n----------------");
            }
            
        }

        protected override void WndProc(ref Message m)
        {
            if (displayCount != Screen.AllScreens.Length) {
                displayCount = Screen.AllScreens.Length;
                this.richTextBox1.AppendText("\nWinProc:" + m + "\nAllScreans.len:" + displayCount);
            }
            base.WndProc(ref m);
        }
    }
}
