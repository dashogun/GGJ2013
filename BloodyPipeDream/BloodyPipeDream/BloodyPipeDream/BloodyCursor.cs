using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPipeDream
{
    class BloodyCursor
    {
        private int grid_x;
        private int grid_y;
		private int XPos, YPos;
		private int Width, Height;
		private int Thickness;

        public BloodyCursor(int xOffset, int yOffset, int width, int height)
        {
            grid_x = 1;
            grid_y = 1;
			XPos = xOffset;
			YPos = yOffset;
			Width = width;
			Height = height;
			Thickness = 4;
        }

        public Vector2 getGridPosition()
        {
            return new Vector2(grid_x, grid_y);
        }

        public void setGridPosition(int grid_x, int grid_y)
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(XPos + grid_x * Width, YPos + grid_y * Height, Width, Height);
			Border border = new Border(r, Thickness, Color.Gold);
			border.Draw(spriteBatch);
        }
    }
}
