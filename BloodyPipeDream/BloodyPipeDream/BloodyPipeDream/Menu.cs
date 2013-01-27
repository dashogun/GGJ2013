using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BloodyPipeDream
{
	class Menu
	{
		public int Position;
		private String[] Options;
		private Texture2D BGTexture;

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
			if (BGTexture == null)
			{
				BGTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				BGTexture.SetData(new Color[] {Color.White});
			}

			// draw the title
			Rectangle title = new Rectangle(0, 0, Game1.ScreenWidth, Game1.ScreenHeight / 4);
			string titleStr = "BLOODY PIPE DREAM";
			Vector2 titleSize = Game1.TitleFont.MeasureString(titleStr);
			Vector2 titlePos = new Vector2((Game1.ScreenWidth / 2) - (titleSize.X / 2), 0);
			spriteBatch.DrawString(Game1.TitleFont, titleStr, titlePos, Color.DarkRed, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			// draw the background
			Rectangle bg = new Rectangle(Game1.ScreenWidth / 4, Game1.ScreenHeight / 4, Game1.ScreenWidth / 2, Game1.ScreenHeight / 2);
			spriteBatch.Draw(BGTexture, bg, Color.Black);

			// draw each option
			int yOffset = bg.Height / Options.Length;
			int yPos = bg.Top + (yOffset / 2);
			foreach (String opt in Options)
			{
				Vector2 size = Game1.Font.MeasureString(opt);
				Vector2 pos = new Vector2((Game1.ScreenWidth / 2) - (size.X / 2), yPos - (size.Y / 2));
				spriteBatch.DrawString(Game1.Font, opt, pos, Color.White);
				yPos += yOffset;
			}

			// draw the selection box
			Vector2 selTextSize = Game1.Font.MeasureString(Options[Position]);
			int selThickness = yOffset / 10;
			Rectangle selTop = new Rectangle(
				bg.Left,
				bg.Top + yOffset * Position,
				bg.Width,
				selThickness);
			Rectangle selBottom = new Rectangle(
				bg.Left,
				bg.Top + yOffset * (Position + 1) - selThickness,
				bg.Width,
				selThickness);
			Rectangle selLeft = new Rectangle(
				bg.Left,
				bg.Top + yOffset * Position,
				selThickness,
				yOffset);
			Rectangle selRight = new Rectangle(
				bg.Right - selThickness,
				bg.Top + yOffset * Position,
				selThickness,
				yOffset);

			spriteBatch.Draw(BGTexture, selTop, Color.Green);
			spriteBatch.Draw(BGTexture, selLeft, Color.Green);
			spriteBatch.Draw(BGTexture, selRight, Color.Green);
			spriteBatch.Draw(BGTexture, selBottom, Color.Green);
		}

		public void MoveUp()
		{
			if (Position - 1 >= 0)
			{
				Position--;
			}
		}

		public void MoveDown()
		{
			if (Position + 1 < Options.Length)
			{
				Position++;
			}
		}
	}
}
