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
    public class GameMode
    {
        public bool paused = false;

        private Game1 game;
        public Lens ActiveLens { get; set; }

        public HUD hud;
        public Sounds sound;
        public Song ActiveSong;

        List<LevelComponent> levels = new List<LevelComponent>();
        public LevelComponent ActiveLevel { get; set; }

        public RedLens RedLens { get; set; }
        public BlueLens BlueLens { get; set; }
        public GreenLens GreenLens { get; set; }
        public Stalker Stalker { get; set; }
        
        public KeyboardState LastKeyboardState { get; set; }
        public Player Player { get; private set; }

        public int lives = 1;
        public bool win = false;

        public GameMode(Game1 game)
        {
            this.game = game;

            ActiveLens = null;
            RedLens = new RedLens(game, this);
            GreenLens = new GreenLens(game, this);
            BlueLens = new BlueLens(game, this);
            Stalker = new Stalker(game, this);
            //Player = new Player(game, new Vector3(1, 0, -4), new Vector3(2.2f, 1, 2.2f), 1, 270);
            Player = new Player(game, new Vector3(0, 1, 0), new Vector3(1.5f, 1, 1.5f), 1, 270);
            game.Components.Add(Player);
            game.Components.Add(RedLens);
            game.Components.Add(GreenLens);
            game.Components.Add(BlueLens);

            game.Components.Add(Stalker);

            hud = new HUD(game, this);
            game.Components.Add(hud);

            sound = new Sounds(game, this);
            game.Components.Add(sound);

            levels.Add(new Level01(game, this));
            ActiveLevel = levels.First();

            foreach (Coin coin in ActiveLevel.Coins)
            {
                game.Components.Add(coin);
            }

            foreach (Wall wall in ActiveLevel.Walls)
            {
                game.Components.Add(wall);
            }

            foreach (Trap trap in ActiveLevel.Traps)
            {
                game.Components.Add(trap);
            }
            game.Components.Add(ActiveLevel.Goal);
        }


        public void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if ((ks.IsKeyDown(Keys.P) && LastKeyboardState.IsKeyUp(Keys.P)) ||
                    (ks.IsKeyDown(Keys.Escape) && LastKeyboardState.IsKeyUp(Keys.Escape)))
            {
                paused = !paused;
            }
            if (paused == false)
            {
                Player.MouseLocked(true);
                playGame(ks);
                Player.Enabled = true;
                Stalker.Enabled = true;
                if (ActiveLens != null)
                    ActiveLens.Enabled = true;
            }
            else {
                game.PauseGame(); 
                Player.MouseLocked(false); //mouse is not locked  
                Player.Enabled = false;
                Stalker.Enabled = false;
                if (ActiveLens != null)
                    ActiveLens.Enabled = false;

                LastKeyboardState = ks;
            }

        }

        public void playGame(KeyboardState ks) {    
            if (ks.IsKeyUp(Keys.Z) && LastKeyboardState.IsKeyDown(Keys.Z))
                ActiveLens = null;
            else if (ks.IsKeyUp(Keys.X) && LastKeyboardState.IsKeyDown(Keys.X))
                ActiveLens = RedLens;
            else if (ks.IsKeyUp(Keys.C) && LastKeyboardState.IsKeyDown(Keys.C))
                ActiveLens = GreenLens;
            else if (ks.IsKeyUp(Keys.V) && LastKeyboardState.IsKeyDown(Keys.V))
                ActiveLens = BlueLens;

            LastKeyboardState = ks;
            //checks to see if player intersects a coin
            for (int i = 0; i < ActiveLevel.Coins.Count; i++)
            {
                Coin coin = ActiveLevel.Coins[i];
                if (coin.Collected())
                {
                    coin.playCollected();
                    ActiveLevel.Coins.Remove(coin);
                    game.Components.Remove(coin);
                    BlueLens.Health = Math.Min(1,BlueLens.Health+.25f);
                    RedLens.Health = Math.Min(1, RedLens.Health + .25f);
                }
            }
            //trap detection
            for (int i = 0; i < ActiveLevel.Traps.Count; i++)
            {
                Trap trap = ActiveLevel.Traps[i];
                if (trap.Trapped())
                {
                    lives = lives - 1;
                    ActiveLevel.Traps.Remove(trap);
                    game.Components.Remove(trap);
                }
            }

            // put life deduction here
            if (lives == 0) {
                ((Game1)game).Disable(hud);
                ActiveSong = sound.activeSong;
                MediaPlayer.Stop();
                game.GameOver();
            }

            if (win)
            {
                ((Game1)game).Disable(hud);
                ActiveSong = sound.activeSong;
                MediaPlayer.Stop();
                game.WinGame();
            }

        }



        public void Draw(SpriteBatch spriteBatch)
        {
            if (ActiveLens is RedLens)
                game.GraphicsDevice.Clear(Color.Red);
            else if (ActiveLens is GreenLens)
                game.GraphicsDevice.Clear(Color.DarkGreen);
            else if (ActiveLens is BlueLens)
                game.GraphicsDevice.Clear(Color.DarkBlue);
            else game.GraphicsDevice.Clear(Color.White);
        }

    }
}
