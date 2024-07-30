using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using static System.Windows.Forms.DataFormats;

namespace Synapse_Z
{
    public partial class Options : Form
    {
        private bool topBarMouseDown;
        private Point offset;
        private Timer fadeInTimer;
  

        public Options()
        {
            InitializeComponent();

            string ranString = GenerateRandomString(12);
            this.Text = ranString;
            this.ShowInTaskbar = true;
            this.TopMost = GlobalVariables.TopMostGlobal;

            this.Opacity = 0; // Set initial opacity to 0

            // Initialize and start the fade-in timer
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Set the timer interval (50ms)
            fadeInTimer.Tick += FadeInTimer_Tick;
            fadeInTimer.Start();

            ClearEditorPrompt.Checked = GlobalVariables.ClearEditorPrompt;
            AutoInject.Checked = GlobalVariables.AutoInject;
            TopMostCheck.Checked = GlobalVariables.TopMostGlobal;
            TabClosingPrompt.Checked = GlobalVariables.TabClosingPrompt;
            UnlockFPS.Checked = GlobalVariables.UnlockFPS;

            comboBox1.SelectedItem = GlobalVariables.CurrentEditorTheme;


        }

        private void FadeInTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                this.Opacity += 0.05; // Increase opacity by 5% each tick
            }
            else
            {
                fadeInTimer.Stop(); // Stop the timer when full opacity is reached
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                topBarMouseDown = true;
                offset = new Point(e.X, e.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            topBarMouseDown = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (topBarMouseDown)
            {
                Point newLocation = new Point(e.X - offset.X, e.Y - offset.Y);
                this.Location = new Point(this.Left + newLocation.X, this.Top + newLocation.Y);
            }
        }

        private void FlatButton_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.BorderColor = Color.Gray;
            }
        }

        private void FlatButton_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                button.FlatAppearance.BorderSize = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void ClearEditorPrompt_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                GlobalVariables.ClearEditorPrompt = checkBox.Checked;
            }
        }

        private void TopMost_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                GlobalVariables.TopMostGlobal = checkBox.Checked;
                this.TopMost = GlobalVariables.TopMostGlobal;
            }
        }

        private void TabClosingPrompt_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                GlobalVariables.TabClosingPrompt = checkBox.Checked;
            }
        }

        private void UnlockFPS_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                GlobalVariables.UnlockFPS = checkBox.Checked;
            }
        }

        private void AutoInject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                GlobalVariables.AutoInject = checkBox.Checked;
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            CustomComboBoxHelper.ApplyCustomDrawing(comboBox1);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assuming the comboBox1 contains the theme names
            string selectedTheme = comboBox1.SelectedItem.ToString();
            GlobalVariables.CurrentEditorTheme = selectedTheme;
            // Access the TabControl from the current instance of Form1
            Manina.Windows.Forms.TabControl tabControl = SynapseZ.Instance.MainTabControl;

            // Iterate through all TabPages in the TabControl
            foreach (var tabPage in tabControl.Tabs)
            {
                // Find the WebView2 control within the TabPage
                foreach (Control tabPageControl in tabPage.Controls)
                {
                    if (tabPageControl is Microsoft.Web.WebView2.WinForms.WebView2 webView2)
                    {
                        // Execute JavaScript to set the theme
                        string script = $"SetTheme('{selectedTheme}');";
                        webView2.ExecuteScriptAsync(script);
                    }
                }
            }
            
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
