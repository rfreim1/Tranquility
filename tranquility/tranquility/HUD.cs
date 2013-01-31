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
    public class HUD : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public int Width { get; set; }
        public int Height { get; set; }
        GameMode gameMode;

        public Texture2D RedLensTexture { get; set; }
        public Texture2D GreenLensTexture { get; set; }
        public Texture2D BlueLensTexture { get; set; }
        public Texture2D BackgroundTexture { get; set; }

        public HUD(Game game, GameMode gameMode)
            : base(game)
        {
            // TODO: Construct any child components here
            this.gameMode = gameMode;

        }

        protected override void LoadContent()
        {
            RedLensTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            RedLensTexture.SetData<Color>(new[] { Color.Red });

            GreenLensTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            GreenLensTexture.SetData<Color>(new[] { Color.Green });

            BlueLensTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            BlueLensTexture.SetData<Color>(new[] { Color.Blue });

            BackgroundTexture = new Texture2D(Game.GraphicsDevice, 1, 1);
            BackgroundTexture.SetData<Color>(new[] { new Color(0,0,0,0.5f) });

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            Height = 15;
            Width = 100;

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ((Game1)Game).SpriteBatch;

            int DeviceWidth = Game.GraphicsDevice.Viewport.Width;
            int DeviceHeight = Game.GraphicsDevice.Viewport.Height;

            sb.Draw(
                BackgroundTexture,
                new Rectangle(
                    0,
                    (int)(DeviceHeight - 1.4 * Height),
                    DeviceWidth,
                    (int)(1.4 * Height)
                ),
                Color.White
            );

            sb.Draw(
                RedLensTexture,
                new Rectangle(
                    //((int)0.2 * Height),
                    10,
                    (int)(DeviceHeight - 1.2 * Height),
                    ((int)(gameMode.RedLens.Health * (float)Width)),
                    Height
                ),
                Color.White
            );
            sb.Draw(
                GreenLensTexture,
                new Rectangle(
                    //((int)1.4 * Height),
                    120,
                    (int)(DeviceHeight - 1.2 * Height),
                    ((int)(gameMode.GreenLens.Health * (float)Width)),
                    Height
                ),
                Color.White
            );
            sb.Draw(
                BlueLensTexture,
                new Rectangle(
                    //((int)2.6 * Height),
                    230,
                    (int)(DeviceHeight - 1.2 * Height),
                    ((int)(gameMode.BlueLens.Health * (float)Width)),
                    Height
                ),
                Color.White
            );
            
            base.Draw(gameTime);
        }
    }
}
