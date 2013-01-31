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
    public class Coin : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GameMode gameMode;

        #region Properties
        public Model Model { get; set; }
        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        
        public Vector3 CameraPosition
        {
            get
            {
                return gameMode.Player.EyePosition;
            }
        }

        public Vector3 LookPosition
        {
            get
            {
                return gameMode.Player.LookPoint;
            }
        }
        #endregion

        private BoundingSphere boundSphere;

        protected float aspectRatio = 0.0f;
        private SoundEffect coinCollected;


        public Coin(Game game, GameMode gameMode)
            : base(game)
        {
            this.gameMode = gameMode;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
            boundSphere = new BoundingSphere(Position, 0.011889f);
        }

        /// <summary>
        /// Loads any component specific content
        /// </summary>
        protected override void LoadContent()
        {
            Model = Game.Content.Load<Model>("coin");
            coinCollected = Game.Content.Load<SoundEffect>("coinCollected");
            aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: collision detection with player.

            Rotation += .1f*(float)Math.Sin(Position.X + gameTime.TotalGameTime.TotalSeconds * .05f);

            base.Update(gameTime);
        }

        /// <summary>
        /// plays sound when coin is collected
        /// </summary>
        public bool Collected(){
            return boundSphere.Intersects(gameMode.Player.BoundingBox);
        }

        public void playCollected()
        {
            coinCollected.Play();
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            Lens activeLens = gameMode.ActiveLens;
            if (!(activeLens is BlueLens))
                return;

            Matrix[] transforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index]
                                 * Matrix.CreateRotationY(Rotation)
                                 * Matrix.CreateTranslation(Position);
                    effect.View = Matrix.CreateLookAt(CameraPosition, LookPosition, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio, 1.0f, 10000.0f);
                }

                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
