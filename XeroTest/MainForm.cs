using System.Net;
using DiscordRPC;
using XeroPresence.Properties;
using IWshRuntimeLibrary;
using System.Diagnostics;
using System;
using Newtonsoft.Json;
using File = System.IO.File;

namespace XeroPresence
{
    public partial class MainForm : Form
    {
        private static DiscordRpcClient discord;
        public bool _discordLoggedIn = false;
        public string _sessionssl = "";
        public MainForm()
        {
            InitializeComponent();
            tb_accesskey.Text = Settings.Default.accesskey;
            tb_accesskeysecret.Text = Settings.Default.accesskeysecret;
            cb_StartWithWindows.Checked = Settings.Default.windows;
            cb_HideInTray.Checked = Settings.Default.tray;
        }

        private static void InitializeDiscord()
        {
            discord.Initialize();
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            bt_login.Text = "Logging in...";
            timer1.Start();
            tb_accesskey.ReadOnly = true;
            tb_accesskeysecret.ReadOnly = true;
            bt_login.Enabled = false;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Process[] xero = Process.GetProcessesByName("xeroclient");

            //We do not want to send requests to the API if the game isn't even running
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
                var client = new HttpClient();

                client.DefaultRequestHeaders.Add("x-api-access-key-id", tb_accesskey.Text);
                client.DefaultRequestHeaders.Add("x-api-secret-access-key", tb_accesskeysecret.Text);

                var response = await client.GetAsync($"https://xero.gg/api/self/status/v?time={DateTime.Now}");
                var responseContent = await response.Content.ReadAsStringAsync();

                dynamic jsonData = JsonConvert.DeserializeObject(responseContent);

                bool _success = jsonData.success;
                if (_success == false)
                {
                    string _reason = jsonData.text;
                    MessageBox.Show($"Error while trying to read the API.\nReason:{_reason}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bt_login.Enabled = true;
                    bt_login.Text = "Start Presence";
                    timer1.Stop();
                    return;
                }

                if (_discordLoggedIn == false)
                {
                    discord = new DiscordRpcClient("1092449168703901756");
                    InitializeDiscord();
                    _discordLoggedIn = true;
                }
                bt_login.Text = "Logged in";
                string _channel = jsonData.game.channel.name;
                string _nickname = jsonData.info.name;
                int _level = jsonData.info.progression.level.value;
                int _xp = jsonData.info.progression.level.progress.current;
                int _xprequired = jsonData.info.progression.level.progress.required;
                int _xppercentage = jsonData.info.progression.level.progress.percentage;
                int _pen = jsonData.currency.pen;
                int _zp = jsonData.currency.zp;
                int _gems = jsonData.currency.gems;
                bool _hasPremium = jsonData.info.premium.enabled;
                var _roomstate = jsonData.game.room;

                if (_roomstate != null)
                {
                    var _matchstate = jsonData.game.room.match;
                    int _id = jsonData.game.room.id;
                    string _name = jsonData.game.room.name;
                    int _timelimit = jsonData.game.room.timeLimit;
                    int _scorelimit = jsonData.game.room.scoreLimit;
                    bool _isfriendly = jsonData.game.room.isFriendly;
                    bool _isPasswordProtected = jsonData.game.room.isPasswordProtected;
                    int _playerlimit = jsonData.game.room.playerLimit;
                    string _weaponlimit = jsonData.game.room.weaponLimit.fullName;
                    string _mode = jsonData.game.room.mode.name;
                    string _map = jsonData.game.room.map.name;
                    string _mapimage = jsonData.game.room.map.image;

                    if (_matchstate != null)
                    {
                        string _gameState = jsonData.game.room.match.gameState.name;
                        string _gameTimeState = jsonData.game.room.match.gameTimeState.name;
                        int _gameTime = jsonData.game.room.match.gameTime;
                        int _roundTime = jsonData.game.room.match.roundTime;

                        var _minutes = _gameTime / 60;
                        var _seconds = _gameTime % 60;
                        var _maxtimeminutes = _timelimit / 60;
                        var _maxtimeseconds = _timelimit % 60;
                        discord.UpdateLargeAsset(_mapimage, $"Playing on {_map} ({_mode})");
                        discord.UpdateDetails($"{_nickname} » {_channel} » #{_id}");

                        if (_mode == "Touchdown" || _mode == "Deathmatch")
                        {
                            int _scoreAlpha = jsonData.game.room.match.modeData.score.alpha;
                            int _scoreBeta = jsonData.game.room.match.modeData.score.beta;
                            discord.UpdateState($"{_name} | {_gameTimeState} | {_scoreAlpha}-{_scoreBeta}");
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
                        if (_isPasswordProtected)
                            discord.UpdateSmallAsset("lock", "This room is password protected");
                        else
                            discord.UpdateSmallAsset("", "");
                        discord.UpdateLargeAsset(_mapimage, $"Playing on {_map} ({_mode})");
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
                    discord.UpdateSmallAsset("", $"");
                    discord.UpdateDetails($"{_nickname} » {_channel}");
                    discord.UpdateState($"");
                    DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                    buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };
                    discord.UpdateClearTime();
                    discord.UpdateButtons(buttons);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.accesskey = tb_accesskey.Text;
            Settings.Default.accesskeysecret = tb_accesskeysecret.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
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
            Settings.Default.accesskey = tb_accesskey.Text;
            Settings.Default.accesskeysecret = tb_accesskeysecret.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.Save();
        }
    }
}