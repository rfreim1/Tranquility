using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tranquility
{
    class Instructions
    {
        MenuComponent menuComponent;
        Texture2D image;
        Rectangle imageRec;
        Game1 game;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        string menuTitle; 
        KeyboardState keyboardstate;
        KeyboardState oldkeyboardstate;

        private string instructionsString =

"  Use Lenses to Navigate the Maze to reach Exit\n\n" +
"  Lens Health drops the longer you stay in\n" +
"  Don't Let the Lenses Health Drop to Zero or the Stalker will get You!!\n\n" +
"  Move with WASD keys\n" +
"  Move Mouse to move Camera & Direction\n\n" +
"  1. Z for NO Lens\n" +
"  2. X for Red Lens/See Walls Use Lens to See Walls, Traps, and Coins\n" +
"  3. C for Green Lens/See Traps \n" +
"  4. V for Blue Lens/See Coins \n" +
"  5. Coins recharges the wall and trap lens health\n" +
"  6. You can Pause your game using P or Escape key\n\n";

        private Vector2 instructionsPosition = new Vector2(0, 100);

        public int SelectedItem
        {
            get { return menuComponent.selectedItem; }
            set { menuComponent.selectedItem = value; }
        }

        public bool CheckKey(Keys theKey)
        {
            return keyboardstate.IsKeyUp(theKey) && oldkeyboardstate.IsKeyDown(theKey);
        }

        public Instructions (Game1 game, SpriteBatch spriteBatch) {
            this.game = game;
            this.spriteBatch = spriteBatch;
            menuTitle = "Instructions";
            spriteFont = game.Content.Load<SpriteFont>("menufont");
            image = game.Content.Load<Texture2D>("MenuBG");
            string[] menuItems = { "Back to Menu" };
            menuComponent = new MenuComponent(game, spriteBatch, spriteFont, menuItems);
            game.Components.Add(menuComponent);
            menuComponent.Position = new Vector2(game.Window.ClientBounds.Width - game.Window.ClientBounds.Width / 4 , 50);
            imageRec = new Rectangle(0, 0, game.Window.ClientBounds.Width, game.Window.ClientBounds.Height);
        }



        public void Update(GameTime gameTime)
        {
            keyboardstate = Keyboard.GetState();
            if (CheckKey(Keys.Enter))
            {
                if (SelectedItem == 0)
                {
                    ((Game1)game).Disable(menuComponent);
                    game.goStartScreen(); 
                }
            }
            oldkeyboardstate = keyboardstate;
        }

        public void Draw(GameTime gameTime)
        {
           
            spriteBatch.Draw(image, imageRec, Color.White);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(game.Window.ClientBounds.Width / 2,
                game.Window.ClientBounds.Height - 440);
            Color titleColor = Color.White;
            Vector2 titleOrigin = spriteFont.MeasureString(menuTitle) / 2;
            spriteBatch.DrawString(spriteFont, menuTitle, titlePosition, titleColor, 
                0, titleOrigin, 2f, SpriteEffects.None, 0);

            spriteBatch.DrawString(spriteFont, instructionsString, instructionsPosition, titleColor);
        }
    }
}

