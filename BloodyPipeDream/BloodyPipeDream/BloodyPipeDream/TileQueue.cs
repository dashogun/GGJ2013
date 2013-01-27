using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
			return tile;
		}

		public void Push(BloodyTile tile)
		{
			Queue.AddLast(tile);
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

			LinkedListNode<BloodyTile> node = Queue.First;
			do 
			{
				Rectangle tileArea = new Rectangle(
					XPos,
					(Game1.ScreenHeight - scaleHeight) / 2,
					scaleWidth,
					scaleWidth);
				node.Value.draw(tileArea, spriteBatch);
			} while (node.Next != null);
		}

		public void Update(SpriteBatch spriteBatch)
		{
		}
	}
}
