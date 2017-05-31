using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Net;
using System.Web;
using Microsoft.AspNet.SignalR.Client;
using WebGames.Menus1;
using System.Timers;
using WebGames;
using static WebGames.Game1;


namespace WebGames.Menus1
{


    class GameOver
    {
        Game1 game;
        SpriteFont Font;
        
        //set up button press variable.
        int buttonPress;

        public KeyboardState oldState;
        bool initialPress;
        private Song backingTrack1;



        //The below line gets its values from an initialize call in the loadcontent() section of the main game.
        public void Initialize(SpriteFont menuText, Game1 game, Song backingtrack)
        {
            Font = menuText;
            backingTrack1 = backingtrack;

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
                //Reset initial game parameters.
                game.ResetGame();
                
                //May also need to add a function here to clear logins/player info.
                game.gameState = Game1.GameState.Login;
                
            }

            //This block of code assigns the current keyboard state to oldState and resets the initial conditions.
            oldState = nwKeyState;
            if (initialPress)
            {
                initialPress = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            // The below block of code draws the background image starting at position 50, 50
            var posTop = new Vector2(575, 80);
            var posTop1 = new Vector2(575, 180);
            var posTop2 = new Vector2(400, 350);
            var posBot = new Vector2(500, 680);
            string Scores = @"  Position  Name    Health  Lives   Date
        1     Player 1   100      3     20/10/16
        2     Player 2     63      2     20/10/16
        3     Player 1     59      1     13/10/15
        4     Player 2     63      1     13/10/15";
            
            //Add code to draw game over in big letters at the centre top of screen
            spriteBatch.DrawString(Font, "Game Over", posTop, Color.Black);
            spriteBatch.DrawString(Font, "Scoreboard", posTop1, Color.White);
            //Add code to draw scoreboard
            spriteBatch.DrawString(Font, Scores, posTop2, Color.White);

            //Add code to draw button that returns to main menu
            spriteBatch.DrawString(Font, "Press Enter For Main Menu", posBot, Color.Black);


        }
    }
}
