namespace Synapse_Z
{
    partial class AccountKeyPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountKeyPrompt));
            this.TopBar = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.Minimize = new System.Windows.Forms.Button();
            this.synlabel = new System.Windows.Forms.Label();
            this.Close = new System.Windows.Forms.Button();
            this.Maximize = new System.Windows.Forms.Button();
            this.Logo = new System.Windows.Forms.PictureBox();
            this.KeyBox = new System.Windows.Forms.TextBox();
            this.SendKey = new System.Windows.Forms.Button();
            this.TopBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // TopBar
            // 
            this.TopBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.TopBar.Controls.Add(this.button1);
            this.TopBar.Controls.Add(this.Minimize);
            this.TopBar.Controls.Add(this.synlabel);
            this.TopBar.Controls.Add(this.Close);
            this.TopBar.Controls.Add(this.Maximize);
            this.TopBar.Controls.Add(this.Logo);
            this.TopBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopBar.Location = new System.Drawing.Point(0, 0);
            this.TopBar.Name = "TopBar";
            this.TopBar.Size = new System.Drawing.Size(530, 31);
            this.TopBar.TabIndex = 2;
            this.TopBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.TopBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.TopBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(78)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(510, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.FlatButton_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.FlatButton_MouseLeave);
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
            this.Minimize.Location = new System.Drawing.Point(1111, 0);
            this.Minimize.Name = "Minimize";
            this.Minimize.Size = new System.Drawing.Size(20, 23);
            this.Minimize.TabIndex = 4;
            this.Minimize.Text = "_";
            this.Minimize.UseVisualStyleBackColor = false;
            // 
            // synlabel
            // 
            this.synlabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.synlabel.BackColor = System.Drawing.Color.Transparent;
            this.synlabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.synlabel.ForeColor = System.Drawing.Color.White;
            this.synlabel.Location = new System.Drawing.Point(131, 0);
            this.synlabel.Name = "synlabel";
            this.synlabel.Size = new System.Drawing.Size(265, 31);
            this.synlabel.TabIndex = 1;
            this.synlabel.Text = "Synapse Z - Account Key System";
            this.synlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.synlabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.synlabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.synlabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
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
            this.Close.Location = new System.Drawing.Point(1163, 0);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(20, 23);
            this.Close.TabIndex = 3;
            this.Close.Text = "X";
            this.Close.UseVisualStyleBackColor = false;
            // 
            // Maximize
            // 
            this.Maximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Maximize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.Maximize.FlatAppearance.BorderSize = 0;
            this.Maximize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.Maximize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(78)))));
            this.Maximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Maximize.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Maximize.ForeColor = System.Drawing.Color.White;
            this.Maximize.Location = new System.Drawing.Point(1137, 0);
            this.Maximize.Name = "Maximize";
            this.Maximize.Size = new System.Drawing.Size(20, 23);
            this.Maximize.TabIndex = 3;
            this.Maximize.Text = "M";
            this.Maximize.UseVisualStyleBackColor = false;
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            this.Logo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("Logo.BackgroundImage")));
            this.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Logo.Location = new System.Drawing.Point(3, 5);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(23, 23);
            this.Logo.TabIndex = 0;
            this.Logo.TabStop = false;
            // 
            // KeyBox
            // 
            this.KeyBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.KeyBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.KeyBox.Font = new System.Drawing.Font("Segoe UI Semibold", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.KeyBox.ForeColor = System.Drawing.Color.White;
            this.KeyBox.Location = new System.Drawing.Point(12, 45);
            this.KeyBox.Name = "KeyBox";
            this.KeyBox.PasswordChar = '*';
            this.KeyBox.PlaceholderText = "Enter your account key here.";
            this.KeyBox.Size = new System.Drawing.Size(506, 22);
            this.KeyBox.TabIndex = 3;
            this.KeyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // SendKey
            // 
            this.SendKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(61)))), ((int)(((byte)(61)))));
            this.SendKey.FlatAppearance.BorderSize = 0;
            this.SendKey.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(72)))), ((int)(((byte)(88)))));
            this.SendKey.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(61)))), ((int)(((byte)(80)))));
            this.SendKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SendKey.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SendKey.ForeColor = System.Drawing.Color.White;
            this.SendKey.Location = new System.Drawing.Point(139, 78);
            this.SendKey.Name = "SendKey";
            this.SendKey.Size = new System.Drawing.Size(248, 33);
            this.SendKey.TabIndex = 4;
            this.SendKey.Text = "OK";
            this.SendKey.UseVisualStyleBackColor = false;
            this.SendKey.Click += new System.EventHandler(this.SendKey_Click);
            this.SendKey.MouseEnter += new System.EventHandler(this.FlatButton_MouseEnter);
            this.SendKey.MouseLeave += new System.EventHandler(this.FlatButton_MouseLeave);
            // 
            // AccountKeyPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(530, 120);
            this.Controls.Add(this.SendKey);
            this.Controls.Add(this.KeyBox);
            this.Controls.Add(this.TopBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AccountKeyPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccountKeyPrompt";
            this.TopMost = true;
            this.TopBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel TopBar;
        private System.Windows.Forms.Button Minimize;
        private System.Windows.Forms.Label synlabel;
        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.Button Maximize;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox KeyBox;
        private System.Windows.Forms.Button SendKey;
        private System.Windows.Forms.PictureBox Logo;
    }
}