using System.Net;
using DiscordRPC;
using XeroTest.Properties;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System;
using Newtonsoft.Json;
using File = System.IO.File;
using Cookie = OpenQA.Selenium.Cookie;

namespace XeroTest
{
    public partial class MainForm : Form
    {
        private static DiscordRpcClient discord;
        public bool _discordLoggedIn = false;
        public string _sessionssl = "";
        public MainForm()
        {
            InitializeComponent();
            tb_friendemail.Text = Settings.Default.email;
            tb_friendpassword.Text = Settings.Default.password;
            cb_StartWithWindows.Checked = Settings.Default.windows;
            cb_HideInTray.Checked = Settings.Default.tray;
            cb_ShowLevel.Checked = Settings.Default.level;
        }

        private static void InitializeDiscord()
        {
            discord.Initialize();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string exePath = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName).Replace("Xero Presence.exe", "");
            string _auth = "";
            string cookieJson = "";

            bt_login.Text = "Logging in...";
            var options = new ChromeOptions();

            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService(exePath, "chromedriver.exe");
            service.HideCommandPromptWindow = true;

            IWebDriver driver = new ChromeDriver(service, options);

            if (File.Exists(exePath + @"\cookies.txt"))
            {
                driver.Navigate().GoToUrl("https://xero.gg/");
                cookieJson = File.ReadAllText(exePath + @"\cookies.txt");
                var cookies = JsonConvert.DeserializeObject<List<MyCookie>>(cookieJson);

                foreach (var cookie in cookies)
                {
                    driver.Manage().Cookies.AddCookie(new Cookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry));
                }

                driver.Navigate().Refresh();
                var loginCookie = driver.Manage().Cookies.GetCookieNamed("xero_login_ssl");

                if (loginCookie != null)
                {
                    _sessionssl = loginCookie.Value;
                }
            }
            else
            {
                driver.Navigate().GoToUrl("https://xero.gg/signin");

                driver.FindElement(By.Id("uniteddb-login-identifier")).SendKeys(tb_friendemail.Text);
                driver.FindElement(By.Id("uniteddb-login-password")).SendKeys(tb_friendpassword.Text);


                Thread.Sleep(650);
                driver.FindElement(By.Id("uniteddb-login-form-submit")).Click();

                Thread.Sleep(1000);
                if (driver.Url == "https://xero.gg/signin/authentication/")
                {
                    InputDialog inputDialog = new InputDialog();
                    DialogResult inputResult = inputDialog.ShowDialog();

                    if (inputResult == DialogResult.OK)
                    {
                        _auth = inputDialog.InputText;
                    }
                    driver.FindElement(By.Id("uniteddb-login-authentication-code")).SendKeys(_auth);
                    Thread.Sleep(650);
                    driver.FindElement(By.Id("uniteddb-login-authentication-submit")).Click();
                }

                Thread.Sleep(2000);
                var cookies = driver.Manage().Cookies.AllCookies;
                List<MyCookie> myCookies = new List<MyCookie>();
                foreach (var cookie in cookies)
                {
                    myCookies.Add(new MyCookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path, cookie.Expiry));
                    if (cookie.Name == "xero_login_ssl")
                    {
                        _sessionssl = cookie.Value;
                    }
                }
                cookieJson = JsonConvert.SerializeObject(myCookies);
                File.WriteAllText(exePath + @"\cookies.txt", cookieJson);
            }

            driver.Quit();
            Thread.Sleep(1000);
            timer1.Start();
            bt_login.Text = "Logged in";
            tb_friendemail.ReadOnly = true;
            tb_friendpassword.ReadOnly = true;
            bt_login.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int _id = 0;
            string _name = "";
            int _timelimit = 0;
            int _scorelimit = 0;
            bool _isfriendly = false;
            bool _isPasswordProtected = false;
            int _playerlimit = 0;
            string _weaponlimit = "";
            string _mode = "";
            string _map = "";
            string _mapimage = "";
            string _gameState = "";
            string _gameTimeState = "";
            int _gameTime = 0;
            int _roundTime = 0;
            int _scoreAlpha = 0;
            int _scoreBeta = 0;

            Process[] xero = Process.GetProcessesByName("xeroclient");

            if (xero.Length == 0)
            {
                try
                {
                    discord.Deinitialize();
                    _discordLoggedIn = false;
                    return;
                }
                catch
                {
                    return;
                }
            }
            else
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://xero.gg/api/self/status");
                    System.Net.Cookie cookie = new System.Net.Cookie("xero_login_ssl", _sessionssl);

                    cookie.Domain = "xero.gg";
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookie);

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    string websiteText = "";
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        websiteText = reader.ReadToEnd();
                    }
                    dynamic jsonData = JsonConvert.DeserializeObject(websiteText);


                    if (_discordLoggedIn == false)
                    {
                        discord = new DiscordRpcClient("1092449168703901756");
                        InitializeDiscord();
                        _discordLoggedIn = true;
                    }

                    string _channel = jsonData.game.channel.name;
                    string _nickname = jsonData.info.name;
                    string _clan = jsonData.info.clan.name;
                    string _level = jsonData.info.progression.level.value;
                    var _roomstate = jsonData.game.room;

                    if (_roomstate != null)
                    {
                        var _matchstate = jsonData.game.room.match;
                        _id = jsonData.game.room.id;
                        _name = jsonData.game.room.name;
                        _timelimit = jsonData.game.room.timeLimit;
                        _scorelimit = jsonData.game.room.scoreLimit;
                        _isfriendly = jsonData.game.room.isFriendly;
                        _isPasswordProtected = jsonData.game.room.isPasswordProtected;
                        _playerlimit = jsonData.game.room.playerLimit;
                        _weaponlimit = jsonData.game.room.weaponLimit.fullName;
                        _mode = jsonData.game.room.mode.name;
                        _map = jsonData.game.room.map.name;
                        _mapimage = jsonData.game.room.map.image;


                        if (_matchstate != null)
                        {
                            _gameState = jsonData.game.room.match.gameState.name;
                            _gameTimeState = jsonData.game.room.match.gameTimeState.name;
                            _gameTime = jsonData.game.room.match.gameTime;
                            _roundTime = jsonData.game.room.match.roundTime;

                            var _minutes = _gameTime / 60;
                            var _seconds = _gameTime % 60;
                            var _maxtimeminutes = _timelimit / 60;
                            var _maxtimeseconds = _timelimit % 60;
                            if (_map == "Random")
                                discord.UpdateLargeAsset("logo", $"Playing on {_map}");
                            else
                                discord.UpdateLargeAsset(_mapimage, $"Playing on {_map}");
                            discord.UpdateDetails($"{_nickname} » {_channel} » #{_id}");

                            if (_mode == "Touchdown" || _mode == "Deathmatch")
                            {
                                _scoreAlpha = jsonData.game.room.match.modeData.score.alpha;
                                _scoreBeta = jsonData.game.room.match.modeData.score.beta;
                                discord.UpdateState($"{_name} | ⏳{_gameTimeState} | 🎯{_scoreAlpha}-{_scoreBeta}");
                                int remainingSeconds = (_timelimit / 2) - _roundTime;
                                discord.UpdateStartTime(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(_roundTime)));
                                discord.UpdateEndTime(DateTime.UtcNow.AddSeconds(remainingSeconds));
                            }
                            if (_mode == "Battle Royal" || _mode == "Chaser")
                            {
                                discord.UpdateState($"{_name} | {_gameState}");
                                int remainingSeconds = _timelimit - _roundTime;
                                discord.UpdateStartTime(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(_roundTime)));
                                discord.UpdateEndTime(DateTime.UtcNow.AddSeconds(remainingSeconds));
                            }

                            if (_isPasswordProtected)
                                discord.UpdateSmallAsset("lock", "This room is password protected");
                            else
                                discord.UpdateSmallAsset("", "");

                            DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                            buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };

                            discord.UpdateButtons(buttons);
                        }
                        else
                        {
                            if (_map == "Random")
                                discord.UpdateLargeAsset("logo", $"Playing on {_map}");
                            else
                                discord.UpdateLargeAsset(_mapimage, $"Playing on {_map}");
                            discord.UpdateDetails($"{_nickname} » {_channel} » #{_id}");
                            discord.UpdateState($"{_name} | Waiting");
                            DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                            buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };
                            discord.UpdateClearTime();
                            discord.UpdateButtons(buttons);
                        }
                    }
                    else
                    {
                        discord.UpdateLargeAsset("logo", $"In Lobby");
                        discord.UpdateDetails($"{_nickname} » {_channel} » Lobby");
                        discord.UpdateState($"");
                        DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                        buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };
                        discord.UpdateClearTime();
                        discord.UpdateButtons(buttons);
                    }
                }
                catch
                {
                    //Do nothing kekw
                }
                
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.level = cb_ShowLevel.Checked;
            Settings.Default.Save();
        }

        private void cb_StartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "Xero Presence.lnk");

            if (cb_StartWithWindows.Checked)
            {
                // Create shortcut to the program's exe file in the startup folder
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.Save();
            }
            else
            {
                // Delete shortcut from the startup folder
                File.Delete(shortcutPath);
            }
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.ShowInTaskbar = true;
                notifyIcon.Visible = false;
                cb_HideInTray.Checked = false;
            }
        }

        private void cb_HideInTray_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_HideInTray.Checked)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (cb_StartWithWindows.Checked)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
            if (cb_HideInTray.Checked)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (cb_StartWithWindows.Checked)
            {
                bt_login.Focus();
                bt_login.PerformClick();
            }
        }

        private void bt_save_Click(object sender, EventArgs e)
        {
            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.level = cb_ShowLevel.Checked;
            Settings.Default.Save();
        }

        public class InputDialog : Form
        {
            private System.Windows.Forms.TextBox textBox1;
            private System.Windows.Forms.Button okButton;
            private System.Windows.Forms.Button cancelButton;

            public string InputText { get { return textBox1.Text; } }

            public InputDialog()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.textBox1 = new System.Windows.Forms.TextBox();
                this.okButton = new System.Windows.Forms.Button();
                this.cancelButton = new System.Windows.Forms.Button();
                this.SuspendLayout();

                // textBox1
                this.textBox1.Location = new System.Drawing.Point(12, 12);
                this.textBox1.Name = "textBox1";
                this.textBox1.Size = new System.Drawing.Size(260, 20);
                this.textBox1.TabIndex = 0;

                // okButton
                this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.okButton.Location = new System.Drawing.Point(116, 51);
                this.okButton.Name = "okButton";
                this.okButton.Size = new System.Drawing.Size(75, 23);
                this.okButton.TabIndex = 1;
                this.okButton.Text = "OK";
                this.okButton.UseVisualStyleBackColor = true;

                // cancelButton
                this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.cancelButton.Location = new System.Drawing.Point(197, 51);
                this.cancelButton.Name = "cancelButton";
                this.cancelButton.Size = new System.Drawing.Size(75, 23);
                this.cancelButton.TabIndex = 2;
                this.cancelButton.Text = "Cancel";
                this.cancelButton.UseVisualStyleBackColor = true;

                // InputDialog
                this.ClientSize = new System.Drawing.Size(284, 86);
                this.Controls.Add(this.cancelButton);
                this.Controls.Add(this.okButton);
                this.Controls.Add(this.textBox1);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "InputDialog";
                this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                this.Text = "Enter 2FA Code";
                this.ResumeLayout(false);
                this.PerformLayout();

            }
        }

        class MyCookie
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Domain { get; set; }
            public string Path { get; set; }
            public DateTime? Expiry { get; set; }

            public MyCookie(string name, string value, string domain, string path, DateTime? expiry)
            {
                Name = name;
                Value = value;
                Domain = domain;
                Path = path;
                Expiry = expiry;
            }
        }
    }
}