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
    public class Trap : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GameMode gameMode;
        private BoundingSphere boundSphere;
     #region Properties

        public Model trapModel;
        public Vector3 Position { get; set; }
        public float Rotation = 0.0f;
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
        protected float aspectRatio;

        public Trap(Game game, GameMode gameMode)
            : base(game)
        {
            this.gameMode = gameMode;
            // TODO: Construct any child components here
        }


        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            boundSphere = new BoundingSphere(Position, 1.5f);
            base.Initialize();
        }

       protected override void LoadContent()
       {
           trapModel = Game.Content.Load<Model>("beartrap");
           aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;

           base.LoadContent();
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

        public bool Trapped()
        {
            return boundSphere.Intersects(gameMode.Player.BoundingBox);
        }


        public override void Draw(GameTime gameTime)
        {
            if(!(gameMode.ActiveLens is GreenLens))
            {
                return;
            }
            Matrix[] transforms = new Matrix[trapModel.Bones.Count];
            trapModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in trapModel.Meshes)
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

        }
    }
}
