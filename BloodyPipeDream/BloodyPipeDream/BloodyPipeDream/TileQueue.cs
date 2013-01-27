using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BloodyPipeDream
{
	class TileQueue
	{
		private static Texture2D Texture = null;
		private LinkedList<BloodyTile> Queue;
		private int XPos, Width;

		public TileQueue(int xPos, int width)
		{
			Queue = new LinkedList<BloodyTile>();
			XPos = xPos;
			Width = width;
		}

		public BloodyTile Pull()
		{
			BloodyTile tile = Queue.First.Value;
			Queue.RemoveFirst();
			Debug.WriteLine("Pulling from queue: tile=" + tile.GetType().Name);
			return tile;
		}

		public void Push(BloodyTile tile)
		{
			Queue.AddLast(tile);
			Debug.WriteLine("Adding to queue: tile=" + tile.GetType().Name);
		}

		public BloodyTile Peek()
		{
			return Queue.First.Value;
		}

		public void Clear()
		{
			Queue.Clear();
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

			int superSecretSeparatorThickness = (int)(7 * scale);
			Rectangle tileArea = new Rectangle(
				XPos + 2 * superSecretSeparatorThickness,
				(Game1.ScreenHeight - scaleHeight) / 2 + 2 * superSecretSeparatorThickness,
				scaleWidth - 4 * superSecretSeparatorThickness,
				scaleWidth - 4 * superSecretSeparatorThickness);

			Rectangle borderArea = tileArea;
			borderArea.X -= 4;
			borderArea.Y -= 4;
			borderArea.Width += 8;
			borderArea.Height += 8;
			Border border = new Border(borderArea, 4, Color.Yellow);

			foreach (BloodyTile tile in Queue)
			{
				tile.draw(tileArea, spriteBatch);
				tileArea.Y += scaleWidth - superSecretSeparatorThickness-1;
			}
			Debug.WriteLine("# tiles in queue: {0}", Queue.Count);

			border.Draw(spriteBatch);
		}

		public void Update(SpriteBatch spriteBatch)
		{
		}
	}
}
