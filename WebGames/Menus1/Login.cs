//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;

//namespace Test_game.Menus1
//{
//    /// <summary>
//    /// Setup login/registration menus
//    /// </summary>
//    class LoginMenu
//    { }
//}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WebGames;
using static WebGames.Game1;

namespace WebGames.Menus1
{


    class Login
    {
        Game1 game;
        SpriteFont Font;

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

            if (nwKeyState.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                if (initialPress == false)
                {
                    buttonPress--;
                }
            }
            else if (nwKeyState.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Up))
            {
                buttonPress++;
            }
            else if (nwKeyState.IsKeyDown(Keys.Enter) && oldState.IsKeyUp(Keys.Enter))
            {
                //Todo: place code here to change the game state and request info from the hub.
                game.gameState = Game1.GameState.ProfilePage; //we access the gameState from the game field

                 
            }

            //The below blocks of code stops the buttonPress being oversampled by the keyboard state functions.

            if(buttonPress < 0)
            {
                buttonPress = 0;
            }
            if(buttonPress > 1)
            {
                buttonPress = 1;
            }

            if(buttonPress == 0)
            {
                game.currentPlayer = "Player 1";
                
            }
            else if(buttonPress == 1)
            {
                game.currentPlayer = "Player 2";
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
            //Add formatting and positions below.
            if (buttonPress == 0)
            {
                spriteBatch.DrawString(Font, "<-Player 1->", new Vector2(150, 150), Color.Black);
                spriteBatch.DrawString(Font, "  Player 2  ", new Vector2(150, 210), Color.White);

            }
            if (buttonPress == 1)
            {
                spriteBatch.DrawString(Font, "  Player 1  ", new Vector2(150, 150), Color.White);
                spriteBatch.DrawString(Font, "<-Player 2->  ", new Vector2(150, 210), Color.Black);

            }
        }
    }
}
