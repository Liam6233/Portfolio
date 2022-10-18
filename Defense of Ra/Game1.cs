using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace DefenseOfRa
{
    // Enum to represent the state of the game
    public enum GameState
    {
        MainMenu,
        ControlsScreeen,
        Credits,
        Options,
        GamePlay,
        PausedGame,
        Victory,
        Failure,
    }

    public class Game1 : Game
    {
        // made static so it is accessible in other classes (specifically GameObject's default constructor)
        private static GraphicsDeviceManager _graphics; 
        private SpriteBatch _spriteBatch;
        private static SpriteFont arial;

        int defaultWidth;
        int defaultHeight;

        // VARIABLES

        private Random rand;
        private double timer;
        private double maxTime;
        private int maxPlayerHealth;
        private int maxRaHealth;
        private Player player;
        private Ra ra; 
        private Spear spearObject;
        //private EnemyManager enemies;
        private StaticObject boatPart1;
        private StaticObject boatPart2;
        private StaticObject boatPart3;
        private StaticObject boatPart4;
        private StaticObject raBottom;

        private SnakeArms snakeEnemy;

        private MainMenu mainMenu;
        private GuideMenu guideMenu;
        private DebugMenu debugMenu;
        private DeathMenu deathMenu;
        private OptionsMenu optionsMenu;
        private VictoryMenu victoryMenu;
        private Button playButton;
        private Button exitButton;
        private Button guideButton;
        private Button debugButton;
        private Button retryButton;
        private Button optionsButton;
        private CheckBox healthCheck;
        private CheckBox timeCheck;
        private CheckBox f3Check;
        private CheckBox fullScreenCheck;
        private CheckBox musicCheck;
        private Texture2D deathScreenImage;
        private Texture2D logo;
        private static KeyboardState lastKeystroke;
        private static MouseState lastMouseState;

        // TEXTURES

        private static Texture2D placeholder;
        private Texture2D playButtonBase;
        private Texture2D playButtonHover;
        private Texture2D exitButtonBase;
        private Texture2D exitButtonHover;
        private Texture2D guideButtonBase;
        private Texture2D guideButtonHover;
        private Texture2D guideMenuImage;
        private Texture2D debugButtonBase;
        private Texture2D debugButtonHover;
        private Texture2D retryButtonBase;
        private Texture2D retryButtonHover;
        private Texture2D optionsButtonBase;
        private Texture2D optionsButtonHover;
        private Texture2D uncheckedBox;
        private Texture2D checkedBox;
        private Texture2D clouds;
        private Texture2D background;
        private Texture2D playerHealthIcon;
        private Texture2D raHealthIcon;
        private Texture2D timerIcon;

        private Texture2D platformSprite;
        private Texture2D playerSprite;
        private Texture2D potteryDecoration;
        private Texture2D spearSprite;
        private Texture2D raSprite;
        private Texture2D snakeSprite;
        private Texture2D mummySprite;
        private Texture2D locustSprite;

        private Texture2D boatEnd;
        private Texture2D boatDeck;
        private Texture2D boatMiddle;
        private Texture2D purpleForeground;

        private Song tempMenuMusic;
        private Song tempGameMusic;

        // animation stuff
        private Texture2D playerIdleSheet;
        private Texture2D playerRunningSheet;
        private Texture2D playerFallingSheet;
        private Texture2D playerThrowSheet;
        private Texture2D playerThrustSheet;
        private Texture2D raIdleSheet;

        private Animation playerIdle;
        private Animation playerRunning;
        private Animation playerFalling;
        private Animation playerThrowing;
        private Animation playerThrusting;
        private Animation raIdle;

        // Parallax variables

        List<ParallaxObject> parallaxObjs;
        private float backgroundSpeed = .3f;
        private float cloudSpeed = 1;

        // platforms
        private Platform plat1;
        private Platform plat2;

        // DEBUG VARS

        public static string isOnPlatform;

        //private BoundsTester snakeBounds;
        private bool infiniteHealth = false;
        private bool infiniteTime = false;
        private bool inF3Mode = false;

        private SoundEffect thrustEffect;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            maxTime = 180f;
            timer = maxTime;
            Window.AllowUserResizing = true;
            maxPlayerHealth = 15;
            maxRaHealth = 30;

            parallaxObjs = new List<ParallaxObject>();
            base.Initialize();
        }
   
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Draw the background first
            background = Content.Load<Texture2D>("background");
            parallaxObjs.Add(new ParallaxObject(background, new Vector2(-background.Width, -500), new Vector2(-background.Width * 2, 0), backgroundSpeed));    // 1st panel
            parallaxObjs.Add(new ParallaxObject(background, new Vector2(0 , -500), new Vector2(-background.Width, 0), backgroundSpeed));   // 2nd panel
            parallaxObjs.Add(new ParallaxObject(background, new Vector2(background.Width, -500), new Vector2(0, 0), backgroundSpeed));    // 3rd panel
            parallaxObjs.Add(new ParallaxObject(background, new Vector2(background.Width * 2, -500), new Vector2(background.Width, 0), backgroundSpeed));    // 3rd panel

            // Testing sprites loading

            placeholder = Content.Load<Texture2D>("square");
            arial = Content.Load<SpriteFont>("Arial");

            // GameObject sprite loading
            
            playerSprite = Content.Load<Texture2D>("emptySprite");
            potteryDecoration = Content.Load<Texture2D>("pottery");
            spearSprite = Content.Load<Texture2D>("spear");
            boatEnd = Content.Load<Texture2D>("boatEnd");
            boatDeck = Content.Load<Texture2D>("boatMiddle");
            boatMiddle = Content.Load<Texture2D>("newBoatMiddle");        
            raSprite = Content.Load<Texture2D>("emptyRa");
            snakeSprite = Content.Load<Texture2D>("snakeArms");
            mummySprite = Content.Load<Texture2D>("mummy");
            locustSprite = Content.Load<Texture2D>("tempLocust");
            //thrustEffect = Content.Load<SoundEffect>("thrustSound");
            platformSprite = Content.Load<Texture2D>("platform");
            // UI sprite loading

            playerHealthIcon = Content.Load<Texture2D>("playerHealthIcon");
            raHealthIcon = Content.Load<Texture2D>("raHealthIcon");
            timerIcon = Content.Load<Texture2D>("watchIcon");
            playButtonBase = Content.Load<Texture2D>("playBtnBase");
            playButtonHover = Content.Load<Texture2D>("playBtnHover");
            exitButtonBase = Content.Load<Texture2D>("exitBtnBase");
            exitButtonHover = Content.Load<Texture2D>("exitBtnHover");
            guideButtonBase = Content.Load<Texture2D>("guideBtnBase");
            guideButtonHover = Content.Load<Texture2D>("guideBtnHover");
            guideMenuImage = Content.Load<Texture2D>("GuideSpriteNew");
            debugButtonBase = Content.Load<Texture2D>("debugBtnBase");
            debugButtonHover = Content.Load<Texture2D>("debugBtnHover");
            retryButtonBase = Content.Load<Texture2D>("retryBtnBase");
            retryButtonHover = Content.Load<Texture2D>("retryBtnHover");
            optionsButtonBase = Content.Load<Texture2D>("optionsBtnBase");
            optionsButtonHover = Content.Load<Texture2D>("optionsBtnHover");
            uncheckedBox = Content.Load<Texture2D>("checkBoxBox");
            checkedBox = Content.Load<Texture2D>("checkBoxCheck");
            logo = Content.Load<Texture2D>("Logo");

            playerIdleSheet = Content.Load<Texture2D>("PlayerIdle");
            playerRunningSheet = Content.Load<Texture2D>("playerRun");
            playerFallingSheet = Content.Load<Texture2D>("playerFall");
            playerThrowSheet = Content.Load<Texture2D>("playerThrow");
            playerThrustSheet = Content.Load<Texture2D>("playerThrust");
            raIdleSheet = Content.Load<Texture2D>("raIdle");

            tempGameMusic = Content.Load<Song>("tempPlayMusic");
            tempMenuMusic = Content.Load<Song>("tempMenuMusic");

            playerIdle = new Animation(playerIdleSheet, 41, 10);
            playerRunning = new Animation(playerRunningSheet, 12, 9);
            playerFalling = new Animation(playerFallingSheet, 6, 6);
            playerThrowing = new Animation(playerThrowSheet, 10, 10);
            playerThrusting = new Animation(playerThrustSheet, 7, 21);
            raIdle = new Animation(raIdleSheet, 6, 3);
            
            // GameObject initialization (whatever is added first is drawn closest to the back)
            boatPart1 = new StaticObject(new Vector2(0, -1), boatEnd);
            boatPart2 = new StaticObject(new Vector2(224, 371), boatDeck);
            raBottom = new StaticObject(new Vector2(584, 371), boatMiddle);
            boatPart4 = new StaticObject(new Vector2(1540, 371), boatDeck);
            boatPart3 = new StaticObject(new Vector2(1900, -1), boatEnd);

            boatPart3.SprtEffct = SpriteEffects.FlipHorizontally;
            boatPart4.SprtEffct = SpriteEffects.FlipHorizontally;
            //pots = new StaticObject(new Vector2(300, 215), potteryDecoration);
            ra = new Ra(new Vector2(978, raBottom.Position.Y - raSprite.Height), raSprite);
            player = new Player(new Vector2(400, 200), playerSprite);
            spearObject = new Spear(player.Position, spearSprite);
            player.Health = maxPlayerHealth;
            ra.Health = maxRaHealth;

            plat1 = new Platform(new Vector2(ra.Position.X - 550,raBottom.Position.Y - 200), platformSprite);
            plat2 = new Platform(new Vector2(ra.Position.X + ra.Bounds.Width + 250, raBottom.Position.Y - 200), platformSprite);
            //snakeEnemy = new SnakeArms(new Vector2(500, 300), snakeSprite);
            // mummy
            // locust
            // adds templates to the enemy manager for the manager to spawn more of
            rand = new Random();
            EnemyManager.InitializeEnemies(snakeSprite, mummySprite, locustSprite, rand);
            //CollisionManager.AddEnemies(snakeEnemy); // debug to add this snake to the collision manager

            player.AddAnimation("moving", playerRunning);
            player.AddAnimation("standing", playerIdle);
            player.AddAnimation("falling", playerFalling);
            player.AddAnimation("throwing", playerThrowing);
            player.AddAnimation("thrusting", playerThrusting);
            ra.AddAnimation("idle", raIdle);
            // DEBUG
            //snakeBounds = new BoundsTester(snakeEnemy);

            

            // Add cloud parallax in front of boat
            clouds = Content.Load<Texture2D>("clouds");
            parallaxObjs.Add(new ParallaxObject(clouds, new Vector2(-clouds.Width, 400), new Vector2(-clouds.Width * 2, 0), cloudSpeed)); // 1st panel
            parallaxObjs.Add(new ParallaxObject(clouds, new Vector2(0, 400), new Vector2(-clouds.Width, 0), cloudSpeed)); // 2nd panel
            parallaxObjs.Add(new ParallaxObject(clouds, new Vector2(clouds.Width, 400), new Vector2(0, 0), cloudSpeed)); // 3rd panel
            parallaxObjs.Add(new ParallaxObject(clouds, new Vector2(clouds.Width * 2, 400), new Vector2(clouds.Width, 0), cloudSpeed)); // 4th panel
            parallaxObjs.Add(new ParallaxObject(clouds, new Vector2(clouds.Width * 3, 400), new Vector2(clouds.Width * 2, 0), cloudSpeed)); // 4th panel

            purpleForeground = Content.Load<Texture2D>("purpleForeground");
            parallaxObjs.Add(new ParallaxObject(purpleForeground, new Vector2(-purpleForeground.Width, 550), new Vector2(0, 0), 0f));
            parallaxObjs.Add(new ParallaxObject(purpleForeground, new Vector2(0, 550), new Vector2(0, 0), 0f));
            parallaxObjs.Add(new ParallaxObject(purpleForeground, new Vector2(purpleForeground.Width, 550), new Vector2(0, 0), 0f));

            // UI initialization

            playButton = new Button(new Vector2(325, 0), playButtonBase, playButtonHover);
            exitButton = new Button(new Vector2(325, 0), exitButtonBase, exitButtonHover);
            guideButton = new Button(guideButtonBase, guideButtonHover);
            debugButton = new Button(debugButtonBase, debugButtonHover);
            retryButton = new Button(retryButtonBase, retryButtonHover);
            optionsButton = new Button(optionsButtonBase, optionsButtonHover);
            healthCheck = new CheckBox(uncheckedBox, checkedBox, infiniteHealth);
            timeCheck = new CheckBox(uncheckedBox, checkedBox, infiniteTime);
            f3Check = new CheckBox(uncheckedBox, checkedBox, inF3Mode);
            fullScreenCheck = new CheckBox(uncheckedBox, checkedBox, false);
            musicCheck = new CheckBox(uncheckedBox, checkedBox, true);

            //pots = new StaticObject(new Vector2(300, 215), potteryDecoration);

            // Menu initialization
            mainMenu = new MainMenu(logo, new Button[] { playButton, guideButton, optionsButton, debugButton, exitButton }, "no text");
            guideMenu = new GuideMenu(guideMenuImage, new Button[] { exitButton}, "guide");
            debugMenu = new DebugMenu(placeholder, new Button[] { f3Check, healthCheck, timeCheck, exitButton}, "F3 Mode, Infinite Health , Infinite Time");
            deathScreenImage = Content.Load<Texture2D>("deathScreenImage");
            optionsMenu = new OptionsMenu(placeholder, new Button[] { fullScreenCheck, musicCheck, exitButton}, "Fullscreen, Audio");
            deathMenu = new DeathMenu(deathScreenImage, new Button[] { retryButton, exitButton}, "no text");
            victoryMenu = new VictoryMenu(placeholder, new Button[] { retryButton, exitButton }, "no text");
            MenuManager.Push(guideMenu);    // start the game in the guide menu
            MenuManager.Push(mainMenu); // have the start menu waiting under hte guide menu

            // Music initialization
            MediaPlayer.Volume = .05f;
            MediaPlayer.Play(tempGameMusic);
            MediaPlayer.IsRepeating = true;
            
        }

        protected override void Update(GameTime gameTime)
        {
            // if in the game and press escape, then go to the main menu...
            if ( SinglePress(Keys.Escape) && MenuManager.Count == 0)
            {
                MenuManager.Push(mainMenu);
                //Exit();
            }
            // else if in a menu and press escape, exit the menu...
            else if(SinglePress(Keys.Escape) && MenuManager.Count > 0)
            {
                MenuManager.Pop();
            }

            if(SinglePress(Keys.F3))
            {
                inF3Mode = f3Check.ToggleCheck();
            }

            // DEBUG to trigger death screen
            if(SinglePress(Keys.L) && inF3Mode)
            {
                MenuManager.Push(deathMenu);
            }

            // TODO: Add your update logic here
            KeyboardState kbState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            // Update Parallax
            foreach(ParallaxObject parallaxObj in parallaxObjs)
            {
                parallaxObj.Update();
            }

            if (MenuManager.Count < 1)    // GAME STATE
            {
                // timer only  ticks down when infinite time setting is 
                if (!infiniteTime)
                {
                    timer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                player.Update(kbState, mState, gameTime, spearObject);
                ra.Update(gameTime);
                spearObject.Update(player);
                // change enemy states base off of time
                EnemyManager.UpdateSpawning(timer, maxTime);
                // chance to spawn an enemy
                if(rand.NextDouble() < .025)
                {
                    EnemyManager.SpawnEnemy(raBottom.Position.Y);
                }
                else
                {
                    // if there are less than a certain amount of enemies,
                    //  spawn enemies every .5 of a second
                    if (EnemyManager.GetEnemyCount() < 10 && ((int)(timer*60)) % 45 == 0)
                    {
                        EnemyManager.SpawnEnemy(raBottom.Position.Y);
                    }
                }
                // have the enemies run their update methods
                EnemyManager.UpdateEnemies(player, ra, gameTime);
                // clear the dead enemies
                EnemyManager.ClearDeadEnemies();
                
                CollisionManager.Update();
                // camera movement
                // follows player with an offset that gets the center of the player's x, and ~third down the screen for y
                Camera.Follow(player, new Vector2((_graphics.GraphicsDevice.Viewport.Width / 2) - (playerSprite.Width / 2), _graphics.GraphicsDevice.Viewport.Height / 2));

                if (player.Health <= 0 || ra.Health <= 0)
                {
                    MenuManager.Push(deathMenu);
                }
            }   
            else               // PAUSE STATE
            {
                // check UI interactions
                CheckButtonClicks();
            }

            if(timer <= 0)
            {
                EnemyManager.RemoveEnemies();
                CollisionManager.ResetEnemies();
                DrawManager.EraseEnemies();

                spearObject.State = SpearState.Recalled;
                player.Health = maxPlayerHealth;
                player.IFrames = 0;
                ra.Health = maxRaHealth;
                ra.IFrames = 0;
                timer = maxTime;

                MenuManager.Push(victoryMenu);
            }

            // save the last inputs
            lastKeystroke = Keyboard.GetState();
            lastMouseState = Mouse.GetState();

            // Debug
            // if the infinite health is turned on, the player and Ra's health are 
            // reset back to full every frame meaning they can never die
            if (infiniteHealth)
            {
                player.Health = maxPlayerHealth;
                ra.Health = maxRaHealth;
            }
            //snakeBounds.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20, 0, 54));

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            // draw the game objects     
            DrawManager.DrawGameObjs(_spriteBatch);

            // then draw the menu background
            if (MenuManager.Count > 0)
                MenuManager.Draw(_spriteBatch);

            // then draw ui (buttons, etc)
            DrawManager.DrawUI(_spriteBatch);

            // TEXT

            if(MenuManager.Count < 1)
            {
                Vector2 healthPos = new Vector2(GraphicsDevice.Viewport.Width /4f - playerHealthIcon.Width, GraphicsDevice.Viewport.Height - playerHealthIcon.Height - 10 );
                Vector2 raHealthPos = new Vector2(GraphicsDevice.Viewport.Width / 1.25f - raHealthIcon.Width, GraphicsDevice.Viewport.Height - raHealthIcon.Height - 10);
                Vector2 timerPos = new Vector2(GraphicsDevice.Viewport.Width / 2f - timerIcon.Width, GraphicsDevice.Viewport.Height - timerIcon.Height - 10);

                /*
                if(player.PState == PlayerStates.ThrustingLeft || player.PState == PlayerStates.ThrustingRight)
                {
                    thrustEffect.Play();
                }
                */
                // draw icons for ui
                _spriteBatch.Draw(playerHealthIcon,
                                  healthPos,
                                  Color.White);

                _spriteBatch.Draw(raHealthIcon,
                                  raHealthPos,
                                  Color.White);

                _spriteBatch.Draw(timerIcon,
                                  timerPos,
                                  Color.White);

                // draw text for ui
                _spriteBatch.DrawString(arial, string.Format($"{(int)timer / 60}:{(int)(timer % 60):D2}"), timerPos + new Vector2(50, 5), Color.White);

                if (player.Health <= (maxPlayerHealth * 0.25f)) // if player health is low then flash red
                {
                    if ((int)gameTime.TotalGameTime.TotalSeconds % 2 == 0)  // intized version of the passed seconds can be checked to be even/odd
                        _spriteBatch.DrawString(arial, "" + player.Health, healthPos + new Vector2(50, 5), Color.Red);
                    else
                        _spriteBatch.DrawString(arial, "" + player.Health, healthPos + new Vector2(50, 5), Color.White);

                }
                else // else white text
                {
                    _spriteBatch.DrawString(arial, "" + player.Health, healthPos + new Vector2(50, 5), Color.White);
                }

                if(ra.Health <= (maxRaHealth * 0.25f))  // if ra health is red flash red
                {
                    if((int)gameTime.TotalGameTime.TotalSeconds % 2 == 0)
                        _spriteBatch.DrawString(arial, "" + ra.Health, raHealthPos + new Vector2(50, 5), Color.Red);
                    else
                        _spriteBatch.DrawString(arial, "" + ra.Health, raHealthPos + new Vector2(50, 5), Color.White);
                }
                else // else white text
                {
                    _spriteBatch.DrawString(arial, "" + ra.Health, raHealthPos + new Vector2(50, 5), Color.White);
                }

            }
            
            if (inF3Mode)
            {
                _spriteBatch.DrawString(arial, "Player State: " + player.PState.ToString(), new Vector2(10, 0), Color.White);
                _spriteBatch.DrawString(arial, "Spear State: " + spearObject.State.ToString(), new Vector2(10, 30), Color.White);
                _spriteBatch.DrawString(arial, "has Spear: " + player.HasSpear.ToString(), new Vector2(10, 60), Color.White);
                _spriteBatch.DrawString(arial, "iFrames: " + player.IFrames, new Vector2(10, 90), Color.White);
                _spriteBatch.DrawString(arial, "Menus open: " + MenuManager.Count, new Vector2(10, 120), Color.White);
                _spriteBatch.DrawString(arial, "On platform: " + isOnPlatform, new Vector2(10, 150), Color.White);
                // statements meant for debugging but now that those features work, they are no longer necessary even in debug mode
                //_spriteBatch.DrawString(arial, $"Menus on the stack: {MenuManager.Count}", new Vector2(20, 100), Color.White);
                //_spriteBatch.DrawString(arial, $"Snake state: {snakeEnemy.EState}", new Vector2(20, 100), Color.White);
                //_spriteBatch.DrawString(arial, $"Got infinite health? {infiniteHealth}", new Vector2(20, 120), Color.White);
            }

            //BACKGROUND AND CLOUD TEXTURES

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // HELPER METHODS

        public static bool SinglePress(Keys key)
        {
            if(Keyboard.GetState().IsKeyDown(key) && !lastKeystroke.IsKeyDown(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SingleLeftClick()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckButtonClicks()
        {
            if(playButton.CheckHover() && SingleLeftClick())    // PLAY button
            {
                MenuManager.Pop();
            }

            if(guideButton.CheckHover() && SingleLeftClick())   // GUIDE button
            {
                // if opened for the first time...
                if(MenuManager.Count > 1)
                {
                    MenuManager.Pop();
                }
                else
                {
                    MenuManager.Push(guideMenu);
                }
                
            }

            if (optionsButton.CheckHover() && SingleLeftClick())   // OPTIONS button
            {
                MenuManager.Push(optionsMenu);
            }

            if (debugButton.CheckHover() && SingleLeftClick())   // DEBUG button
            {
                MenuManager.Push(debugMenu);
            }

            if(retryButton.CheckHover() && SingleLeftClick())   // RETRY button
            {
                MenuManager.Pop();

                ResetGame();

                // Now the difference between exit and retry button is that retry immediately resets the game
               // MenuManager.Push(mainMenu);
            }

            if(healthCheck.CheckHover() && SingleLeftClick())     // infinite HEALTH checkbox
            {
                infiniteHealth = healthCheck.ToggleCheck();
            }

            if (timeCheck.CheckHover() && SingleLeftClick())     // infinite TIME checkbox
            {
                infiniteTime = timeCheck.ToggleCheck();
            }

            if (f3Check.CheckHover() && SingleLeftClick())     // F3 checkbox
            {
                inF3Mode = f3Check.ToggleCheck();
            }

            if(fullScreenCheck.CheckHover() && SingleLeftClick()) // FULLSCREEN checkbox
            {
                fullScreenCheck.ToggleCheck();
                if(fullScreenCheck.IsChecked)
                {
                    defaultWidth = Graphics.PreferredBackBufferWidth;
                    defaultHeight = Graphics.PreferredBackBufferHeight;

                    Graphics.PreferredBackBufferWidth = 1920;
                    Graphics.PreferredBackBufferHeight = 1080;
                    Graphics.IsFullScreen = true;
                }
                else
                {
                    Graphics.PreferredBackBufferWidth = defaultWidth;
                    Graphics.PreferredBackBufferHeight = defaultHeight;
                    Graphics.IsFullScreen = false;
                }
               

                Graphics.ApplyChanges();
            }

            if (musicCheck.CheckHover() && SingleLeftClick())     // MUSIC checkbox
            {
                musicCheck.ToggleCheck();

                if(musicCheck.IsChecked)
                {
                    MediaPlayer.Resume();
                }
                else
                {
                    MediaPlayer.Pause();
                }
            }

            if (exitButton.CheckHover() && SingleLeftClick())    // EXIT button
            {
                if(MenuManager.Peek() == mainMenu) 
                {
                    Exit();
                }
                else if(MenuManager.Peek() == deathMenu || MenuManager.Peek() == victoryMenu)
                {
                    MenuManager.Pop();
                    ResetGame();
                    MenuManager.Push(mainMenu);
                }
                else
                {
                    MenuManager.Pop();
                }
                
            }
        }

        private void ResetGame()
        {
            EnemyManager.RemoveEnemies();
            CollisionManager.ResetEnemies();
            DrawManager.EraseEnemies();

            spearObject.State = SpearState.Recalled;
            player.Health = maxPlayerHealth;
            player.IFrames = 0;
            ra.Health = maxRaHealth;
            ra.IFrames = 0;
            timer = maxTime;
        }

        // PROPERTIES

        public static GraphicsDeviceManager Graphics
        {
            get { return _graphics; }
        }

        public static Texture2D Placeholder
        {
            get { return placeholder; }
        }

        public static SpriteFont Arial
        {
            get { return arial; }
        }

    }
}
