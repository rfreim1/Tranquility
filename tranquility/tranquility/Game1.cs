using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;

namespace tranquility
{
    public enum Screen
    {
        StartScreen,
        GameMode,
        GameOverScreen, 
        VictoryScreen,
        PauseScreen,
        Instructions
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        StartScreen startScreen;
        GameMode gameMode;
        Screen activeScreen;
        GameOverScreen gameOverScreen;
        VictoryScreen victoryScreen;
        PauseScreen pauseScreen;
        Instructions instructions;

        /// <summary>
        /// Public getter for the gameMode property
        /// </summary>
        public GameMode GameMode
        {
            get
            {
                return gameMode;
            }
        }

        public GraphicsDeviceManager graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public AnimatedSprite Sprite{get; set;}

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Sprite = new AnimatedSprite(Content.Load<Texture2D>("Bat"), 1, 32, 31);
            startScreen = new StartScreen(this, this.SpriteBatch);
            
            activeScreen = Screen.StartScreen;
            base.LoadContent();

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

            switch (activeScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Update(gameTime);
                    break;
                case Screen.GameMode:
                    if (gameMode != null)
                         gameMode.Update(gameTime);
                    break; 
                case Screen.GameOverScreen:
                    if (gameOverScreen != null)
                        gameOverScreen.Update(gameTime);
                    break;
                case Screen.VictoryScreen:
                    if (victoryScreen != null)
                        victoryScreen.Update(gameTime);
                    break;
                case Screen.PauseScreen:
                    if (pauseScreen != null)
                        pauseScreen.Update(gameTime);
                    break;
                case Screen.Instructions:
                    if (instructions != null)
                        instructions.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            SpriteBatch.Begin();

            switch (activeScreen)
            {
                case Screen.StartScreen:
                    if (startScreen != null)
                        startScreen.Draw(gameTime);
                    break;
                case Screen.GameMode:
                    if (gameMode != null)
                         gameMode.Draw(SpriteBatch);
                     break;
                case Screen.GameOverScreen:
                     if (gameOverScreen != null)
                         gameOverScreen.Draw(gameTime);
                     break;
                case Screen.VictoryScreen:
                     if (victoryScreen != null)
                         victoryScreen.Draw(gameTime);
                     break;
                case Screen.PauseScreen:
                     if (pauseScreen != null)
                         pauseScreen.Draw(gameTime);
                     gameMode.Draw(SpriteBatch);
                     break;
                case Screen.Instructions:
                     if (instructions != null)
                         instructions.Draw(gameTime);
                     break;
            }
            base.Draw(gameTime);
            SpriteBatch.End();
            
            
        }
        public void startGameMode()
        {
            gameMode = new GameMode(this);
            activeScreen = Screen.GameMode;
            startScreen = null;
            gameOverScreen = null;
        }

        public void PauseGame()
        {
            pauseScreen = new PauseScreen(this, SpriteBatch);
            activeScreen = Screen.PauseScreen;
        }

        public void UnPause()
        {
            activeScreen = Screen.GameMode;
            pauseScreen = null;
            gameMode.paused = false;

        }

        public void showInstructions()
        {
            instructions = new Instructions(this, this.SpriteBatch);
            activeScreen = Screen.Instructions;
            startScreen = null;
        }

        public void goStartScreen()
        {
            startScreen = new StartScreen(this, this.SpriteBatch);
            activeScreen = Screen.StartScreen;
            instructions = null;
        }


        public void WinGame() {
            victoryScreen = new VictoryScreen(this, SpriteBatch);
            activeScreen = Screen.VictoryScreen;
            List<GameComponent> toRemove = new List<GameComponent>();
            foreach (GameComponent component in Components)
            {
                if (component is Wall || component is Trap || component is Coin ||
                    component is Lens || component is Player || component is Sounds ||
                    component is HUD || component is Goal || component is Stalker) toRemove.Add(component);
            }
            foreach (GameComponent gameComponent in toRemove)
            {
                Components.Remove(gameComponent);
            }
            gameMode = null;
        }

        public void GameOver()
        {
            gameOverScreen = new GameOverScreen(this, SpriteBatch, Sprite);
            activeScreen = Screen.GameOverScreen;
            gameMode = null;
            List<GameComponent>toRemove = new List<GameComponent>();
            foreach (GameComponent component in Components)
            {
                if (component is Wall || component is Trap || component is Coin || 
                    component is Lens || component is Player || component is Sounds || 
                    component is HUD || component is Goal || component is Stalker)toRemove.Add(component);
            }
            foreach (GameComponent gameComponent in toRemove)
            {
                Components.Remove(gameComponent);
            }
        }


        public void Disable(DrawableGameComponent d)
        {
            if (d!=null)
            {
                d.Enabled = false;
                d.Visible = false;
            }
        }
        public void Enable(DrawableGameComponent d)
        {
            if(d!=null)
            {
                d.Enabled = true;
                d.Visible = true;
            }
        }
    }
}
