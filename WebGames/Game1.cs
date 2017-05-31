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

namespace WebGames
    {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player _player;
        weapon_bullet _bullet;
        Enemy[] _enemyrabbitscollection;
        Collectable[] _corecollection;

        weapon _axe;   //a axe to attach to our player

        public GameState gameState;

        SpriteFont menuText;
        public int lives = 3;
        public bool gameOver = false;

        //To get the below working we had to add "using Microsoft.AspNet.SignalR.Client;" to the above code and add a ref to it using the Nu get package manager.

        private IHubProxy proxy;
        HubConnection connection;

        public enum GameState
        {
            Login,
            ProfilePage,
            Lobby,
            Playing,
            GameOver
        }

        SoundEffect gunfire;
        Song backingTrack;
        Song menu;
        private Song _Success;
        private SoundEffect _hurt;
        private SoundEffect _collect;

        Texture2D _core;
        SimpleSprite _core2;
        Texture2D __background;
        Texture2D _menu1;
        Texture2D _menu2;
        Texture2D _menu3;

        public int _playerHealth = 100;
        private SpriteFont _healthFont;
        //  SimpleSprite _background;

        Vector2 bounds;


        private Vector2 _playerStartPosition;

        public string welcomeText = "";
        public static Timer t;
        public static Timer t2;
        public int count = 0;
        Menus1.Login nwLogin;
        Menus1.Lobby nwLobby;
        Menus1.ProfilePage nwProfilePage;
        Menus1.GameOver nwGameOver;
        public string playerProfile = "";
        public string currentPlayer = "";
        public int collected = 0;
        public int level = 0;
        public int numberOfPlayers;
        public string currentId;

        public int playerCount = 0;
        public string lobbyMessage = "";
        public int xp = 0;
        public string xpLevel = "";

        public Collectable _specialItem;
        public Timer t4;
        public Timer t5;
        public int randTime;
        public string specialNotification = "";
        public int newCount;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1550;
            graphics.PreferredBackBufferHeight = 825;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;  //make the mouse visable on the screen
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Sets up the connection between the client and the hub.
            connection = new HubConnection("http://localhost:63810");
            proxy = connection.CreateHubProxy("MyHub");

            //The below line gets the unique context connection Id of the current client.
            currentId = connection.ConnectionId;

            //The below line should limit the number of connections but it seems to still work need to investigate later.
            ServicePointManager.DefaultConnectionLimit = 15;

            //The below line of code starts the connection between the client and server.
            connection.Start(); //do we need the .Wait() command here?;
            
            //Menus

            nwLogin = new Menus1.Login();
            nwLobby = new Menus1.Lobby();
            nwProfilePage = new Menus1.ProfilePage();
            nwGameOver = new Menus1.GameOver();


            //Messages expected from the hub.
            Action<string> msgText = msgHandler;// This passes the message to a handler which converts it to a string in the assigned to var welcomeText.
            proxy.On("welcomeMessage", msgText);
            Action<string> lobbyText = lobbyHandler;
            proxy.On("lobbyMessage", lobbyText);
            Action<string> profileText = profileHandler;
            proxy.On("sendPlayerProfile", profileText);
            Action<string> otherId = joinedHandler;
            proxy.On("sendOtherId", otherId);

            if (connection.State == ConnectionState.Connected)
            {
                
                gameState = GameState.Login;
            }


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            __background = Content.Load<Texture2D>(@"images/Background");
            _menu1 = Content.Load<Texture2D>(@"images/Login");
            _menu2 = Content.Load<Texture2D>(@"images/menu2");
            _menu3 = Content.Load<Texture2D>(@"images/menu3");
            _healthFont = Content.Load<SpriteFont>("health");
            menuText = Content.Load<SpriteFont>("menuText");

            SoundEffect[] _PlayerSounds = new SoundEffect[5];
            for (int i = 0; i < _PlayerSounds.Length; i++)
                _PlayerSounds[i] =
                    Content.Load<SoundEffect>(@"sounds/PlayerDirection/" + i.ToString());

            menu = Content.Load<Song>(@"sounds/menus");
            backingTrack = Content.Load<Song>(@"Sounds\Acruta Lao Dnor");
            MediaPlayer.Play(backingTrack);
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Volume = 0.5f;

            _Success = Content.Load<Song>(@"sounds/success");
            _hurt = Content.Load<SoundEffect>(@"sounds/hurt");
            _collect = Content.Load<SoundEffect>(@"sounds/hurt");


            Texture2D[] textures = new Texture2D[5];
            textures[0] = Content.Load<Texture2D>(@"images/l_strip");
            textures[1] = Content.Load<Texture2D>(@"images/r_strip");
            textures[2] = Content.Load<Texture2D>(@"images/u_strip");
            textures[3] = Content.Load<Texture2D>(@"images/d_strip");
            textures[4] = Content.Load<Texture2D>(@"images/s_strip");

            ResetRabbits(Content.Load<Texture2D>(@"images/Boxing_Rabbit"));
            ResetCores(Content.Load<Texture2D>(@"images/core_collectable"));
            specialItem(Content.Load<Texture2D>(@"images/special_core"));

            Texture2D weapontexture = Content.Load<Texture2D>(@"images/pickaxe");
            Texture2D bulletTexture = Content.Load<Texture2D>(@"images/pickaxe");    //load our bullet texture

            //set up our bullets
            List<weapon_bullet> bullets = new List<weapon_bullet>();
            for (int i = 0; i < 50; i++)
            {
                gunfire = Content.Load<SoundEffect>(@"Sounds/weapon/fire_bullet");
                bullets.Add(new weapon_bullet(bulletTexture, Vector2.Zero, 40));
            }

            /*
            SoundEffect[] _weaponSounds = new SoundEffect[1];
            for (int i = 0; i < _weaponSounds.Length; i++)
                _PlayerSounds[i] =
                    Content.Load<SoundEffect>(@"sounds/weapon/" + i.ToString());
            */

            //instantiate the vehicle attachment
            _axe = new weapon(weapontexture,
                new Vector2(30, 40),
                new Rectangle(0, 0, 30, 30),
                bullets);

            //instantiate the vehicleBody
            _player = new Player(textures, _PlayerSounds,
                                             Vector2.Zero, 8, 0, 5.0f, _axe);


            // Move the player to the Centre
            _player.Move(new Vector2(GraphicsDevice.Viewport.Width / 2 - _player.SpriteWidth / 2,
                GraphicsDevice.Viewport.Height / 2 - _player.SpriteHeight / 2));

            _playerStartPosition = _player.position;

            //the below line initialises the pages and passes it the field font, and class this.
            nwLogin.Initialize(menuText, this);
            nwLobby.Initialize(menuText, this);
            nwProfilePage.Initialize(menuText, this);
            nwGameOver.Initialize(menuText, this, backingTrack);
            // TODO: use this.Content to load your game content here

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            _player.update(gameTime);

            foreach (Enemy rb in _enemyrabbitscollection)
            {

                if (rb.Visible && _player.BoundingRect.Intersects(rb.BoundingRect))
                {
                    _playerHealth -= rb.Value;
                    rb.Visible = false;
                    _hurt.Play();
                    //_player.position = _playerStartPosition; //uncomment this if want to reset player to start position after each hit.
                }
            }

            //The below codeblock is not working right now, but is supposed to remove enemies when hit with the axe.
            foreach (Enemy rb in _enemyrabbitscollection)
            {
                    if (_bullet != null)
                {
                    if (rb.Visible && _bullet.BoundingAxe.Intersects(rb.BoundingRect))
                    {
                        rb.Visible = false;
                    }
                }
            }
           

            foreach (Collectable ce in _corecollection)
                if (ce.Visible && _player.BoundingRect.Intersects(ce.BoundingRect))
                {
                    xp += ce.Value;
                    ce.Visible = false;
                    collected++;
                    _collect.Play();
                    // _player.position = _playerStartPosition;
                }

            
            if (_specialItem.Visible && _player.BoundingRect.Intersects(_specialItem.BoundingRect))
                {
                    xp += _specialItem.Value;
                    _specialItem.Visible = false;
                    _collect.Play();
                    // _player.position = _playerStartPosition;
                }

            //Mechanics to define end of game.

            //Respawns items to create a new level and only initiates end of game after level 3.
            if (collected == _corecollection.Count() && level <2)
            {
                ResetRabbits(Content.Load<Texture2D>(@"images/Boxing_Rabbit"));
                ResetCores(Content.Load<Texture2D>(@"images/core_collectable"));
                gameOver = false;
                collected = 0;
                welcomeText = "";
                t.Stop();
                //t2.Stop();
                t.Interval = 10000;
                t.Elapsed += T_Elapsed;
                //t.Start();
                count = 0;
                _player.position = _playerStartPosition;
                MediaPlayer.Play(backingTrack);
                
                level++;
            }
           if (collected == _corecollection.Count() && level == 2)
           {
                gameOver = true;
            }
            
           

           if (_playerHealth <= 0)
            {
                //Could add lives
                if (lives > 0)
                {
                    lives--;
                    _playerHealth = 100;
                    _player.position = _playerStartPosition;
                    //ResetGame();

                }
                else if (lives  <= 0)
                {
                    gameOver = true;  

                }
                 //ResetGame();
            }

            if (gameOver)
            {
                gameState = GameState.GameOver;
                MediaPlayer.Play(_Success);
                
                //ADD REQUEST FOR SCOREBOARD HERE.
            }


            
            
            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
            int adjustPos = 10;
            int adjustPos2 = 30;

           //Adjustments to player position to keep player movements within the screen boundary.
            if (_player.position.X < 0)
                _player.position.X += (_player.SpriteWidth + adjustPos);
            if (_player.position.Y < 0)
                _player.position.Y += (_player.SpriteHeight + adjustPos);
            if (_player.position.X > screenWidth)
                _player.position.X = _player.position.X - (_player.position.X - screenWidth) - _player.SpriteWidth - adjustPos2;
            if (_player.position.Y > screenHeight)
                _player.position.Y = _player.position.Y - (_player.position.Y - screenHeight) - _player.SpriteHeight - adjustPos2;

            //Player level up logic.
            if (xp <= 10)
            {
                xpLevel = "Beginner";
            }
            else if (xp > 10 && xp <= 25)
            {
                xpLevel = "Rookie";
            }
            else if (xp > 25 && xp <= 45)
            {
                xpLevel = "Intermediate";
            }
            else if (xp > 45 )
            {
                xpLevel = "Veteran";
            }
            

            //Game state specific requests.
            if (gameState == GameState.Login)
            {
                

                nwLogin.Update(gameTime);

            }

            if (gameState == GameState.ProfilePage)
            {
                if (connection.State == ConnectionState.Connected)
                {
                    if (playerProfile.Length <= 0)
                    {
                        if (currentPlayer == "Player 1")
                        {
                            proxy.Invoke("PlayerOne");
                        }
                        if (currentPlayer == "Player 2")
                        {
                            proxy.Invoke("PlayerTwo");
                        }
                    }
                }
                nwProfilePage.Update(gameTime);
                
            }

            if (gameState == GameState.Lobby)
            {
                if (connection.State == ConnectionState.Connected)
                {
                    proxy.Invoke("PlayerJoin", currentPlayer);

                }
                    nwLobby.Update(gameTime);
            }

            if (gameState == GameState.Playing)
            {
                //MediaPlayer.Stop();
                //MediaPlayer.Play(backingTrack);
                //MediaPlayer.IsRepeating = true;
                //MediaPlayer.Volume = 0.5f;
                //This code sets up a new timer set for 10s.
                //to do this we needed to add "using system.timers" and add a public static Timer t.

                t = new Timer(10000);
                t.Elapsed += T_Elapsed;
                t.Start();
                t.AutoReset = false;

               
            }

            if (gameState == GameState.GameOver)
            {
                
                nwGameOver.Update(gameTime);
                
                //Add conditions here.

            }


            base.Update(gameTime);
        }
        public void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Requests sent to hub.


            if (welcomeText.Length <= 0 && count <= 0 && gameState == GameState.Playing) //The count control on the if statement is to remove the influence of the 60 samples per second in the gameTime update function.
            {
                proxy.Invoke("WelcomeMessage", level);
                count++;

                //would still need a way to reset to empty after 5 - 10 seconds or on player movement.

                //The above invoke, as well as the action/proxy requests in initialize and the msgHandler could all be replaced
                //client side by just having the variable welcomeText assigned the below value after the timer has elapsed.
                //This method is more reliable too as it is less likely to be influenced between any client - hub delays during message transmission.
                //welcomeText = "Welcome to level 1!!!";
                t2 = new Timer(5000);
                t2.Elapsed += T2_Elapsed;
                t2.Start();
                t2.AutoReset = false;
            }
            
        }

        public void T2_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Reset welcomeText to empty to remove announcement.
            welcomeText = ""; 

        }

        private void msgHandler(string msgText)
        {
            welcomeText = msgText;
        }

        private void lobbyHandler(string lobbyText)
        {
            lobbyMessage = lobbyText;
        }

        private void joinedHandler(string otherId)
        {
            //The below code creates an empty list called joined players. 
            //It then adds the currentId to this list and any Id broadcast by other users in the lobby.        
            List<string> joinedPlayers = new List<string>();
            joinedPlayers.Add(currentId);
            joinedPlayers.Add(otherId);
            playerCount = joinedPlayers.Count;
            numberOfPlayers = playerCount;
        }

        private void profileHandler(string profileText)
        {
            playerProfile = profileText;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            spriteBatch.Begin();
            if (gameState == GameState.Login)
            {
                spriteBatch.Draw(_menu1, new Vector2(0, 0), Color.White);
                nwLogin.Draw(spriteBatch);
                
            }
            //Connection test if statement, if hub is connected then image will show, if not then plain background will show.
            if (gameState == GameState.ProfilePage)
            {

                //ADD REFS HERE ONCE PROFILE IS FILLED IN

                spriteBatch.Draw(_menu2, new Vector2(0, 0), Color.White);
                nwProfilePage.Draw(spriteBatch);
                
            }

            if (gameState == GameState.Lobby)
            {
                //ADD REFS HERE ONCE PROFILE IS FILLED IN
                nwLobby.Draw(spriteBatch);
            }


            if (gameState == GameState.Playing)
            {
                              
                //_background.draw(spriteBatch);
                spriteBatch.Draw(__background, new Vector2(0, 0), Color.White);
                //spriteBatch.Draw(_core, new Vector2(100, 100), Color.White);
                // _core2.draw(spriteBatch);
                spriteBatch.DrawString(_healthFont, "Health : " + _playerHealth.ToString(), new Vector2(80, 20), Color.White);  //draws score
                spriteBatch.DrawString(_healthFont, "Lives : " + lives.ToString(), new Vector2(300, 20), Color.White);  //draws lives remaining
                spriteBatch.DrawString(_healthFont, "Xp : " + xp.ToString() + " Rank: "+ xpLevel, new Vector2(455, 20), Color.White);  //draws xp and level
                foreach (Enemy item in _enemyrabbitscollection)
                    item.draw(spriteBatch);

                foreach (Collectable core_fragment in _corecollection)
                    core_fragment.draw(spriteBatch);


                //Timer to trigger the release of the special item.
                t4 = new Timer(randTimeInterval());
                t4.Elapsed += T4_Elapsed;
                t4.Start();
                t4.AutoReset = false;
                
                
                //Draw special item.


                if (newCount >= 1 && gameState == GameState.Playing)
                {
                    _specialItem.draw(spriteBatch);
                    spriteBatch.DrawString(menuText, specialNotification, new Vector2(_playerStartPosition.X - 150, _playerStartPosition.Y - 150), Color.Black);

                }


                _player.Draw(spriteBatch, _healthFont);
                
                //if (connection.State == ConnectionState.Connected)
                //{
                //make the below image position relative to window width and height.
               ;
                spriteBatch.DrawString(menuText, welcomeText, new Vector2(_playerStartPosition.X - 150, _playerStartPosition.Y), Color.Black);
            }
            if(gameState == GameState.GameOver)
            {
                spriteBatch.Draw(_menu3, new Vector2(0, 0), Color.White);
                nwGameOver.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public int randTimeInterval()
        {
            //Set up the random time for release of the special item.
            Random r = new Random();
            randTime = r.Next(17000, 25000);
            return randTime; 
        }


        public void T4_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (specialNotification.Length == 0 && newCount < 1 && gameState == GameState.Playing)
            {
                //draw special item notification.
                specialNotification = "Special Item has appeared";
                t5 = new Timer(5000);
                t5.Elapsed += T5_Elapsed;
                t5.Start();
                t5.AutoReset = false;
                newCount++;
            }
        }
        public void T5_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Reset welcomeText to empty to remove announcement.
            
            specialNotification = "";
        }


        public Vector2 RandomVector(Rectangle bounds)
        {
            Random r = new Random();
            return new Vector2(r.Next(bounds.Left, bounds.Right),
                r.Next(bounds.Top, bounds.Bottom));
        }

        public void ResetGame()
        {
            ResetRabbits(Content.Load<Texture2D>(@"images/Boxing_Rabbit"));
            ResetCores(Content.Load<Texture2D>(@"images/core_collectable"));
            gameOver = false;
            level = 0;
            lives = 3;
            collected = 0;
            welcomeText = "";
            t.Stop();
            t.Interval = 10000;
            t.Elapsed += T_Elapsed;
            count = 0;
            xp = 0;
            _playerHealth = 100;
            _player.position = _playerStartPosition;
            MediaPlayer.Play(backingTrack);
            newCount = 0;
            specialNotification = "";
            t4.Interval = randTime;
            t5.Interval = 5000;

        }

        public void ResetRabbits(Texture2D tx)
        {
            float posX2;
            float posY2;
            
            // Part 5 and Part 11
            Random r = new Random();
            int noOfEnemies = r.Next(15, 30);
            _enemyrabbitscollection = new Enemy[noOfEnemies];
            for (int i = 0; i < noOfEnemies; i++)
            {
                posX2 = r.Next(120, 1550 - 120);
                posY2 = r.Next(120, 825 - 120);
                _enemyrabbitscollection[i] = new Enemy(tx, Vector2.Zero);
                // Set the position of the collectables randomly
                _enemyrabbitscollection[i].Move(new Vector2(posX2, posY2)); 

            }

        }


        public void ResetCores(Texture2D cx)
        {
            // Part 5 and Part 11
            float posX1;
            float posY1;

            Random r = new Random();
            int noOfCores = r.Next(4, 10);
            _corecollection = new Collectable[noOfCores];
            for (int i = 0; i < noOfCores; i++)
            {
                posX1 = r.Next(120, 1550 - 120);
                posY1 = r.Next(120, 825 - 120);
                _corecollection[i] = new Collectable(cx, Vector2.Zero);
                // Set the position of the collectables randomly
                _corecollection[i].Move(new Vector2(posX1, posY1));
                    
            }

        }

        public void specialItem(Texture2D cx)
        {
            // Part 5 and Part 11
            float posX3;
            float posY3;

            Random r = new Random();
            posX3 = r.Next(170, 1550 - 170);
            posY3 = r.Next(170, 825 - 170);
            var _value = 50;
            _specialItem = new Collectable(cx, Vector2.Zero, _value);
            // Set the position of the collectables randomly
            _specialItem.Move(new Vector2(posX3, posY3));

        }
        
    }
}