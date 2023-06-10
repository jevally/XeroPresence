using System.Text.RegularExpressions;

namespace XeroPresence
{
    public static class ReplaceTags
    {
        public static string Lobby(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueLobby(match.Groups[1].Value));
        }

        public static string Room(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|PlayerCount|Mode|Map|MapImage|Ping|Team)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueRoom(match.Groups[1].Value));
        }

        public static string TD(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|PlayerCount|Mode|Map|MapImage|GameState|GameTimeState|ScoreAlpha|ScoreBeta|Team|State|Ping|TotalScore|Kills|Goals|Deaths)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueTD(match.Groups[1].Value));
        }

        public static string DM(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|PlayerCount|Mode|Map|MapImage|GameState|GameTimeState|ScoreAlpha|ScoreBeta|Team|State|Ping|TotalScore|Kills|Deaths)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueDM(match.Groups[1].Value));
        }

        public static string BR(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|PlayerCount|Mode|Map|MapImage|GameState|Team|State|Ping|TotalScore|Kills|Deaths)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueBR(match.Groups[1].Value));
        }

        public static string CH(string input)
        {
            string pattern = @"\[(PEN|ZP|Gems|Username|Clan|ClanImage|Level|LevelImage|XP|XPRequired|XPPercentage|TotalXP|Channel|RoomID|RoomName|ScoreLimit|PlayerLimit|PlayerCount|Mode|Map|MapImage|GameState|Team|State|Ping|TotalScore|Kills|Deaths|ChaserCount|Wins|Survived)\]";
            Regex regex = new Regex(pattern);
            return regex.Replace(input, match => GetReplacementValueCH(match.Groups[1].Value));
        }

        public static string GetReplacementValueLobby(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                default:
                    return string.Empty;
            }
        }

        public static string GetReplacementValueRoom(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                case "RoomID":
                    return MainForm._id.ToString();
                case "RoomName":
                    return MainForm._name;
                case "ScoreLimit":
                    return MainForm._scorelimit.ToString();
                case "PlayerLimit":
                    return MainForm._playerlimit.ToString();
                case "PlayerCount":
                    return MainForm._playercount.ToString();
                case "Mode":
                    return MainForm._mode;
                case "Map":
                    return MainForm._map;
                case "MapImage":
                    return MainForm._mapimage;
                default:
                    return string.Empty;
            }
        }

        public static string GetReplacementValueTD(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                case "RoomID":
                    return MainForm._id.ToString();
                case "RoomName":
                    return MainForm._name;
                case "ScoreLimit":
                    return MainForm._scorelimit.ToString();
                case "PlayerLimit":
                    return MainForm._playerlimit.ToString();
                case "PlayerCount":
                    return MainForm._playercount.ToString();
                case "Mode":
                    return MainForm._mode;
                case "Map":
                    return MainForm._map;
                case "MapImage":
                    return MainForm._mapimage;
                case "GameState":
                    return MainForm._gameState;
                case "GameTimeState":
                    return MainForm._gameTimeState;
                case "ScoreAlpha":
                    return MainForm._scoreAlpha.ToString();
                case "ScoreBeta":
                    return MainForm._scoreBeta.ToString();
                case "Team":
                    return MainForm._team;
                case "State":
                    return MainForm._status;
                case "Ping":
                    return MainForm._ping.ToString();
                case "TotalScore":
                    return MainForm._totalScore.ToString();
                case "Kills":
                    return MainForm._kills.ToString();
                case "Goals":
                    return MainForm._goals.ToString();
                case "Deaths":
                    return MainForm._deaths.ToString();
                default:
                    return string.Empty;
            }
        }

        public static string GetReplacementValueDM(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                case "RoomID":
                    return MainForm._id.ToString();
                case "RoomName":
                    return MainForm._name;
                case "ScoreLimit":
                    return MainForm._scorelimit.ToString();
                case "PlayerLimit":
                    return MainForm._playerlimit.ToString();
                case "PlayerCount":
                    return MainForm._playercount.ToString();
                case "Mode":
                    return MainForm._mode;
                case "Map":
                    return MainForm._map;
                case "MapImage":
                    return MainForm._mapimage;
                case "GameState":
                    return MainForm._gameState;
                case "GameTimeState":
                    return MainForm._gameTimeState;
                case "ScoreAlpha":
                    return MainForm._scoreAlpha.ToString();
                case "ScoreBeta":
                    return MainForm._scoreBeta.ToString();
                case "Team":
                    return MainForm._team;
                case "State":
                    return MainForm._status;
                case "Ping":
                    return MainForm._ping.ToString();
                case "TotalScore":
                    return MainForm._totalScore.ToString();
                case "Kills":
                    return MainForm._kills.ToString();
                case "Deaths":
                    return MainForm._deaths.ToString();
                default:
                    return string.Empty;
            }
        }

        public static string GetReplacementValueBR(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                case "RoomID":
                    return MainForm._id.ToString();
                case "RoomName":
                    return MainForm._name;
                case "ScoreLimit":
                    return MainForm._scorelimit.ToString();
                case "PlayerLimit":
                    return MainForm._playerlimit.ToString();
                case "PlayerCount":
                    return MainForm._playercount.ToString();
                case "Mode":
                    return MainForm._mode;
                case "Map":
                    return MainForm._map;
                case "MapImage":
                    return MainForm._mapimage;
                case "GameState":
                    return MainForm._gameState;
                case "State":
                    return MainForm._status;
                case "Ping":
                    return MainForm._ping.ToString();
                case "TotalScore":
                    return MainForm._totalScore.ToString();
                case "Kills":
                    return MainForm._kills.ToString();
                case "Deaths":
                    return MainForm._deaths.ToString();
                default:
                    return string.Empty;
            }
        }

        public static string GetReplacementValueCH(string tag)
        {
            switch (tag)
            {
                case "PEN":
                    return MainForm._pen.ToString("#,##0");
                case "ZP":
                    return MainForm._zp.ToString("#,##0");
                case "Gems":
                    return MainForm._gems.ToString("#,##0");
                case "Username":
                    return MainForm._nickname;
                case "Clan":
                    return MainForm._clan;
                case "ClanImage":
                    return MainForm._clanimage;
                case "Level":
                    return MainForm._level.ToString();
                case "LevelImage":
                    return MainForm._levelimage;
                case "XP":
                    return MainForm._xp.ToString("#,##0");
                case "XPRequired":
                    return MainForm._xprequired.ToString("#,##0");
                case "XPPercentage":
                    return MainForm._xppercentage.ToString();
                case "TotalXP":
                    return MainForm._xptotal.ToString("#,##0");
                case "Channel":
                    return MainForm._channel;
                case "RoomID":
                    return MainForm._id.ToString();
                case "RoomName":
                    return MainForm._name;
                case "ScoreLimit":
                    return MainForm._scorelimit.ToString();
                case "PlayerLimit":
                    return MainForm._playerlimit.ToString();
                case "PlayerCount":
                    return MainForm._playercount.ToString();
                case "Mode":
                    return MainForm._mode;
                case "Map":
                    return MainForm._map;
                case "MapImage":
                    return MainForm._mapimage;
                case "GameState":
                    return MainForm._gameState;
                case "State":
                    return MainForm._status;
                case "Ping":
                    return MainForm._ping.ToString();
                case "TotalScore":
                    return MainForm._totalScore.ToString();
                case "Kills":
                    return MainForm._kills.ToString();
                case "Deaths":
                    return MainForm._deaths.ToString();
                case "ChaserCount":
                    return MainForm._chaserCount.ToString();
                case "Wins":
                    return MainForm._wins.ToString();
                case "Survived":
                    return MainForm._survived.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}
