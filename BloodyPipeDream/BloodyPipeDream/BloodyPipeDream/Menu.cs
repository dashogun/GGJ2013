using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPipeDream
{
	class Menu
	{
		public int Position;
		private String[] Options;

		public Menu(String[] options)
		{
			Options = options;
			Position = 0;
		}

		public void Update(GameTime gameTime)
		{
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle bg = new Rectangle(Game1.ScreenWidth / 4, Game1.ScreenHeight / 4, Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
			Texture2D bgTexture = new Texture2D(spriteBatch.GraphicsDevice, bg.Width, bg.Height);
			spriteBatch.Draw(bgTexture, bg, Color.Black);

			int x = bg.Width / 2;
			int y = bg.Height / (Options.Length + 1);
			foreach (String opt in Options)
			{
				spriteBatch.DrawString(Game1.Font, opt, );
			}
		}

		public void MoveUp()
		{
			if (Position + 1 < Options.Length)
			{
				Position++;
			}
		}

		public void MoveDown()
		{
			if (Position - 1 > 0)
			{
				Position--;
			}
		}
	}
}
