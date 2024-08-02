using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Manina.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Brush = System.Drawing.Brush;
using Color = System.Drawing.Color;
using Formatting = Newtonsoft.Json.Formatting;
using Pen = System.Drawing.Pen;

namespace Synapse_Z
{
    public partial class SynapseZ : Form
    {
        private System.Windows.Forms.Timer injTimer;
        private bool topBarMouseDown;
        private Point offset;
        private Process process;
        private Tab addTabButton;
        private Microsoft.Web.WebView2.WinForms.WebView2 currentWebView2;

        private BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Timer fadeInTimer;
        private ContextMenuStrip scriptContextMenu;
        private FileSystemWatcher fileSystemWatcher;
        private ScriptHub scriptHubFormInstance;
        private Options optionsFormInstance;

        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int BorderWidth = 10;

        private string webView2FolderPath;
        private string tabsFolderPath;
        private ContextMenuStrip tabContextMenu;


        private static readonly HttpClient client = new HttpClient();
        private const string currentVersion = "v1.0.7"; // Replace with the current version of your application

        private static SynapseZ _instance;

        public string FinishedExpTop = "";

        public static SynapseZ Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SynapseZ();
                }
                return _instance;
            }
        }

        public Manina.Windows.Forms.TabControl MainTabControl
        {
            get { return tabControl1; } // Assuming tabControl1 is the name of your Manina TabControl
        }

        public SynapseZ()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string synapseZPath = Path.Combine(currentDirectory, "Synapse-Z Boostrapper.exe");
            string synapsePath = Path.Combine(currentDirectory, "Synapse Bootstrapper.exe");

            if (File.Exists(synapseZPath) && File.Exists(synapsePath))
            {
                try
                {
                    File.Delete(synapseZPath);
                    this.Close();
                    Process.Start(synapsePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            GlobalVariables.LoadSettingsAsync();
            // Initialize AutoInjectManager with a reference to this form
            AutoInjectManager.Initialize(this);
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeScriptContextMenu();
            SetupWebView2Environment();
            InitializeTabContextMenu(); // Initialize the tab context menu
         
            ThemeManager.Instance.ApplyTheme(this);

            if (GlobalVariables.ShowVersion == true)
            {
                FinishedExpTop = $"{GlobalVariables.ExploitName} - {currentVersion}";
            }
            else
            {
                FinishedExpTop = $"{GlobalVariables.ExploitName}";
            }
            
            synlabel.Text = FinishedExpTop;
            string ranString = GenerateRandomString(12);
            this.Text = ranString;


            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(853, 381); // Set the default size as the minimum size
            this.Size = new Size(853, 381); // Set the default size

            // Hide the console window
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            

            _instance = this;

            // Initialize the injection timer
            injTimer = new System.Windows.Forms.Timer();
            injTimer.Interval = 100;
            injTimer.Tick += InjTimer_Tick;

            // Initialize tab control
            tabControl1.Padding = new Padding(22, 4, 22, 4);
            //tabControl1.BackColor = Color.FromArgb(61, 61, 61);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.ShowCloseTabButtons = true;
            tabControl1.CloseTabButtonClick += tabControl1_CloseTabButtonClick;
            tabControl1.TabClick += tabControl1_TabClick;
            tabControl1.LayoutTabs += TabControl1_LayoutTabs;
            tabControl1.PageChanged += tabControl1_PageChanged;
            tabControl1.ForeColor = ThemeManager.Instance.GetThemeColor("SynapseZ.TabControl.ForeColor"); // Set tab text color to white
            tabControl1.MouseUp += TabControl1_MouseUp; // Add MouseUp event for showing context menu

            this.Opacity = 0; // Set initial opacity to 0

            // Initialize and start the fade-in timer
            fadeInTimer = new System.Windows.Forms.Timer();
            fadeInTimer.Interval = 5; // Set the timer interval (50ms)
            fadeInTimer.Tick += FadeInTimer_Tick;
            fadeInTimer.Start();

            // Ensure the scripts folder exists
            string scriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
            if (!Directory.Exists(scriptsPath))
            {
                Directory.CreateDirectory(scriptsPath);
            }

            string scripthubPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "scripthub");
            if (!Directory.Exists(scripthubPath))
            {
                Directory.CreateDirectory(scripthubPath);
            }

            string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(binPath))
            {
                Directory.CreateDirectory(binPath);
            }

            // Ensure the tabs folder exists
            tabsFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "tabs");
            if (!Directory.Exists(tabsFolderPath))
            {
                Directory.CreateDirectory(tabsFolderPath);
            }

            // Load previously saved tabs
            LoadSavedTabs();

            // Load scripts into Scriptbox
            LoadScriptsIntoScriptbox(scriptsPath);

            // Initialize and start the file system watcher
            InitializeFileSystemWatcher(scriptsPath);

            // Set custom draw mode for Scriptbox
            Scriptbox.DrawMode = DrawMode.OwnerDrawFixed;
            Scriptbox.ItemHeight = 15; // Adjust item height to fit the 9pt font
            Scriptbox.DrawItem += Scriptbox_DrawItem;
            this.FormClosing += SynapseZ_FormClosing;
            GlobalVariables.RegisterOnTopMostGlobalChanged(UpdateTopMost);


            CheckForUpdates();


        }

  
        private async void CheckForUpdates()
        {
            string owner = "Coolmandfgfgdvcgfg"; // Replace with the repository owner's name
            string repo = "Synapse-ZUI"; // Replace with the repository name

            try
            {
                var latestRelease = await GetLatestRelease(owner, repo);
                string latestVersion = latestRelease["tag_name"].ToString();

                if (IsNewVersionAvailable(currentVersion, latestVersion))
                {
                    this.TopMost = false;
                    MessageBox.Show("A new update is available. The application will now update.", "Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RunBootstrapper();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while checking for updates: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task<JObject> GetLatestRelease(string owner, string repo)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/releases/latest";
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; GrandCircus/1.0)");
            var response = await client.GetStringAsync(url);
            return JObject.Parse(response);
        }

        private bool IsNewVersionAvailable(string currentVersion, string latestVersion)
        {
            Version current = ParseVersion(currentVersion);
            Version latest = ParseVersion(latestVersion);
            // MessageBox.Show(currentVersion);
            //MessageBox.Show(latestVersion);
            return latest > current;
        }

        private Version ParseVersion(string version)
        {
            // Remove any non-numeric prefix (like 'v')
            string numericVersion = Regex.Replace(version, @"^[^\d]*", "");
            return new Version(numericVersion);
        }

        private void RunBootstrapper()
        {
            string bootstrapperPath = Path.Combine(Directory.GetCurrentDirectory(), "Synapse Bootstrapper.exe");
            if (File.Exists(bootstrapperPath))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = bootstrapperPath,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            else
            {
                MessageBox.Show("Bootstrapper not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpdateTopMost(bool topMost)
        {
            this.TopMost = topMost;
        }

        private async void SynapseZ_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!e.Cancel)
            {
                // Cancel the closing event
                e.Cancel = true;
           
                // Save all tabs asynchronously
                await SaveAllTabsAsync();

                if (GlobalVariables.noSave == false)
                {
                    await GlobalVariables.SaveSettingsAsync();
                }


                // After saving, close the form
                e.Cancel = false;
                this.FormClosing -= SynapseZ_FormClosing; // Remove the event handler to avoid recursion
                this.Close();
            }
        }

        private async Task SaveAllTabsAsync()
        {
            foreach (Tab tab in tabControl1.Tabs)
            {
                if (tab != null && tab.Text != "+")
                {
                    Microsoft.Web.WebView2.WinForms.WebView2 webView = tab.Controls.OfType<Microsoft.Web.WebView2.WinForms.WebView2>().FirstOrDefault();
                    if (webView != null && webView.CoreWebView2 != null)
                    {
                        string script = "GetText();";
                        string result = await webView.CoreWebView2.ExecuteScriptAsync(script);
                        if (!string.IsNullOrEmpty(result) && result != "null")
                        {
                            result = result.Trim('"');
                            string unescapedString = Regex.Unescape(result);
                            if (!string.IsNullOrWhiteSpace(unescapedString))
                            {
                                SaveTabContent(tab.Text, unescapedString, tab.Text);
                            }
                        }
                    }
                }
            }
        }


        private async void ClearTabsMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Delay(1); // Add a slight delay
            if (GlobalVariables.TabClosingPrompt)
            {
                var result = MessageBox.Show($"Are you sure you want to close all tabs?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    ClearAllTabs();
                    AddNewTab("Script 1");
                }
            }
            else
            {
                ClearAllTabs();
                AddNewTab("Script 1");
            }
        }

        private async void ClearAllTabs()
        {
            // Collect all tabs except the add tab button
            var tabsToRemove = tabControl1.Tabs.Where(tab => tab.Text != "+").ToList();

            // Remove the collected tabs
            foreach (var tab in tabsToRemove)
            {
                try
                {
                    tabControl1.Tabs.Remove(tab);
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Ignore the error and continue
                }
            }
            await Task.Delay(1); // Add a slight delay
            // Clear all tab files
            var tabFiles = Directory.GetFiles(tabsFolderPath, "*.json");
            foreach (var tabFile in tabFiles)
            {
                File.Delete(tabFile);
            }
        }

        private async void RenameTabMenuItem_Click(object sender, EventArgs e)
        {
            Tab selectedTab = tabControl1.SelectedTab;
            if (selectedTab != null && selectedTab.Text != "+")
            {
                this.TopMost = false;
                string currentName = selectedTab.Text;
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter new name for the tab (max 20 characters):", "Rename Tab", currentName);

                if (!string.IsNullOrEmpty(newName) && newName.Length <= 20 && newName != currentName)
                {
                    this.TopMost = GlobalVariables.TopMostGlobal;
                    // Ensure the new name is unique
                    if (tabControl1.Tabs.Any(tab => tab.Text == newName))
                    {
                        MessageBox.Show("A tab with this name already exists. Please choose a different name.", "Rename Tab", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // Update the tab name
                        selectedTab.Text = newName;

                        // Update the saved file to reflect the new tab name
                        string oldFileName = Path.Combine(tabsFolderPath, $"{currentName}.json");
                        string newFileName = Path.Combine(tabsFolderPath, $"{newName}.json");

                        if (File.Exists(oldFileName))
                        {
                            File.Move(oldFileName, newFileName);
                        }

                        // Save the tab content with the new name
                        string content = await GetTabContentAsync(selectedTab);
                        var tabData = new TabData { Name = newName, Content = content, FileName = newName };
                        SaveTabContent(newName, tabData.Content, newName);

                        // Move the renamed tab to the top
                        tabControl1.Tabs.Remove(selectedTab);
                        tabControl1.Tabs.Insert(0, selectedTab);
                        tabControl1.SelectedTab = selectedTab;
                    }
                }
                else if (newName.Length > 20)
                {
                    MessageBox.Show("The new tab name exceeds the 20-character limit. Please choose a shorter name.", "Rename Tab", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private async Task<string> GetTabContentAsync(Tab tab)
        {
            Microsoft.Web.WebView2.WinForms.WebView2 webView = tab.Controls.OfType<Microsoft.Web.WebView2.WinForms.WebView2>().FirstOrDefault();
            if (webView != null && webView.CoreWebView2 != null)
            {
                string script = "GetText();";
                string result = await webView.CoreWebView2.ExecuteScriptAsync(script);
                if (!string.IsNullOrEmpty(result) && result != "null")
                {
                    result = result.Trim('"');
                    return Regex.Unescape(result);
                }
            }
            return string.Empty;
        }

        private void TabControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < tabControl1.Tabs.Count; i++)
                {
                    var tab = tabControl1.Tabs[i];
                    Rectangle r = tabControl1.GetTabBounds(tab);
                    if (r.Contains(e.Location))
                    {
                        tabControl1.SelectedIndex = i;
                        tabContextMenu.Show(tabControl1, e.Location);
                        break;
                    }
                }
            }
        }

        private async void SaveTabMenuItem_Click(object sender, EventArgs e)
        {
            Tab selectedTab = tabControl1.SelectedTab;
            if (selectedTab != null && selectedTab.Text != "+")
            {
                Microsoft.Web.WebView2.WinForms.WebView2 webView = selectedTab.Controls.OfType<Microsoft.Web.WebView2.WinForms.WebView2>().FirstOrDefault();
                if (webView != null && webView.CoreWebView2 != null)
                {
                    string script = "GetText();";
                    string result = await webView.CoreWebView2.ExecuteScriptAsync(script);
                    if (!string.IsNullOrEmpty(result) && result != "null")
                    {
                        result = result.Trim('"');
                        string unescapedString = Regex.Unescape(result);
                        if (!string.IsNullOrWhiteSpace(unescapedString))
                        {
                            SaveTabContent(selectedTab.Text, unescapedString, selectedTab.Text);
                            MessageBox.Show("Tab content saved successfully!", "Save Tab", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTCAPTION = 2;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                // Prevent resizing if the form is maximized
                if (this.WindowState == FormWindowState.Maximized)
                {
                    return;
                }

                if ((int)m.Result == HTCLIENT)
                {
                    Point screenPoint = new Point(m.LParam.ToInt32());
                    Point clientPoint = this.PointToClient(screenPoint);
                    if (clientPoint.Y <= BorderWidth)
                    {
                        if (clientPoint.X <= BorderWidth)
                            m.Result = (IntPtr)HTTOPLEFT;
                        else if (clientPoint.X < (Size.Width - BorderWidth))
                            m.Result = (IntPtr)HTTOP;
                        else
                            m.Result = (IntPtr)HTTOPRIGHT;
                    }
                    else if (clientPoint.Y <= (Size.Height - BorderWidth))
                    {
                        if (clientPoint.X <= BorderWidth)
                            m.Result = (IntPtr)HTLEFT;
                        else if (clientPoint.X > (Size.Width - BorderWidth))
                            m.Result = (IntPtr)HTRIGHT;
                    }
                    else
                    {
                        if (clientPoint.X <= BorderWidth)
                            m.Result = (IntPtr)HTBOTTOMLEFT;
                        else if (clientPoint.X < (Size.Width - BorderWidth))
                            m.Result = (IntPtr)HTBOTTOM;
                        else
                            m.Result = (IntPtr)HTBOTTOMRIGHT;
                    }
                }
                return;
            }
            base.WndProc(ref m);
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
                e.Graphics.FillRectangle(new SolidBrush(Scriptbox.BackColor), e.Bounds);
            }

            // Set font to 8pt
            Font font = new Font(e.Font.FontFamily, 9, e.Font.Style);
            
            // Draw text

            System.Drawing.StringFormat StringFormat = new System.Drawing.StringFormat();
            Brush newbrush = new SolidBrush(Scriptbox.ForeColor);
            e.Graphics.DrawString(Scriptbox.Items[e.Index].ToString(), font, newbrush, e.Bounds, StringFormat.GenericDefault);

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

        private void InitializeScriptContextMenu()
        {
            scriptContextMenu = new ContextMenuStrip();
            ToolStripMenuItem executeMenuItem = new ToolStripMenuItem("Execute");
            executeMenuItem.Click += ExecuteMenuItem_Click;
            scriptContextMenu.Items.Add(executeMenuItem);

            ToolStripMenuItem loadEditorMenuItem = new ToolStripMenuItem("Load Into Editor");
            loadEditorMenuItem.Click += LoadEditorMenuItem_Click;
            scriptContextMenu.Items.Add(loadEditorMenuItem);

            Scriptbox.ContextMenuStrip = scriptContextMenu;
            Scriptbox.MouseDown += Scriptbox_MouseDown;
            Scriptbox.MouseDoubleClick += Scriptbox_MouseDoubleClick; // Add this line
        }

        private async void Scriptbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = Scriptbox.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", Scriptbox.Items[index].ToString());
                string fileContent = File.ReadAllText(filePath);

                // Load script into a new tab
                string tabName = Path.GetFileNameWithoutExtension(filePath);
                AddNewTab(tabName, fileContent);
            }
        }

        private void Scriptbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = Scriptbox.IndexFromPoint(e.Location);
                if (index != ListBox.NoMatches)
                {
                    Scriptbox.SelectedIndex = index;
                }
            }
        }

        private void ExecuteMenuItem_Click(object sender, EventArgs e)
        {
            if (Scriptbox.SelectedItem != null)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", Scriptbox.SelectedItem.ToString());
                string fileContent = File.ReadAllText(filePath);
                // Execute script functionality
                string unescapedString = EscapeJavaScriptString(fileContent);

                ExecuteForSelectedClients(unescapedString);
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

        private async void LoadEditorMenuItem_Click(object sender, EventArgs e)
        {
            if (Scriptbox.SelectedItem != null)
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts", Scriptbox.SelectedItem.ToString());
                // Load script into editor functionality
                var tabData = File.ReadAllText(filePath);
                string script = $"SetText(`{EscapeJavaScriptString(tabData)}`);";
                await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        private void LoadScriptsIntoScriptbox(string scriptsPath)
        {
            Scriptbox.Items.Clear();
            var scriptFiles = Directory.GetFiles(scriptsPath, "*.*")
                .Where(f => f.EndsWith(".lua") || f.EndsWith(".txt"))
                .Select(Path.GetFileName);
            Scriptbox.Items.AddRange(scriptFiles.ToArray());
        }

        private void InitializeFileSystemWatcher(string scriptsPath)
        {
            fileSystemWatcher = new FileSystemWatcher
            {
                Path = scriptsPath,
                Filter = "*.*",
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite
            };

            fileSystemWatcher.Created += OnScriptsFolderChanged;
            fileSystemWatcher.Deleted += OnScriptsFolderChanged;
            fileSystemWatcher.Renamed += OnScriptsFolderChanged;

            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void OnScriptsFolderChanged(object sender, FileSystemEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => LoadScriptsIntoScriptbox(fileSystemWatcher.Path)));
            }
            else
            {
                LoadScriptsIntoScriptbox(fileSystemWatcher.Path);
            }
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

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        private void InitializeBackgroundWorker()
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                HideCommandPromptWindows();
                Thread.Sleep(1); // Check every 500 milliseconds
            }
        }

        private void HideCommandPromptWindows()
        {
            // Find command prompt windows
            Process[] processes = Process.GetProcessesByName("WindowsTerminal");
            foreach (var process in processes)
            {
                ShowWindow(process.MainWindowHandle, SW_HIDE);
            }

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetTopMost(this.Handle);
        }

        private async void SynapseZ_Load(object sender, EventArgs e)
        {

            // Start the AutoInject task if AutoInject is true
            if (GlobalVariables.AutoInject)
            {
                AutoInjectManager.StartAutoInjectTask();
            }

            // Remove all existing tabs on load
            tabControl1.Tabs.Clear();


            // Add the initial "Add Tab" button
            AddNewTabButton();

            this.BringToFront();
            this.TopMost = GlobalVariables.TopMostGlobal;
            this.ShowInTaskbar = true;

            await Task.Run(ReInjectMissingPIDs);
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

        private string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to close {GlobalVariables.ExploitName}?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void InjTimer_Tick(object sender, EventArgs e)
        {
            if (process == null) return;
            UpdateLabelText($"{FinishedExpTop} (Injecting...)");
            process.Refresh();
            IntPtr mainWindowHandle = process.MainWindowHandle;
            if (mainWindowHandle != IntPtr.Zero)
            {

                // Process window is shown, now wait until it hides
                injTimer.Interval = 1000; // Change interval for next phase
                injTimer.Tick -= InjTimer_Tick;
            }
        }


        private void MaximizeButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void SetupWebView2Environment()
        {
            // Define the folder path where WebView2 runtime files are located
            webView2FolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", "WebView2Files");
        }

        private async void AddNewTab(string tabName = null, string tabContent = null, string fileName = null)
        {
            await Task.Delay(1); // Add a slight delay
            int scriptNumber = tabControl1.Tabs.Count - 1;
            string baseTabText = tabName ?? $"Script {scriptNumber + 1}";
            string tabText = baseTabText;
            int count = 1;

            // Ensure the tab name is unique
            while (tabControl1.Tabs.Any(tab => tab.Text == tabText))
            {
                tabText = $"{baseTabText} ({count})";
                count++;
            }

            Color currentColor = tabControl1.BackColor;

            // Add 8 to each RGB component, ensuring the values remain within the valid range (0-255)
            int newRed = Math.Min(currentColor.R + 68, 255);
            int newGreen = Math.Min(currentColor.G + 68, 255);
            int newBlue = Math.Min(currentColor.B + 68, 255);

            // Create a new Color with the updated RGB values
            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            Tab newTab = new Tab
            {
                Text = tabText,
                BackColor = newColor,
                ForeColor = ThemeManager.Instance.GetThemeColor("SynapseZ.TabControl.ForeColor") // Set tab text color to white
            };

            // Add WebView2 control to the new tab
            Microsoft.Web.WebView2.WinForms.WebView2 webView = new Microsoft.Web.WebView2.WinForms.WebView2
            {
                Dock = DockStyle.Fill
            };
            newTab.Controls.Add(webView);

            tabControl1.Tabs.Insert(tabControl1.Tabs.Count - 1, newTab);
            tabControl1.SelectedTab = newTab;

            PositionAddTabButton();
            currentWebView2 = webView;

            var environment = await CoreWebView2Environment.CreateAsync(null, webView2FolderPath);
            await webView.EnsureCoreWebView2Async(environment);
            string ThemeFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "theme");
            string editorFilePath = Path.Combine(ThemeFolder, "editor.html");
            if (File.Exists(editorFilePath))
            {
                // Convert the path to the required format
                GlobalVariables.CurrentEditorHTML = "file:///" + editorFilePath.Replace("\\", "/");
                // Navigate to the editor HTML file or default Ace editor content if not found
                webView.CoreWebView2.Navigate(GlobalVariables.CurrentEditorHTML);
            }
            else
            {
                // Load the default Ace editor content from embedded resources
                GlobalVariables.CurrentEditorHTML = GetEmbeddedHtmlContent("Synapse_Z.Resources.editor.html");
                // Navigate to the editor HTML file or default Ace editor content if not found
                webView.NavigateToString(GlobalVariables.CurrentEditorHTML);
            }

            webView.NavigationCompleted += async (sender, args) =>
            {
                if (webView.CoreWebView2 != null)
                {

                    // Set the text content if provided
                    if (tabContent != null)
                    {
                        string scriptSetText = $"SetText(`{EscapeJavaScriptString(tabContent)}`);";
                        await webView.CoreWebView2.ExecuteScriptAsync(scriptSetText);
                    }
                }
                string scriptSetTheme = $"SetTheme('{GlobalVariables.CurrentEditorTheme}');";
                await webView.CoreWebView2.ExecuteScriptAsync(scriptSetTheme);
            };
            
            //MessageBox.Show(GlobalVariables.CurrentEditorTheme);
        }


        private string GetEmbeddedHtmlContent(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Resource not found: " + resourceName);
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    string html = reader.ReadToEnd();
                    html = EmbedJavaScriptContent(html);
                    return html;
                }
            }
        }

        private string EmbedJavaScriptContent(string html)
        {
            html = html.Replace("src=\"ace.js\"", $"src=\"data:text/javascript;base64,{GetEmbeddedResourceBase64("Synapse_Z.Resources.ace.js")}\"");
            html = html.Replace("src=\"ext-language_tools.js\"", $"src=\"data:text/javascript;base64,{GetEmbeddedResourceBase64("Synapse_Z.Resources.ext-language_tools.js")}\"");
            html = html.Replace("src=\"mode-lua.js\"", $"src=\"data:text/javascript;base64,{GetEmbeddedResourceBase64("Synapse_Z.Resources.mode-lua.js")}\"");
            return html;
        }

        private string GetEmbeddedResourceBase64(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Resource not found: " + resourceName);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }


        private void AddNewTabButton()
        {
            Color currentColor = tabControl1.BackColor;

            // Add 8 to each RGB component, ensuring the values remain within the valid range (0-255)
            int newRed = Math.Min(currentColor.R + 8, 255);
            int newGreen = Math.Min(currentColor.G + 8, 255);
            int newBlue = Math.Min(currentColor.B + 8, 255);

            // Create a new Color with the updated RGB values
            Color newColor = Color.FromArgb(newRed, newGreen, newBlue);

            addTabButton = new Tab
            {
                Text = "+",
                BackColor = newColor,

                ForeColor = ThemeManager.Instance.GetThemeColor("SynapseZ.TabControl.ForeColor"), // Set add button text color to white
                Width = tabControl1.TabSize.Height, // Make the add button a perfect square
                Height = tabControl1.TabSize.Height
            };
            tabControl1.Tabs.Add(addTabButton);
        }

        private void tabControl1_CloseTabButtonClick(object sender, CancelTabEventArgs e)
        {
            if (e.Tab.Text != "+" && tabControl1.Tabs.Count > 2) // Prevent deletion if only one tab is left
            {
                if (GlobalVariables.TabClosingPrompt)
                {
                    var result = MessageBox.Show($"Are you sure you want to close {e.Tab.Text}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Yes)
                    {
                        // Ensure only the selected tab is removed
                        DeleteTabFile(e.Tab.Text); // Delete the corresponding JSON file
                        e.Cancel = true;
                        tabControl1.Tabs.Remove(e.Tab);
                        PositionAddTabButton();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
                else
                {
                    // Ensure only the selected tab is removed
                    DeleteTabFile(e.Tab.Text); // Delete the corresponding JSON file
                    e.Cancel = true;
                    tabControl1.Tabs.Remove(e.Tab);
                    PositionAddTabButton();
                }
            }
            else
            {
                MessageBox.Show("You cannot delete the last tab!", "Error");
                e.Cancel = true; // Cancel the tab close action to prevent removing the last tab
            }
        }

        private void DeleteTabFile(string tabName)
        {
            var tabFiles = Directory.GetFiles(tabsFolderPath, "*.json");
            foreach (var tabFile in tabFiles)
            {
                var tabData = JsonConvert.DeserializeObject<TabData>(File.ReadAllText(tabFile));
                if (tabData.Name == tabName)
                {
                    File.Delete(tabFile);
                    break;
                }
            }
        }

        private void tabControl1_TabClick(object sender, TabMouseEventArgs e)
        {
            if (e.Tab.Text == "+")
            {
                AddNewTab();
            }
        }

        private void PositionAddTabButton()
        {
            // Ensure the add tab button is always the last tab
            if (tabControl1.Tabs.Contains(addTabButton))
            {
                tabControl1.Tabs.Remove(addTabButton);
                tabControl1.Tabs.Add(addTabButton);
            }
        }

        private async void tabControl1_PageChanged(object sender, PageChangedEventArgs e)
        {
            // If the selected tab is the "Add Tab" button, switch to the newly created tab
            if (e.OldPage != null && e.OldPage.Text == "+")
            {
                await Task.Delay(1); // Add a slight delay
                var lastTab = tabControl1.Tabs[tabControl1.Tabs.Count - 2];
                tabControl1.SelectedTab = lastTab;
            }

            // Update the current WebView2 based on the selected tab
            if (tabControl1.SelectedTab != null)
            {
                currentWebView2 = tabControl1.SelectedTab.Controls.OfType<Microsoft.Web.WebView2.WinForms.WebView2>().FirstOrDefault();
            }
        }

        private void LoadSavedTabs()
        {
            var tabFiles = Directory.GetFiles(tabsFolderPath, "*.json");
            if (tabFiles.Length == 0)
            {
                AddNewTab("Script 1");
            }
            else
            {
                foreach (var tabFile in tabFiles)
                {
                    var tabData = JsonConvert.DeserializeObject<TabData>(File.ReadAllText(tabFile));
                    AddNewTab(tabData.Name, tabData.Content);
                }
            }
        }

        private async void SaveTabContent(string tabName, string content, string fileName)
        {
            if (string.IsNullOrWhiteSpace(content)) return; // Don't save blank tabs

            var tabData = new TabData { Name = tabName, Content = content, FileName = fileName };
            var json = JsonConvert.SerializeObject(tabData, Formatting.Indented);
            File.WriteAllText(Path.Combine(tabsFolderPath, $"{fileName}.json"), json);
        }

        private void InitializeTabContextMenu()
        {
            tabContextMenu = new ContextMenuStrip();

            // Save Tab Menu Item
            ToolStripMenuItem saveTabMenuItem = new ToolStripMenuItem("Save Tab");
            saveTabMenuItem.Click += SaveTabMenuItem_Click;
            tabContextMenu.Items.Add(saveTabMenuItem);

            // Rename Tab Menu Item
            ToolStripMenuItem renameTabMenuItem = new ToolStripMenuItem("Rename Tab");
            renameTabMenuItem.Click += RenameTabMenuItem_Click;
            tabContextMenu.Items.Add(renameTabMenuItem);

            // Clear Tabs Menu Item
            ToolStripMenuItem clearTabsMenuItem = new ToolStripMenuItem("Clear Tabs");
            clearTabsMenuItem.Click += ClearTabsMenuItem_Click;
            tabContextMenu.Items.Add(clearTabsMenuItem);
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

        private void TabControl1_LayoutTabs(object sender, LayoutTabsEventArgs e)
        {
            // Custom layout logic for tabs, if needed
        }

        public string[] GetOtherExecutablePaths()
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string currentExeName = Path.GetFileName(Application.ExecutablePath);
            string[] allExePaths = Directory.GetFiles(currentDirectory, "*.exe", SearchOption.TopDirectoryOnly);

            // Exclude the current executable
            string[] otherExePaths = allExePaths.Where(path => !path.EndsWith(currentExeName, StringComparison.OrdinalIgnoreCase)).ToArray();

            // Method to verify the existence of ".grh0", ".grh1" & ".grh2" in the file contents
            bool ContainsRequiredPatterns(string filePath)
            {
                byte[] pattern0 = Encoding.ASCII.GetBytes(".grh0");
                byte[] pattern1 = Encoding.ASCII.GetBytes(".grh1");
                byte[] pattern2 = Encoding.ASCII.GetBytes(".grh2");

                bool foundPattern0 = false;
                bool foundPattern1 = false;
                bool foundPattern2 = false;

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer = new byte[4096]; // Read in chunks
                    int bytesRead;

                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        if (!foundPattern0 && buffer.ContainsSequence(pattern0))
                        {
                            foundPattern0 = true;
                        }
                        if (!foundPattern1 && buffer.ContainsSequence(pattern1))
                        {
                            foundPattern1 = true;
                        }
                        if (!foundPattern2 && buffer.ContainsSequence(pattern2))
                        {
                            foundPattern2 = true;
                        }

                        if (foundPattern0 && foundPattern1 && foundPattern2)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            // Check each executable for the required patterns
            List<string> verifiedExePaths = new List<string>();
            foreach (var exePath in otherExePaths)
            {
                if (ContainsRequiredPatterns(exePath))
                {
                    verifiedExePaths.Add(exePath);
                }
            }

            if (verifiedExePaths.Count > 0)
            {
                return verifiedExePaths.ToArray();
            }
            else
            {
                MessageBox.Show("Please insert your Synapse Z loader into the same directory as this EXE.", "No Loader Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
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

        private async void Execute_Click(object sender, EventArgs e)
        {
            if (currentWebView2 != null && currentWebView2.CoreWebView2 != null)
            {
                try
                {
                    // Call the getText function defined in the HTML
                    string script = "GetText();";

                    // Execute the script
                    string result = await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);

                    // Log the result for debugging
                    //MessageBox.Show("Script result: " + result);

                    // Remove the enclosing quotes from the result if it's not null
                    if (!string.IsNullOrEmpty(result) && result != "null")
                    {

                        result = result.Trim('"');
                        string unescapedString = Regex.Unescape(result);
                        // WebSocketServerManager.ExecuteScriptForUserId("243198651", unescapedString);
                        ExecuteForSelectedClients(unescapedString);
                    }
                    else
                    {
                        MessageBox.Show("Element not found or script returned null.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error executing script: " + ex.Message);
                }
            }
        }

        private async void Clear_Click(object sender, EventArgs e)
        {
            if (currentWebView2 != null && currentWebView2.CoreWebView2 != null)
            {
                try
                {
                    if (GlobalVariables.ClearEditorPrompt)
                    {
                        var result = MessageBox.Show($"Are you sure you want to clear this editor?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                        if (result == DialogResult.Yes)
                        {
                            // Call the setText function defined in the HTML
                            string script = "ClearText();";

                            // Execute the script
                            await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);
                        }
                    }
                    else
                    {
                        // Call the setText function defined in the HTML
                        string script = "ClearText();";

                        // Execute the script
                        await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error clearing editor: " + ex.Message);
                }
            }
        }

        private async void Attach_Click(object sender, EventArgs e)
        {
            // Load existing PIDs from the PID file
            string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            string pidFilePath = Path.Combine(binDirectory, "pid.txt");
            List<int> existingPids = new List<int>();
            if (File.Exists(pidFilePath))
            {
                existingPids = File.ReadAllLines(pidFilePath).Select(int.Parse).ToList();
            }

            // Get all RobloxPlayerBeta processes
            Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");

            if (robloxProcesses.Length == 0)
            {
                MessageBox.Show("Please open Roblox.", "Injection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Filter out already injected processes
            var processesToInject = robloxProcesses.Where(p => !existingPids.Contains(p.Id)).ToArray();

            if (processesToInject.Length == 0)
            {
                MessageBox.Show("All instances of Roblox are already injected!", "Injection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Open one instance of the launcher
            string[] otherExePaths = GetOtherExecutablePaths();
            if (otherExePaths != null && otherExePaths.Length > 0)
            {
                string launcherPath = otherExePaths.First();
                Inject(launcherPath, false, processesToInject.Select(p => p.Id).ToArray());
            }
        }


        public async void Inject(string path, bool autoInject, int[] processIds)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                MessageBox.Show($"The executable at '{path}' does not exist. Please check the path and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ensure bin directory exists
            string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(binDirectory))
            {
                Directory.CreateDirectory(binDirectory);
            }

            // Ensure workspace directory exists
            string workspaceDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "workspace");
            if (!Directory.Exists(workspaceDirectory))
            {
                Directory.CreateDirectory(workspaceDirectory);
            }

            string targetFilePath = Path.Combine(workspaceDirectory, "0.635.0.6350588");

            // Check if the file exists and delete it
            if (File.Exists(targetFilePath))
            {
                try
                {
                    File.Delete(targetFilePath);
                }
                catch (Exception ex)
                {
                    return;
                }
            }

            // Load existing PIDs from the PID file
            string pidFilePath = Path.Combine(binDirectory, "pid.txt");
            List<int> existingPids = new List<int>();
            if (File.Exists(pidFilePath))
            {
                existingPids = File.ReadAllLines(pidFilePath).Select(int.Parse).ToList();
            }

            foreach (int processId in processIds)
            {
                try
                {
                    Process robloxProcess = Process.GetProcessById(processId);

                    robloxProcess.EnableRaisingEvents = true;
                    robloxProcess.Exited += (s, e) => RemoveFromExecutionPIDS(processId); // Update this line to call RemoveFromExecutionPIDS method

                    if (!existingPids.Contains(processId))
                    {
                        existingPids.Add(processId);
                        File.WriteAllLines(pidFilePath, existingPids.Select(pid => pid.ToString()));
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to get the process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            UpdateLabelText($"{FinishedExpTop} (Checking Whitelist...)");

            bool authSynExists = File.Exists(Path.Combine(binDirectory, "auth.syn"));
            bool launchSynExists = File.Exists(Path.Combine(binDirectory, "launch.syn"));

            if (authSynExists && !launchSynExists)
            {
                ClearBinFolder(binDirectory);
                MessageBox.Show("Could not find launch.syn", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!authSynExists && launchSynExists)
            {
                ClearBinFolder(binDirectory);
                MessageBox.Show("Could not find auth.syn", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!backgroundWorker.IsBusy)
            {
                backgroundWorker.RunWorkerAsync();
            }

            if (!authSynExists || !launchSynExists)
            {
                if (GlobalVariables.CurrentKey != null & GlobalVariables.CurrentKey != "")
                {
                    UpdateLabelText($"{FinishedExpTop} (Checking Whitelist...)");
                    string key = GlobalVariables.CurrentKey;
                    if (string.IsNullOrEmpty(key))
                    {
                        MessageBox.Show("No key entered. Aborting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    GlobalVariables.injecting = true;

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        WorkingDirectory = Path.GetDirectoryName(path),
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
                    process.Exited += (s, e) => Done(processIds);

                    process.Start();
                    ShowWindow(process.MainWindowHandle, SW_HIDE); // Hide the process window
                    using (StreamWriter writer = process.StandardInput)
                    {
                        writer.WriteLine(key);
                    }

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    injTimer.Start();
                }
                else
                {
                    using (var promptForm = new AccountKeyPrompt())
                    {
                        if (promptForm.ShowDialog(this) == DialogResult.OK)
                        {
                            UpdateLabelText($"{FinishedExpTop} (Checking Whitelist...)");
                            string key = promptForm.Key;
                            if (string.IsNullOrEmpty(key))
                            {
                                MessageBox.Show("No key entered. Aborting.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            GlobalVariables.injecting = true;

                            var startInfo = new ProcessStartInfo
                            {
                                FileName = path,
                                WorkingDirectory = Path.GetDirectoryName(path),
                                RedirectStandardOutput = true,
                                RedirectStandardError = true,
                                RedirectStandardInput = true,
                                UseShellExecute = false,
                                CreateNoWindow = true
                            };

                            process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
                            process.Exited += (s, e) => Done(processIds);

                            process.Start();
                            ShowWindow(process.MainWindowHandle, SW_HIDE); // Hide the process window
                            using (StreamWriter writer = process.StandardInput)
                            {
                                writer.WriteLine(key);
                            }

                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();

                            injTimer.Start();
                        }
                    }
                }
            }
            else
            {
                ClearSchedulerFolder();

                GlobalVariables.injecting = true;

                if (autoInject)
                {
                    UpdateLabelText($"{FinishedExpTop} (Waiting for roblox...)");
                    await Task.Delay(1000);
                }
                UpdateLabelText($"{FinishedExpTop} (Checking Whitelist...)");

                var startInfo = new ProcessStartInfo
                {
                    FileName = path,
                    WorkingDirectory = Path.GetDirectoryName(path),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                process = null;
                GlobalVariables.isDone = false;

                try
                {
                    process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
                    process.Exited += (s, e) => Done(processIds);

                    process.Start();
                    ShowWindow(process.MainWindowHandle, SW_HIDE); // Hide the process window
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    injTimer.Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error starting process: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Done(processIds);
                }
            }
        }


        private async void Abort(int processId)
        {
            if (GlobalVariables.injecting == true)
            {
                UpdateLabelText($"{FinishedExpTop} (Injection Aborted)");
            }
            GlobalVariables.injecting = false;

            Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
            if (robloxProcesses.Length == 0)
            {
                GlobalVariables.isDone = false;
            }

            string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(binDirectory))
            {
                Directory.CreateDirectory(binDirectory);
            }

            string pidFilePath = Path.Combine(binDirectory, "pid.txt");
            List<int> pids = new List<int>();
            if (File.Exists(pidFilePath))
            {
                pids = File.ReadAllLines(pidFilePath).Select(int.Parse).ToList();
            }

            // Remove the specified process ID from the list
            pids.Remove(processId);

            // Update the PID file
            File.WriteAllLines(pidFilePath, pids.Select(pid => pid.ToString()));

            // Remove the PID from ExecutionPIDS
            RemoveFromExecutionPIDS(processId);

            injTimer.Stop();

            await Task.Delay(1000); // Example delay, replace with your settings.injectionDelay
            UpdateLabelText(FinishedExpTop);
        }


        void ClearBinFolder(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", $"Could not clear the bin folder: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearSchedulerFolder()
        {
            string schedulerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "scheduler");
            if (Directory.Exists(schedulerPath))
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(schedulerPath);
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error clearing scheduler folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async void Done(int[] processIds)
        {
            if (GlobalVariables.isDone) return;
            if (!GlobalVariables.injecting) return;

            UpdateLabelText($"{FinishedExpTop} (Scanning...)");
            GlobalVariables.isDone = true;
            GlobalVariables.injecting = false;

            injTimer.Stop();

            if (process != null && !process.HasExited)
            {
                process.Kill();
                process.Dispose();
            }

            await Task.Delay(500);

            string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            if (!Directory.Exists(binDirectory))
            {
                Directory.CreateDirectory(binDirectory);
            }

            bool launchSynExists = File.Exists(Path.Combine(binDirectory, "launch.syn"));
            if (!launchSynExists)
            {
                ClearBinFolder(binDirectory);
                MessageBox.Show("Could not find launch.syn", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateLabelText($"{FinishedExpTop} (Injection Aborted)");
                foreach (int processId in processIds)
                {
                    Abort(processId);
                }
                return;
            }

            foreach (int processId in processIds)
            {
                // Add a timer to verify the process completion status
                var verificationTimer = new System.Windows.Forms.Timer();
                verificationTimer.Interval = 5000; // Set to 5 seconds or adjust as needed
                verificationTimer.Tick += (s, e) =>
                {
                    Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
                    if (robloxProcesses.All(p => p.Id != processId))
                    {
                        UpdateLabelText($"{FinishedExpTop} (Process Crashed)");
                        verificationTimer.Stop();
                        Abort(processId);
                    }
                    else
                    {
                        UpdateLabelText($"{FinishedExpTop}");
                        verificationTimer.Stop();
                    }
                };
                verificationTimer.Start();

                AddToExecutionPIDS(processId);
            }

            UpdateLabelText($"{FinishedExpTop} (Ready!)");

            if (GlobalVariables.UnlockFPS)
            {
                foreach (var entry in GlobalVariables.ExecutionPIDS)
                {
                    if ((bool)entry.Value)  // If the status is True
                    {
                        ExecuteScript("setfflag('DFIntTaskSchedulerTargetFps', 999999)", entry.Key);
                    }
                }
            }

            Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
            if (robloxProcesses.Length == 0)
            {
                foreach (int processId in processIds)
                {
                    Abort(processId);
                }
                return;
            }

            await Task.Delay(1000);

            UpdateLabelText(FinishedExpTop);
        }



        private void UpdateLabelText(string text)
        {
            if (synlabel.InvokeRequired)
            {
                synlabel.Invoke(new Action(() => synlabel.Text = text));
            }
            else
            {
                synlabel.Text = text;
            }
        }

        private void SetTopMost(IntPtr handle)
        {
            SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOSIZE = 0x0001;

        private async void OpenFile_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow the user to select a .txt or .lua file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|Luau Files (*.luau)|*.luau|All Files (*.*)|*.*",
                Title = "Open Script File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of the selected file
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                // Execute the setText function on the current WebView2
                if (currentWebView2 != null && currentWebView2.CoreWebView2 != null)
                {
                    try
                    {
                        string script = $"SetText(`{EscapeJavaScriptString(fileContent)}`);";
                        await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error executing script: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No active web view found to set the text.");
                }
            }
        }

        // Helper method to escape special characters in the file content for JavaScript
        private string EscapeJavaScriptString(string value)
        {
            return value.Replace("\\", "\\\\").Replace("`", "\\`").Replace("$", "\\$");
        }

        private async void SaveFile_Click(object sender, EventArgs e)
        {
            if (currentWebView2 != null && currentWebView2.CoreWebView2 != null)
            {
                try
                {
                    // Call the getText function defined in the HTML
                    string script = "GetText();";

                    // Execute the script
                    string result = await currentWebView2.CoreWebView2.ExecuteScriptAsync(script);

                    // Log the result for debugging
                    //MessageBox.Show("Script result: " + result);

                    // Remove the enclosing quotes from the result if it's not null
                    if (!string.IsNullOrEmpty(result) && result != "null")
                    {

                        result = result.Trim('"');

                    }
                    else
                    {
                        MessageBox.Show("Element not found or script returned null.");
                    }


                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        // Set the initial directory and default file name
                        saveFileDialog.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts");
                        saveFileDialog.Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt|Luau Files (*.luau)|*.luau|All Files (*.*)|*.*";
                        saveFileDialog.DefaultExt = "lua";
                        saveFileDialog.AddExtension = true;

                        // Check if the scripts folder exists, if not, create it
                        if (!Directory.Exists(saveFileDialog.InitialDirectory))
                        {
                            Directory.CreateDirectory(saveFileDialog.InitialDirectory);
                        }

                        // Show the dialog and get the result
                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            // Write the Lua script to the specified file
                            string unescapedString = Regex.Unescape(result);

                            File.WriteAllText(saveFileDialog.FileName, unescapedString);
                            MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error executing script: " + ex.Message);
                }
            }
        }

        private void ScriptHub_Click(object sender, EventArgs e)
        {
            if (scriptHubFormInstance == null || scriptHubFormInstance.IsDisposed)
            {
                // Create a new instance if it doesn't exist or is disposed
                scriptHubFormInstance = new ScriptHub();

                // Show the subform
                scriptHubFormInstance.Show();
            }
            else
            {
                // Bring the existing form to the front
                scriptHubFormInstance.WindowState = FormWindowState.Normal;
                scriptHubFormInstance.BringToFront();
            }
        }
        // Class to hold tab data
        private class TabData
        {
            public string Name { get; set; }
            public string Content { get; set; }
            public string FileName { get; set; }
        }

        private void Options_Click(object sender, EventArgs e)
        {
            if (optionsFormInstance == null || optionsFormInstance.IsDisposed)
            {
                // Create a new instance if it doesn't exist or is disposed
                optionsFormInstance = new Options();


                // Show the subform
                optionsFormInstance.Show();
            }
            else
            {
                // Bring the existing form to the front
                optionsFormInstance.WindowState = FormWindowState.Normal;
                optionsFormInstance.BringToFront();
            }
        }

        private void ExecuteFile_Click(object sender, EventArgs e)
        {
            // Create an OpenFileDialog to allow the user to select a .txt or .lua file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|Lua Files (*.lua)|*.lua|Luau Files (*.luau)|*.luau|All Files (*.*)|*.*",
                Title = "Execute Script File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of the selected file
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                // Execute the setText function on the current WebView2
                if (currentWebView2 != null && currentWebView2.CoreWebView2 != null)
                {
                    try
                    {
                        string script = EscapeJavaScriptString(fileContent);

                        ExecuteForSelectedClients(script);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error executing script: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No active web view found to set the text.");
                }
            }
        }

        private void AddToExecutionPIDS(int processId)
        {
            if (!GlobalVariables.ExecutionPIDS.ContainsKey(processId.ToString()))
            {
                bool anySelected = GlobalVariables.ExecutionPIDS.Values.Any(value => (bool)value);
                GlobalVariables.ExecutionPIDS[processId.ToString()] = !anySelected;
            }
        }

        private void RemoveFromExecutionPIDS(int processId)
        {
            if (GlobalVariables.ExecutionPIDS.ContainsKey(processId.ToString()))
            {
                GlobalVariables.ExecutionPIDS.Remove(processId.ToString());
            }
        }

        private async Task ReInjectMissingPIDs()
        {
            string binDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin");
            string pidFilePath = Path.Combine(binDirectory, "pid.txt");

            if (File.Exists(pidFilePath))
            {
                var existingPids = File.ReadAllLines(pidFilePath)
                                        .Where(line => int.TryParse(line, out _))
                                        .Select(int.Parse)
                                        .ToList();

                // Get all RobloxPlayerBeta processes
                Process[] robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");

                // Filter out already injected processes
                var processesToInject = robloxProcesses.Where(p => !GlobalVariables.ExecutionPIDS.ContainsKey(p.Id.ToString())).ToArray();

                if (processesToInject.Length == 0)
                {
                    // No processes to inject, clean up PIDs that are no longer running
                    CleanUpInvalidPIDs(existingPids, pidFilePath);
                    return;
                }

                // To avoid modifying the collection while enumerating, use a temporary list
                List<int> pidsToRemove = new List<int>();

                foreach (var pid in existingPids)
                {
                    try
                    {
                        Process reinjectProcess = Process.GetProcessById(pid);

                        // Check if the process is still running
                        if (!reinjectProcess.HasExited)
                        {
                            if (!GlobalVariables.ExecutionPIDS.ContainsKey(pid.ToString()))
                            {
                                GlobalVariables.injecting = true;

                                // Update synlabel text
                                UpdateLabelText($"{FinishedExpTop} (Re-Injecting...)");

                                // Add to ExecutionPIDS and attach close event for abort
                                AddToExecutionPIDS(pid);
                                reinjectProcess.EnableRaisingEvents = true;
                                reinjectProcess.Exited += (s, e) => Abort(pid);

                                GlobalVariables.injecting = false;
                                GlobalVariables.isDone = true;

                                await Task.Delay(100); // Add a slight delay

                                UpdateLabelText($"{FinishedExpTop} (Ready!)");
                                await Task.Delay(1000); // Add a slight delay

                                UpdateLabelText($"{FinishedExpTop}");
                            }
                        }
                        else
                        {
                            // Process has exited, add to the list of PIDs to remove
                            pidsToRemove.Add(pid);
                        }
                    }
                    catch (ArgumentException)
                    {
                        // Process not found, add to the list of PIDs to remove
                        pidsToRemove.Add(pid);
                    }
                }

                // Remove invalid PIDs after enumeration
                foreach (var pid in pidsToRemove)
                {
                    existingPids.Remove(pid);
                }

                // Update the PID file to remove invalid PIDs
                File.WriteAllLines(pidFilePath, existingPids.Select(pid => pid.ToString()));
            }
            else
            {
                // PID file does not exist
                Console.WriteLine("PID file does not exist");
            }
        }

        private void CleanUpInvalidPIDs(List<int> existingPids, string pidFilePath)
        {
            List<int> validPids = new List<int>();

            foreach (var pid in existingPids)
            {
                try
                {
                    Process.GetProcessById(pid);
                    validPids.Add(pid);
                }
                catch (ArgumentException)
                {
                    // Process not found, do not add to validPids
                }
            }

            // Update the PID file to only include valid PIDs
            File.WriteAllLines(pidFilePath, validPids.Select(pid => pid.ToString()));
        }
    }
    public static class ByteExtensions
    {
        public static bool ContainsSequence(this byte[] buffer, byte[] sequence)
        {
            for (int i = 0; i < buffer.Length - sequence.Length + 1; i++)
            {
                bool found = true;
                for (int j = 0; j < sequence.Length; j++)
                {
                    if (buffer[i + j] != sequence[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
