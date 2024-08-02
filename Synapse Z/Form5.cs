using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Button = System.Windows.Forms.Button;
using CheckBox = System.Windows.Forms.CheckBox;
using Label = System.Windows.Forms.Label;
using Panel = System.Windows.Forms.Panel;
using Timer = System.Windows.Forms.Timer;

namespace Synapse_Z
{
    public partial class ClientManager : Form
    {
        private bool topBarMouseDown;
        private Point offset;
        private Timer fadeInTimer;
        private Dictionary<string, Panel> executionPanels = new Dictionary<string, Panel>();
        private Dictionary<string, CancellationTokenSource> panelCancellationTokens = new Dictionary<string, CancellationTokenSource>();

        public ClientManager()
        {
            InitializeComponent();
            ThemeManager.Instance.ApplyTheme(this);

            string ranString = GenerateRandomString(12);
            this.Text = ranString;
            this.ShowInTaskbar = true;
            this.TopMost = true;

            this.Opacity = 0; // Set initial opacity to 0

            synlabel.Text = $"{GlobalVariables.ExploitName} - Client Manager";
            // Initialize and start the fade-in timer
            fadeInTimer = new Timer();
            fadeInTimer.Interval = 5; // Set the timer interval (50ms)
            fadeInTimer.Tick += FadeInTimer_Tick;
            fadeInTimer.Start();

            // Subscribe to the ExecutionPIDSChanged event
            GlobalVariables.ExecutionPIDSChanged += OnExecutionPIDSChanged;

            UpdateExecutionPanels();
        }

        private void OnExecutionPIDSChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateExecutionPanels));
            }
            else
            {
                UpdateExecutionPanels();
            }
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

        private new void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void UpdateExecutionPanels()
        {
            // Add new panels for PIDs that don't already have them
            foreach (var pidString in GlobalVariables.ExecutionPIDS.Keys)
            {
                if (!executionPanels.ContainsKey(pidString))
                {
                    AddExecutionPanel(pidString);
                }
            }

            // Remove panels for PIDs that no longer exist
            var currentPids = executionPanels.Keys.ToList();
            var pidsToRemove = currentPids.Except(GlobalVariables.ExecutionPIDS.Keys).ToList();
            foreach (var pid in pidsToRemove)
            {
                RemoveExecutionPanel(pid);
            }
        }

        private async void AddExecutionPanel(string pid)
        {
            Panel newPanel = CreateExamplePanel(pid);

            
            // Position the new panel
            if (executionPanels.Count > 0)
            {
                var lastPanel = executionPanels.Values.Last();
                newPanel.Location = new Point(lastPanel.Location.X, lastPanel.Location.Y + lastPanel.Height + 10);
            }
            else
            {
                newPanel.Location = new Point((mainPanel.Width - newPanel.Width) / 2, 10);
            }

            newPanel.Name = $"Panel_{pid}";

            executionPanels.Add(pid, newPanel);
            mainPanel.Controls.Add(newPanel); // Assuming mainPanel is the container panel

            StartStreaming(pid, newPanel);
            await Task.Delay(1);
            ThemeManager.Instance.ApplyTheme(this);
        }

        private void RemoveExecutionPanel(string pid)
        {
            if (executionPanels.TryGetValue(pid, out Panel panel))
            {
                mainPanel.Controls.Remove(panel);
                executionPanels.Remove(pid);

                // Cancel streaming for the removed panel
                if (panelCancellationTokens.TryGetValue(pid, out var cts))
                {
                    cts.Cancel();
                    panelCancellationTokens.Remove(pid);
                }

                // Reposition remaining panels
                int index = 0;
                foreach (var remainingPanel in executionPanels.Values)
                {
                    remainingPanel.Location = new Point((mainPanel.Width - remainingPanel.Width) / 2, 10 + index * (remainingPanel.Height + 10));
                    index++;
                }

                // Delete the screenshot file associated with this panel
                var screenshotPath = GetScreenshotPath(pid);
                if (System.IO.File.Exists(screenshotPath))
                {
                    System.IO.File.Delete(screenshotPath);
                }
            }
        }

        private Panel CreateExamplePanel(string pid)
        {
            Panel panel = new Panel
            {
                Size = new Size(470, 100), // Width remains the same, height will be adjusted later
                BackColor = Color.FromArgb(51, 51, 51)
            };

            PictureBox pictureBox1 = new PictureBox
            {
                BackColor = Color.Transparent,
                Location = new Point(panel.Width - 170, 3), // Adjust the size and position as needed
                Name = "pictureBox1",
                Size = new Size(160, 94), // Adjust the size as needed
                SizeMode = PictureBoxSizeMode.Zoom // Avoid stretching
            };

            Button killProcess = new Button
            {
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                BackColor = Color.FromArgb(61, 61, 61),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = Color.White,
                Location = new Point(7, 63),
                Name = "KillProcess",
                Size = new Size(65, 34),
                Text = "Kill",
                UseVisualStyleBackColor = false
            };
            killProcess.FlatAppearance.BorderSize = 0;
            killProcess.FlatAppearance.MouseDownBackColor = Color.FromArgb(54, 72, 88);
            killProcess.FlatAppearance.MouseOverBackColor = Color.FromArgb(44, 62, 78);
            killProcess.Click += (sender, e) => KillProcess(pid);
            killProcess.MouseEnter += FlatButton_MouseEnter;
            killProcess.MouseLeave += FlatButton_MouseLeave;

            Label label2 = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(3, 3),
                Name = "label2",
                Size = new Size(44, 15),
                Text = "Roblox"
            };

            Label pidLabel = new Label
            {
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(3, 21),
                Name = "PIDlabel",
                Size = new Size(64, 15),
                Text = $"PID: {pid}"
            };

            CheckBox enabledCheck = new CheckBox
            {
                AutoSize = true,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(7, 38),
                Name = "EnabledCheck",
                Size = new Size(65, 19),
                Text = "Enabled",
                UseVisualStyleBackColor = true
            };

            // Check if the PID exists in the dictionary and set the Checked state
            if (GlobalVariables.ExecutionPIDS.TryGetValue(pid, out var isChecked))
            {
                enabledCheck.Checked = (bool)isChecked;
            }
            else
            {
                enabledCheck.Checked = false;
                MessageBox.Show($"PID {pid} not found in GlobalVariables.ExecutionPIDS");
            }

            enabledCheck.CheckedChanged += (sender, e) =>
            {
                GlobalVariables.ExecutionPIDS[pid] = enabledCheck.Checked;
            };

            panel.Controls.Add(pictureBox1);
            panel.Controls.Add(killProcess);
            panel.Controls.Add(label2);
            panel.Controls.Add(pidLabel);
            panel.Controls.Add(enabledCheck);

            return panel;
        }

        private void KillProcess(string pid)
        {
            try
            {
                int processId = int.Parse(pid);
                Process process = Process.GetProcessById(processId);
                process.Kill();
                RemoveExecutionPanel(pid); // Remove the panel once the process is killed
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to kill process {pid}: {ex.Message}");
            }
        }

        private void StartStreaming(string pid, Panel panel)
        {
            int processId = int.Parse(pid);
            PictureBox pictureBox = panel.Controls.OfType<PictureBox>().FirstOrDefault();

            if (pictureBox == null)
                return;

            CancellationTokenSource cts = new CancellationTokenSource();
            panelCancellationTokens[pid] = cts;

            Task.Run(() => StreamWindow(processId, pictureBox, cts.Token), cts.Token);
        }

        private async Task StreamWindow(int processId, PictureBox pictureBox, CancellationToken token)
        {
            IntPtr hWnd = GetWindowHandleByPID(processId);
            if (hWnd == IntPtr.Zero)
            {
                return;
            }

            while (!token.IsCancellationRequested)
            {
                Bitmap screenshot = CaptureWindow(hWnd);

                pictureBox.Invoke((Action)(() =>
                {
                    pictureBox.Image = (Bitmap)screenshot.Clone();
                    ResizePictureBoxToFit(pictureBox, screenshot);
                }));

                await Task.Delay(100); // Adjust the delay as needed
            }
        }

        private void ResizePictureBoxToFit(PictureBox pictureBox, Bitmap image)
        {
            int panelWidth = pictureBox.Parent.Width;
            int panelHeight = pictureBox.Parent.Height;

            float aspectRatio = (float)image.Width / image.Height;
            int newWidth, newHeight;

            if (panelWidth / aspectRatio <= panelHeight)
            {
                newWidth = panelWidth;
                newHeight = (int)(panelWidth / aspectRatio);
            }
            else
            {
                newWidth = (int)(panelHeight * aspectRatio);
                newHeight = panelHeight;
            }

            pictureBox.Size = new Size(newWidth, newHeight);
            pictureBox.Location = new Point(pictureBox.Parent.Width - newWidth - 10, 3); // Adjust position as needed
        }

        public static Bitmap CaptureWindow(IntPtr hwnd)
        {
            RECT rc;
            GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            IntPtr hdcWindow = GetWindowDC(hwnd);
            BitBlt(hdcBitmap, 0, 0, rc.Width, rc.Height, hdcWindow, 0, 0, SRCCOPY);

            ReleaseDC(hwnd, hdcWindow);
            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        private string GetScreenshotPath(string pid)
        {
            return System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"screenshot_{pid}.png");
        }

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

        private const uint SRCCOPY = 0x00CC0020;

        private IntPtr GetWindowHandleByPID(int pid)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.Id == pid)
                {
                    return p.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public int Width => Right - Left;
            public int Height => Bottom - Top;
        }
    }
}
