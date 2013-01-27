using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace BloodyPipeDream
{
	class Border
	{
		private Rectangle Target;
		private int Thickness;
		private Color Color;
		private Texture2D BGTexture;

		public Border(Rectangle area, int thickness, Color color)
		{
			Target = area;
			Thickness = thickness;
			Color = color;
		}

        public void setTargetRectangle(Rectangle rectangle)
        {
            Target = rectangle;
        }

		public void Draw(SpriteBatch spriteBatch)
		{
			if (BGTexture == null)
			{
				BGTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
				BGTexture.SetData(new Color[] { Color.White });
			}

			Rectangle selTop = new Rectangle(
				Target.Left,
				Target.Top,
				Target.Width,
				Thickness);
			Rectangle selBottom = new Rectangle(
				Target.Left,
				Target.Top + Target.Height - Thickness,
				Target.Width,
				Thickness);
			Rectangle selLeft = new Rectangle(
				Target.Left,
				Target.Top,
				Thickness,
				Target.Height);
			Rectangle selRight = new Rectangle(
				Target.Right - Thickness,
				Target.Top,
				Thickness,
				Target.Height);

            spriteBatch.Draw(BGTexture, selTop, Color);
            spriteBatch.Draw(BGTexture, selLeft, Color);
            spriteBatch.Draw(BGTexture, selRight, Color);
            spriteBatch.Draw(BGTexture, selBottom, Color);
		}
	}
}
