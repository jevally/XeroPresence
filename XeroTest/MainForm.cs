using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using DiscordRPC;
using XeroTest.Properties;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using IWshRuntimeLibrary;
using System.Diagnostics;

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
            tb_friendnickname.Text = Settings.Default.nickname;
            tb_friendemail.Text = Settings.Default.email;
            tb_friendpassword.Text = Settings.Default.password;
            cb_StartWithWindows.Checked = Settings.Default.windows;
            cb_HideInTray.Checked = Settings.Default.tray;
            cb_ShowLevel.Checked = Settings.Default.level;
        }

        private static void InitializeDiscord()
        {
            discord.Initialize();
            discord.SetPresence(new RichPresence()
            {
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow.AddSeconds(1)
                }
            });
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string exePath = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName).Replace("Xero Presence.exe", "");

            bt_login.Text = "Logging in...";
            var options = new ChromeOptions();

            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService(exePath, "chromedriver.exe");
            service.HideCommandPromptWindow = true;

            IWebDriver driver = new ChromeDriver(service, options);

            driver.Navigate().GoToUrl("https://xero.gg/signin");

            driver.FindElement(By.Id("uniteddb-login-identifier")).SendKeys(tb_friendemail.Text);
            driver.FindElement(By.Id("uniteddb-login-password")).SendKeys(tb_friendpassword.Text);


            Thread.Sleep(650);
            driver.FindElement(By.Id("uniteddb-login-form-submit")).Click();

            Thread.Sleep(2000);
            var cookies = driver.Manage().Cookies.AllCookies;

            foreach (var cookie in cookies)
            {
                if (cookie.Name.Contains("session_ssl"))
                    _sessionssl = cookie.Value;
            }

            driver.Quit();
            Thread.Sleep(1000);
            timer1.Start();
            bt_login.Text = "Logged in";
            tb_friendnickname.ReadOnly = true;
            tb_friendemail.ReadOnly = true;
            tb_friendpassword.ReadOnly = true;
            bt_login.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var _room = "";
            var _nickname = "";
            var _level = "";
            var _experience = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://xero.gg/player/{tb_friendnickname.Text}/social");
            System.Net.Cookie cookie = new System.Net.Cookie("xero_session_ssl", _sessionssl);

            cookie.Domain = "xero.gg";
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(cookie);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            HtmlDocument doc = new HtmlDocument();
            doc.Load(response.GetResponseStream());

            //Room
            HtmlNode node1 = doc.DocumentNode.SelectSingleNode("//*[@id=\"settings-data-container\"]/div[3]/div/div/a/div[2]/div/div[2]");
            //Nickname
            HtmlNode node2 = doc.DocumentNode.SelectSingleNode("//*[@id=\"settings-data-container\"]/div[3]/div/div/a/div[2]/div/div[1]");

            if (node1 != null)
                _room = node1.InnerText;
            else
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
            if (node2 != null)
                _nickname = node2.InnerText;

            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create($"https://xero.gg/player/{_nickname}");
            request2.CookieContainer = request.CookieContainer;

            HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
            HtmlDocument doc2 = new HtmlDocument();
            doc2.Load(response2.GetResponseStream());

            //Clan, Level and Experience
            HtmlNode node4 = doc2.DocumentNode.SelectSingleNode("/html/head/meta[5]");

            if (node4 != null)
            {
                string imgHtml = node4.OuterHtml;

                var regex = new Regex(@"\s+(?<attributeName>\S+)\s*=\s*""(?<attributeValue>[^""]*)""");
                var matches = regex.Matches(imgHtml);

                foreach (Match match in matches)
                {
                    string attributeName = match.Groups["attributeName"].Value;
                    string attributeValue = match.Groups["attributeValue"].Value;

                    if (attributeName.ToLower() == "content")
                    {
                        _level = $"{attributeValue.Split(" | ")[1]}"; //Level
                        _experience = $"{attributeValue.Split(" | ")[2]}"; //Experience
                        continue;
                    }
                }
            }

            if (_discordLoggedIn == false)
            {
                discord = new DiscordRpcClient("1092449168703901756");
                InitializeDiscord();
                _discordLoggedIn = true;
            }

            if (cb_ShowLevel.Checked)
                discord.UpdateLargeAsset($"https://xero.gg/assets/img/grade/xero/84/{_level.Replace("Level: ", "")}.png", $"{_level}");
            else
                discord.UpdateLargeAsset("https://dekirai.crygod.de/rpc/xero/logo.png", $"Xero");
            discord.UpdateDetails($"{_nickname} » {_room}");
            if (cb_ShowLevel.Checked)
                discord.UpdateState($"{_experience}");
            else
                discord.UpdateState($"{_level} | {_experience}");
            DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
            buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };

            discord.UpdateButtons(buttons);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.nickname = tb_friendnickname.Text;
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
                System.IO.File.Delete(shortcutPath);
            }

            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.nickname = tb_friendnickname.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.level = cb_ShowLevel.Checked;
            Settings.Default.Save();
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

        private void cb_ShowLevel_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.nickname = tb_friendnickname.Text;
            Settings.Default.windows = cb_StartWithWindows.Checked;
            Settings.Default.tray = cb_HideInTray.Checked;
            Settings.Default.level = cb_ShowLevel.Checked;
            Settings.Default.Save();
        }
    }
}