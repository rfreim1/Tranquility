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
    public class Wall : Microsoft.Xna.Framework.DrawableGameComponent
    {
        GameMode gameMode;

        #region Constants

        const int NUM_TRIANGLES = 12;
        const int NUM_VERTICES = 36;

        #endregion

        #region Properties

        public Texture2D Texture { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Size { get; set; }
        public float Rotation = 0.0f;

        // should be updated to use player's camera eventually
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

        #region Private Fields

        // Array of vertex information - contains position, normal and texture data
        private VertexPositionNormalTexture[] _vertices;

        // The vertex buffer where we load the vertices before drawing the shape
        private VertexBuffer _shapeBuffer;

        // Lets us check if the data has been constructed or not to improve performance
        private bool _isConstructed = false;

        private float aspectRatio = 0.0f;
        
        private BasicEffect effect;

        private BoundingBox _boundingBox;

        #endregion

        public Wall(Game game, GameMode gameMode)
            : base(game)
        {
            // TODO: Construct any child components here
            this.gameMode = gameMode;
        }

     /*   public Wall(Game game, Vector3 position, Vector3 size)
            : base(game)
        {
            Position = position;
            Size = size;
        }  */

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {

            _boundingBox = new BoundingBox(Position, Position + Size);
            _vertices = new VertexPositionNormalTexture[NUM_VERTICES];
            Vector3 min = _boundingBox.Min;
            Vector3 max = _boundingBox.Max;
            // Calculate the position of the vertices on the top face.
            Vector3 topLeftFront = new Vector3(min.X, max.Y, min.Z) ;
            Vector3 topLeftBack = new Vector3(min.X, max.Y, max.Z) ;
            Vector3 topRightFront = new Vector3(max.X, max.Y, min.Z) ;
            Vector3 topRightBack = new Vector3(max.X, max.Y, max.Z) ;

            // Calculate the position of the vertices on the bottom face.
            Vector3 btmLeftFront = new Vector3(min.X, min.Y, min.Z) ;
            Vector3 btmLeftBack = new Vector3(min.X, min.Y, max.Z) ;
            Vector3 btmRightFront = new Vector3(max.X, min.Y, min.Z) ;
            Vector3 btmRightBack = new Vector3(max.X, min.Y, max.Z) ;

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

        protected override void LoadContent()
        {
            Texture = new Texture2D(Game.GraphicsDevice, 1, 1);
            Texture.SetData<Color>(new[] { Color.Red });

            aspectRatio = Game.GraphicsDevice.Viewport.AspectRatio;
            effect = new BasicEffect(Game.GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if(_boundingBox.Intersects(gameMode.Player.BoundingBox))
            {
                gameMode.Player.Collided(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Lens activeLens = gameMode.ActiveLens;
            if (!(activeLens is RedLens))
                return;

            effect.World = 
                Matrix.CreateRotationY(MathHelper.ToRadians(Rotation)) *
                Matrix.CreateRotationX(MathHelper.ToRadians(Rotation)) * 
                Matrix.CreateTranslation(Vector3.Zero);

            effect.View = Matrix.CreateLookAt(CameraPosition, LookPosition, Vector3.Up);
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 0.5f, 1000.0f);

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
