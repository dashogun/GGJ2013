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
            Rectangle r = new Rectangle(0, 0, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            border = new Border(r, 2, Color.Yellow);

            grid_x = 0;
            grid_y = 0;
        }

        public Vector2 getGridPosition()
        {
            return new Vector2(grid_x, grid_y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(grid_x * Globals.TILE_WIDTH, grid_y * Globals.TILE_HEIGHT, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            border.setTargetRectangle(r);
            border.Draw(spriteBatch);
        }

        public void moveLeft()
        {
            if (grid_x > 0) grid_x--;
        }

        public void moveRight()
        {
            if (grid_x < Globals.GRID_SIZE-1)
            {
                grid_x++;
            }
        }

        public void moveUp()
        {
            if (grid_y > 0)
            {
                grid_y--;
            }
        }

        public void moveDown()
        {
            if (grid_y < Globals.GRID_SIZE - 1)
            {
                grid_y++;
            }
        }
    }
}
