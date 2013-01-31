
#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace tranquility
{
    /// <summary>
    /// This is A game component that is notified when it needs to draw itself. 
    /// It implements IDrawable.
    /// </summary>
    public partial class Stalker : DrawableGameComponent
    {
        #region Constants

        const int NUM_TRIANGLES = 12;
        const int NUM_VERTICES = 36;

        #endregion

        #region Properties

        public Texture2D Texture { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public float Rotation = 0.0f;

        #endregion

        private float _maxDist = 30;
        private float angToPlayer = 180;
        private Lens prevLens;
        private Random random = new Random();
        private GameMode _gameMode;
        
        #region Private Fields

        // Array of vertex information - contains position, normal and texture data
        private VertexPositionNormalTexture[] _vertices;

        // The vertex buffer where we load the vertices before drawing the shape
        private VertexBuffer _shapeBuffer;

        // Lets us check if the data has been constructed or not to improve performance
        private bool _isConstructed = false;

        private float aspectRatio = 0.0f;

        private BasicEffect effect;

        #endregion
        public Stalker(Game1 game, GameMode gm)
            : base(game)
        {
            // TODO: Construct any child components here
            Size = Vector3.One/4;
            Position = Vector3.Zero;
            _gameMode = gm;
            prevLens = gm.ActiveLens;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _vertices = new VertexPositionNormalTexture[NUM_VERTICES];

            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = Position + new Vector3(-1.0f, 1.0f, -1.0f) * Size;
            Vector3 topLeftBack = Position + new Vector3(-1.0f, 1.0f, 1.0f) * Size;
            Vector3 topRightFront = Position + new Vector3(1.0f, 1.0f, -1.0f) * Size;
            Vector3 topRightBack = Position + new Vector3(1.0f, 1.0f, 1.0f) * Size;

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = Position + new Vector3(-1.0f, -1.0f, -1.0f) * Size;
            Vector3 btmLeftBack = Position + new Vector3(-1.0f, -1.0f, 1.0f) * Size;
            Vector3 btmRightFront = Position + new Vector3(1.0f, -1.0f, -1.0f) * Size;
            Vector3 btmRightBack = Position + new Vector3(1.0f, -1.0f, 1.0f) * Size;

            // Normal vectors for each face (needed for lighting / display)
            Vector3 normalFront = new Vector3(0.0f, 0.0f, 1.0f) * Size;
            Vector3 normalBack = new Vector3(0.0f, 0.0f, -1.0f) * Size;
            Vector3 normalTop = new Vector3(0.0f, 1.0f, 0.0f) * Size;
            Vector3 normalBottom = new Vector3(0.0f, -1.0f, 0.0f) * Size;
            Vector3 normalLeft = new Vector3(-1.0f, 0.0f, 0.0f) * Size;
            Vector3 normalRight = new Vector3(1.0f, 0.0f, 0.0f) * Size;

            // UV texture coordinates
            Vector2 textureTopLeft = new Vector2(1.0f * Size.X, 0.0f * Size.Y);
            Vector2 textureTopRight = new Vector2(0.0f * Size.X, 0.0f * Size.Y);
            Vector2 textureBottomLeft = new Vector2(1.0f * Size.X, 1.0f * Size.Y);
            Vector2 textureBottomRight = new Vector2(0.0f * Size.X, 1.0f * Size.Y);

            // Add the vertices for the FRONT face.
            _vertices[0] = new VertexPositionNormalTexture(topLeftFront, normalFront, textureTopLeft);
            _vertices[1] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            _vertices[2] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);
            _vertices[3] = new VertexPositionNormalTexture(btmLeftFront, normalFront, textureBottomLeft);
            _vertices[4] = new VertexPositionNormalTexture(btmRightFront, normalFront, textureBottomRight);
            _vertices[5] = new VertexPositionNormalTexture(topRightFront, normalFront, textureTopRight);

            // Add the vertices for the BACK face.
            _vertices[6] = new VertexPositionNormalTexture(topLeftBack, normalBack, textureTopRight);
            _vertices[7] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            _vertices[8] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            _vertices[9] = new VertexPositionNormalTexture(btmLeftBack, normalBack, textureBottomRight);
            _vertices[10] = new VertexPositionNormalTexture(topRightBack, normalBack, textureTopLeft);
            _vertices[11] = new VertexPositionNormalTexture(btmRightBack, normalBack, textureBottomLeft);

            // Add the vertices for the TOP face.
            _vertices[12] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            _vertices[13] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            _vertices[14] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            _vertices[15] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureBottomLeft);
            _vertices[16] = new VertexPositionNormalTexture(topRightFront, normalTop, textureBottomRight);
            _vertices[17] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);

            // Add the vertices for the BOTTOM face. 
            _vertices[18] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            _vertices[19] = new VertexPositionNormalTexture(btmLeftBack, normalBottom, textureBottomLeft);
            _vertices[20] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            _vertices[21] = new VertexPositionNormalTexture(btmLeftFront, normalBottom, textureTopLeft);
            _vertices[22] = new VertexPositionNormalTexture(btmRightBack, normalBottom, textureBottomRight);
            _vertices[23] = new VertexPositionNormalTexture(btmRightFront, normalBottom, textureTopRight);

            // Add the vertices for the LEFT face.
            _vertices[24] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);
            _vertices[25] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            _vertices[26] = new VertexPositionNormalTexture(btmLeftFront, normalLeft, textureBottomRight);
            _vertices[27] = new VertexPositionNormalTexture(topLeftBack, normalLeft, textureTopLeft);
            _vertices[28] = new VertexPositionNormalTexture(btmLeftBack, normalLeft, textureBottomLeft);
            _vertices[29] = new VertexPositionNormalTexture(topLeftFront, normalLeft, textureTopRight);

            // Add the vertices for the RIGHT face. 
            _vertices[30] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            _vertices[31] = new VertexPositionNormalTexture(btmRightFront, normalRight, textureBottomLeft);
            _vertices[32] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);
            _vertices[33] = new VertexPositionNormalTexture(topRightBack, normalRight, textureTopRight);
            _vertices[34] = new VertexPositionNormalTexture(topRightFront, normalRight, textureTopLeft);
            _vertices[35] = new VertexPositionNormalTexture(btmRightBack, normalRight, textureBottomRight);

            base.Initialize();
        }

        /// <summary>
        /// Load graphics content specific to your component.
        /// </summary>
        protected override void LoadContent()
        {
            Texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            Texture.SetData<Color>(new[] { Color.Gray });

            aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            effect = new BasicEffect(Game.GraphicsDevice);
            
            base.LoadContent();
        }

        /// <summary>
        /// Allows the component to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            Player player = _gameMode.Player;
            float distToPlayer = _maxDist;
            if (_gameMode.ActiveLens != null)
            {
                distToPlayer *= _gameMode.ActiveLens.Health;
                if(_gameMode.ActiveLens.Health <= 0)
                {
                    _gameMode.lives -= 1;
                }
                if(_gameMode.ActiveLens != prevLens)
                {
                    angToPlayer = 180;
                    prevLens = _gameMode.ActiveLens;
                }
                angToPlayer += 2/_gameMode.ActiveLens.Health;
            }
            Position = player.Position +
                       new Vector3((float) Math.Cos(MathHelper.ToRadians(player.yAng+angToPlayer)), 0,
                                   (float) Math.Sin(MathHelper.ToRadians(player.yAng+angToPlayer)))*distToPlayer;
            //float idealAng = (float)random.Next(0, 360);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the component should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            //Lens activeLens = ((Game1)Game).ActiveLens;
            //if (activeLens == null)
            //    return;
            Player player = _gameMode.Player;
            
            effect.World =
                Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) *
                Matrix.CreateRotationX(MathHelper.ToRadians(Rotation)) *
                Matrix.CreateTranslation(Position);
            effect.View = Matrix.CreateLookAt(player.EyePosition, player.LookPoint, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1.0f, 1000.0f);

            effect.TextureEnabled = true;
            effect.Texture = Texture;

            effect.EnableDefaultLighting();

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                renderToDevice(Game.GraphicsDevice);
            }
            base.Draw(gameTime);
        }
        private void renderToDevice(GraphicsDevice device)
        {
            // Create the shape buffer and dispose of it to prevent out of memory
            using (VertexBuffer buffer = new VertexBuffer(
                device,
                VertexPositionNormalTexture.VertexDeclaration,
                NUM_VERTICES,
                BufferUsage.WriteOnly))
            {
                // Load the buffer
                buffer.SetData(_vertices);

                // Send the vertex buffer to the device
                device.SetVertexBuffer(buffer);
            }

            // Draw the primitives from the vertex buffer to the device as triangles
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, NUM_TRIANGLES);
        }
    }
}


