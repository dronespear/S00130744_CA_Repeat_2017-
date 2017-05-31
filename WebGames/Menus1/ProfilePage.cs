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


    class ProfilePage
    {
        Game1 game;
        SpriteFont Font;



        //set up button press variable.
        int buttonPress;

        public KeyboardState oldState;
        bool initialPress;
        private string playerInfo;



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
                if (buttonPress == 0)
                {
                    game.gameState = Game1.GameState.Playing;
                }
                if (buttonPress == 1)
                {
                    game.gameState = Game1.GameState.Lobby;
                }
                //we access the gameState from the game field

                //Login and register should have different functionality one leads to a page that request username/password,
                //the other leads to a sign up page where you enter name/username/password. 
            }

            //The below blocks of code stops the buttonPress being oversampled by the keyboard state functions.

            if (buttonPress < 0)
            {
                buttonPress = 0;
            }
            if (buttonPress > 1)
            {
                buttonPress = 1;
            }

            //This block of code assigns the current keyboard state to oldState and resets the initial conditions.
            oldState = nwKeyState;
            if (initialPress)
            {
                initialPress = false;
            }
            playerInfo = game.playerProfile;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //Add formatting and positions below.
            var posTop1 = new Vector2(150, 80);
            var posTop2 = new Vector2(150, 200);
            var posBot1 = new Vector2(500, 680);
            var posBot2 = new Vector2(500, 740);

            spriteBatch.DrawString(Font, "Player Profile", posTop1, Color.White);
            spriteBatch.DrawString(Font, playerInfo, posTop2, Color.White);


            if (buttonPress == 0)
            {
                spriteBatch.DrawString(Font, "<-New Game->", posBot1, Color.Black);
                spriteBatch.DrawString(Font, "  New Multi-Player Game  ", posBot2, Color.White);
            }

            if (buttonPress == 1)
            {
                spriteBatch.DrawString(Font, "  New Game  ", posBot1, Color.White);
                spriteBatch.DrawString(Font, "<-New Multi-Player Game->", posBot2, Color.Black);
            }


        }
    }
}
