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
        private readonly string resources = Path.GetFullPath("./res/");
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
            if (displayCount != Screen.AllScreens.Length) //detect change in number of displays
            {
                updateWallpaper();

                this.richTextBox1.AppendText("\nWinProc:" + m + "\nAllScreans.len:" + displayCount);
            }
            else if (m.Msg == NativeMethod.WM_SHOW_Yourself) //if user attend to start more then one instance of application
            {
                this.Location = new Point(Cursor.Position.X - 150, Cursor.Position.Y - 150);
                this.Show();
                this.Activate();             
                MessageBox.Show("Program is already runnig");
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// Change wallpaper acording to number of connected displays
        /// </summary>
        /// <remarks>Works only up to 2 displays, 0 is also not recognized</remarks>
        private void updateWallpaper()
        {
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
                    this.richTextBox1.AppendText("\nNumber of displays not recognised");
                    break;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                WallpaperChanger.RestoreState();
            }
            catch (NotBackedUpExeption)
            {
                //ignore
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
