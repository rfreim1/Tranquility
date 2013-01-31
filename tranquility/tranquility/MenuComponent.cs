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
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        string[] menuItems;
        int SelectedItem;

        Color usual = Color.Gray;
        Color pressed = Color.White;

        KeyboardState keyboardstate;
        KeyboardState oldkeyboardstate;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Game1 game;

        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        float width = 0;
        float height = 0;

        public int selectedItem {
            get {
                return SelectedItem; 
            }
            set {
                SelectedItem = value;
                if (SelectedItem < 0) 
                    SelectedItem = 0;
                if (SelectedItem >= menuItems.Length)
                    SelectedItem = menuItems.Length - 1;
            }
        }

        private void MenuSize() {
            height = 0;
            width = 0;

            foreach (string item in menuItems) {
                Vector2 size = spriteFont.MeasureString(item);
                if (size.X > width)
                    width = size.X;
                height += spriteFont.LineSpacing + 10;
            }

            position = new Vector2((Game.Window.ClientBounds.Width - width) / 2, (2 * Game.Window.ClientBounds.Height) / 3);
        }


        public MenuComponent(Game1 game, SpriteBatch spriteBatch, SpriteFont spriteFont, string[] menuItems)
            : base(game)
        {
            // TODO: Construct any child components here
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.spriteFont = spriteFont;
            this.menuItems = menuItems;
            MenuSize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }

        public bool CheckKey(Keys theKey)
        {
            return keyboardstate.IsKeyUp(theKey) && oldkeyboardstate.IsKeyDown(theKey);
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            keyboardstate = Keyboard.GetState();

            if (CheckKey(Keys.Down)){
                SelectedItem++;
                if (SelectedItem == menuItems.Length)
                    SelectedItem = 0;            
            }
            if (CheckKey(Keys.Up)){
                SelectedItem--;
                if (SelectedItem < 0)
                    SelectedItem = menuItems.Length-1;
            }
            base.Update(gameTime);
            
            oldkeyboardstate = keyboardstate;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Vector2 location = position;
            Color tint;
            for (int i = 0; i < menuItems.Length; i++)
            {
                if (i == SelectedItem)
                    tint = pressed;
                else
                    tint = usual;
                spriteBatch.DrawString(spriteFont, menuItems[i], location, tint);
                location.Y += spriteFont.LineSpacing + 5;
            }
        }

    }
}
