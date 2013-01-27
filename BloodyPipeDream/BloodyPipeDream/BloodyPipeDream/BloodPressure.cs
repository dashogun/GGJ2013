using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BloodyPipeDream
{
	class BloodPressure
	{
		private static Texture2D TextureFG = null;
		private static Texture2D TextureMG = null;
		private static Texture2D TextureBG = null;
		private int XPos, Width;
		private int BP;

		public BloodPressure(int xPos, int width)
		{
			XPos = xPos;
			Width = width;
			BP = 0;
		}

		public static void loadContent(Game game)
		{
			if (TextureFG == null)
			{
				TextureFG = game.Content.Load<Texture2D>("img/vial_foreground");
			}
			if (TextureMG == null)
			{
				TextureMG = new Texture2D(game.GraphicsDevice, 1, 1);
				TextureMG.SetData(new Color[] { Color.DarkRed });
			}
			if (TextureBG == null)
			{
				TextureBG = game.Content.Load<Texture2D>("img/vial_background");
			}
		}

		public void setPressure(int bp)
		{
			BP = bp;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle size = TextureBG.Bounds;
			float scale = (float)Width / size.Width;
			int scaleWidth = (int)(size.Width * scale);
			int scaleHeight = (int)(size.Height * scale);
			Rectangle area = new Rectangle(
				XPos,
				(Game1.ScreenHeight - scaleHeight) / 2,
				scaleWidth,
				scaleHeight);

			spriteBatch.Draw(TextureBG, area, Color.White);

			if (BP > 0)
			{
				// calculate size of blood
				Rectangle bloodArea = area;
				bloodArea.Height = bloodArea.Height * BP / 100;
				bloodArea.Y = bloodArea.Y + area.Height - bloodArea.Height;
				spriteBatch.Draw(TextureMG, bloodArea, Color.White);
			}

			spriteBatch.Draw(TextureFG, area, Color.White);
		}
	}
}
