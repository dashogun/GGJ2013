using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace BloodyPipeDream
{
    public abstract class BloodyTile
    {
        protected int mFill;
        protected int mMaxFill;

        public BloodyTile()
        {
            mFill = 0;
            mMaxFill = 1000;
        }

        //Returns index of connection to next cell
        //If input index doesn't correspond to a valid connection point, returns -1
        public virtual int getNextIndex(int index) { return -1; }

        //Add blood to this tile
        //Returns amount of blood that doesn't fit in tile
        public virtual int fill(int amount, GraphicsDevice graphicsDevice)
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

        public Texture2D CreateCircle(bool direction, float percent, GraphicsDevice graphicsDevice)
        {
            int radius = 128;
            int outerRadius = 130; // So circle doesn't go out of bounds
            Texture2D texture = new Texture2D(graphicsDevice, outerRadius, outerRadius);

            Color[] data = new Color[outerRadius * outerRadius];

            // Colour the entire texture transparent first.
            for (int i = 0; i < data.Length; i++)
                data[i] = Color.Transparent;

            // Work out the minimum step necessary using trigonometry + sine approximation.
            double angleStep = 1f / 128;

            double theta = percent * Math.PI / 2;
            double thetaStart, thetaEnd;
            thetaStart = direction ? thetaStart = Math.PI / 2 : Math.PI - theta;
            thetaEnd = direction ? Math.PI / 2 + theta : Math.PI;            

            for (double angle = thetaStart; angle <= thetaEnd; angle += angleStep)
            {
                // Use the parametric definition of a circle: http://en.wikipedia.org/wiki/Circle#Cartesian_coordinates

                for (double r = -100; r <= -28; r += 0.5)
                {
                    int x = (int)Math.Round(r * Math.Cos(angle)) + radius;
                    int y = (int)Math.Round(r * Math.Sin(angle)) + radius;
                    data[y * outerRadius + x + 1] = Color.White;
                }
            }

            texture.SetData(data);
            return texture;
        }

        public Texture2D CreateRectangle(Rectangle size, Rectangle filled, GraphicsDevice graphicsDevice)
        {
            Texture2D texture = new Texture2D(graphicsDevice, size.Width, size.Height);

            Color[] data = new Color[size.Width * size.Height];

            // Colour the entire texture transparent first.
            for (int x = filled.Left; x < filled.Right; ++x)
            {
                for (int y = filled.Top; y < filled.Bottom; ++y)
                {
                    data[y * size.Width + x] = Color.White;
                }
            }

            texture.SetData(data);
            return texture;
        }

        public abstract void draw(Rectangle area, SpriteBatch spritebatch);

		public virtual bool isNullTile()
		{
			return false;
		}
	}

    class BloodyNullTile : BloodyTile
    {
        private static Texture2D texture = null;

        int mOutIndex;

        public BloodyNullTile(int outIndex = -1)
        {
            mOutIndex = outIndex;
        }

		public override bool isNullTile()
		{
			return true;
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

		public override void draw(Rectangle area, SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, area, Color.White);
        }
    }

    class BloodyStartTile : BloodyTile
    {
        private static Texture2D[] textures = null;
        private Texture2D fillTex = null;
        int mOutIndex; // 0 = down, 1 = left, 2 = right, 3 = up

        public BloodyStartTile(int outIndex = 1)
        {
            mOutIndex = outIndex;
        }

        public override int getNextIndex(int index) { return mOutIndex; }

        public static void loadContent(Game game)
        {
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

        public override int fill(int amount, GraphicsDevice graphicsDevice)
        {
            int oldAmount = amount;
            amount = base.fill(amount, graphicsDevice);
            if (amount != oldAmount)
            {
                int fillPixels = (int)((mFill / (double)mMaxFill) * 128);
                int x0 = mOutIndex != 1 ? 0 : 128 - fillPixels;
                int y0 = mOutIndex != 3 ? 0 : 128 - fillPixels;
                int width = mOutIndex == 0 || mOutIndex == 3 ? 128 : fillPixels;
                int height = mOutIndex == 1 || mOutIndex == 2 ? 128 : fillPixels;
                //fillTex = CreateRectangle(new Rectangle(0, 0, 128, 128), new Rectangle(x0, y0, width, height), graphicsDevice);
                fillTex = CreateCircle(false, mFill / (float)mMaxFill, graphicsDevice);
            }
            return amount;
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

       	public override void draw(Rectangle area, SpriteBatch spritebatch) 
        { 
            Texture2D texture = null;
            SpriteEffects flip = SpriteEffects.None;
            getTextureFlip(ref texture, ref flip);
            spritebatch.Draw(texture, area, null, Color.White, 0, new Vector2(0,0), flip, 0);
            if (null!= fillTex)
                spritebatch.Draw(fillTex, area, null, Color.Red, 0, new Vector2(0, 0), flip, 0);
        }
    }



    class BloodyEndTile : BloodyTile
    {
        private static Texture2D texture = null;

        public override int getNextIndex(int index) { return -1; }

        public override int fill(int amount, GraphicsDevice graphicsDevice)
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

		public override void draw(Rectangle area, SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, area, null, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }

    class BloodyStraightTile : BloodyTile
    {
        int mRotation; //0 = vertical or 1 = horizontal

        private static Texture2D[] textures = null;
        private Texture2D fillTex = null;

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

        public override int fill(int amount, GraphicsDevice graphicsDevice)
        {
            int oldAmount = amount;
            amount = base.fill(amount, graphicsDevice);
            if (amount != oldAmount)
            {
                int fillPixels = (int)((mFill / (double)mMaxFill) * 128);
                int x0 = 0 == mRotation || (true) ? 0 : 128 - fillPixels;
                int y0 = 1 == mRotation || (true) ? 0 : 128 - fillPixels;
                int width = mRotation == 0 ? 128 : fillPixels;
                int height = mRotation == 1 ? 128 : fillPixels;
                fillTex = CreateRectangle(new Rectangle(0, 0, 128, 128), new Rectangle(x0, y0, width, height), graphicsDevice);
            }
            return amount;
        }

		public override void draw(Rectangle area, SpriteBatch spritebatch)
        {
            Texture2D texture = (mRotation == 1) ? textures[1] : textures[0];
			spritebatch.Draw(texture, area, Color.White);

            if (null != fillTex)
                spritebatch.Draw(fillTex, area, Color.Red);
        }
    }

    class BloodyCurvedTile : BloodyTile
    {
        int mRotation; //0 = bottom<->right, 1 = right<->top, 2 = top<->left, 3 = left<->bottom

        private static Texture2D texture = null;
        private Texture2D fillTex = null;

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


		public override void draw(Rectangle area, SpriteBatch spritebatch)
        {
            SpriteEffects flip = getTextureFlip();
            spritebatch.Draw(texture, area, null, Color.White, 0, new Vector2(0, 0), flip, 0);

            if (null != fillTex)
                spritebatch.Draw(fillTex, area, null, Color.Red, 0, new Vector2(0, 0), flip, 0);
        }
    }

    class BloodyGrid
    {
        int mRows, mCols;
        int mInnerRows, mInnerCols;
        int mStartX, mStartY;
        int mEndX, mEndY;
		Rectangle mArea;
		int[] mTileSize;
		Random Rand;

        //x,y
        int[,] mAdjacencyLUT;

        BloodyTile[,] mGrid;

        public BloodyCursor cursor = null;
        public BloodyGrid(int rows, int cols, Rectangle area)
		{
			Rand = new Random();
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

			mArea = area;

			initialize(rows, cols);

        }

		public void initialize(int rows, int cols)
		{
			mInnerRows = rows;
			mInnerCols = cols;
			mRows = rows + 2;
			mCols = cols + 2;
			mTileSize = new int[2] { mArea.Width / mRows, mArea.Height / mCols };
			mGrid = new BloodyTile[mRows, mCols];
			cursor = new BloodyCursor(mArea.Left, mArea.Top, mTileSize[0], mTileSize[1]);
			cursor.setGridPosition(1, 1);

			// zero out the grid with null tiles
			this.clearGrid();


			int startX = 0, startY = 1, endX = 1, endY = 0;
			int startOrientation = 0; // 0 = down, 1 = left, 2 = right, 3 = up
			do
			{
				// set random start and end positions
				int startSide = Rand.Next(4);
				int endSide = Rand.Next(4);
				int startPos = Rand.Next(mRows - 2);
				int endPos = Rand.Next(mCols - 2);
				switch (startSide)
				{
					case 0:
						startX = 0;
						startY = startPos + 1; // +1 to make sure the start isn't in the very corner
						startOrientation = 2;
						break;
					case 1:
						startX = startPos + 1;
						startY = 0;
						startOrientation = 0;
						break;
					case 2:
						startX = mRows - 1;
						startY = startPos + 1;
						startOrientation = 1;
						break;
					case 3:
						startX = startPos + 1;
						startY = mCols - 1;
						startOrientation = 3;
						break;
					default:
						throw new Exception("invalid start side in bloody grid initialization");
				}
				switch (endSide)
				{
					case 0:
						endX = 0;
						endY = endPos + 1; // +1 to make sure the start isn't in the very corner
						break;
					case 1:
						endX = endPos + 1;
						endY = 0;
						break;
					case 2:
						endX = mRows - 1;
						endY = endPos + 1;
						break;
					case 3:
						endX = endPos + 1;
						endY = mCols - 1;
						break;
					default:
						throw new Exception("invalid end side in bloody grid initialization");
				}
			} while (startX == endX && startY == endY);

			setStart(new BloodyStartTile(startOrientation), startX, startY);
			setEnd(new BloodyEndTile(), endX, endY);
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
            return !(x < 0 || y < 0 || x >= mRows || y >= mCols);
        }

		public bool canDrawTile(int x, int y)
		{
			return isInInnerGrid(x, y) || (x == mStartX && y == mStartY) || (x == mEndX && y == mEndY);
		}
		
        public bool isInInnerGrid(int x, int y)
        {
            return !(x < 1 || y < 1 || x >= mInnerRows+1 || y >= mInnerCols+1);
        }

        public BloodyTile getTile(int x, int y)
        {
            if (!isInGrid(x, y))
                return null;

            return mGrid[x, y];
        }

        public void moveCursor(int dx, int dy)
        {
            Vector2 ipos = cursor.getGridPosition();

            Vector2 dpos = new Vector2(dx, dy);

            Vector2 fpos = ipos + dpos;

            if (isInInnerGrid((int)fpos.X, (int)fpos.Y))
            {
                cursor.setGridPosition((int)fpos.X, (int)fpos.Y);
            }
        }

        //Returns the next tile in the sequence
        // all parameters are updated so that getNext may be run again on the next tile
        // Check that both io_index == -1 and return value is null to check for failed trace
        public BloodyTile getNext(ref int io_index, ref int io_x, ref int io_y)
        {
            BloodyTile curTile = getTile(io_x, io_y);
			io_index = curTile.getNextIndex(io_index);
			if (-1 == io_index)
				return null;

            io_x += mAdjacencyLUT[io_index, 0];
            io_y += mAdjacencyLUT[io_index, 1];
            io_index = ~io_index & 0x3;
            

            return getTile(io_x, io_y);
        }

        //Follows connections of tiles starting at specified position, entering tile from specified index
        public void trace(ref int io_index, ref int io_x, ref int io_y)
        {
            while (null != getNext(ref io_index, ref io_x, ref io_y));
        }

        public bool canInsert(BloodyTile toInsert, int x, int y)
        {
            if (isInInnerGrid(x, y) && getTile(x, y).isNullTile())
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
        public virtual bool fill(int amount, GraphicsDevice graphicsDevice)
        {
            BloodyTile curTile = getTile(mStartX, mStartY);
            int index = -1;
            int x = mStartX;
            int y = mStartY;

            while (0 != amount && null != curTile)
            {
                amount = curTile.fill(amount, graphicsDevice);
                curTile = getNext(ref index, ref x, ref y);
            }

            return 0 == amount;
        }

        public void clearGrid()
        {
            for (int i = 0; i < mCols; i++)
            {
                for (int j = 0; j < mRows; j++)
                {
                    mGrid[i, j] = new BloodyNullTile();
                    Debug.WriteLine("adding null tile to location {0},{1}", i, j);
                }
            }
        }

		public void attemptInsertAtCursor(ref TileQueue tileQueue)
		{
			BloodyTile nextTile = tileQueue.Peek();
			Vector2 cursorPos = cursor.getGridPosition();

			if (canInsert(nextTile, (int)cursorPos.X, (int)cursorPos.Y))
			{
				insert(tileQueue.Pull(), (int)cursorPos.X, (int)cursorPos.Y);
				tileQueue.Push(generateRandomTile());
			}
			else
			{
				//cursor.signalError();
				// play error sound
				Debug.WriteLine("Cannot place tile {0} at {1}, {2}", nextTile.GetType().Name, (int)cursorPos.X, (int)cursorPos.Y);
			}
		}

		public BloodyTile generateRandomTile()
		{
			int type = Rand.Next(6);
			switch (type)
			{
				case 0: return new BloodyStraightTile(0);
				case 1: return new BloodyStraightTile(1);
				case 2: return new BloodyCurvedTile(0);
				case 3: return new BloodyCurvedTile(1);
				case 4: return new BloodyCurvedTile(2);
				case 5: return new BloodyCurvedTile(3);
				default: throw new Exception(String.Format("invalid number for random tile generated: {0}", type));
			}
		}

        public void drawCursor(SpriteBatch spritebatch)
        {
            cursor.Draw(spritebatch);
        }

        public void drawTiles(SpriteBatch spritebatch)
        {
            int xloc = mArea.Left;
            int yloc = mArea.Top;

            // for each row
            for (int i = 0; i < mCols; i++)
            {
                // reset x to zero each time we move up a row
                xloc = mArea.Left;

                // for each column
                for (int j = 0; j < mRows; j++)
                {
					if (canDrawTile(j, i))
                    {
						//Debug.WriteLine("Drawing [{0},{1}] ({2}) at location ({3},{4})", j, i, mGrid[j, i].GetType(), xloc, yloc);
						Rectangle tileArea = new Rectangle(xloc, yloc, mTileSize[0], mTileSize[1]);
						mGrid[j, i].draw(tileArea, spritebatch);

						// debug testing
// 						String posStr = String.Format("({0},{1})\n({2},{3})", j, i, xloc, yloc);
// 						Vector2 posSize = Game1.SmallFont.MeasureString(posStr);
// 						Vector2 pos = new Vector2(xloc, yloc);
// 						spritebatch.DrawString(Game1.SmallFont, posStr, pos, Color.White);

					}
					xloc += mTileSize[0];
				}

				yloc += mTileSize[1];
			}
		}
	}
}
