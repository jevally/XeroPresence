# Documentation

## config.json
Here is a blank config.json preset  
Copy the code and save it to a new file named config.json  
The file should be in the same directory as XeroPresence.exe
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

| Tag  | Description | Usable in? |
| ------------- | ------------- | ------------- |
| [PEN]  | Prints your amount of PEN  | Everywhere  | 
| [ZP]  | Prints your amount of ZP  | Everywhere  | 
| [Gems]  | Prints your amount of Gems  | Everywhere  | 
| [Username]  | Prints your Username  | Everywhere  | 
| [Level]  | Prints your current Level  | Everywhere  | 
| [LevelImage]  | Prints the image of your level as URL | Everywhere  | 
| [XP]  | Prints your current XP  | Everywhere  | 
| [XPRequired]  | Prints your required XP  | Everywhere  | 
| [XPPercentage]  | Prints the percentage of XP you have  | Everywhere  | 
| [TotalXP]  | Prints the total amount of XP you've gained  | Everywhere  | 
| [Channel]  | Prints the name of the channel you are in  | Everywhere  | 
| [RoomID] | Prints the ID of the room you are in | Room |
| [RoomName] | Prints the name of the room you are in | Room |
| [ScoreLimit] | Prints the required score to win | Room |
| [PlayerLimit] | Prints the max player limit of the room | Room |
| [Mode] | Prints the current game mode of the room | Room |
| [Map] | Prints the current map of the room | Room |
| [MapImage] | Prints the image of the map as URL | Room |
| [GameState] | Prints the current game state of the match | Match (All modes) |
| [GameTimeState] | Prints the current time state of the match | Match (TD & DM) |
| [ScoreAlpha] | Prints the current score of team alpha | Match (TD & DM) |
| [ScoreBeta] | Prints the current score of team beta | Match (TD & DM) |
| [Team] | Prints the current team you are playing on | Match (TD & DM) |
| [State] | Prints your current state (Alive, Dead...) | Match (All modes) | 
| [Ping] | Prints your current ping to the room | Match (All modes) |
| [TotalScore] | Prints your total score obtained in the match | Match (All modes) | 
| [Kills] | Prints your total amount of kills done in the match | Match (All modes) |
| [Goals] | Prints your amount of touch downs done in the match | Match (TD) |
| [Deaths] | Prints your amount of deaths in the match | Match (All Modes) |
| [ChaserCount] | Prints your amount of being chaser in the match | Match (Chaser) |
| [Wins] | Prints your amount of wins in the match | Match (Chaser) |
| [Survived] | Prints your amount rounds survived | Match (Chaser) |
