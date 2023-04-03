namespace XeroTest
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
            bt_login = new Button();
            tb_sessionid = new TextBox();
            label1 = new Label();
            label2 = new Label();
            tb_friendnickname = new TextBox();
            timer1 = new System.Windows.Forms.Timer(components);
            cb_ShowLevel = new CheckBox();
            SuspendLayout();
            // 
            // bt_login
            // 
            bt_login.Location = new Point(12, 124);
            bt_login.Name = "bt_login";
            bt_login.Size = new Size(272, 23);
            bt_login.TabIndex = 0;
            bt_login.Text = "Start Presence";
            bt_login.UseVisualStyleBackColor = true;
            bt_login.Click += button1_Click;
            // 
            // tb_sessionid
            // 
            tb_sessionid.Location = new Point(12, 26);
            tb_sessionid.Name = "tb_sessionid";
            tb_sessionid.Size = new Size(272, 23);
            tb_sessionid.TabIndex = 1;
            tb_sessionid.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 8);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 2;
            label1.Text = "Enter session id:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 52);
            label2.Name = "label2";
            label2.Size = new Size(100, 15);
            label2.TabIndex = 4;
            label2.Text = "Friend Nickname:";
            // 
            // tb_friendnickname
            // 
            tb_friendnickname.Location = new Point(12, 70);
            tb_friendnickname.Name = "tb_friendnickname";
            tb_friendnickname.Size = new Size(272, 23);
            tb_friendnickname.TabIndex = 3;
            // 
            // timer1
            // 
            timer1.Interval = 5000;
            timer1.Tick += timer1_Tick;
            // 
            // cb_ShowLevel
            // 
            cb_ShowLevel.AutoSize = true;
            cb_ShowLevel.Location = new Point(12, 99);
            cb_ShowLevel.Name = "cb_ShowLevel";
            cb_ShowLevel.Size = new Size(159, 19);
            cb_ShowLevel.TabIndex = 5;
            cb_ShowLevel.Text = "Show Level as LargeAsset";
            cb_ShowLevel.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(300, 154);
            Controls.Add(cb_ShowLevel);
            Controls.Add(label2);
            Controls.Add(tb_friendnickname);
            Controls.Add(label1);
            Controls.Add(tb_sessionid);
            Controls.Add(bt_login);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Xero Presence";
            FormClosing += Form1_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button bt_login;
        private TextBox tb_sessionid;
        private Label label1;
        private Label label2;
        private TextBox tb_friendnickname;
        private System.Windows.Forms.Timer timer1;
        private CheckBox cb_ShowLevel;
    }
}