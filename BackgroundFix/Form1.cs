using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundFix
{
    public partial class Form1 : Form
    {
        private int displayCount;
        private readonly string resources = Application.StartupPath+"\\res\\"; //Path.GetFullPath("./res/");
        private bool toggle = false;
        private bool firstTimeShow = true;
        public Form1()
        {
            InitializeComponent();
            displayCount = Screen.AllScreens.Length;

            //sets checkbox accordint to registry state
            checkBox1.Checked = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false).GetValueNames().Contains(Application.ProductName);

            //To prevent file not found when lookin for res
            if (!Directory.Exists(resources))
            {
                Directory.CreateDirectory(resources);
            } else
            {
                try
                {
                    updateWallpaper();
                } 
                catch (FileNotFoundException) 
                { 
                    //ignore
                }
            }
        }
        /// <summary>
        /// Tests changing wallpaper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                WallpaperChanger.Set(resources + "Dualscreen.jpg");
                richTextBox1.AppendText("\n" + resources + "Dualscreen.jpg");

            } else
            {
                toggle = true;
                WallpaperChanger.SilentSet(resources + "Singelscreen.png");
                richTextBox1.AppendText("\n" + resources + "Singelscreen.jpg");
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
            firstTimeShow = false;
        }
        /// <summary>
        /// Making app start and not start at startup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBox1.Checked)
            {
                key.SetValue(Application.ProductName, Application.ExecutablePath);
              
            } else
            {
                key.DeleteValue(Application.ProductName, false);
            }
            key.Close();
        }
        /// <summary>
        /// Setting images for wallpaper sets walues to textBoxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExplore_Click(object sender, EventArgs e)
        {
            bool choosingSingle = sender == buttonExplore1;

            openFileDialog.DefaultExt = Application.StartupPath;
            if (choosingSingle)
            {
                openFileDialog.Title = "Singlescreen";
                openFileDialog.FileName = "Singlescreen.png";
            } else
            {
                openFileDialog.Title = "Dualscreen";
                openFileDialog.FileName = "Dualscreen.png";
            }
            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                if (choosingSingle)
                {
                    textBox1.Text = filePath;
                }
                else
                {
                    textBox2.Text = filePath;
                }
            }

        }
        /// <summary>
        /// Copy images from textBoxes to local res directory
        /// also create memory.txt if not exist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            bool allSuccessfullySaved = true;
            //Copy image Singlescreen
            if (File.Exists(textBox1.Text))
            {
                File.Copy(textBox1.Text, resources + "Singelscreen.png", true);
                richTextBox1.AppendText("\nSavet s. to: " + resources + "Singelscreen.png");
            }
            else
            {
                MessageBox.Show("Non existing file for Singlescreen\n" + textBox1.Text + "\nPlese make sure entered path or file exists", "WARMING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allSuccessfullySaved = false;
            }
            //Copy image DualScreen
            if (File.Exists(textBox2.Text))
            {
                File.Copy(textBox2.Text, resources + "Dualscreen.jpg", true);
                richTextBox1.AppendText("\nSavet d. to: " + resources + "Dualscreen.jpg");
            }
            else
            {
                MessageBox.Show("Non existing file for Dualcreen\n" + textBox2.Text + "\nPlese make sure entered path or file exists", "WARMING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allSuccessfullySaved = false;
            }

            //Check / create memory.txt
            if (!File.Exists(resources + "memory.txt") && allSuccessfullySaved)
            {
                File.WriteAllText(resources + "memory.txt", "Not first start");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            WallpaperChanger.RestoreState();
        }

        public void hideIfSetUp() 
        {
            if (File.Exists(resources + "memory.txt")) //Thanks to file memory.txt form know where to hide
            {
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hideIfSetUp();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (firstTimeShow)
            {
                hideIfSetUp() ;
            }
        }
    }
}
