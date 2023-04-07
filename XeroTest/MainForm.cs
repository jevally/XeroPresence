using HtmlAgilityPack;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using DiscordRPC;
using XeroTest.Properties;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

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
            var options = new ChromeOptions();

            options.AddArgument("--headless");
            var service = ChromeDriverService.CreateDefaultService();
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
            var _experience_raw = "";
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
            //Level Icon URL
            HtmlNode node3 = doc.DocumentNode.SelectSingleNode("//*[@id=\"settings-data-container\"]/div[3]");

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
            if (node3 != null)
            {
                string imgHtml = node3.OuterHtml;

                var regex = new Regex(@"\s+(?<attributeName>\S+)\s*=\s*""(?<attributeValue>[^""]*)""");
                var matches = regex.Matches(imgHtml);

                foreach (Match match in matches)
                {
                    string attributeName = match.Groups["attributeName"].Value;
                    string attributeValue = match.Groups["attributeValue"].Value;

                    if (attributeName.ToLower() == "src")
                        _level = ($"{attributeValue.Replace("/assets/img/grade/xero/84/", "").Replace(".png", "")}");
                }
            }

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
                        _experience_raw = $"{attributeValue.Split(" | ")[2]}"; //Experience
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
                discord.UpdateLargeAsset($"https://xero.gg/assets/img/grade/xero/84/{_level}.png", $"Level {_level}");
            else
                discord.UpdateLargeAsset("https://dekirai.crygod.de/rpc/xero/logo.png", $"Xero");
            _experience = Regex.Replace(_experience_raw, @"\s*\([^)]*\)", "");
            if (cb_ShowLevel.Checked)
                discord.UpdateDetails($"{_nickname} | {_experience}");
            else
                discord.UpdateDetails($"{_nickname} | Lv. {_level} | {_experience}");
            discord.UpdateState(_room);
            DiscordRPC.Button[] buttons = new DiscordRPC.Button[1];
            buttons[0] = new DiscordRPC.Button { Label = "View Profile", Url = $"https://xero.gg/player/{_nickname}" };

            discord.UpdateButtons(buttons);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.email = tb_friendemail.Text;
            Settings.Default.password = tb_friendpassword.Text;
            Settings.Default.nickname = tb_friendnickname.Text;
            Settings.Default.Save();
        }
    }
}