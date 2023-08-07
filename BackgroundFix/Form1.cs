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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundFix
{
    public partial class Form1 : Form
    {/// <summary>
    /// This form is doing the most of application  logic
    /// </summary>
    /// <remarks>I am not satisfied whith this solution. In the future I will redo program to meke more sense structure-wise, mainly
    /// make it Windows service</remarks>
        private int displayCount;
        private readonly string resources = Application.StartupPath+"\\res\\"; //Path.GetFullPath("./res/");
        private bool toggle = false;
        private bool firstTimeShow = true;
        private string extencionSinglescreen;
        private string extencionDualscreen;
        public Form1()
        {
            InitializeComponent();
            displayCount = Screen.AllScreens.Length;

            //sets checkbox accordint to registry state
            checkBox1.Checked = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false).GetValueNames().Contains(Application.ProductName);

            //To prevent file not found when lookin for res
            //When ther are more files with same name but diferent extension
            if (!Directory.Exists(resources))
            {
                Directory.CreateDirectory(resources);
            } else
            {
                try
                {
                    setExtensions();
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
            this.richTextBox1.AppendText("\nScreens:");
            foreach (Screen screen in Screen.AllScreens) {
                this.richTextBox1.AppendText("\nworkArea: "+screen.WorkingArea + "\nbounds: " + screen.Bounds + "\nbitsPerBixell: " + screen.BitsPerPixel + "\ndeviceName: " +screen.DeviceName);
                this.richTextBox1.AppendText("\n----------------");
            }

            if (toggle)
            {
                toggle = false;
                WallpaperChanger.Set(resources + "Dualscreen" + extencionDualscreen);
                richTextBox1.AppendText("\n" + resources + "Dualscreen" + extencionDualscreen);
            } else
            {
                toggle = true;
                WallpaperChanger.Set(Path.Combine(resources, "Singelscreen" + extencionSinglescreen));
                richTextBox1.AppendText("\n" + Path.Combine(resources, "Singelscreen" + extencionSinglescreen));
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
                //this.Location = new Point(Cursor.Position.X - 150, Cursor.Position.Y - 150);
                //this.Show();
                this.CenterToScreen();
                this.Show();
                this.Activate();
                this.TopMost = true;
                richTextBox1.AppendText("\nProgram si already running");
                this.TopMost = false;
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

            richTextBox1.AppendText("\nNumber of displays: "+ displayCount+" time: "+ System.DateTime.Now);

            switch (displayCount)
            {
                case 1:
                    WallpaperChanger.SilentSet(Path.Combine(resources, "Singelscreen"+extencionSinglescreen));
                    break;
                case 2:
                    WallpaperChanger.SilentSet(Path.Combine(resources, "Dualscreen"+extencionDualscreen));
                    break;
                default:
                    this.richTextBox1.AppendText("\nNumber of displays not recognised");
                    break;
            }
        }
        /// <summary>
        /// Handles Quitting application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //richTextBox1.AppendText("\n"+sender);

            //DialogResult messageBoxResult = MessageBox.Show("If you want program to continu working just hide it via \"Hide\" button or choose [NO].\n" +
            //    "Aditional changes to its seting can be made by running executable aggain.\n" +
            //    "\nWould like program to TERMINATE and STOP changing wallpaper?", "Ake you sure you want to terminate me?", 
            //    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            //Application.Run(new MyCloseDialog(this, e));

            Thread closeDialog = new Thread(() => Application.Run(new MyCloseDialog(this.Location)));
            closeDialog.Start();
            closeDialog.Join();

            canceDialog(MyCloseDialog.result, e);
        }

        public void canceDialog(MyDialogResults result , FormClosingEventArgs e)
        {
            
            if (result == MyDialogResults.YES)
            {
                e.Cancel = false;
                if (MessageBox.Show("Do you want revert changes made by program?", "Restore state?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
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

            }
            else
            {
                e.Cancel = true;
                if (result == MyDialogResults.NO)
                    button3_Click(this, null);
            }
        }

        /// <summary>
        /// Hide when button is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine( resources, "Singelscreen"+extencionSinglescreen)) && File.Exists( Path.Combine(resources, "Dualscreen"+extencionDualscreen))){
                this.Hide();
            } else
            {
                if (MessageBox.Show("Wallpaper images could not be found!\n\nProgram will probably not work correctly (in one or more states wallpaper will be back). Please make sure you CLICKED SAVE and paths to images are valid\n\nDo you want still hide this window?",
                    "Image not found", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Hide();
                }
                else
                    return;
            }
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
        /// <param name="sender">Depending on which button call method will set paht to coresponding image</param>
        /// <param name="e"></param>
        private void buttonExplore_Click(object sender, EventArgs e)
        {
            bool choosingSingle = sender == buttonExplore1;

            openFileDialog.DefaultExt = Application.StartupPath;
            if (choosingSingle)
            {
                openFileDialog.Title = "Singlescreen";
                openFileDialog.FilterIndex = 1;
                openFileDialog.FileName = "Singlescreen.jpg";
            } else
            {
                openFileDialog.Title = "Dualscreen";
                openFileDialog.FilterIndex = 1;
                openFileDialog.FileName = "Dualscreen.jpg";
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
                this.extencionSinglescreen = Path.GetExtension(textBox1.Text);
                File.Copy(textBox1.Text, resources + "Singelscreen" + extencionSinglescreen, true);
                richTextBox1.AppendText("\nSavet s. to: " + resources + "Singelscreen"+ extencionSinglescreen);
            }
            else
            {
                MessageBox.Show("Non existing file for Singlescreen\n" + textBox1.Text + "\nPlese make sure entered path or file exists", "WARMING", MessageBoxButtons.OK, MessageBoxIcon.Error);
                allSuccessfullySaved = false;
            }
            //Copy image DualScreen
            if (File.Exists(textBox2.Text))
            {
                extencionDualscreen = Path.GetExtension(textBox2.Text);
                File.Copy(textBox2.Text, resources + "Dualscreen" + extencionDualscreen, true);
                richTextBox1.AppendText("\nSavet d. to: " + resources + "Dualscreen" + extencionDualscreen);
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

            //Check for files with same mame
            try
            {
                getExtension("Singelscreen");
            } catch(MoreFilesWithSameNameException exept)
            {
                foreach (var path in exept.FilesFound)
                {
                    if (Path.GetExtension(path) != extencionSinglescreen)
                    {
                        File.Delete(path);
                    }
                }
            }
            try
            {
                getExtension("Dualscreen");
            }
            catch (MoreFilesWithSameNameException exept)
            {
                foreach (var path in exept.FilesFound)
                {
                    if (Path.GetExtension(path) != extencionDualscreen)
                    {
                        File.Delete(path);
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                WallpaperChanger.RestoreState();
            }
            catch (NotBackedUpExeption)
            { //ignore
            }
        }

        private void hideIfSetUp() 
        {
            if (File.Exists(resources + "memory.txt")) //Thanks to file memory.txt form know where to hide
            {
                this.Hide();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (firstTimeShow)
            {
                hideIfSetUp() ;
            }
        }
        private void setExtensions()
        {
            int delete = 2;//represents action what to do when more files with same name found
                           //0 => do nothing
                           //1 => delete all file exept for one
                           //2 => none duplicated names so far
            try
            {
                this.extencionSinglescreen = this.getExtension("Singelscreen");
            }
            catch (MoreFilesWithSameNameException e)
            {
                delete = (MessageBox.Show("More files with same name found. \nYes - all files will be deleted exept for one (may result in usage of unwanted Image)\nNo - no action will be taken, please navigate to:\n" + resources + "\n and meke sure only one of eatch Singescreen and Dualscreen images are present",
                        "More files with same name", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) ? 1 : 0;
                if (delete==1)
                {
                    deleteExeptOne(e.FilesFound);
                }
            }

            try
            {
                this.extencionDualscreen = this.getExtension("Dualscreen");
            }
            catch (MoreFilesWithSameNameException e)
            {
                switch (delete)
                {
                    //NO
                    case 0:
                        //nothing
                        break;
                    //YES
                    case 1:
                        deleteExeptOne(e.FilesFound);
                        break;
                    //Not thrown
                    case 2:
                        if ((MessageBox.Show("More files with same name found. \nYes - all files will be deleted exept for one (may result in usage of unwanted Image)\nNo - no action will be taken, please navidate to:\n" + resources + "\n and meke sure only one of eatch Singescreen and Dualscreen images are present",
                        "More files with same name", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes))
                        {
                            deleteExeptOne(e.FilesFound);
                        }
                        break;
                }
            }
            //opens file explorer if NO selected
            if (delete == 0)
            {
                System.Diagnostics.Process.Start("explorer.exe", "\"" + resources);
            }
        }

        private void deleteExeptOne(string[] filesFound)
        {
            for (int i = 0; i < filesFound.Length - 1; i++)
            {
                File.Delete(filesFound[i]);
            }

        }

        private string getExtension (string fileName)
        {
            var helper = (Directory.GetFiles(resources, fileName+".*"));
            if (helper.Length == 1)
            {
                return Path.GetExtension(helper[0]);
            }
            else if (helper.Length > 1)
            {
                throw new MoreFilesWithSameNameException("When searching for extension of " + fileName + " more files with same name found", helper);
            } else { return ""; }
        }
        /// <summary>
        /// Opens File explorer in res forder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "\"" + resources);
        }
    }
}
