using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synapse_Z
{
    public partial class ScriptHub : Form
    {
        public string Key { get; private set; }
        private bool topBarMouseDown;
        private Point offset;
        private Timer fadeInTimer;
        private string scriptHubPath = "lib/scripthub";

        public ScriptHub()
        {
            InitializeComponent();
            ThemeManager.Instance.ApplyTheme(this);
            synlabel.Text = $"{GlobalVariables.ExploitName} - Script Hub";
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

            // Set custom draw mode for Scriptbox
            ScriptHubBox.DrawMode = DrawMode.OwnerDrawFixed;
            ScriptHubBox.ItemHeight = 18; // Adjust item height to fit the 9pt font
            ScriptHubBox.DrawItem += Scriptbox_DrawItem;

            // Load items from the script hub directory
            LoadScriptHubItems();

            // Handle item selection
            ScriptHubBox.SelectedIndexChanged += ScriptHubBox_SelectedIndexChanged;

        }

        private void Scriptbox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Draw background
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Execute.FlatAppearance.MouseDownBackColor), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(ThemeManager.Instance.GetThemeColor("ScriptHub.ListBox.BackColor")), e.Bounds);
            }

            // Set font to 8pt
            Font font = new Font(e.Font.FontFamily, 9, e.Font.Style);

            // Draw text
            e.Graphics.DrawString(ScriptHubBox.Items[e.Index].ToString(), font, new SolidBrush(ThemeManager.Instance.GetThemeColor("ScriptHub.ListBox.ForeColor")), e.Bounds, StringFormat.GenericDefault);

            // Draw grey outline if selected
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                using (Pen pen = new Pen(Color.Gray))
                {
                    e.Graphics.DrawRectangle(pen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
                }
            }

            // Draw focus rectangle if needed
            e.DrawFocusRectangle();
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
            this.WindowState = FormWindowState.Minimized;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoadScriptHubItems()
        {
            if (Directory.Exists(scriptHubPath))
            {
                var directories = Directory.GetDirectories(scriptHubPath);
                foreach (var dir in directories)
                {
                    ScriptHubBox.Items.Add(Path.GetFileName(dir));
                }
            }
        }

        private void ScriptHubBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ScriptHubBox.SelectedIndex >= 0)
            {
                string selectedItem = ScriptHubBox.SelectedItem.ToString();
                string selectedPath = Path.Combine(scriptHubPath, selectedItem);

                // Display the image
                string imagePath = Path.Combine(selectedPath, "image.png");
                if (File.Exists(imagePath))
                {
                    DisplayBox.Image = Image.FromFile(imagePath);
                }

                // Display the description
                string descriptionPath = Path.Combine(selectedPath, "description.txt");
                if (File.Exists(descriptionPath))
                {
                    Description.Text = File.ReadAllText(descriptionPath);
                }
            }
        }



        private async void ExecuteScript(string script, string pid = null)
        {
            string uniqueId = Guid.NewGuid().ToString(); // Generate a unique identifier

            await Task.Delay(1);
            if (GlobalVariables.injecting == false && GlobalVariables.isDone == true)
            {
                try
                {
                    string schedulerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "scheduler");
                    if (!Directory.Exists(schedulerPath))
                    {
                        Directory.CreateDirectory(schedulerPath);
                    }

                    // Name the file according to the pid if provided
                    string fileName = pid != null ? $"PID{pid}_{Guid.NewGuid().ToString()}.lua" : $"{Guid.NewGuid().ToString()}.lua";
                    string filePath = Path.Combine(schedulerPath, fileName);
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        await writer.WriteLineAsync(script + "@@FileFullyWritten@@");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error writing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Not Injected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecuteForSelectedClients(string unescapedString)
        {
            bool anySelected = false;

            foreach (var entry in GlobalVariables.ExecutionPIDS)
            {
                if ((bool)entry.Value)  // If the status is True
                {
                    anySelected = true;
                    ExecuteScript(unescapedString, entry.Key);
                }
            }

            if (!anySelected)
            {
                MessageBox.Show("No selected clients.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Execute_Click(object sender, EventArgs e)
        {
            // Ensure there is a selected item
            if (ScriptHubBox.SelectedItem != null)
            {
                // Get the selected item (assuming it contains the directory path)
                string selectedItem = ScriptHubBox.SelectedItem.ToString();
                string selectedItemPath = Path.Combine(scriptHubPath, selectedItem);

                // Construct the path to the main.lua file
                string mainLuaPath = Path.Combine(selectedItemPath, "main.lua");

                if (File.Exists(mainLuaPath))
                {
                    try
                    {
                        // Read the contents of the main.lua file
                        string fileContent = File.ReadAllText(mainLuaPath);

                       ExecuteForSelectedClients(fileContent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("The file 'main.lua' does not exist in the selected directory.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("No item is selected in the ScriptHub box.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
