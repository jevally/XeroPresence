namespace XeroPresence
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            bt_login = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            label1 = new Label();
            tb_accesskey = new TextBox();
            label3 = new Label();
            tb_accesskeysecret = new TextBox();
            notifyIcon = new NotifyIcon(components);
            bt_save = new Button();
            groupBox1 = new GroupBox();
            cb_HideInTray = new CheckBox();
            cb_StartWithWindows = new CheckBox();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // bt_login
            // 
            bt_login.Location = new Point(12, 206);
            bt_login.Name = "bt_login";
            bt_login.Size = new Size(272, 23);
            bt_login.TabIndex = 0;
            bt_login.Text = "Start Presence";
            bt_login.UseVisualStyleBackColor = true;
            bt_login.Click += button1_Click;
            // 
            // timer1
            // 
            timer1.Interval = 10000;
            timer1.Tick += timer1_Tick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 7;
            label1.Text = "Access Key ID:";
            // 
            // tb_accesskey
            // 
            tb_accesskey.Location = new Point(12, 27);
            tb_accesskey.Name = "tb_accesskey";
            tb_accesskey.Size = new Size(272, 23);
            tb_accesskey.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 53);
            label3.Name = "label3";
            label3.Size = new Size(103, 15);
            label3.TabIndex = 9;
            label3.Text = "Secret Access Key:";
            // 
            // tb_accesskeysecret
            // 
            tb_accesskeysecret.Location = new Point(12, 71);
            tb_accesskeysecret.Name = "tb_accesskeysecret";
            tb_accesskeysecret.Size = new Size(272, 23);
            tb_accesskeysecret.TabIndex = 2;
            tb_accesskeysecret.UseSystemPasswordChar = true;
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "Xero Presence";
            notifyIcon.MouseClick += notifyIcon_MouseClick;
            // 
            // bt_save
            // 
            bt_save.Location = new Point(12, 177);
            bt_save.Name = "bt_save";
            bt_save.Size = new Size(272, 23);
            bt_save.TabIndex = 11;
            bt_save.Text = "Save Settings";
            bt_save.UseVisualStyleBackColor = true;
            bt_save.Click += bt_save_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cb_HideInTray);
            groupBox1.Controls.Add(cb_StartWithWindows);
            groupBox1.Location = new Point(12, 101);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(272, 71);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Settings";
            // 
            // cb_HideInTray
            // 
            cb_HideInTray.AutoSize = true;
            cb_HideInTray.Location = new Point(6, 46);
            cb_HideInTray.Name = "cb_HideInTray";
            cb_HideInTray.Size = new Size(88, 19);
            cb_HideInTray.TabIndex = 6;
            cb_HideInTray.Text = "Hide in Tray";
            cb_HideInTray.UseVisualStyleBackColor = true;
            cb_HideInTray.CheckedChanged += cb_HideInTray_CheckedChanged;
            // 
            // cb_StartWithWindows
            // 
            cb_StartWithWindows.AutoSize = true;
            cb_StartWithWindows.Location = new Point(6, 21);
            cb_StartWithWindows.Name = "cb_StartWithWindows";
            cb_StartWithWindows.Size = new Size(128, 19);
            cb_StartWithWindows.TabIndex = 5;
            cb_StartWithWindows.Text = "Start with Windows";
            cb_StartWithWindows.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 234);
            Controls.Add(bt_save);
            Controls.Add(groupBox1);
            Controls.Add(label3);
            Controls.Add(tb_accesskeysecret);
            Controls.Add(label1);
            Controls.Add(tb_accesskey);
            Controls.Add(bt_login);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Xero Presence";
            FormClosing += Form1_FormClosing;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_login;
        private System.Windows.Forms.Timer timer1;
        private Label label1;
        private TextBox tb_accesskey;
        private Label label3;
        private TextBox tb_accesskeysecret;
        private NotifyIcon notifyIcon;
        private Button bt_save;
        private GroupBox groupBox1;
        private CheckBox cb_HideInTray;
        private CheckBox cb_StartWithWindows;
    }
}