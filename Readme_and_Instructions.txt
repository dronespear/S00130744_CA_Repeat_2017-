Install and run instructions:

The game has been created and compiled in Visual studio community 2015, and requires Monogame 3.6.0, and Signalr 2.2.1.

The game has been built for a screen resolution of 1550 x 825.

Download and unzip the game, then navigate to the folder and click on the main solution file "WebGames.sln". Once this opens right click on the solution and rebuild it allowing NuGet to install any missing packages. If the solution throws a error due to a missing .exe file, then build each component first (e.g. WebGames, then GameHub and finally rebuild the main solution).

The game should then run on clicking start. To open multiple clients navigate to the bin folder of "WebGames" and click on the WebGames.exe file.



Game description and features:

The game operates by passing messages and information between monogame/C# clients and a central locally hosted signalr Hub.

The game has a basic menu system and a choice of two players (each with pre-seeded profiles sent from the hub on character selection).

Approximately 10 seconds into the game a welcome message is displayed to all connected clients with an instruction for the game objectives.

The game is a collecting base game, with three randomly generated levels (though more levels could easily be added by changing one parameter). The objective of the game is to collect cores. Once all cores are collected the level ends. 

Players have 3 lives. Player health goes down on contact with enemy rabbits, the strength of rabbit is randomly assigned (blue tint rabbit stronger than white tint). Once health reaches 0, health resets to 100 and the player looses a life. The game ends when all collectables on all three levels are collected or the players health and lives are gone,both leads to game over.

Players gain experience by each collectable collected, the values add up and once they pass different levels the character is given a new rank (four levels: beginner, rookie, intermediate, veteran).
A special item is release at a random time point in the game (between 17 and 25 seconds) and this gives the player a large amount of XP.

The game can be played in single or multiplayer mode**. In single player mode the game starts directly. In multiplayer mode the game waits for a second player to join the session before starting automatically. It also notifys the opposite players who has joined the session. Note single player games will not trigger the multiplayer session even if a player is waiting and will not recieve their timed messages.

In the game character movement is controlled by the arrow keys, and the weapon is aimed using the mouse and fired with the left mouse button. 

When the game ends a pre-seeded scoreboard window is produced and the game can be reset to the main menu by following on-screen directions.



Todo/In progress:

Migrate code from localhost to Azure and add webAPI authetication to allow live data to be stored and then recalled for the scoreboard and player profiles.

**Improve multiplayer mode, the games currently are synced to start at the same time and recieve the same messages. But game play and collectables is not snyced (e.g. item collected in player 1 doesn't affect player 2 game). End of game mechanics need to be updated for the multiplayer session (e.g. if a player leaves the game or leaves the server).



Known issues:

On first run server is slow to load the player profiles (loads much quicker in second client). 

Some messages on timer are delayed and take to recieve(slightly longer potentially due to server lag?) Also when the game is reset/level respawned some timer elasped event issues have been noticed and one example is the special collectable is effected by it. These are being investigated to find potential solution.

Different songs were selected for menu screens, playing screen, and end of game screen. But a bug in the monogame system only allowed one track to be played consistently. If multiple tracks included the music would just stop and not play the next track.

Collectables overlap with enemies and sometimes with the player causing a loss of life on game start. This is because the position of the items is random. Some better controls on item location are needed to fix this.

Axe colision with enemies not working properly (but character colision  with collectable and character colision with enemy is working fine).
