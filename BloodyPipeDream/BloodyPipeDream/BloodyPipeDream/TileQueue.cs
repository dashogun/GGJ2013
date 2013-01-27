using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPipeDream
{
	class TileQueue
	{
		private static Texture2D Texture = null;
		private Rectangle Area;
		private Queue<BloodyTile> Queue;
		private int XPos, Width;

		public TileQueue(int xPos, int width)
		{
			Queue = new Queue<BloodyTile>(5);
			XPos = xPos;
			Width = width;
		}

		public BloodyTile Pull()
		{
			return Queue.Dequeue();
		}

		public void Push(BloodyTile tile)
		{
			Queue.Enqueue(tile);
		}

		public static void loadContent(Game game)
		{
			if (Texture == null)
			{
				Texture = game.Content.Load<Texture2D>("img/tile_lookahead");
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle size = Texture.Bounds;
			float scale = (float)Width / size.Width;
			int scaleWidth = (int)(size.Width * scale);
			int scaleHeight = (int)(size.Height * scale);
			Rectangle r = new Rectangle(
				XPos,
				(Game1.ScreenHeight - scaleHeight) / 2,
				scaleWidth,
				scaleHeight
				);
			spriteBatch.Draw(Texture, r, Color.White);
		}

		public void Update(SpriteBatch spriteBatch)
		{
		}
	}
}
