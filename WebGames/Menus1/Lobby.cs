using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace WebGames.Menus1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Lobby
    {

        Game1 game;
        SpriteFont Font;

        
        public string lobbyText = "";
        public Timer t3;

        //set up button press variable.
        int buttonPress;

        public KeyboardState oldState;
        bool initialPress;

        //The below line gets its values from an initialize call in the loadcontent() section of the main game.
        public void Initialize(SpriteFont menuText, Game1 game)
        {
            Font = menuText;


            this.game = game;
            buttonPress = 0;


            oldState = Keyboard.GetState();
            initialPress = true;
        }

        public void Update(GameTime gameTime)
        {
            handleInput(gameTime);
        }

        private void handleInput(GameTime gameTime)
        {
            if (initialPress)
            {
                oldState = Keyboard.GetState();
            }

            var nwKeyState = Keyboard.GetState();

            if (nwKeyState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                //REPLACE THE BELOW WITH UPDATE FUNCTION THAT AUTOMATICALLY STARTS THE GAME ONCE TWO PLAYERS ARE DETECTED.
                //game.gameState = Game1.GameState.Playing;


            }

            //This block of code assigns the current keyboard state to oldState and resets the initial conditions.
            oldState = nwKeyState;
            if (initialPress)
            {
                initialPress = false;
            }
        }


        public void T3_Elapsed(object sender, ElapsedEventArgs e)
        {
            game.lobbyMessage = "";
            game.gameState = Game1.GameState.Playing;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (game.numberOfPlayers <= 1)
            {
                lobbyText = "Waiting for Other Players to Join";
            }
            else if (game.numberOfPlayers >= 2)
            {
                lobbyText = "Game Starting in: 10 seconds"; //+ create custom timer so we can display countdown to game start.
                t3 = new Timer(10000);
                t3.Elapsed += T3_Elapsed;
                t3.Start();
                t3.AutoReset = false;
                
            }
           
            // The game.gameState == GameState.Playing would then be executed in the elapsed function.

            var posTop = new Vector2(475, 280);
            var posBot = new Vector2(500, 680);
        

            //Add code to draw game over in big letters at the centre top of screen
            spriteBatch.DrawString(Font, lobbyText, new Vector2(posTop.X, posTop.Y + 100), Color.White);
            spriteBatch.DrawString(Font, game.lobbyMessage, posTop, Color.White);

            //Add code to do a game start countdown

            //spriteBatch.DrawString(Font, "Player Joined Game Starting", posTop, Color.White);


        }
    }
}

