using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundFix
{
    public partial class Form1 : Form
    {
        private int displayCount;
       // private const string resources = "./res/";
        private const string resources = "D:/Junk/VisualStudio/,C#/Repositories/BackgroundFix/BackgroundFix/bin/Debug/res/";
        private bool toggle = false;
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

            if (toggle)
            {
                toggle = false;
                WallpaperChanger.SilentSet(resources + "Dualscreen.jpg");
            } else
            {
                toggle = true;
                WallpaperChanger.SilentSet(resources + "Singelscreen.png");
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (displayCount != Screen.AllScreens.Length) {
                displayCount = Screen.AllScreens.Length;

                switch (displayCount)
                {
                    case 1:
                        WallpaperChanger.SilentSet(resources + "Singelscreen.png");
                        break;
                    case 2:
                        WallpaperChanger.SilentSet(resources + "Dualscreen.jpg");
                        break;
                    default:
                        this.richTextBox1.AppendText("\nDefault number of displays not recognised");
                        break;
                }

                this.richTextBox1.AppendText("\nWinProc:" + m + "\nAllScreans.len:" + displayCount);
            }
            base.WndProc(ref m);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WallpaperChanger.RestoreState();
        }
    }
}
