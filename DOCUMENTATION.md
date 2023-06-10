# Documentation

## config.json
Here is an example config.json  
Copy the code and save it to a new file named config.json  
The file should be in the same directory as XeroPresence.exe    

A blank config.json can be downloaded here:
[config.json](https://raw.githubusercontent.com/Dekirai/XeroPresence/main/config.json) (Right-Click and click "Save link as...")
```json
{
   "Lobby":{
      "OverwriteDetails":true,
      "OverwriteState":true,
      "OverwriteLargeAsset":true,
      "OverwriteSmallAsset":false,
      "Details":"[Username] ([XP] / [XPRequired] XP)",
      "State":"In Lobby ([Channel])",
      "LargeAssetURL":"https://xero.gg/assets/img/grade/xero/50/[Level].png",
      "LargeAssetText":"Level [Level]",
      "SmallAssetURL":"",
      "SmallAssetText":""
   },
   "Room":{
      "OverwriteDetails":true,
      "OverwriteState":true,
      "OverwriteLargeAsset":true,
      "OverwriteSmallAsset":false,
      "Details":"[Username] is waiting in Room #[RoomID]",
      "State":"[RoomName] - [Map]",
      "LargeAssetURL":"[MapImage]",
      "LargeAssetText":"[Map]",
      "SmallAssetURL":"",
      "SmallAssetText":""
   },
   "Touchdown":{
      "OverwriteDetails":true,
      "OverwriteState":false,
      "OverwriteLargeAsset":false,
      "OverwriteSmallAsset":false,
      "Details":"I'm in team [Team] and have a score of [TotalScore]",
      "State":"",
      "LargeAssetURL":"",
      "LargeAssetText":"",
      "SmallAssetURL":"",
      "SmallAssetText":""
   },
   "Deathmatch":{
      "OverwriteDetails":false,
      "OverwriteState":false,
      "OverwriteLargeAsset":false,
      "OverwriteSmallAsset":false,
      "Details":"",
      "State":"",
      "LargeAssetURL":"",
      "LargeAssetText":"",
      "SmallAssetURL":"",
      "SmallAssetText":""
   },
   "Chaser":{
      "OverwriteDetails":false,
      "OverwriteState":false,
      "OverwriteLargeAsset":false,
      "OverwriteSmallAsset":false,
      "Details":"",
      "State":"",
      "LargeAssetURL":"",
      "LargeAssetText":"",
      "SmallAssetURL":"",
      "SmallAssetText":""
   },
   "BattleRoyal":{
      "OverwriteDetails":false,
      "OverwriteState":true,
      "OverwriteLargeAsset":false,
      "OverwriteSmallAsset":false,
      "Details":"",
      "State":"My ping is [Ping]",
      "LargeAssetURL":"",
      "LargeAssetText":"",
      "SmallAssetURL":"",
      "SmallAssetText":""
   }
}
```
## Available Tags

| Tag | Description | Usable in? |
| ------------- | ------------- | ------------- |
| [PEN] | Prints your amount of PEN | Everywhere | 
| [ZP] | Prints your amount of ZP | Everywhere | 
| [Gems] | Prints your amount of Gems | Everywhere | 
| [Username] | Prints your Username | Everywhere | 
| [Clan] | Prints the name of your clan | Everywhere | 
| [ClanImage] | Prints the image URL of your clan | Everywhere | 
| [Level] | Prints your current level | Everywhere | 
| [LevelImage] | Prints the image URL of your level | Everywhere | 
| [XP] | Prints your current XP | Everywhere | 
| [XPRequired] | Prints your required XP | Everywhere | 
| [XPPercentage] | Prints the percentage of XP you have | Everywhere | 
| [TotalXP] | Prints the total amount of XP you've gained | Everywhere | 
| [Channel] | Prints the name of the channel you are in | Everywhere | 
| [RoomID] | Prints the ID of the room you are in | Room & Match (All modes) |
| [RoomName] | Prints the name of the room you are in | Room & Match (All modes) |
| [ScoreLimit] | Prints the required score to win | Room & Match (All modes) |
| [PlayerLimit] | Prints the max player limit of the room | Room & Match (All modes) |
| [PlayerCount] | Prints the current amount of players in the room | Room & Match (All modes) |
| [Mode] | Prints the current game mode of the room | Room & Match (All modes) |
| [Map] | Prints the current map of the room | Room & Match (All modes) |
| [MapImage] | Prints the image of the map as URL | Room & Match (All modes) |
| [Team] | Prints the current team you are playing on | Room & Match (TD & DM) |
| [Ping] | Prints your current ping to the room | Room & Match (All modes) |
| [GameState] | Prints the current game state of the match | Match (All modes) |
| [GameTimeState] | Prints the current time state of the match | Match (TD & DM) |
| [ScoreAlpha] | Prints the current score of team alpha | Match (TD & DM) |
| [ScoreBeta] | Prints the current score of team beta | Match (TD & DM) |
| [State] | Prints your current state (Alive, Dead...) | Match (All modes) | 
| [TotalScore] | Prints your total score gained in the match | Match (All modes) | 
| [Kills] | Prints your total amount of kills done in the match | Match (All modes) |
| [Goals] | Prints your amount of touch downs done in the match | Match (TD) |
| [Deaths] | Prints your amount of deaths in the match | Match (All Modes) |
| [ChaserCount] | Prints your amount of being chaser in the match | Match (Chaser) |
| [Wins] | Prints your amount of wins in the match | Match (Chaser) |
| [Survived] | Prints your amount rounds survived | Match (Chaser) |
