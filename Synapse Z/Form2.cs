using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synapse_Z
{
    public partial class AccountKeyPrompt : Form
    {
        public string Key { get; private set; }
        private bool topBarMouseDown;
        private Point offset;
        private Timer fadeInTimer;

        public AccountKeyPrompt()
        {
            InitializeComponent();
            ThemeManager.Instance.ApplyTheme(this);

            string ranString = GenerateRandomString(12);
            this.Text = ranString;
            this.ShowInTaskbar = true;
            this.TopMost = true;
            SystemSounds.Asterisk.Play();
            this.Opacity = 0; // Set initial opacity to 0

            synlabel.Text = $"{GlobalVariables.ExploitName} - Account Key System";
            // Initialize and start the fade-in timer
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Set the timer interval (50ms)
            fadeInTimer.Tick += FadeInTimer_Tick;
            fadeInTimer.Start();
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void SendKey_Click(object sender, EventArgs e)
        {
            Key = KeyBox.Text;
            GlobalVariables.CurrentKey = Key;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
