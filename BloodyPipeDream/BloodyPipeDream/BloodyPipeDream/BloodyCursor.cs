using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPipeDream
{
    class BloodyCursor
    {
        private int grid_x;
        private int grid_y;

        BloodyTile next_tile = null; // the tile next in the queue

        Border border = null;

        public BloodyCursor()
        {
            grid_x = 1;
            grid_y = 1;

            Rectangle r = new Rectangle(0, 0, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            border = new Border(r, 2, Color.Yellow);
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
            Rectangle r = new Rectangle(grid_x * Globals.TILE_WIDTH, grid_y * Globals.TILE_HEIGHT, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            border.setTargetRectangle(r);
            border.Draw(spriteBatch);
        }

        
    }
}
