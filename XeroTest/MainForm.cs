using DiscordRPC;
using XeroPresence.Properties;
using System.Diagnostics;
using Newtonsoft.Json;
using File = System.IO.File;
using System.Text.RegularExpressions;
using IWshRuntimeLibrary;

namespace XeroPresence
{
    public partial class MainForm : Form
    {
        private static DiscordRpcClient discord;
        public static bool _discordLoggedIn = false;
        public MainForm()
        {
            InitializeComponent();
            tb_accesskey.Text = Settings.Default.accesskey;
            tb_accesskeysecret.Text = Settings.Default.accesskeysecret;
            cb_StartWithWindows.Checked = Settings.Default.windows;
            cb_HideInTray.Checked = Settings.Default.tray;
            cb_ShowLevel.Checked = Settings.Default.showlevel;
        }

        private static void InitializeDiscord()
        {
            discord.Initialize();
            _discordLoggedIn = discord.IsInitialized;
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
            try
            {
                string customtext = "";
                Process[] xero = Process.GetProcessesByName("xeroclient");

                //We do not want to send requests to the API if the game isn't even running
                if (xero.Length == 0)
                {
                    bt_login.Text = "Waiting for Xero...";
                    if (_discordLoggedIn == true)
                    {
                        try
                        {
                            discord.Deinitialize();
                            _discordLoggedIn = false;
                            return;
                        }
                        catch
                        {
                            _discordLoggedIn = false;
                            return;
                        }
                    }
                }
                else
                {

                    if (_discordLoggedIn == false)
                    {
                        discord = new DiscordRpcClient("1092449168703901756");
                        InitializeDiscord();
                    }

                    var client = new HttpClient();

                    client.DefaultRequestHeaders.Add("x-api-access-key-id", tb_accesskey.Text);
                    client.DefaultRequestHeaders.Add("x-api-secret-access-key", tb_accesskeysecret.Text);

                    var response = await client.GetAsync($"https://xero.gg/api/self/status/v?time={DateTime.Now}");
                    var responseContent = await response.Content.ReadAsStringAsync();

                    dynamic jsonData = JsonConvert.DeserializeObject(responseContent);

                    string exePath = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName).Replace("XeroPresence.exe", "");

                    bool _success = jsonData.success;
                    if (_success == false)
                    {
                        string _reason = jsonData.text;
                        if (_reason == "Please wait a moment.")
                            return;
                        MessageBox.Show($"Error while trying to read the API.\nReason: {_reason}\n\nPlease click on 'Start Presence' again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bt_login.Enabled = true;
                        bt_login.Text = "Start Presence";
                        this.Visible = true;
                        this.ShowInTaskbar = true;
                        notifyIcon.Visible = false;
                        cb_HideInTray.Checked = false;
                        try
                        {
                            _discordLoggedIn = false;
                            discord.Deinitialize();
                            timer1.Stop();
                            return;
                        }
                        catch
                        {
                            timer1.Stop();
                            return;
                        }
                    }

                    bt_login.Text = "Logged in";

                    var _isOnline = jsonData.game.online;
                    var _isServer = jsonData.game.server;
                    var _isChannel = jsonData.game.channel;

                    if (_isOnline == null || _isServer == null || _isChannel == null)
                        return;

                    if (File.Exists(exePath + "/config.json"))
                        customtext = File.ReadAllText(exePath + "/config.json");

                    string _channel = jsonData.game.channel.name;
                    string _nickname = jsonData.info.name;
                    int _level = jsonData.info.progression.level.value;
                    string _levelimage = jsonData.info.progression.level.image;
                    int _xp = jsonData.info.progression.level.progress.current;
                    int _xprequired = jsonData.info.progression.level.progress.required;
                    int _xppercentage = jsonData.info.progression.level.progress.percentage;
                    int _xptotal = jsonData.info.progression.xp;
                    int _pen = jsonData.currency.pen;
                    int _zp = jsonData.currency.zp;
                    int _gems = jsonData.currency.gems;
                    bool _hasPremium = jsonData.info.premium.enabled;
                    var _roomstate = jsonData.game.room;

                    if (_roomstate != null)
                    {
                        var _playerdata = jsonData.game.room.match.playerData;
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

                        if (_playerdata != null)
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
                                discord.UpdateState($"{_name} | {_gameTimeState} | {_scoreAlpha} - {_scoreBeta}");
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
                            string pattern = @"\[(PEN|ZP|Gems|Username|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|Mode|Map|MapImage)\]";
                            Regex regex = new Regex(pattern);

                            dynamic _customtext = JsonConvert.DeserializeObject(customtext);
                            bool _overwritedetails = _customtext.Room.OverwriteDetails;
                            bool _overwritestate = _customtext.Room.OverwriteState;
                            bool _overwritelargeasset = _customtext.Room.OverwriteLargeAsset;
                            bool _overwritesmallasset = _customtext.Room.OverwriteSmallAsset;

                            string _details = _customtext.Room.Details;
                            string _state = _customtext.Room.State;
                            string _largeasseturl = _customtext.Room.LargeAssetURL;
                            string _largeassettext = _customtext.Room.LargeAssetText;
                            string _smallasseturl = _customtext.Room.SmallAssetURL;
                            string _smallassettext = _customtext.Room.SmallAssetText;

                            string _newdetails = regex.Replace(_details, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            string _newstate = regex.Replace(_state, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            string _newlargeasseturl = regex.Replace(_largeasseturl, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            string _newlargeassettext = regex.Replace(_largeassettext, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            string _newsmallasseturl = regex.Replace(_smallasseturl, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            string _newsmallassettext = regex.Replace(_smallassettext, match =>
                            {
                                string tag = match.Groups[1].Value;
                                switch (tag)
                                {
                                    case "PEN":
                                        return _pen.ToString("#,##0");
                                    case "ZP":
                                        return _zp.ToString("#,##0");
                                    case "Gems":
                                        return _gems.ToString("#,##0");
                                    case "Username":
                                        return _nickname;
                                    case "Level":
                                        return _level.ToString();
                                    case "LevelImage":
                                        return _levelimage;
                                    case "XP":
                                        return _xp.ToString("#,##0");
                                    case "XPRequired":
                                        return _xprequired.ToString("#,##0");
                                    case "XPPercentage":
                                        return _xppercentage.ToString();
                                    case "TotalXP":
                                        return _xptotal.ToString("#,##0");
                                    case "Channel":
                                        return _channel;
                                    case "RoomID":
                                        return _id.ToString();
                                    case "RoomName":
                                        return _name;
                                    case "ScoreLimit":
                                        return _scorelimit.ToString();
                                    case "PlayerLimit":
                                        return _playerlimit.ToString();
                                    case "Mode":
                                        return _mode;
                                    case "Map":
                                        return _map;
                                    case "MapImage":
                                        return _mapimage;
                                    default:
                                        return match.Value;
                                }
                            });

                            if (_overwritesmallasset)
                                discord.UpdateSmallAsset(_newsmallasseturl, _newsmallassettext);
                            else
                            {
                                if (_isPasswordProtected)
                                    discord.UpdateSmallAsset("lock", "This room is password protected");
                                else
                                    discord.UpdateSmallAsset("", "");
                            }
                            if (_overwritelargeasset)
                                discord.UpdateLargeAsset(_newlargeasseturl, _newlargeassettext);
                            else
                                discord.UpdateLargeAsset(_mapimage, $"Playing on {_map} ({_mode})");
                            if (_overwritedetails)
                                discord.UpdateDetails(_newdetails);
                            else
                                discord.UpdateDetails($"{_nickname} » {_channel} » #{_id}");
                            if (_overwritestate)
                                discord.UpdateState(_newstate);
                            else
                                discord.UpdateState($"{_name} | Waiting");
                            DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                            buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };
                            discord.UpdateClearTime();
                            discord.UpdateButtons(buttons);
                        }
                    }
                    else
                    {
                        string pattern = @"\[(PEN|ZP|Gems|Username|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel)\]";
                        Regex regex = new Regex(pattern);

                        dynamic _customtext = JsonConvert.DeserializeObject(customtext);
                        bool _overwritedetails = _customtext.Lobby.OverwriteDetails;
                        bool _overwritestate = _customtext.Lobby.OverwriteState;
                        bool _overwritelargeasset = _customtext.Lobby.OverwriteLargeAsset;
                        bool _overwritesmallasset = _customtext.Lobby.OverwriteSmallAsset;

                        string _details = _customtext.Lobby.Details;
                        string _state = _customtext.Lobby.State;
                        string _largeasseturl = _customtext.Lobby.LargeAssetURL;
                        string _largeassettext = _customtext.Lobby.LargeAssetText;
                        string _smallasseturl = _customtext.Lobby.SmallAssetURL;
                        string _smallassettext = _customtext.Lobby.SmallAssetText;

                        string _newdetails = regex.Replace(_details, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        string _newstate = regex.Replace(_state, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        string _newlargeasseturl = regex.Replace(_largeasseturl, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        string _newlargeassettext = regex.Replace(_largeassettext, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        string _newsmallasseturl = regex.Replace(_smallasseturl, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        string _newsmallassettext = regex.Replace(_smallassettext, match =>
                        {
                            string tag = match.Groups[1].Value;
                            switch (tag)
                            {
                                case "PEN":
                                    return _pen.ToString("#,##0");
                                case "ZP":
                                    return _zp.ToString("#,##0");
                                case "Gems":
                                    return _gems.ToString("#,##0");
                                case "Username":
                                    return _nickname;
                                case "Level":
                                    return _level.ToString();
                                case "LevelImage":
                                    return _levelimage;
                                case "XP":
                                    return _xp.ToString("#,##0");
                                case "XPRequired":
                                    return _xprequired.ToString("#,##0");
                                case "XPPercentage":
                                    return _xppercentage.ToString();
                                case "TotalXP":
                                    return _xptotal.ToString("#,##0");
                                case "Channel":
                                    return _channel;
                                default:
                                    return match.Value;
                            }
                        });

                        if (_overwritelargeasset)
                            discord.UpdateLargeAsset(_newlargeasseturl, _newlargeassettext);
                        else
                        {
                            if (cb_ShowLevel.Checked)
                                discord.UpdateLargeAsset(_levelimage, $"Level {_level}");
                            else
                                discord.UpdateLargeAsset("logo", $"In Lobby");
                        }
                        if (_overwritesmallasset)
                            discord.UpdateSmallAsset(_newsmallasseturl, _newsmallassettext);
                        else
                            discord.UpdateSmallAsset("", "");
                        if (_overwritedetails)
                            discord.UpdateDetails(_newdetails);
                        else
                            discord.UpdateDetails($"{_nickname} » {_channel}");
                        if (_overwritestate)
                            discord.UpdateState(_newstate);
                        else
                            discord.UpdateState($"Level {_level}, XP: {_xp.ToString("#,##0")}/{_xprequired.ToString("#,##0")} ({_xppercentage}%)");
                        DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
                        buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };
                        discord.UpdateClearTime();
                        discord.UpdateButtons(buttons);
                    }
                }
            }
            catch
            {
                //Do nothing
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.accesskey = tb_accesskey.Text;
            Settings.Default.accesskeysecret = tb_accesskeysecret.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.showlevel = cb_ShowLevel.Checked;
            Settings.Default.Save();
        }

        private void cb_StartWithWindows_CheckedChanged(object sender, EventArgs e)
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string shortcutPath = Path.Combine(startupFolder, "XeroPresence.lnk");

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
            Settings.Default.showlevel = cb_ShowLevel.Checked;
            Settings.Default.Save();
        }
    }
}