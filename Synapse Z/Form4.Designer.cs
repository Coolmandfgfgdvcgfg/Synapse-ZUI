namespace Synapse_Z
{
    partial class Options
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.panel2 = new System.Windows.Forms.Panel();
            this.Minimize = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.synlabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.UnlockFPS = new System.Windows.Forms.CheckBox();
            this.AutoInject = new System.Windows.Forms.CheckBox();
            this.ClearEditorPrompt = new System.Windows.Forms.CheckBox();
            this.TabClosingPrompt = new System.Windows.Forms.CheckBox();
            this.TopMostCheck = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.GetCurrentKey = new System.Windows.Forms.Button();
            this.EnterKey = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.KillRBLX = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.panel2.Controls.Add(this.Minimize);
            this.panel2.Controls.Add(this.Close);
            this.panel2.Controls.Add(this.synlabel);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(330, 31);
            this.panel2.TabIndex = 2;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // Minimize
            // 
            this.Minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Minimize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.Minimize.FlatAppearance.BorderSize = 0;
            this.Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(78)))));
            this.Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Minimize.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Minimize.ForeColor = System.Drawing.Color.White;
            this.Minimize.Location = new System.Drawing.Point(284, 0);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(20, 23);
            this.Minimize.TabIndex = 4;
            this.Minimize.Text = "_";
            this.Minimize.UseVisualStyleBackColor = false;
            this.Minimize.Click += new System.EventHandler(this.Minimize_Click);
            this.Minimize.MouseEnter += new System.EventHandler(this.FlatButton_MouseEnter);
            this.Minimize.MouseLeave += new System.EventHandler(this.FlatButton_MouseLeave);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.Close.FlatAppearance.BorderSize = 0;
            this.Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(78)))));
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Close.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Close.ForeColor = System.Drawing.Color.White;
            this.Close.Location = new System.Drawing.Point(310, 0);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(20, 23);
            this.Close.TabIndex = 3;
            this.Close.Text = "X";
            this.Close.UseVisualStyleBackColor = false;
            this.Close.Click += new System.EventHandler(this.CloseBtn_Click);
            this.Close.MouseEnter += new System.EventHandler(this.FlatButton_MouseEnter);
            this.Close.MouseLeave += new System.EventHandler(this.FlatButton_MouseLeave);
            // 
            // synlabel
            // 
            this.synlabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.synlabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.synlabel.ForeColor = System.Drawing.Color.White;
            this.synlabel.Location = new System.Drawing.Point(33, 0);
            this.synlabel.Name = "synlabel";
            this.synlabel.Size = new System.Drawing.Size(252, 31);
            this.synlabel.TabIndex = 1;
            this.synlabel.Text = "Synapse Z - Options";
            this.synlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.synlabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.synlabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.synlabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(23, 23);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // UnlockFPS
            // 
            this.UnlockFPS.AutoSize = true;
            this.UnlockFPS.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.UnlockFPS.ForeColor = System.Drawing.Color.White;
            this.UnlockFPS.Location = new System.Drawing.Point(17, 45);
            this.UnlockFPS.Name = "UnlockFPS";
            this.UnlockFPS.Size = new System.Drawing.Size(273, 25);
            this.UnlockFPS.TabIndex = 3;
            this.UnlockFPS.Text = "                                         Unlock FPS";
            this.UnlockFPS.UseVisualStyleBackColor = true;
            this.UnlockFPS.CheckedChanged += new System.EventHandler(this.UnlockFPS_CheckedChanged);
            // 
            // AutoInject
            // 
            this.AutoInject.AutoSize = true;
            this.AutoInject.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AutoInject.ForeColor = System.Drawing.Color.White;
            this.AutoInject.Location = new System.Drawing.Point(17, 12);
            this.AutoInject.Name = "AutoInject";
            this.AutoInject.Size = new System.Drawing.Size(274, 25);
            this.AutoInject.TabIndex = 3;
            this.AutoInject.Text = "                                         Auto Inject";
            this.AutoInject.UseVisualStyleBackColor = true;
            this.AutoInject.CheckedChanged += new System.EventHandler(this.AutoInject_CheckedChanged);
            // 
            // ClearEditorPrompt
            // 
            this.ClearEditorPrompt.AutoSize = true;
            this.ClearEditorPrompt.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.ClearEditorPrompt.ForeColor = System.Drawing.Color.White;
            this.ClearEditorPrompt.Location = new System.Drawing.Point(17, 76);
            this.ClearEditorPrompt.Name = "ClearEditorPrompt";
            this.ClearEditorPrompt.Size = new System.Drawing.Size(273, 25);
            this.ClearEditorPrompt.TabIndex = 3;
            this.ClearEditorPrompt.Text = "                         Clear Editor Prompt";
            this.ClearEditorPrompt.UseVisualStyleBackColor = true;
            this.ClearEditorPrompt.CheckedChanged += new System.EventHandler(this.ClearEditorPrompt_CheckedChanged);
            // 
            // TabClosingPrompt
            // 
            this.TabClosingPrompt.AutoSize = true;
            this.TabClosingPrompt.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TabClosingPrompt.ForeColor = System.Drawing.Color.White;
            this.TabClosingPrompt.Location = new System.Drawing.Point(17, 109);
            this.TabClosingPrompt.Name = "TabClosingPrompt";
            this.TabClosingPrompt.Size = new System.Drawing.Size(271, 25);
            this.TabClosingPrompt.TabIndex = 3;
            this.TabClosingPrompt.Text = "                         Tab Closing Prompt";
            this.TabClosingPrompt.UseVisualStyleBackColor = true;
            this.TabClosingPrompt.CheckedChanged += new System.EventHandler(this.TabClosingPrompt_CheckedChanged);
            // 
            // TopMostCheck
            // 
            this.TopMostCheck.AutoSize = true;
            this.TopMostCheck.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.TopMostCheck.ForeColor = System.Drawing.Color.White;
            this.TopMostCheck.Location = new System.Drawing.Point(17, 142);
            this.TopMostCheck.Name = "TopMostCheck";
            this.TopMostCheck.Size = new System.Drawing.Size(270, 25);
            this.TopMostCheck.TabIndex = 3;
            this.TopMostCheck.Text = "                                           Top Most";
            this.TopMostCheck.UseVisualStyleBackColor = true;
            this.TopMostCheck.CheckedChanged += new System.EventHandler(this.TopMost_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.GetCurrentKey);
            this.panel1.Controls.Add(this.EnterKey);
            this.panel1.Controls.Add(this.Reset);
            this.panel1.Controls.Add(this.KillRBLX);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.AutoInject);
            this.panel1.Controls.Add(this.UnlockFPS);
            this.panel1.Controls.Add(this.TopMostCheck);
            this.panel1.Controls.Add(this.ClearEditorPrompt);
            this.panel1.Controls.Add(this.TabClosingPrompt);
            this.panel1.Location = new System.Drawing.Point(12, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 348);
            this.panel1.TabIndex = 4;
            // 
            // GetCurrentKey
            // 
            this.GetCurrentKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GetCurrentKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.GetCurrentKey.FlatAppearance.BorderSize = 0;
            this.GetCurrentKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.GetCurrentKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(61)))), ((int)(((byte)(80)))));
            this.GetCurrentKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetCurrentKey.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.GetCurrentKey.ForeColor = System.Drawing.Color.White;
            this.GetCurrentKey.Location = new System.Drawing.Point(158, 278);
            this.GetCurrentKey.Name = "GetCurrentKey";
            this.GetCurrentKey.Size = new System.Drawing.Size(97, 26);
            this.GetCurrentKey.TabIndex = 10;
            this.GetCurrentKey.Text = "Get Current Key";
            this.GetCurrentKey.UseVisualStyleBackColor = false;
            this.GetCurrentKey.Click += new System.EventHandler(this.GetCurrentKey_Click);
            // 
            // EnterKey
            // 
            this.EnterKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EnterKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.EnterKey.FlatAppearance.BorderSize = 0;
            this.EnterKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.EnterKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(61)))), ((int)(((byte)(80)))));
            this.EnterKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.EnterKey.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.EnterKey.ForeColor = System.Drawing.Color.White;
            this.EnterKey.Location = new System.Drawing.Point(50, 278);
            this.EnterKey.Name = "EnterKey";
            this.EnterKey.Size = new System.Drawing.Size(97, 26);
            this.EnterKey.TabIndex = 10;
            this.EnterKey.Text = "Set Current Key";
            this.EnterKey.UseVisualStyleBackColor = false;
            this.EnterKey.Click += new System.EventHandler(this.EnterKey_Click);
            // 
            // Reset
            // 
            this.Reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.Reset.FlatAppearance.BorderSize = 0;
            this.Reset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.Reset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(61)))), ((int)(((byte)(80)))));
            this.Reset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Reset.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Reset.ForeColor = System.Drawing.Color.White;
            this.Reset.Location = new System.Drawing.Point(50, 308);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(205, 26);
            this.Reset.TabIndex = 10;
            this.Reset.Text = "Reset Settings";
            this.Reset.UseVisualStyleBackColor = false;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // KillRBLX
            // 
            this.KillRBLX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.KillRBLX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.KillRBLX.FlatAppearance.BorderSize = 0;
            this.KillRBLX.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.KillRBLX.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(61)))), ((int)(((byte)(80)))));
            this.KillRBLX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.KillRBLX.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.KillRBLX.ForeColor = System.Drawing.Color.White;
            this.KillRBLX.Location = new System.Drawing.Point(50, 187);
            this.KillRBLX.Name = "KillRBLX";
            this.KillRBLX.Size = new System.Drawing.Size(205, 26);
            this.KillRBLX.TabIndex = 9;
            this.KillRBLX.Text = "Kill Roblox";
            this.KillRBLX.UseVisualStyleBackColor = false;
            this.KillRBLX.Click += new System.EventHandler(this.KillRBLX_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(116, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Editor Theme";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.comboBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "chrome",
            "clouds",
            "crimson_editor",
            "dawn",
            "dreamweaver",
            "eclipse",
            "github",
            "iplastic",
            "katzenmilch",
            "kuroir",
            "solarized_light",
            "sqlserver",
            "textmate",
            "tomorrow",
            "xcode",
            "ambiance",
            "chaos",
            "clouds_midnight",
            "cobalt",
            "dracula",
            "gob",
            "gruvbox",
            "idle_fingers",
            "kr_theme",
            "merbivore",
            "merbivore_soft",
            "mono_industrial",
            "monokai",
            "pastel_on_dark",
            "solarized_dark",
            "terminal",
            "tomorrow_night",
            "tomorrow_night_blue",
            "tomorrow_night_bright",
            "tomorrow_night_eighties",
            "twilight"});
            this.comboBox1.Location = new System.Drawing.Point(17, 246);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(273, 24);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(330, 397);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form4";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Minimize;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Label synlabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox UnlockFPS;
        private System.Windows.Forms.CheckBox AutoInject;
        private System.Windows.Forms.CheckBox ClearEditorPrompt;
        private System.Windows.Forms.CheckBox TabClosingPrompt;
        private System.Windows.Forms.CheckBox TopMostCheck;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button KillRBLX;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Button EnterKey;
        private System.Windows.Forms.Button GetCurrentKey;
    }
}