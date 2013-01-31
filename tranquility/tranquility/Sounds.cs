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


namespace tranquility
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Sounds : Microsoft.Xna.Framework.GameComponent
    {
        GameMode gameMode;
        private Lens prevLens;

        private Song white;
        private Song red;
        private Song green;
        private Song blue;
        private SoundEffect transition;
        public Song activeSong{get; set;}
        

        public Sounds(Game game, GameMode gameMode)
            : base(game)
        {
            this.gameMode = gameMode;
            // TODO: Construct any child components here
            prevLens = gameMode.ActiveLens;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            transition = Game.Content.Load<SoundEffect>("transition");
            red = Game.Content.Load<Song>("red");
            green = Game.Content.Load<Song>("green");
            blue = Game.Content.Load<Song>("blue");
            white = Game.Content.Load<Song>("white");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(white);
            activeSong = white;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            Lens currentLens = gameMode.ActiveLens;
            if (currentLens != prevLens)
            {
                transition.Play();
                if (currentLens is RedLens)
                {
                    MediaPlayer.Play(red);
                    activeSong = red;
                }
                else if (currentLens is GreenLens)
                {
                    MediaPlayer.Play(green);
                    activeSong = green;
                }
                else if (currentLens is BlueLens)
                {
                    MediaPlayer.Play(blue);
                    activeSong = blue;
                }
                else
                {
                    MediaPlayer.Play(white);
                    activeSong = white;
                }
            }
            prevLens = currentLens;
            base.Update(gameTime);
        }
    }
}
