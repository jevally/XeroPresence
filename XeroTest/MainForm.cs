using HtmlAgilityPack;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using DiscordRPC;
using XeroTest.Properties;

namespace XeroTest
{
    public partial class MainForm : Form
    {
        private static DiscordRpcClient discord;
        public bool _discordLoggedIn = false;
        public MainForm()
        {
            InitializeComponent();
            tb_friendnickname.Text = Settings.Default.nickname;
            tb_sessionid.Text = Settings.Default.sessionid;
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
            timer1.Start();
            tb_friendnickname.ReadOnly = true;
            tb_sessionid.ReadOnly = true;
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

            Cookie cookie = new Cookie("xero_session_ssl", tb_sessionid.Text);
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
            timer1.Interval = 30000;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.sessionid = tb_sessionid.Text;
            Settings.Default.nickname = tb_friendnickname.Text;
            Settings.Default.Save();
        }
    }
}