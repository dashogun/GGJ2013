using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BloodyPipeDream
{
    public class BloodyTile
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
    }

    class BloodyStartTile : BloodyTile
    {
        int mOutIndex;

        public BloodyStartTile(int outIndex = 1)
        {
            mOutIndex = outIndex;
        }

        public override int getNextIndex(int index) { return mOutIndex; }
    }

    class BloodyEndTile : BloodyTile
    {
        public override int getNextIndex(int index) { return -1; }

        public virtual int fill(int amount)
        {
            //TODO: signal game end
            return 0;
        }
    }

    class BloodyStraightTile : BloodyTile
    {
        int mRotation; //0 = vertical or 1 = horizontal

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
    }

    class BloodyCurvedTile : BloodyTile
    {
        int mRotation; //0 = bottom<->right, 1 = right<->top, 2 = top<->left, 3 = left<->bottom
        public BloodyCurvedTile(int rotation)
        {
            mRotation = rotation;
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
    }

    class BloodyGrid
    {
        int mWidth, mHeight;
        int mStartX, mStartY;

        //x,y
        int[,] mAdjacencyLUT;

        BloodyTile[,] mGrid;

        public BloodyGrid(int width, int height)
        {
            mWidth = width;
            mHeight = height;
            mGrid = new BloodyTile[width, height];
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
        }

        public void setStart(BloodyStartTile start, int startX, int startY)
        {
            mGrid[startX, startY] = start;
            mStartX = startX;
            mStartY = startY;
        }

        public bool isInGrid(int x, int y)
        {
            return !(x < 0 || y < 0 || x >= mWidth || y >= mHeight);
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
            if (isInGrid(x, y) && null == getTile(x, y))
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
                    return isInGrid(traceX, traceY);
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
    }
}
