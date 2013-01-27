using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BloodyPipeDream
{
    public abstract class BloodyTile
    {
        int mFill;
        int mMaxFill;

        public BloodyTile()
        {
            mFill = 0;
            mMaxFill = 100;
        }

        //Returns index of connection to next cell
        //If input index doesn't correspond to a valid connection point, returns -1
        public virtual int getNextIndex(int index) { return -1; }

        //Add blood to this tile
        //Returns amount of blood that doesn't fit in tile
        public virtual int fill(int amount)
        {
            if (mFill >= mMaxFill)
                return amount;
            
            mFill += amount;
            if (mFill > mMaxFill)
            {
                amount = mFill - mMaxFill;
                mFill = mMaxFill;
                return amount;
            }

            return 0;
        }

        public abstract void draw(int pos_x, int pos_y, SpriteBatch spritebatch);
    }

    class BloodyNullTile : BloodyTile
    {
        private static Texture2D texture = null;

        int mOutIndex;

        public BloodyNullTile(int outIndex = -1)
        {
            mOutIndex = outIndex;
        }

        public override int getNextIndex(int index) { return mOutIndex; }

        public static void loadContent(Game game)
        {
            if (texture == null)
            {
                texture = new Texture2D(game.GraphicsDevice, 1, 1);
                texture.SetData(new Color[] { Color.Black });
            }
        }

        public override void draw(int pos_x, int pos_y, SpriteBatch spritebatch)
        {
            // todo; get values for tile width from somewhere else
            Rectangle r = new Rectangle(pos_x, pos_y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            spritebatch.Draw(texture, r, Color.White);
        }
    }

    class BloodyStartTile : BloodyTile
    {
        private static Texture2D[] textures = null;
        int mOutIndex;

        public BloodyStartTile(int outIndex = 1)
        {
            mOutIndex = outIndex;
        }

        public override int getNextIndex(int index) { return mOutIndex; }

        public static void loadContent(Game game)
        {
            //TODO rotate tile appropriatelly
            if (textures == null)
            {
                textures = new Texture2D[2];
                Debug.WriteLine("Initializing static value for start tile texture");
                textures[0] = game.Content.Load<Texture2D>("img/start_pipe_vertical_128");
                textures[1] = game.Content.Load<Texture2D>("img/start_pipe_horizontal_128");
            }
            else
            {
                Debug.WriteLine("start pipe texture is already initialized");
            }
        }

        void getTextureFlip(ref Texture2D texture, ref SpriteEffects flip)
        {
            switch (mOutIndex)
            {
                case 0:
                    texture = textures[0];
                    flip = SpriteEffects.None;
                    break;
                case 1:
                    texture = textures[1];
                    flip = SpriteEffects.None;
                    break;
                case 2:
                    texture = textures[1];
                    flip = SpriteEffects.FlipHorizontally;
                    break;
                case 3:
                    texture = textures[0];
                    flip = SpriteEffects.FlipVertically;
                    break;
            }
        }
        public override void draw(int pos_x, int pos_y, SpriteBatch spritebatch)
        {
            // todo; get values for tile width from somewhere else
            Rectangle dr = new Rectangle(pos_x, pos_y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);            
            Rectangle sr = new Rectangle(0, 0, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            Texture2D texture = null;
            SpriteEffects flip = SpriteEffects.None;
            getTextureFlip(ref texture, ref flip);
            spritebatch.Draw(texture, dr, null, Color.White, 0, new Vector2(0,0), flip, 0);
            //spritebatch.Draw(texture, r, Color.White);
        }
    }



    class BloodyEndTile : BloodyTile
    {
        private static Texture2D texture = null;

        public override int getNextIndex(int index) { return -1; }

        public virtual int fill(int amount)
        {
            //TODO: signal game end
            return 0;
        }

        public static void loadContent(Game game)
        {
            //TODO rotate tile appropriatelly
            if (texture == null)
            {
                Debug.WriteLine("Initializing static value for straight tile texture");
                texture = game.Content.Load<Texture2D>("img/end_pipe_128");
        }
            else
            {
                Debug.WriteLine("straight pipe texture is already initialized");
            }
        }

        public override void draw(int pos_x, int pos_y, SpriteBatch spritebatch)
        {
            Rectangle r = new Rectangle(pos_x, pos_y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            spritebatch.Draw(texture, r, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }

    class BloodyStraightTile : BloodyTile
    {
        int mRotation; //0 = vertical or 1 = horizontal

        private static Texture2D[] textures = null;

        public static void loadContent(Game game)
        {
            //TODO rotate tile appropriatelly
            if (textures == null)
            {
                textures = new Texture2D[2];
                Debug.WriteLine("Initializing static value for straight tile texture");
                textures[0] = game.Content.Load<Texture2D>("img/straight_pipe_vertical_128");
                textures[1] = game.Content.Load<Texture2D>("img/straight_pipe_horizontal_128");
            }
            else
            {
                Debug.WriteLine("straight pipe texture is already initialized");
            }
        }

        public BloodyStraightTile(int rotation)
        {
            mRotation = rotation;
        }

        public override int getNextIndex(int index)
        {
            if (0 == mRotation)
            {
                if (0 == index || 3 == index)
                    return ~index & 0x3;
            }
            else if (1 == mRotation)
            {
                if (1 == index || 2 == index)
                    return ~index & 0x3;
            }
            return -1;
        }


        public override void draw(int pos_x, int pos_y, SpriteBatch spritebatch)
        {
            // todo; get values for tile width from somewhere else
            Rectangle r = new Rectangle(pos_x, pos_y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            Texture2D texture = (mRotation == 1) ? textures[1] : textures[0];
            spritebatch.Draw(texture, r, Color.White);
        }
    }

    class BloodyCurvedTile : BloodyTile
    {
        int mRotation; //0 = bottom<->right, 1 = right<->top, 2 = top<->left, 3 = left<->bottom

        private static Texture2D texture = null;

        public BloodyCurvedTile(int rotation)
        {
            mRotation = rotation;
        }

        public static void loadContent(Game game)
        {
            //TODO rotate tile appropriatelly
            if (texture == null)
            {
                Debug.WriteLine("Initializing static value for straight tile texture");
                texture = game.Content.Load<Texture2D>("img/curved_pipe_128");
            }
            else
            {
                Debug.WriteLine("straight pipe texture is already initialized");
            }
        }

        public override int getNextIndex(int index)
        {
            if (0 == mRotation)
            {
                if (0 == index)
                    return 2;
                else if (2 == index)
                    return 0;
            }
            else if (1 == mRotation)
            {
                if (2 == index)
                    return 3;
                else if (3 == index)
                    return 2;
            }
            else if (2 == mRotation)
            {
                if (1 == index)
                    return 3;
                else if (3 == index)
                    return 1;
            }
            else if (3 == mRotation)
            {
                if (0 == index)
                    return 1;
                else if (1 == index)
                    return 0;
            }
            return -1;
        }

        SpriteEffects getTextureFlip()
        {
            switch (mRotation)
            {
                case 0:
                    return SpriteEffects.None;
                case 1:
                    return SpriteEffects.FlipVertically;
                case 2:
                    return SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally;
                case 3:
                    return SpriteEffects.FlipHorizontally;
            }
            return SpriteEffects.None;
        }

        public override void draw(int pos_x, int pos_y, SpriteBatch spritebatch)
        {
            // todo; get values for tile width from somewhere else
            Rectangle r = new Rectangle(pos_x, pos_y, Globals.TILE_WIDTH, Globals.TILE_HEIGHT);
            spritebatch.Draw(texture, r, null, Color.White, 0, new Vector2(0, 0), getTextureFlip(), 0);
        }
    }

    class BloodyGrid
    {
        int mWidth, mHeight;
        int mInnerWidth, mInnerHeight;
        int mStartX, mStartY;
        int mEndX, mEndY;

        //x,y
        int[,] mAdjacencyLUT;

        BloodyTile[,] mGrid;

        public BloodyCursor cursor = null;

        public BloodyGrid(int width, int height)
        {
            mInnerWidth = width;
            mInnerHeight = height;
            mWidth = width + 2;
            mHeight = height + 2;
            mGrid = new BloodyTile[mWidth, mHeight];
            mAdjacencyLUT = new int[4, 2];

            //Bottom
            mAdjacencyLUT[0, 0] = 0; 
            mAdjacencyLUT[0, 1] = -1;

            //Left
            mAdjacencyLUT[1, 0] = -1;
            mAdjacencyLUT[1, 1] = 0;

            //Right
            mAdjacencyLUT[2, 0] = 1;
            mAdjacencyLUT[2, 1] = 0;

            //Top
            mAdjacencyLUT[3, 0] = 0;
            mAdjacencyLUT[3, 1] = 1;

            cursor = new BloodyCursor();

            // zero out the grid with null tiles
            this.clearGrid();
        }

        public void setStart(BloodyStartTile start, int startX, int startY)
        {
            mGrid[startX, startY] = start;
            mStartX = startX;
            mStartY = startY;
        }

        public void setEnd(BloodyEndTile end, int endX, int endY)
        {
            mGrid[endX, endY] = end;
            mEndX = endX;
            mEndY = endY;
        }

        bool isInGrid(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= mWidth || y >= mHeight);
        }

        public bool isInInnerGrid(int x, int y)
        {
            return !(x < 1 || y < 1 || x >= mInnerWidth+1 || y >= mInnerHeight+1) || (x == mStartX && y == mStartY) || (x == mEndX && y == mEndY);
        }

        public BloodyTile getTile(int x, int y)
        {
            if (!isInGrid(x, y))
                return null;

            return mGrid[x, y];
        }

        //Returns the next tile in the sequence
        // all parameters are updated so that getNext may be run again on the next tile
        // Check that both io_index == -1 and return value is null to check for failed trace
        public BloodyTile getNext(ref int io_index, ref int io_x, ref int io_y)
        {
            BloodyTile curTile = getTile(io_x, io_y);
            io_index = curTile.getNextIndex(io_index);
            io_x += mAdjacencyLUT[io_index, 0];
            io_y += mAdjacencyLUT[io_index, 1];
            io_index = ~io_index & 0x3;
            
            if (-1 == io_index)
                return null;

            return getTile(io_x, io_y);
        }

        //Follows connections of tiles starting at specified position, entering tile from specified index
        public void trace(ref int io_index, ref int io_x, ref int io_y)
        {
            while (null != getNext(ref io_index, ref io_x, ref io_y));
        }

        public bool canInsert(BloodyTile toInsert, int x, int y)
        {
            if (isInInnerGrid(x, y) && null == getTile(x, y))
            {
                int index = -1;
                int traceX = mStartX;
                int traceY = mStartY;
                trace(ref index, ref traceX, ref traceY);

                if (x == traceX && y == traceY)
                {
                    index = toInsert.getNextIndex(index);
                    if (-1 == index)
                        return false;
                    traceX += mAdjacencyLUT[index, 0];
                    traceY += mAdjacencyLUT[index, 1];
                    return isInInnerGrid(traceX, traceY);
                }

                return true;
            }
            return false;
        }

        public void insert(BloodyTile toInsert, int x, int y)
        {
            mGrid[x, y] = toInsert;
        }

        //Returns false if there aren't enough connected tiles to hold all of the blood
        public virtual bool fill(int amount)
        {
            BloodyTile curTile = getTile(mStartX, mStartY);
            int index = -1;
            int x = mStartX;
            int y = mStartY;

            while (0 != amount && null != curTile)
            {
                amount = curTile.fill(amount);
                curTile = getNext(ref index, ref x, ref y);
            }

            return 0 == amount;
        }

        public void clearGrid()
        {
            for (int i = 0; i < mHeight; i++)
            {
                for (int j = 0; j < mWidth; j++)
                {
                    mGrid[i, j] = new BloodyNullTile();
                    Debug.WriteLine("adding null tile to location {0},{1}", i, j);
                }
            }
        }

        public void drawCursor(SpriteBatch spritebatch)
        {
            cursor.Draw(spritebatch);
        }

        public void drawTiles(SpriteBatch spritebatch)
        {

            // for each row
            for (int i = 0; i < mHeight; i++)
            {
                // for each column
                for (int j = 0; j < mWidth; j++)
                {
                    if (isInInnerGrid(j, i))
                    {
                        //Debug.WriteLine("Drawing [{0},{1}] ({2}) at location ({3},{4})", j, i, mGrid[j, i].GetType(), Globals.TILE_WIDTH * j, Globals.TILE_HEIGHT * i);
                        mGrid[j, i].draw(Globals.TILE_WIDTH * j, Globals.TILE_HEIGHT * i, spritebatch);
                    }
                }
            }
        }
    }
}
